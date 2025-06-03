using System;
using System.Collections;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// ToDictionaryConfiguration
/// </summary>
public sealed class ToDictionaryConfiguration : BaseConfiguration<ToDictionaryConfiguration>
{
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

    private static bool DefaultComplexTypeChecker(Type type) =>
        type.IsClass && !type.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(type);
}
