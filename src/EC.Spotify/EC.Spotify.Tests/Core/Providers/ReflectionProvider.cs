using System.Reflection;

namespace EC.Spotify.Tests.Core.Providers;

internal static class ReflectionProvider
{
    private static Dictionary<Type, object> _processedObjects = [];

    public static T? PopulateObject<T>() where T : class => (T?)PopulateObjectRecursive(typeof(T));
    public static object? PopulateObjectRecursive(Type? type)
    {
        if (type == null) return default;

        if (_processedObjects.ContainsKey(type)) return _processedObjects[type];

        
        if (type.IsPrimitive || type == typeof(string) || type.IsValueType) return GetSampleValue(type);

        var instance = Activator.CreateInstance(type);
        if (instance is null) return default;

        _processedObjects[type] = instance;

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanWrite) continue;
            try
            {
                var propertyType = property.PropertyType;
                var propertyValue = (propertyType.IsPrimitive || propertyType == typeof(string) || propertyType.IsValueType)
                        ? GetSampleValue(propertyType)
                        : PopulateObjectRecursive(propertyType);

                if (propertyValue is not null) property.SetValue(instance, propertyValue);
            }
            catch (Exception)
            {

            }
        }

        return instance;
    }

    private static object? GetSampleValue(Type type)
    {
        if (type == typeof(int)) return 1;
        if (type == typeof(string)) return "SampleString";
        if (type == typeof(bool)) return true;
        if (type == typeof(DateTime)) return DateTime.Now;

        // Add more types ?
        return Activator.CreateInstance(type);
    }
}
