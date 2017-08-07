using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTests.Equality
{
    [TestClass]
    public class HashCodeTest
    {
        

        [TestMethod]
        public void ValueType()
        {
            var x = 128;
            var y = "Isabelle";
            var h1 = x.GetHashCode();
            var h2 = y.GetHashCode();
            var z = x.GetHashCode() ^ y.GetHashCode();

            var sept = 7;                   //7 => 111
            var cinq = 5;                   //5 => 101
            var exclusiveOrResult = sept ^ cinq;  //2 => 010
            Assert.AreEqual(exclusiveOrResult, 2);

            var quinze = 15;                    //15 => 1111    13 => 1101    
                                                //5  => 0101    7  => 0111
            Assert.AreEqual(quinze ^ cinq, 10); //10 => 1010    10 => 1010
            Assert.AreEqual(13 ^ 7, 10);

        }
    }

}
