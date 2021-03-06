﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace CSharpTests.PropertiesAndFields
{
    /// <summary>
    /// Generalement on utilise un field lorsque l'om veux manipuler une valeur encapsulée
    /// Si l on veut exposer cette valeur a l'exterieur on expose ce field via une property.
    /// Fields $ Properties are STATE , Methods Are BEHAVIOR
    /// </summary>
    [TestClass]
    public class PropertiesAndFieldsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public class Alpha
        {
            /// <summary>
            /// This is an AutoGenerated Property That Hides a field behind the field is implicitly created by the compiler
            /// </summary>
            public string Adress { get; set; }

            /// <summary>
            /// This is a field 
            /// </summary>
            private string _name;

            /// <summary>
            /// This is an Explicit Property That Wraps the private field
            /// </summary>
            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != null)
                    {
                        _name = value;
                    }
                }
            }


        }

        [TestMethod]
        public void Assert_That_The_Alpha_Type_Contains_Two_Fields()
        {
            var alphaType = typeof(Alpha);
            var fields = alphaType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList();
            foreach (var field in fields)
            {
                TestContext.WriteLine($"{field.Name}");
                TestContext.WriteLine($"{field.FieldType}");
            }

            Assert.AreEqual(fields.Count, 2);

        }

        [TestMethod]
        public void Assert_That_The_Alpha_Type_Contains_One_Property()
        {
            var lambdaType = typeof(Alpha);
            var properties = lambdaType.GetProperties().ToList();
            foreach (var prop in properties)
            {
                TestContext.WriteLine($"assembly :{prop.Module.Assembly.FullName} module : {prop.Module.Name}");
                TestContext.WriteLine($"name :{prop.Name} isSpecialName : {prop.IsSpecialName}");
                TestContext.WriteLine($"getMethod :{prop.GetMethod.Name} setMethod : {prop.SetMethod}");
                TestContext.WriteLine($"canRead :{prop.CanRead} canWrite : {prop.CanWrite}");

                TestContext.WriteLine($"CustomAttributes");

                var attributes = prop.CustomAttributes.ToList();
                foreach (var attribute in attributes)
                {
                    TestContext.WriteLine($"attributeType :{attribute.AttributeType.FullName}");
                }


            }

            Assert.AreEqual(properties.Count, 2);

        }

    }
}
