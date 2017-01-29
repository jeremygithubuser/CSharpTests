using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTests.Parameters
{
    [TestClass]
    public class ParametersTest
    {
        public class Lambda
        {
            public string Name { get; set; }
        }
        public class Beta
        {
            public static void ChangeName(Lambda l, string newName)
            {
                l.Name = newName;
            }
            public static void ChangeVariablePointerValue(Lambda l)
            {
                l = new Lambda();
            }
            public static void ChangeVariablePointerValueByRef(ref Lambda l)
            {
                l = new Lambda();
            }
            public static void ChangeVariablePointerValueByOut(out Lambda l)
            {
                l = new Lambda { Name = "lambda" };
            }
            public static void ChangeVariableValueByRef(ref int i)
            {
                i = 4;
            }
            public static void ChangeValue(string val, string newVal)
            {
                val = newVal;
            }
        }

        [TestMethod]
        public void Is_Parameter_A_Copy_Of_A_Pointer_To_Lambda_Instance()
        {
            var l = new Lambda { Name = "lambda" };
            Beta.ChangeName(l, "beta");
            Assert.AreEqual(l.Name, "beta");
        }

        [TestMethod]
        public void Is_Parameter_A_Copy_Of_The_Value_That_Is_Holded_By_Variable()
        {
            var l = "lambda";
            Beta.ChangeValue(l, "beta");
            Assert.AreEqual(l, "lambda");
        }

        [TestMethod]
        public void The_Variable_Still_Holds_A_Pointer_To_The_Same_Object_When_We_Changed_The_Pointer_Of_The_Variable_Copy()
        {
            var l = new Lambda { Name = "lambda" };
            Beta.ChangeVariablePointerValue(l);
            Assert.AreEqual(l.Name, "lambda");
        }

        [TestMethod]
        public void The_Variable_Holds_A_Pointer_To_An_Other_Object_When_We_Changed_The_Pointer_Of_The_Variable_Copy()
        {
            var l = new Lambda { Name = "lambda" };
            //Copy l pointer in l2
            var l2 = l;

            // Inside the function the variable holds a pointer to the l pointer
            // We can use this pointer to interact to the object instance
            // If we use the variable to change the poiinted instance we change the value of the l pointer via the nested pointer
            // and the nested pointer still point to the l pointer
            Beta.ChangeVariablePointerValueByRef(ref l);


            //Now l holds a pointer to an other Lambda Instance
            Assert.AreNotEqual(l.Name, "lambda");
            //l2 still holds a pointer to the original instance
            Assert.AreEqual(l2.Name, "lambda");
        }

        [TestMethod]
        public void The_UnInitialized_Variable_Holds_A_Pointer_To_An_Other_Object_When_We_Changed_The_Pointer_Of_The_Variable_Copy()
        {
            Lambda l;
            Beta.ChangeVariablePointerValueByOut(out l);
            Assert.AreEqual(l.Name, "lambda");
        }

        [TestMethod]
        public void The_Variable_Holds_A_Different_Value_When_Manipulating_A_Pointer_To_The_Variable_Value()
        {
            var a = 3;
            Beta.ChangeVariableValueByRef(ref a);
            Assert.AreEqual(a, 4);
        }
    }
}
