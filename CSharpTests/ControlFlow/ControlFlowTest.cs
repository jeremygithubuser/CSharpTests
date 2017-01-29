using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CSharpTests.ControlFlow
{
    [TestClass]
    public class ControlFlowTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Branching_Test()
        {
            int result;

            var i = 95;

            if (i < 95)
            {
                result = 0;
            }
            else if (i < 100)
            {
                result = 1;
            }
            else if (i < 96)
            {
                result = 2;
            }
            else
            {
                result = 3;
            }
            Assert.AreEqual(result, 1);
        }

        enum Instrument
        {
            Violon,
            Guitare,
            Piano,
            Pipo,
            Banjo
        }
        /// <summary>
        /// Works with integers,chars,string,enums
        /// </summary>
        [TestMethod]
        public void SwitchTest()
        {
            string result = "je joue du ";

            Instrument i = Instrument.Pipo;

            switch (i)
            {
                case Instrument.Pipo:
                    result += "pipo";
                    break;
                case Instrument.Banjo:
                    result += "banjo";
                    break;
                case Instrument.Guitare:
                    result += "guitare";
                    break;
                case Instrument.Piano:
                    result += "piano";
                    break;
                case Instrument.Violon:
                    result += "violon";
                    break;
                default:
                    result += "de rien du tout";
                    break;
            }
            Assert.AreEqual(result, "je joue du pipo");
        }

        [TestMethod]
        public void IterateTest()
        {
            string[] instruments = {
                "Violon",
                "Guitare",
                "Piano",
                "Pipo",
                "Banjo"
            };


            var resultA = string.Empty;
            var resultB = string.Empty;

            for (int i = 0; i < instruments.Length; i++)
            {
                resultA += instruments[i];
            }
            for (int i = instruments.Length; i > 0; i--)
            {
                resultB += instruments[i - 1];
            }

            Assert.AreEqual(resultA, string.Join(string.Empty, instruments));
            Assert.AreEqual(resultB, string.Join(string.Empty, instruments.Reverse()));
        }

        [TestMethod]
        public void JumpTest()
        {
            string[] instruments = {
                "Violon",
                "Guitare",
                "Piano",
                "Pipo",
                "Banjo"
            };


            var resultA = string.Empty;
            var resultB = string.Empty;

            for (int i = 0; i < instruments.Length; i++)
            {
                if (instruments[i] == "Piano")
                {
                    break;
                }
                resultA = instruments[i];
            }

            Assert.AreEqual(resultA, "Guitare");

            for (int i = 0; i < instruments.Length; i++)
            {
                if (i % 2 == 0)
                {
                    continue;
                }
                resultB += instruments[i];
            }

            Assert.AreEqual(resultB, "GuitarePipo");

            var resultC = 1;
            goto skip;
            resultC = 5;
            skip:
            Assert.AreEqual(resultC, 1);

        }

        [TestMethod]
        public void ThrowHandle()
        {
            var hasFinalyBeenCalled = false;
            try
            {
                throw new InvalidOperationException();
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(exception.GetType(), typeof(InvalidOperationException));
                try
                {
                    throw;
                }
                catch (Exception e)
                {
                    Assert.AreEqual(e.GetType(), typeof(InvalidOperationException));

                }
            }
            finally
            {
                hasFinalyBeenCalled = true;
                Assert.AreEqual(hasFinalyBeenCalled, true);
            }

        }
    }
}
