using System;
using System.Collections;
using System.Reflection;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// ToDictionaryConfiguration
/// </summary>
public sealed class ToDictionaryConfiguration : BaseConfiguration<ToDictionaryConfiguration>
{
    internal ToDictionaryConfiguration() => SetSelf(this);
    internal Func<PropertyInfo, object, object> ValueMapper { get; private set; } = DefaultValueMapper;
    internal Func<Type, bool> ComplexTypeChecker { get; private set; } = DefaultComplexTypeChecker;

    /// <summary>
    /// Allows for custom detection of a complex type
    /// </summary>
    /// <remarks>
    /// This can be used to override the default behaviour if 
    /// it does not cover your needs
    /// </remarks>
    /// <param name="isAComplexTypeChecker"></param>
    /// <returns></returns>
    public BaseConfiguration<ToDictionaryConfiguration> WithComplexTypeChecker(Func<Type, bool> isAComplexTypeChecker)
    {
        ComplexTypeChecker = isAComplexTypeChecker.AssertNotNull(nameof(isAComplexTypeChecker));
        return this;
    }


    /// <summary>
    /// Use a custom value mapper
    /// </summary>
    /// <remarks>
    /// Can be useful for DateTime and JObject issues
    /// </remarks>
    /// <param name="valueMapper"></param>
    /// <returns></returns>
    public BaseConfiguration<ToDictionaryConfiguration> WithValueMapper(Func<PropertyInfo, object, object> valueMapper)
    {
        ValueMapper = valueMapper.AssertNotNull(nameof(valueMapper));
        return this;
    }

    private static object DefaultValueMapper(PropertyInfo propertyInfo, object value) => value;
    
    private static bool DefaultComplexTypeChecker(Type type) =>
        type.IsClass && !type.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(type);
}
