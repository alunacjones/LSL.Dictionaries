using System;
using System.Collections;
using System.Reflection;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// ToDictionaryConfiguration
/// </summary>
public sealed class ToDictionaryConfiguration
{
    internal Func<Type, bool> ComplexTypeChecker { get; private set; } = DefaultComplexTypeChecker;
    internal Func<PropertyInfo, string> PropertyNameProvider { get; private set; } = propertyInfo => propertyInfo.Name;
    internal Func<PropertyInfo, object, bool> PropertyFilter { get; private set; } = DefaultPropertyFilter;

    /// <summary>
    /// Provide a custom property name provider
    /// </summary>
    /// <param name="propertyNameProvider"></param>
    /// <returns></returns>
    public ToDictionaryConfiguration WithPropertyNameProvider(Func<PropertyInfo, string> propertyNameProvider)
    {
        PropertyNameProvider = propertyNameProvider.AssertNotNull(nameof(propertyNameProvider));
        return this;
    }

    /// <summary>
    /// Allows for custom detection of a complex type
    /// </summary>
    /// <remarks>
    /// This can be used to override the default behaviour if 
    /// it does not cover your needs
    /// </remarks>
    /// <param name="isAComplexTypeChecker"></param>
    /// <returns></returns>
    public ToDictionaryConfiguration WithComplexTypeChecker(Func<Type, bool> isAComplexTypeChecker)
    {
        ComplexTypeChecker = isAComplexTypeChecker.AssertNotNull(nameof(isAComplexTypeChecker));
        return this;
    }

    /// <summary>
    /// Use a custom property filter
    /// </summary>
    /// <remarks>
    /// The default filter allows all properties
    /// </remarks>
    /// <param name="propertyFilter"></param>
    /// <returns></returns>
    public ToDictionaryConfiguration WithPropertyFilter(Func<PropertyInfo, object, bool> propertyFilter)
    {
        PropertyFilter = propertyFilter.AssertNotNull(nameof(propertyFilter));
        return this;
    }

    private static bool DefaultComplexTypeChecker(Type type) =>
        type.IsClass && !type.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(type);

    private static bool DefaultPropertyFilter(PropertyInfo info, object arg2) => true;        
}