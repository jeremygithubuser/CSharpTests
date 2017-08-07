using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTests.Equality
{
    [TestClass]
    public class WhyEqualityIsSoHard
    {
        public class Alpha
        {
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
        public struct BadEqualityStruct : IEquatable<BadEqualityStruct>
        {
            public string Name { get; set; }
            public Alpha Alpha { get; set; }
            public override string ToString()
            {
                return Name;
            }
            //Can be interessting for value type as we dont need the boxing operation
            //That is performed by the object.Equals inherited method.
            public bool Equals(BadEqualityStruct other)
            {
                throw new NotImplementedException();
            }
        }
        public class BadEqualityClass : IEquatable<BadEqualityClass>
        {
            public string Name { get; set; }

            public bool Equals(BadEqualityClass other)
            {
                if (Name == other.Name)
                {
                    return true;
                }
                return false;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() == typeof(BadEqualityClass))
                {
                    if (this.Name.Equals(((BadEqualityClass)obj).Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public TestContext TestContext { get; set; }

        /// <summary>
        /// System.Int32 does overload equality operator
        /// The two integers are compared by value
        /// </summary>
        [TestMethod]
        public void Int32_Overloads_Equality_Operator()
        {
            var x = 1;
            var y = 1;
            Assert.IsTrue(x == y);
        }

        /// <summary>
        /// System.ValueType does not overload equality operator
        /// At compilation time the object equality operator is called then compares two boxed objects by reference
        /// </summary>
        [TestMethod]
        public void System_ValueType_Does_Not_Overload_Equality_Operator()
        {
            var x = 1;
            var y = 1;
            var result = (System.ValueType)x == (System.ValueType)y;
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Both value type are boxed in an object type that is a reference type
        /// The two object dont have the same memory location.
        /// </summary>
        [TestMethod]
        public void Object_Equality_Operator_Overload_Compares_By_Value()
        {
            var x = 1;
            var y = 1;
            var result = (object)x == (object)y;
            Assert.IsFalse(result);
        }

        /// <summary>
        /// System.Int32 overrides the object.Equals virtual method to compare by value
        /// </summary>
        [TestMethod]
        public void System_Int32_Overrides_Object_Equal_Method()
        {
            var x = 1;
            var y = 1;
            var o1 = (object)x;
            var o2 = (object)y;
            var result = o1.Equals(o2);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Both value type are casted in an interface type that is a reference type
        /// The two interface dont have the same memory location.
        /// </summary>
        [TestMethod]
        public void System_Int32_Hold_By_Interface_Are_Compared_By_Object_Equality_Operator_Overload()
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
        public void ReferenceType_Is_Compared_By_Value()
        {

            var x = new Alpha { Name = "diego" };
            var y = new Alpha { Name = "diego" };

            //Same Location in memory
            var z = x;
            Assert.IsFalse(x == y);
            Assert.IsTrue(x == z);
        }

        [TestMethod]
        public void ReferenceType_Inherits_Virtual_Equal_Method_From_Object()
        {

            var x = new Alpha { Name = "diego" };
            var y = new Alpha { Name = "diego" };

            Assert.IsFalse(x.Equals(y));
        }


        /// <summary>
        /// String is a reference type that acts as a value type
        /// x and y are not pointing on the same instance but Microsoft has overriden the String's Equals Method
        /// That Now Compares Values and not Memory location.
        /// </summary>
        [TestMethod]
        public void System_String_Overrides_Object_Equal()
        {

            var x = "helloWorld";
            var y = string.Copy(x);

            Assert.IsTrue(x.Equals(y));             //Iquatable<T>
            Assert.IsTrue(x.Equals((object)y));     //Override Object.Equals
            Assert.IsFalse(object.ReferenceEquals(x, y));

        }

        [TestMethod]
        public void System_String_Overloads_Equality_Operator()
        {

            var x = "helloWorld";
            var y = string.Copy(x);

            Assert.IsTrue(x == y);             //Static System.String Equality operator overload 
        }

        /// <summary>
        /// Object.Equals compares values for structs
        /// Because struct : System.ValueType : System.Object
        /// And System.ValueType Overrides System.Object.Equals
        /// </summary>
        [TestMethod]
        public void Struct_Inherits_System_ValueType_Equals_Method()
        {

            var x = new BadEqualityStruct { Name = "diego" };
            var y = new BadEqualityStruct { Name = "diego" };
            var z = x;

            Assert.IsTrue(x.Equals(y)); //Inherited System.ValueType Equals Method

            z.Name = "Armando";
            //z is an independant copy of x
            Assert.IsFalse(x.Equals(z));
        }

        /// <summary>
        /// System.ValueType.Equals Compare all Fields by reflextion
        /// Each Field can call his Equals Method (the behavior wont be the same if the field is a struct or a class)
        /// </summary>
        [TestMethod]
        public void Struct_Calls_System_ValueType_Equals_Method_With_A_Class_Field()
        {
            //Both Beta contains a different Alpha Objects
            var x = new BadEqualityStruct { Name = "diego", Alpha = new Alpha() };
            var y = new BadEqualityStruct { Name = "diego", Alpha = new Alpha() };

            Assert.IsFalse(x.Equals(y));

            //Both Beta contains the same Alpha Objects
            var alpha = new Alpha();
            x.Alpha = alpha;
            y.Alpha = alpha;
            Assert.IsTrue(x.Equals(y));

        }


        [TestMethod]
        public void Bad_Equality_Implementation()
        {
            BadEqualityClass x = new BadEqualityClass() { Name = "Diego" };
            BadEqualityClass y = new BadEqualityClass() { Name = "diego" };

            Assert.IsTrue(x.Equals((object)y)); //Object.Equals Override

            Assert.IsFalse(x.Equals(y)); //IEquatable<T> Implementation

        }



    }

    [TestClass]
    public class ImplementingEqualityForValueTypes
    {
        public struct TestStruct : IEquatable<TestStruct>
        {
            public int propA { get; set; }
            public int propB { get; set; }


            //IEquatable<T> Implementation To avoid Boxing to Reference type
            public bool Equals(TestStruct other)
            {
                return propA == other.propA && propB == other.propB;
            }

            public override bool Equals(object obj)
            {

                #region Notes
                //- typeof takes a type name(which you specify at compile time).
                //- GetType gets the runtime type of an instance.
                //- is returns true if an instance is in the inheritance tree.
                //           Exemple
                //            class Animal { }
                //            class Dog : Animal { }

                //            void PrintTypes(Animal a)
                //            {
                //                print(a.GetType() == typeof(Animal)) // false 
                //                print(a is Animal)                   // true 
                //                print(a.GetType() == typeof(Dog))    // true
                //            }
                //            Dog spot = new Dog();
                //            PrintTypes(spot); 
                #endregion

                if (obj is TestStruct)
                {
                    return Equals((TestStruct)obj);
                }
                else
                {
                    return false;
                }
            }

            public static bool operator ==(TestStruct lhs, TestStruct rhs)
            {
                //no null check is needed because value type cannot be null
                return lhs.Equals(rhs);
            }

            public static bool operator !=(TestStruct lhs, TestStruct rhs)
            {
                return !lhs.Equals(rhs);
            }
            public override int GetHashCode()
            {
                return propA.GetHashCode() ^ propB.GetHashCode();
            }
        }

        [TestMethod]
        public void ValueTypeEqualityTest()
        {
            var s1 = new TestStruct { propA = 1, propB = 2 };
            var s2 = new TestStruct { propA = 1, propB = 2 };

            Assert.IsTrue(s1 == s2);
            Assert.IsTrue(s1.Equals(s2));
            Assert.IsTrue(s1.Equals((object)s2));
            Assert.IsTrue(s1.GetHashCode() == s2.GetHashCode());
        }
    }

    [TestClass]
    public class ImplementingEqualityForReferenceTypes
    {
        public class TestClass
        {
            public TestClass(string name, string address)
            {
                Name = name;
                Address = address;
            }

            public string Name { get; set; }
            public string Address { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (object.ReferenceEquals(this, obj))
                {
                    return true;
                }
                var t = this.GetType();
                if (this.GetType() != obj.GetType())
                {
                    return false;
                }
                TestClass other = obj as TestClass;
                return Name == other.Name && Address == other.Address;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode() ^ Address.GetHashCode();
            }

            public static bool operator ==(TestClass lhs, TestClass rhs)
            {
                // Object Equals will check for null ,check for reference equality, 
                // then will call the virtual method 
                return object.Equals(lhs, rhs);
            }

            public static bool operator !=(TestClass lhs, TestClass rhs)
            {
                return !object.Equals(lhs, rhs);
            }

        }

        public class DerivedTestClass : TestClass
        {

            public string Email { get; set; }

            public DerivedTestClass(string name, string address, string email) : base(name, address)
            {
                Email = email;
            }
            public override bool Equals(object obj)
            {
                if (!base.Equals(obj))
                {
                    return false;
                }
                if (this.GetType() != obj.GetType())
                {
                    return false;
                }
                DerivedTestClass d = (DerivedTestClass)obj;
                return Email == d.Email;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode() ^ Email.GetHashCode();
            }
            public static bool operator ==(DerivedTestClass lhs, DerivedTestClass rhs)
            {
                // Object Equals will check for null , then will call the virtual method 
                return object.Equals(lhs, rhs);
            }

            public static bool operator !=(DerivedTestClass lhs, DerivedTestClass rhs)
            {
                return !object.Equals(lhs, rhs);
            }
        }

        [TestMethod]
        public void ReferenceTypeEqualityTest()
        {

            TestClass baseTypeInstance1 = new TestClass("testName", "testAddress");
            TestClass baseTypeInstance2 = new TestClass("testName", "testAddress");
            TestClass derivedTypeInstance1 = new DerivedTestClass("testName", "testAddress", "testEmail");
            TestClass derivedTypeInstance2 = new DerivedTestClass("testName", "testAddress", "testEmail");

            Assert.IsTrue(baseTypeInstance1 == baseTypeInstance2);
            Assert.IsFalse(baseTypeInstance1 == derivedTypeInstance1);
            Assert.IsFalse(baseTypeInstance1 == derivedTypeInstance2);
            Assert.IsFalse(baseTypeInstance1.Equals(derivedTypeInstance1));
            Assert.IsFalse(baseTypeInstance1.Equals(derivedTypeInstance2));
            Assert.IsFalse(baseTypeInstance1 == derivedTypeInstance2);
            Assert.IsTrue(derivedTypeInstance1 == derivedTypeInstance2);
            Assert.IsTrue(derivedTypeInstance1.Equals(derivedTypeInstance2));
            Assert.IsTrue(derivedTypeInstance1.GetHashCode() == derivedTypeInstance2.GetHashCode());
        }

        public bool GenericOperatorComparer<T>(T lhs, T rhs) where T : class
        {
            /* WRONG WAY */
            /*The type T is known at runtime*/
            /*But the equal operator is evaluated at compilation type*/
            /*the type will be object*/
            /*And the object equality operator compares by references*/
            return lhs == rhs;
        }
        public bool GenericMethodComparer<T>(T lhs, T rhs) where T : class
        {
            /* GOOD WAY */
            /*The type T is known at runtime*/
            /*The object.Equals Method will call the lhs.Equals method */
            return object.Equals(lhs, rhs);
        }

        [TestMethod]
        public void ReferenceTypeGenericEqualityTest()
        {
            TestClass baseTypeInstance1 = new TestClass("testName", "testAddress");
            TestClass baseTypeInstance2 = new TestClass("testName", "testAddress");

            Assert.IsFalse(GenericOperatorComparer(baseTypeInstance1, baseTypeInstance2));
            Assert.IsTrue(GenericMethodComparer(baseTypeInstance1, baseTypeInstance2));

        }

    }
}
