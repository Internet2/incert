﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Extensions;

namespace EngineTests.Extensions
{
    [TestClass]
    public class StringExtensionTests
    {

        [TestMethod]
        public void ToStringOrDefaultTest()
        {
            const string defaultValue = "[default value]";
            
            var result = ((object)null).ToStringOrDefault(defaultValue);
            Assert.IsTrue(result.Equals(defaultValue, StringComparison.InvariantCulture), "null object should produce default string");

            result = "".ToStringOrDefault(defaultValue);
            Assert.IsTrue(result.Equals(defaultValue, StringComparison.InvariantCulture), "empty string should produce default string");

            result = " ".ToStringOrDefault(defaultValue);
            Assert.IsTrue(result.Equals(defaultValue, StringComparison.InvariantCulture), "empty string should produce default string");

            var validObject = Guid.NewGuid();
            result = validObject.ToStringOrDefault(defaultValue);

            Assert.IsTrue(result.Equals(validObject.ToString(), StringComparison.InvariantCulture), "not null object should produce its 'ToString' value");
        }
    }
}
