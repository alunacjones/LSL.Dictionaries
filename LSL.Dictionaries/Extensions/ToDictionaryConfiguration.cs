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

    private static bool DefaultComplexTypeChecker(Type type) =>
        type.IsClass && !type.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(type);
}