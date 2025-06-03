using System;
using System.Reflection;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// SetPropertyValueException
/// </summary>
/// <remarks>
/// Default constructor
/// </remarks>
/// <param name="propertyInfo"></param>
/// <param name="target"></param>
/// <param name="value"></param>
/// <param name="innerException"></param>
public class SetPropertyValueException(
    PropertyInfo propertyInfo,
    object target,
    object value,
    Exception innerException) : Exception(CreateMessage(propertyInfo, target, value, innerException), innerException)
{

    /// <summary>
    /// The target <see cref="PropertyInfo"/> that could not be set
    /// </summary>
    public PropertyInfo PropertyInfo { get; } = propertyInfo;

    /// <summary>
    /// The target instance whose property was being set
    /// </summary>
    public object Target { get; } = target;

    /// <summary>
    /// The value that should have been written to the property
    /// </summary>
    public object Value { get; } = value;

    private static string CreateMessage(PropertyInfo propertyInfo, object target, object value, Exception innerException) =>
        $"""
        Unable to write value '{value}' of type '{value.GetType().Name}' to property '{propertyInfo.Name}' of the target type of '{target.GetType().Name}'.

        Inner exception message: {innerException.Message}
        """;
}