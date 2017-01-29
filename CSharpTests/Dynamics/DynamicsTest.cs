using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;

namespace CSharpTests.Dynamics
{
    #region Classes
    public class Alpha
    {
        public int Print(int i)
        {
            return 0;
        }
        public int Print(long l)
        {
            return 1;
        }
        public int Print(dynamic d)
        {
            return 2;
        }
    } 
    #endregion
    /// <summary>
    /// var = Compiler working out the type
    /// dynamic = Runtime working out the type
    /// </summary>
    [TestClass]
    public class DynamicsTest
    {
        [TestMethod]
        public void Assert_A_RuntimeBinderException_Will_Be_Thrown()
        {
            try
            {
                dynamic d = DateTime.Now;
                string time = d.WhatTimeIsIt();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(RuntimeBinderException));
            }
        }

        [TestMethod]
        public void Assert_The_Dynamic_Type_Is_Implicitly_Convertible()
        {

            dynamic d = 1515;
            string s = "Marignan";
            Assert.AreEqual(s + d, "Marignan1515");
        }

        [TestMethod]
        public void Assert_The_Dynamic_Type_Is_Implicitly_Convertible_Only_If_The_Type_That_Dynamic_Holds_Is_Convertible_To_The_Second_One()
        {

            long l = 1515;
            dynamic d = l;

            try
            {
                int i = d;
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(RuntimeBinderException));
            }

        }

        [TestMethod]
        public void Assert_The_Dynamic_Type_Can_Hold_Different_Types_At_Runtime()
        {

            long l = 1515;
            dynamic d = l;
            Assert.IsInstanceOfType(d,typeof(long));

            d = "Marignan";
            Assert.IsInstanceOfType(d, typeof(string));

        }
        
        [TestMethod]
        public void Assert_Runtime_Chose_The_More_Specific_Method()
        {

            var alpha = new Alpha();

            dynamic d = 15;
            Assert.AreEqual(alpha.Print(d),0);

            d = 15.5;
            Assert.AreEqual(alpha.Print(d),2);

            d = long.MaxValue;
            Assert.AreEqual(alpha.Print(d), 1);

        }
    }
   
}
