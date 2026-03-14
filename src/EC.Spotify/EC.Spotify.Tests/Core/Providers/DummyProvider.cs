using System.Reflection;

namespace EC.Spotify.Tests.Core.Providers;

internal static class DummyProvider
{
    private static Dictionary<Type, object> _processedObjects = [];

    public static T? DummyObject<T>() where T : class => (T?)DummyObject(typeof(T));
    public static object? DummyObject(Type? type)
    {
        if (type == null) return default;
        if (_processedObjects.ContainsKey(type)) return _processedObjects[type];

        // create core instance
        var instance = PopulateInstance(type);
        if (instance is null) return default;        

        _processedObjects[type] = instance;

        return instance;
    }

    private static object? PopulateInstance(Type? type)
    {
        if (type is null) return default;

        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)
            ? PopulateListObject(type)
            : PopulateObject(type);
    }

    private static object? PopulateObject(Type? type, object? instance = null)
    {
        if (type is null) return instance;
        if (type.IsPrimitive || type == typeof(string) || type.IsValueType) return GetSampleValue(type);

        instance ??= Activator.CreateInstance(type);

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanWrite) continue;
            try
            {
                var propertyType = property.PropertyType;
                var propertyValue = (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType.IsValueType)
                        ? GetSampleValue(propertyType)
                        : PopulateInstance(propertyType);
                if (propertyValue is not null) property.SetValue(instance, propertyValue);
            }
            catch (Exception)
            {

            }
        }
        return instance;
    }

    private static object? PopulateListObject(Type? type, object? instance = null)
    {
        if (type is null) return instance;

        instance ??= Activator.CreateInstance(type);

        var listType = type.GetGenericArguments().ElementAtOrDefault(0);
        if (listType is null) return instance;

        var listInstance = typeof(List<>).MakeGenericType(listType);
        var listAddMethod = listInstance.GetMethod("Add");

        var childInstance = PopulateInstance(listType);
        if (childInstance is null) return instance;

        listAddMethod?.Invoke(instance, [childInstance]);

        return instance;
    }

    private static object? GetSampleValue(Type type)
    {
        if (type == typeof(int)) return 1;
        if (type == typeof(string)) return "SampleString";
        if (type == typeof(bool)) return true;
        if (type == typeof(DateTime)) return DateTime.UtcNow;

        return Activator.CreateInstance(type);
    }
}
