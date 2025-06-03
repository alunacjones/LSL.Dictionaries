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
    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary(null);
    }

    /// <summary>
    /// Converts an object to a dictionary
    /// </summary>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDictionary<string, object> ToDictionary(this object source, Action<ToDictionaryConfiguration> action)
    {
        if (source == null) return null;
        action ??= new Action<ToDictionaryConfiguration>(_ => { });

        var configuration = new ToDictionaryConfiguration();
        action?.Invoke(configuration);

        return source.ToDictionaryInternal(configuration);
    }

    internal static IDictionary<string, object> ToDictionaryInternal(this object source, ToDictionaryConfiguration configuration)
    {
        var result = new Dictionary<string, object>();

        foreach (var property in source.GetType().GetProperties())
        {
            var name = configuration.PropertyNameProvider(property);
            if (configuration.ComplexTypeChecker(property.PropertyType))
            {
                result[name] = property.GetValue(source).ToDictionaryInternal(configuration);
            }
            else
            {
                result[name] = property.GetValue(source);
            }
        }

        return result;
    }
}