using System;
using System.Collections;
using System.Collections.Generic;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// ObjectExtensions for Dictionaries
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Converts an object into a dictionary
    /// </summary>
    /// <param name="source">Source object to convert to a dictionary</param>
    /// <returns>The dictionary version of the source object</returns>
    public static IDictionary<string, object> ToDictionary(this object source) => source.ToDictionary(null);

    /// <summary>
    /// Converts an object to a dictionary
    /// </summary>
    /// <param name="source"></param>
    /// <param name="configurator"></param>
    /// <returns></returns>
    public static IDictionary<string, object> ToDictionary(this object source, Action<ToDictionaryConfiguration> configurator)
    {
        configurator ??= new Action<ToDictionaryConfiguration>(_ => { });

        var configuration = new ToDictionaryConfiguration();
        configurator?.Invoke(configuration);

        return source.ToDictionaryInternal(configuration);
    }

    internal static IDictionary<string, object> ToDictionaryInternal(this object source, ToDictionaryConfiguration configuration)
    {
        if (source == null) return null;

        var result = new Dictionary<string, object>();

        foreach (var property in source.GetType().GetProperties())
        {
            var value = configuration.ValueMapper(property, property.GetValue(source));
            var name = configuration.PropertyNameProvider(property);

            if (configuration.PropertyFilter(property, value) is false) continue;

            if (configuration.ComplexTypeChecker(property.PropertyType))
            {
                result[name] = value.ToDictionaryInternal(configuration);
            }
            else
            {
                result[name] = value;
            }
        }

        return result;
    }
}
