using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTests.Exceptions
{
    
    [TestClass]
    public class ExceptionTest
    {
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            Trace.Listeners.Add(new TestListener(context));
        }
        public class Alpha
        {
            public int DoSomethingGood()
            {
                try
                {
                    return 0;
                }
                catch (Exception ex)
                {
                    return 1;
                }
                finally
                {
                    Trace.WriteLine("finally was Called");
                }
            }
            public int DoSomethingBad()
            {
                try
                {
                    throw new Exception("Error");
                    return 0;
                }
                catch (Exception ex)
                {
                    return 1;
                }
                finally
                {
                    Trace.WriteLine("finally was Called");
                }
            }
        }
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
        [TestMethod]
        public void Assert_Finally_Is_Called_When_No_Error_Occurs()
        {
            var a = new Alpha();
            var result = a.DoSomethingGood();
            Assert.AreEqual(result,0);
        }
        [TestMethod]
        public void Assert_Finally_Is_Called_When_Something_Is_Returned_In_The_Catch_Clause()
        {
            var a = new Alpha();
            var result = a.DoSomethingBad();
            Assert.AreEqual(result, 1);
        }
    }
    public class TestListener : TraceListener
    {
        private TestContext _testContext;
        public TestListener(TestContext t)
        {
            _testContext = t;
        }
        public override void Write(string message)
        {
            _testContext.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            _testContext.WriteLine(message);
        }
    }

}
