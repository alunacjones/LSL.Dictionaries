using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using FluentAssertions;
using LSL.Dictionaries.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace LSL.Dictionaries.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Test]
    public void GivenAnObjectWithSimpleTypes_ItShouldReturnTheExpectedDictionary()
    {
        var fixture = new Fixture();
        var input = fixture.Create<TestClass>();

        input.ToDictionary()
            .Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                ["IntValue"] = input.IntValue,
                ["StringValue"] = input.StringValue,
                ["DecimalValue"] = input.DecimalValue,
                ["DateTimeValue"] = input.DateTimeValue,
                ["DateTimeOffsetValue"] = input.DateTimeOffsetValue,
                ["ByteValue"] = input.ByteValue
            });
    }

    [TestCase(null)]
    [TestCase(12)]
    public void GivenAnObjectWithComplexTypes_ItShouldReturnTheExpectedDictionary(int? age)
    {
        var fixture = new Fixture();
        var input = fixture.Create<ComplexClass>();
        input.Child.Age = age;

        input.ToDictionary()
            .Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                ["Name"] = input.Name,
                ["Child"] = new Dictionary<string, object>
                {
                    ["Name"] = input.Child.Name,
                    ["Age"] = input.Child.Age
                },
            });
    }

    [TestCase(null)]
    [TestCase(12)]
    public void GivenAnObjectWithComplexTypesAndCustomConfiguration_ItShouldReturnTheExpectedDictionary(int? age)
    {
        var fixture = new Fixture();
        var input = fixture.Create<BaseComplexClass>();
        input.Child.Age = age;

        var result = input
            .ToDictionary(c => c
            .WithComplexTypeChecker(type => type.IsClass && !type.IsAssignableFrom(typeof(string)) && !typeof(IEnumerable).IsAssignableFrom(type))
            .WithPropertyNameProvider(p => p.Name)
            .WithValueMapper((pi, v) => v)
            .WithPropertyFilter((pi, v) => true));

        result
            .Should()
            .BeEquivalentTo(new Dictionary<string, object>
            {
                ["Child"] = new Dictionary<string, object>
                {
                    ["Name"] = input.Child.Name,
                    ["Age"] = input.Child.Age
                },
            });

        result.ToObject<ComplexClass>().Should().BeEquivalentTo(input);

        result = JsonConvert.DeserializeObject<IDictionary<string, object>>(JsonConvert.SerializeObject(result));

        var otherResult = result.ToObject<ComplexClass>(c => c
            .WithValueMapper(ValueMapper)
            .WithInstanceFactory(Activator.CreateInstance)
            .WithPropertyNameProvider(p => p.Name)            
        );

        otherResult.Should().BeEquivalentTo(input);

        static object ValueMapper(PropertyInfo pi, object value)
        {
            var result = value switch
            {
                JObject jObject => jObject.ToObject<IDictionary<string, object>>(),
                //long aLong => pi.PropertyType.IsAssignableFrom(typeof(long)) ? aLong : Convert.ToInt32(aLong),
                _ => value
            };

            if (!pi.PropertyType.IsAssignableFrom(typeof(long)) && value is long aLong)
            {
                return Convert.ToInt32(aLong);
            }

            return result;
        }
    }

    [Test]
    public void GivenADictionaryThatNeedsToSetADatePropertyButCannotConvertIt_ItShouldThrowTheExpectedException()
    {
        var dictionary = new Dictionary<string, object>
        {
            ["MyDate"] = "12-May-2020"
        };

        new Action(() => dictionary.ToObject<BadClass>())
            .Should().ThrowExactly<SetPropertyValueException>()
            .WithMessage(
                """
                Unable to write value '12-May-2020' of type 'String' to property 'MyDate' of the target type of 'BadClass'.

                Inner exception message: Object of type 'System.String' cannot be converted to type 'System.DateTime'.
                """.ReplaceLineEndings());
    }

    [Test]
    public void GivenAnObjectWithInvalidComplexTypeCheckerConfiguration_ItShouldThrowAnException()
    {
        var fixture = new Fixture();
        var input = fixture.Create<ComplexClass>();
        input.Child.Age = 12;

        new Action(() => input.ToDictionary(c => c.WithComplexTypeChecker(null)))
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("isAComplexTypeChecker");
    }

    [Test]
    public void GivenAnObjectWithInvalidPropertyNameProviderConfiguration_ItShouldThrowAnException()
    {
        var fixture = new Fixture();
        var input = fixture.Create<ComplexClass>();
        input.Child.Age = 12;

        new Action(() => input.ToDictionary(c => c.WithPropertyNameProvider(null)))
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .And.ParamName.Should().Be("propertyNameProvider");
    }

    [Test]
    public void GivenANullObject_ItShouldReturnNull()
    {
        object nullObject = null;

        nullObject.ToDictionary().Should().BeNull();
    }

    [Test]
    public void GivenAnObjectWithAnEnumerable_ItShouldJustUseTheEnumerableAndNotItsProperties()
    {
        new TestEnumerable
        {
            Items = new[] { 1, 2, 3 },
            ItemsArray = new[] { 1, 2, 3 },
            ItemsList = new List<int> { 1, 2, 3 }
        }.ToDictionary()
        .Should()
        .BeEquivalentTo(new Dictionary<string, object>
        {
            ["Items"] = new[] { 1, 2, 3 },
            ["ItemsArray"] = new[] { 1, 2, 3 },
            ["ItemsList"] = new[] { 1, 2, 3 }
        });
    }

    private class TestEnumerable
    {
        public IEnumerable<int> Items { get; set; }
        public int[] ItemsArray { get; set; }
        public List<int> ItemsList { get; set; }
    }

    private class TestClass
    {
        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public DateTimeOffset DateTimeOffsetValue { get; set; }
        public byte ByteValue { get; set; }
    }

    private class BaseComplexClass
    {
        public ChildClass Child { get; set; }
    }

    public class BadClass
    {
        public DateTime MyDate { get; set; }
    }

    private class ComplexClass : BaseComplexClass
    {
        public string Name { get; set; }
    }

    private class ChildClass
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }
}