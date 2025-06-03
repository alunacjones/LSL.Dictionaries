using System;
using System.Collections.Generic;
using System.Linq;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// DictionaryExtensions
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Converts a dictionary to an instance of <typeparamref name="T"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static T ToObject<T>(this IDictionary<string, object> source, Action<FromDictionaryConfiguration> configurator)
    {
        var configuration = new FromDictionaryConfiguration();
        configurator.AssertNotNull(nameof(configurator)).Invoke(configuration);

        return (T)source.ToObjectInternal(typeof(T), configuration);

    }

    /// <summary>
    /// Converts a dictionary to an instance of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T ToObject<T>(this IDictionary<string, object> source) =>
        (T)source.ToObjectInternal(typeof(T), new FromDictionaryConfiguration());

    internal static object ToObjectInternal(this IDictionary<string, object> source, Type type, FromDictionaryConfiguration configuration)
    {
        source.AssertNotNull(nameof(source));

        var result = Activator.CreateInstance(type);

        foreach (var property in type.GetProperties())
        {
            var name = configuration.PropertyNameProvider(property);
            var value = source.TryGetValue(name, out var foundValue) ? foundValue : null;

            if (configuration.PropertyFilter(property, value) is false) continue;

            if (value is IDictionary<string, object> dictionary)
            {
                property.SetValue(result, dictionary.ToObjectInternal(property.PropertyType, configuration));
            }
            else
            {
                property.SetValue(result, value);
            }
        }

        return result;
    } 
}