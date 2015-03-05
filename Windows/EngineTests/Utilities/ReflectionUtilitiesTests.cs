
using NUnit.Framework;
using Org.InCommon.InCert.Engine.Utilities;

namespace EngineTests.Utilities
{
    [TestFixture]
    public class ReflectionUtilitiesTests
    {
        [Test]
        public void GetPropertyValueShouldReturnCorrectResultWhenPropertyChainSpecified()
        {
            var test = new TestObject1();

            var value1 = ReflectionUtilities.GetPropertyValue(test, new[] { "Value" });
            Assert.That(value1, Is.EqualTo(test.Value));

            var value2 = ReflectionUtilities.GetPropertyValue(test, new[] {"Nested", "Value"});
            Assert.That(value2, Is.EqualTo(test.Nested.Value));

            var value3 = ReflectionUtilities.GetPropertyValue(test, new[] { "Nested", "Nested", "Value" });
            Assert.That(value3, Is.EqualTo(test.Nested.Nested.Value));
        }

        [Test]
        public void GetPropertyValueShouldReturnNullIfChainContainsInvalidPropertyName()
        {
            var test = new TestObject1();

            var value1 = ReflectionUtilities.GetPropertyValue(test, new[] { "Value2" });
            Assert.IsNull(value1);

            var value2 = ReflectionUtilities.GetPropertyValue(test, new[] { "Nested","Value2" });
            Assert.IsNull(value2);


            var value3 = ReflectionUtilities.GetPropertyValue(test, new[] { "Nested", "" });
            Assert.IsNull(value3);
        }

        private class TestObject1
        {
            public string Value { get { return "test value 1"; } }
            public TestObject2 Nested { get { return new TestObject2();} }
        }

        private class TestObject2
        {
            public string Value { get { return "test value 2"; } }
            public TestObject3 Nested { get { return new TestObject3(); } }
        }

        private class TestObject3
        {
            public string Value { get { return "test value 3"; } }
        }
    }
}
