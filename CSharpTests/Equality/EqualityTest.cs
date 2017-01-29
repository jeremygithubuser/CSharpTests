using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTests.Equality
{
    public class Alpha
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
    public struct Beta : IEquatable<Beta>
    {
        public string Name { get; set; }
        public Alpha Alpha { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            //if (this.GetType()==obj.GetType())
            //{
            //    return true;
            //}
            //return false;
            return base.Equals(obj);
        }
        //Can be interessting for value type as we dont need the boxing operation
        //That is performed by the object.Equals inherited method.
        public bool Equals(Beta other)
        {
            throw new NotImplementedException();
        }
    }
    public class Gamma : IEquatable<Gamma>
    {
        public string Name { get; set; }

        public bool Equals(Gamma other)
        {
            if (Name == other.Name)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Gamma))
            {
                if (this.Name.Equals(((Gamma)obj).Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

    [TestClass]
    public class EqualityTest
    {
        public TestContext TestContext { get; set; }
        /// <summary>
        /// Equal operator compares Value types by value
        /// </summary>
        [TestMethod]
        public void ValueType()
        {
            var x = 1;
            var y = 1;
            Assert.IsTrue(x == y);
            Assert.IsTrue(x == x);
        }

        /// <summary>
        /// Both value type are boxed in an object type that is a reference type
        /// The two object dont have the same memory location.
        /// </summary>
        [TestMethod]
        public void BoxedValueType()
        {
            var x = 1;
            var y = 1;
            var result = (object)x == (object)y;
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Both value type are casted in an interface type that is a reference type
        /// The two interface dont have the same memory location.
        /// </summary>
        [TestMethod]
        public void ValueTypeCastedAsInterface()
        {
            IComparable x = 1;
            IComparable y = 1;
            var result = x == y;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckFloatEquality()
        {
            float a = 0.95f;
            float b = 5.05f;
            Assert.IsFalse((a + b == 6));
        }

        [TestMethod]
        public void CheckDoubleEquality()
        {
            double a = 0.95d;
            double b = 5.05d;
            Assert.IsTrue((a + b == 6));
        }

        /// <summary>
        /// Equal operator compares Reference types 
        /// by checkin if they point on the same Memory Location
        /// </summary>
        [TestMethod]
        public void ReferenceType()
        {

            var x = new Alpha { Name = "diego" };
            var y = new Alpha { Name = "diego" };
            //Same Location in memory
            var z = x;
            Assert.IsFalse(x == y);
            Assert.IsTrue(x == z);
        }

        [TestMethod]
        public void ReferenceTypeEqualsMethod()
        {

            var x = new Alpha { Name = "diego" };
            var y = new Alpha { Name = "diego" };

            Assert.IsFalse(x.Equals(y));
            Assert.IsTrue(object.ReferenceEquals(x, x));

        }


        /// <summary>
        /// String is a reference type that acts as a value type
        /// x and y are not pointing on the same instance but Microsoft has overriden the String's Equals Method
        /// That Now Compares Values and not Memory location.
        /// </summary>
        [TestMethod]
        public void StringType()
        {

            var x = "helloWorld";
            var y = string.Copy(x);

            Assert.IsTrue(x.Equals(y));
            Assert.IsTrue(((object)x).Equals((object)y));
            //x and y are two differents objetcs
            Assert.IsFalse(object.ReferenceEquals(x, y));
        }

        /// <summary>
        /// Object.Equals compares values for structs
        /// Because struct : System.ValueType : System.Object
        /// And System.ValueType Overrides System.Object.Equals
        /// </summary>
        [TestMethod]
        public void Struct_Calls_System_ValueType_Equals_Method()
        {

            var x = new Beta { Name = "diego" };
            var y = new Beta { Name = "diego" };
            var z = x;
            
            Assert.IsTrue(x.Equals((object)y));
            z.Name = "Armando";
            //z is an independant copy of x
            Assert.IsFalse(x.Equals((object)z));
        }

        /// <summary>
        /// System.ValueType.Equals Compare all Fields by reflextion
        /// Each Field can call his Equals Method (the behavior wont be the same if the field is a struct or a class)
        /// </summary>
        [TestMethod]
        public void Struct_Calls_System_ValueType_Equals_Method_With_A_Class_Field()
        {
            //Both Beta contains a different Alpha Objects
            var x = new Beta { Name = "diego", Alpha = new Alpha() };
            var y = new Beta { Name = "diego", Alpha = new Alpha() };

            Assert.IsFalse(x.Equals((object)y));

            //Both Beta contains the same Alpha Objects
            var alpha = new Alpha();
            x.Alpha = alpha;
            y.Alpha = alpha;
            Assert.IsTrue(x.Equals((object)y));

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Struct_Calls_System_ValueType_Static_Equals_Method()
        {
            //Both Beta contains a different Alpha Objects
            var x = new Beta { Name = "diego", Alpha = new Alpha() };
            var y = new Beta { Name = "diego", Alpha = new Alpha() };

            var z = x;

            Assert.IsFalse(object.Equals(x, y));

            //Both Beta contains the same Alpha Objects
            var alpha = new Alpha();
            x.Alpha = alpha;
            y.Alpha = alpha;

            //object.Equals first check if the object in memory is the same , then check for null
            //object.Equals then calls the obj1 virtual Method

            Assert.IsTrue(object.Equals(x, y));

            //ReferenceEquals always returns false on structs
            Assert.IsFalse(object.ReferenceEquals(x, x));

            //== always returns false on structs
            Assert.IsFalse((object)x == (object)x);

            Assert.IsTrue(object.Equals(null, null));

        }

        [TestMethod]
        public void This_Is_Why_We_Must_Be_Carrefull_with_equality()
        {
            Gamma x = new Gamma() { Name = "Diego" };
            Gamma y = new Gamma() { Name = "diego" };

            Assert.IsTrue(x.Equals((object)y));

            Assert.IsFalse(x.Equals((Gamma)y));

        }


    }
}
