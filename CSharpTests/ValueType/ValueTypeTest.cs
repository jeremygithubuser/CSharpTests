using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTests.ValueType
{
    [TestClass]
    public class ValueTypeTest
    {
        public void ChangeString(string s)
        {
            s = "Le cheval bleu d'henry IV";
        }
        public void ChangeString(ref string s)
        {
            s = "Le cheval bleu d'henry IV";
        }

        [TestMethod]
        public void Integer_Cannot_Change()
        {
            int x = 5;
            var y = x + 3;
            Assert.AreEqual(x, 5);
        }
        [TestMethod]
        public void String_Cannot_Change()
        {
            //string is a reference type but behave as a value type 
            //string is immutable
            string x = "Le cheval blanc d'henry IV";
            x.Trim();
            Assert.AreNotEqual(x, "Lechevalblancd'henryIV");
        }
        [TestMethod]
        public void Does_String_Cannot_Change()
        {
            //string is a reference type but behave as a value type 
            //string is immutable
            string s = "Le cheval blanc d'henry IV";
            var s2 = s;
            ChangeString(s);
            Assert.AreEqual(s, "Le cheval blanc d'henry IV");

            ChangeString(ref s);
            Assert.AreEqual(s, "Le cheval bleu d'henry IV");
            Assert.AreEqual(s2, "Le cheval blanc d'henry IV");

        }
        [TestMethod]
        public void DateTime_Cannot_Change()
        {
            DateTime d = new DateTime(1979, 09, 26);
            d.AddDays(1);
            Assert.AreEqual(d, new DateTime(1979, 09, 26));
            Assert.AreNotEqual(d, new DateTime(1979, 09, 27));
        }

    }
}
