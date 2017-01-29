using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTests.CSharpTypeSystem
{


    #region KEYWORDS class , struct , delegate , enum , interface creates TYPES

    public class Alpha
    {
        //The Type created by the class keyword can hold other Types
        public dynamic dynamicProp { get; set; }

        public object objectProp { get; set; }

        public Beta structProp { get; set; }

        public Gamma delegateProp { get; set; }

        public Delta enumProp { get; set; }

        public Epsilon interfaceProp { get; set; }
      
    }

    public struct Beta
    {
    }

    public delegate void Gamma(string s); 

    public enum Delta {a,b,c,d}

    public interface Epsilon
    {

    }

    public class Zeta : Alpha { }

    #endregion


    [TestClass]
    public class CSharpTypeSystemTest
    {
        [TestMethod]
        public void Assert_Types()
        {


            var dynamicPropType = typeof(Alpha).GetProperty("dynamicProp").PropertyType;
            var objectPropType = typeof(Alpha).GetProperty("objectProp").PropertyType;
            var interfacePropType = typeof(Alpha).GetProperty("interfaceProp").PropertyType;
            var structPropType = typeof(Alpha).GetProperty("structProp").PropertyType;
            var enumPropType = typeof(Alpha).GetProperty("enumProp").PropertyType;
            var delegatePropType = typeof(Alpha).GetProperty("delegateProp").PropertyType;


            //By default dynamic Property Type is object 
            //But a special attribute on the Property enables compilator to change the Type at runtime
            Assert.AreEqual(dynamicPropType,typeof(object));

            Assert.IsTrue(typeof(Alpha)
                .GetProperty("dynamicProp")
                .CustomAttributes.Any( a => { return a.AttributeType == typeof(System.Runtime.CompilerServices.DynamicAttribute); })
                );

            Assert.IsFalse(typeof(Alpha)
                .GetProperty("objectProp")
                .CustomAttributes.Any(a => { return a.AttributeType == typeof(System.Runtime.CompilerServices.DynamicAttribute); })
                );

            //Type created by interface keyword do not derives from Type object
            Assert.AreEqual(interfacePropType.BaseType, null);

            Assert.AreEqual(structPropType.BaseType, typeof(System.ValueType));

            Assert.AreEqual(enumPropType.BaseType, typeof(System.Enum));

            Assert.AreEqual(delegatePropType.BaseType, typeof(System.MulticastDelegate));

        }

        [TestMethod]
        public void Assert_Zeta_Is_Type_Zeta_When_Holded_By_Alpha_Variable()
        {
            Alpha a = new Zeta();

            Assert.IsInstanceOfType(a, typeof(Zeta));
        }
    }
}
