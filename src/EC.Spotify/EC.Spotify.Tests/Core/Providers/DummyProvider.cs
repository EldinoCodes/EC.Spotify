using System.Collections.Concurrent;
using System.Reflection;

namespace EC.Spotify.Tests.Core.Providers;

/*
 * i might end up turning this into something more fun at some point... kind of a crazy idea
 * think i will call it stubby or something like that, but for now it's just DummyProvider
 */

internal static class DummyProvider
{
    // Shared forever — PropertyInfo[] for a type never changes, safe across threads
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
    // Cached Add/AddRange MethodInfo per concrete collection type
    private static readonly ConcurrentDictionary<Type, MethodInfo?> _addMethodCache = new();
    // Cached concrete implementations per interface or abstract type; never changes across runs
    private static readonly ConcurrentDictionary<Type, Type[]> _implementationCache = new();
    // Per-run object pool; call Reset() between test classes to avoid cross-test pollution
    private static readonly ConcurrentDictionary<Type, object> _processedObjects = new();

    // Per-run object pool; call Reset() between test classes to avoid cross-test pollution
    private static readonly ConcurrentDictionary<Type, object?> _oneTimeObjects = new();

    // ThreadStatic guards against circular references within a single construction call-tree
    [ThreadStatic]
    private static HashSet<Type>? _inProgress;

    public static void Reset()
    {
        _oneTimeObjects.Clear();
        _processedObjects.Clear();        
    }

    public static T? DummyObject<T>() => (T?)DummyObject(typeof(T));
    public static object? DummyObject(Type? type)
    {
        if (type is null) return null;

        var instance = PopulateInstance(type);
        if (instance is null) return null;

        _processedObjects.TryAdd(type, instance);
        return instance;
    }

    public static void AddDummy<T>(T? obj) => _processedObjects.AddOrUpdate(typeof(T), obj!, (t, _) => obj!);
    public static void AddOneTimeDummy<T>(T? obj) => _oneTimeObjects.AddOrUpdate(typeof(T), obj!, (t, _) => obj!);

    /// <summary>Returns one populated instance of every concrete type that implements <typeparamref name="T"/>.</summary>
    public static IEnumerable<T> DummyObjects<T>() => DummyObjects(typeof(T)).Cast<T>();
    public static IEnumerable<object> DummyObjects(Type type) => GetConcreteImplementations(type).Select(t => PopulateInstance(t)).OfType<object>();
    

    private static object? PopulateInstance(Type? type)
    {
        if (type is null) return null;

        // Short-circuit: one-time override for this type
        if (_oneTimeObjects.TryRemove(type, out var oneTimeCached)) return oneTimeCached;

        // Short-circuit: already built in this run
        if (_processedObjects.TryGetValue(type, out var cached)) return cached;

        // Primitives, strings and value types go straight to sample values
        if (type.IsPrimitive || type == typeof(string) || type.IsEnum) return GetSampleValue(type);

        // Unwrap Nullable<T> and populate the inner type
        var nullableUnderlying = Nullable.GetUnderlyingType(type);
        if (nullableUnderlying is not null) 
            return PopulateInstance(nullableUnderlying);

        if (type.IsValueType) 
            return GetSampleValue(type);

        if (type.IsArray) 
            return PopulateArray(type);

        if (type.IsGenericType)
        {
            var genericDef = type.GetGenericTypeDefinition();

            if (genericDef == typeof(List<>)        ||
                genericDef == typeof(IList<>)       ||
                genericDef == typeof(ICollection<>) ||
                genericDef == typeof(IEnumerable<>))
                return PopulateList(type);

            if (genericDef == typeof(Dictionary<,>) || genericDef == typeof(IDictionary<,>))
                return PopulateDictionary(type);

            return PopulateObject(type);
        }

        return PopulateObject(type);
    }

    private static object? PopulateObject(Type type)
    {
        if (type.IsInterface || type.IsAbstract)
            return GetConcreteImplementations(type)
                .Select(t => PopulateInstance(t))
                .FirstOrDefault(o => o is not null);

        // Guard against circular references within the same call tree
        _inProgress ??= [];
        if (!_inProgress.Add(type)) return null;

        try
        {
            // Close open generic types by substituting each unconstrained parameter with object
            Type concreteType = type;
            if (type.ContainsGenericParameters)
            {
                if (!type.IsGenericTypeDefinition) return null;
                var typeArgs = type.GetGenericArguments()
                                   .Select(a => a.GetGenericParameterConstraints()
                                                 .FirstOrDefault(c => c.IsClass) ?? typeof(object))
                                   .ToArray();
                concreteType = type.MakeGenericType(typeArgs);
            }

            var instance = Activator.CreateInstance(concreteType);
            if (instance is null) return null;

            // Register before populating properties so recursive calls find it and break cycles
            _processedObjects[type] = instance;

            var properties = _propertyCache.GetOrAdd(
                concreteType, 
                t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            );

            foreach (var property in properties)
            {
                if (!property.CanWrite) continue;
                try
                {
                    var value = PopulateInstance(property.PropertyType);
                    if (value is not null) property.SetValue(instance, value);
                }
                catch { }
            }

            return instance;
        }
        finally
        {
            _inProgress.Remove(type);
        }
    }

    private static object? PopulateList(Type type)
    {
        var elementType = type.GetGenericArguments()[0];
        var listType = typeof(List<>).MakeGenericType(elementType);
        var addMethod = _addMethodCache.GetOrAdd(listType, t => t.GetMethod("Add"));

        var list = Activator.CreateInstance(listType);

        var implementations = GetConcreteImplementations(elementType);
        foreach (var implementation in implementations)
        {
            var element = PopulateInstance(implementation);
            if (element is not null) 
                addMethod?.Invoke(list, [element]);
        }

        return list;
    }

    private static object? PopulateArray(Type type)
    {
        var elementType = type.GetElementType();
        if (elementType is null) return null;

        var implementations = GetConcreteImplementations(elementType);
        var array = Array.CreateInstance(elementType, implementations.Length);

        for(var idx = 0; idx < implementations.Length; idx++)
        {
            var element = PopulateInstance(implementations[idx]);
            if (element is not null) array.SetValue(element, idx);
        }

        return array;
    }

    private static object? PopulateDictionary(Type type)
    {
        var args = type.GetGenericArguments();
        var dictType = typeof(Dictionary<,>).MakeGenericType(args[0], args[1]);
        var dict = Activator.CreateInstance(dictType);
        var addMethod = _addMethodCache.GetOrAdd(dictType, t => t.GetMethod("Add"));

        var key = PopulateInstance(args[0]);
        var value = PopulateInstance(args[1]);
        if (key is not null && value is not null) addMethod?.Invoke(dict, [key, value]);

        return dict;
    }

    private static object? GetSampleValue(Type type)
    {
        if (type == typeof(string))        return "SampleString";
        if (type == typeof(bool))          return true;
        if (type == typeof(int))           return 1;
        if (type == typeof(uint))          return 1u;
        if (type == typeof(long))          return 1L;
        if (type == typeof(ulong))         return 1ul;
        if (type == typeof(short))         return (short)1;
        if (type == typeof(ushort))        return (ushort)1;
        if (type == typeof(byte))          return (byte)1;
        if (type == typeof(sbyte))         return (sbyte)1;
        if (type == typeof(float))         return 1.0f;
        if (type == typeof(double))        return 1.0d;
        if (type == typeof(decimal))       return 1.0m;
        if (type == typeof(char))          return 'A';
        if (type == typeof(Guid))          return Guid.NewGuid();
        if (type == typeof(DateTime))      return DateTime.UtcNow;
        if (type == typeof(DateTimeOffset)) return DateTimeOffset.UtcNow;
        if (type == typeof(TimeSpan))      return TimeSpan.FromSeconds(1);
        if (type.IsEnum)
        {
            var values = Enum.GetValues(type);
            return values.Length > 1 ? values.GetValue(1) : values.GetValue(0);
        }

        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    private static Type[] GetConcreteImplementations(Type type) =>
        _implementationCache.GetOrAdd(type, t =>
            [.. AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => { try { return a.GetTypes(); } catch { return []; } })
                    .Where(t2 => !t2.IsAbstract && !t2.IsInterface && !t2.ContainsGenericParameters && t.IsAssignableFrom(t2))]);
}
