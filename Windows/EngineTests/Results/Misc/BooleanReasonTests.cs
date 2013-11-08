using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.InCommon.InCert.Engine.Results.Misc;

namespace EngineTests.Results.Misc
{
    [TestClass]
    public class BooleanReasonTests
    {

        [TestMethod]
        public void TestInvert()
        {
            var result = new BooleanReason(true, "testing");

            result = result.Invert();
            Assert.IsFalse(result.Result, "After inversion result should be false");
            Assert.IsTrue(result.Reason.Equals("testing", StringComparison.InvariantCulture), "After invert reason text should remain same.");
        }
    }
}
