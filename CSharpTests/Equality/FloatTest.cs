using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTests.Equality
{
    [TestClass]
    public class FloatTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ConvertoBinary()
        {
            var expectedOutput = "01001001011101000001110000100110";
            var output = FloatConverter.ConvertToBinary("999874.375");
            Assert.AreEqual(expectedOutput, output);
        }

    }
}
