using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTests.Maths
{
    [TestClass]
    public class Integers
    {
        [TestMethod]
        public void SbyteTest()
        {
            sbyte sba = -128;

            Console.WriteLine($"{sba} : {Convert.ToString(sba, 2).Substring(8, 8)} en base {2}");
            Console.WriteLine($"{sba} : {Convert.ToString(sba, 16).Substring(2, 2)} en base {16}");

            sba = -1;
            Console.WriteLine($"{sba} : {Convert.ToString(sba, 2).Substring(8, 8)} en base {2}");
            Console.WriteLine($"{sba} : {Convert.ToString(sba, 16).Substring(2, 2)} en base {16}");


        }
    }
}
