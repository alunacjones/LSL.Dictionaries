using System;
using System.Collections;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using LSL.Dictionaries.Extensions;
using NUnit.Framework;

namespace LSL.Dictionaries.Tests.Extensions
{
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
            result.ToObject<ComplexClass>(c => c.WithPropertyNameProvider(p => p.Name)).Should().BeEquivalentTo(input);
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
}
