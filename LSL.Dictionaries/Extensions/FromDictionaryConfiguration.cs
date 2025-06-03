using System;
using System.Reflection;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// FromDictionaryConfiguration
/// </summary>
public class FromDictionaryConfiguration : BaseConfiguration<FromDictionaryConfiguration>
{
    internal FromDictionaryConfiguration() => SetSelf(this);

    internal Func<PropertyInfo, object, object> ValueMapper { get; private set; } = DefaultValueMapper;

    /// <summary>
    /// Use a custom value mapper
    /// </summary>
    /// <remarks>
    /// Can be useful for DateTime and JObject issues
    /// </remarks>
    /// <param name="valueMapper"></param>
    /// <returns></returns>
    public BaseConfiguration<FromDictionaryConfiguration> WithValueMapper(Func<PropertyInfo, object, object> valueMapper)
    {
        ValueMapper = valueMapper.AssertNotNull(nameof(valueMapper));
        return this;
    }

    private static object DefaultValueMapper(PropertyInfo propertyInfo, object value) => value;
}