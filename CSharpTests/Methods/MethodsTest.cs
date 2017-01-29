using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTests.Methods
{
    [TestClass]
    public class MethodTest
    {
        public class Lamda
        {
            public void SayHello(string s)
            {
                Console.WriteLine(s);
            }
            public void SayHello(string s, int i)
            {
                Console.WriteLine(s);
            }

            public static int SayHello(params int[] s)
            {
                int r = 0;
                foreach (var v in s)
                {
                    r += v;
                }
                return r;
            }

            //CompilerError error the return type is not a part of the signature!!
            //public string SayHello(string s)
            //{
            //    return s;
            //}
        }
        [TestMethod]
        public void TestParams()
        {
            var x = Lamda.SayHello(1, 2, 3, 4, 5, 6);
            Assert.AreEqual(x, 21);
        }
    }
}
