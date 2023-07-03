using System;
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
                    ["Child"] = new Dictionary<string, object>
                    {
                        ["Name"] = input.Child.Name,
                        ["Age"] = input.Child.Age
                    },
                });
        }        

        [Test]
        public void GiveANullObject_ITShouldReturnNull()
        {
            object nullObject = null;

            nullObject.ToDictionary().Should().BeNull();
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

        private class ComplexClass
        {
            public ChildClass Child { get; set; }
        }

        private class ChildClass
        {
            public string Name { get; set; }
            public int? Age { get; set; }
        }
    }
}
