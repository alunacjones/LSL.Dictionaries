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
    internal Func<Type, object> InstanceFactory { get; private set; } = Activator.CreateInstance;

    /// <summary>
    /// Use a custom value mapper
    /// </summary>
    /// <remarks>
    /// Can be useful for DateTime and JObject issues
    /// </remarks>
    /// <param name="valueMapper"></param>
    /// <returns></returns>
    public FromDictionaryConfiguration WithValueMapper(Func<PropertyInfo, object, object> valueMapper)
    {
        ValueMapper = valueMapper.AssertNotNull(nameof(valueMapper));
        return this;
    }

    /// <summary>
    /// Use a custom object instance factory
    /// </summary>
    /// <remarks>The default uses <see cref="Activator.CreateInstance(Type)"/></remarks>
    /// <param name="instanceFactory"></param>
    /// <returns></returns>
    public FromDictionaryConfiguration WithInstanceFactory(Func<Type, object> instanceFactory)
    {
        InstanceFactory = instanceFactory.AssertNotNull(nameof(instanceFactory));
        return this;
    }

    private static object DefaultValueMapper(PropertyInfo propertyInfo, object value) => value;
}