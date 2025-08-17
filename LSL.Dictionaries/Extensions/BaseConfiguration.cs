using System;
using System.Reflection;

namespace LSL.Dictionaries.Extensions;

/// <summary>
/// BaseConfiguration
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public abstract class BaseConfiguration<TSelf>
    where TSelf : BaseConfiguration<TSelf>
{
    private TSelf _self;

    internal Func<PropertyInfo, object, bool> PropertyFilter { get; private set; } = DefaultPropertyFilter;
    internal Func<PropertyInfo, string> PropertyNameProvider { get; private set; } = propertyInfo => propertyInfo.Name;

    /// <summary>
    /// Use a custom property filter
    /// </summary>
    /// <remarks>
    /// The default filter allows all properties
    /// </remarks>
    /// <param name="propertyFilter"></param>
    /// <returns></returns>
    public TSelf WithPropertyFilter(Func<PropertyInfo, object, bool> propertyFilter)
    {
        PropertyFilter = propertyFilter.AssertNotNull(nameof(propertyFilter));
        return _self;
    }

    /// <summary>
    /// Provide a custom property name provider
    /// </summary>
    /// <param name="propertyNameProvider"></param>
    /// <returns></returns>
    public TSelf WithPropertyNameProvider(Func<PropertyInfo, string> propertyNameProvider)
    {
        PropertyNameProvider = propertyNameProvider.AssertNotNull(nameof(propertyNameProvider));
        return _self;
    }

    internal void SetSelf(TSelf self) => _self = self;
    
    private static bool DefaultPropertyFilter(PropertyInfo info, object arg2) => true;
}