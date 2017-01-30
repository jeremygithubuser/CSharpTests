using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTests.MSIL
{
    public class Person
    {
        public string Name { get; set; }
        public string Speak()
        {
            return $"Hello my name is {Name}";
        }
    }

    [TestClass]
    public class MsilTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Assert_Methods_Can_Be_Invoked_With_Reflection()
        {
            var jim = new Person { Name = "jim" };
            var type = typeof(Person);
            type.GetMethods().ToList().ForEach(m => TestContext.WriteLine(m.Name));
            var setNameMethod = type.GetMethod("set_Name");
            var speakMethod = type.GetMethod("Speak");
            setNameMethod.Invoke(jim, new object[] { "jimki" });
            Assert.AreEqual(speakMethod.Invoke(jim, null), "Hello my name is jimki");
        }

        /// <summary>
        /// public static double Divide(int a, int b)
        ///{
        ///    return a / b;
        ///}
        /// </summary>
        [TestMethod]
        public void Assert_Divide_Method_Is_Created_With_Reflection()
        {
            var dynamicDivideMethod = new DynamicMethod(
                    "DivideMethod",
                    MethodAttributes.Public | MethodAttributes.Static,
                    CallingConventions.Standard,
                    typeof(double),
                    new[] { typeof(int), typeof(int) },
                    typeof(MsilTest).Module, true);

            var il = dynamicDivideMethod.GetILGenerator();

            //put the two parameters in the evaluation stack
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            //perform the division and the result is pushed to the bottom of the evaluation stack
            il.Emit(OpCodes.Div);
            //Convert to double
            il.Emit(OpCodes.Conv_R4);
            //Retutn the value at the bottom of the evaluation stack
            il.Emit(OpCodes.Ret);

            //two ways to call the generated method
            var x = dynamicDivideMethod.Invoke(null, new object[] { 10, 2 });
            var divideMethodPointer = (Func<int, int, double>)dynamicDivideMethod.CreateDelegate(typeof(Func<int, int, double>));
            var y = divideMethodPointer(10, 2);
            Assert.AreEqual((double)x, 5);
            Assert.AreEqual((double)y, 5);
        }

        /// <summary>
        /// public static int Calculate(int a, int b, int c)
        ///{
        ///    var result = a * b;
        ///
        ///   return result - c;
        ///}
        /// </summary>
        [TestMethod]
        public void Assert_Calculate_Method_Is_Created_With_Reflection()
        {
            var dynamicCalculateMethod = new DynamicMethod(
                         "CalculateMethod",
                         typeof(int),
                         new[] { typeof(int), typeof(int), typeof(int) },
                         typeof(MsilTest).Module);

            var il = dynamicCalculateMethod.GetILGenerator();
            // push the first and second argument to the evaluation stack
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            // the result is pushed at the bottom of the evaluation stack
            il.Emit(OpCodes.Mul);
            // push the third argument at the bottom of the evaluation stack
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldarg_2);
            // the result of sub is pushed at the bottom of the evaluation stack
            il.Emit(OpCodes.Sub);
            il.Emit(OpCodes.Ret);

            var calculateMethodPointer = (Func<int, int, int, int>)dynamicCalculateMethod.CreateDelegate(typeof(Func<int, int, int, int>));
            Assert.AreEqual(calculateMethodPointer(2, 5, 6), 4);
        }

        /// <summary>
        ///public static class SomeClass
        ///{
        ///    public static int Calculate(int x)
        ///   {
        ///        int result = 0;
        ///        for (int i = 0; i < 3; i++)
        ///        {
        ///            result += i * x;
        ///        }
        ///        return result;
        ///    }
        ///}
        /// </summary>
        [TestMethod]
        public void Assert_SomeClass_Calculate_Method_Is_Created_With_Reflection()
        {
            var dynamicSomeClassCalculateMethod = new DynamicMethod(
                            "SomeClassCalculateMethod",
                            typeof(int),
                            new[] { typeof(int) },
                            typeof(MsilTest).Module);

            var il = dynamicSomeClassCalculateMethod.GetILGenerator();
            var loopStart = il.DefineLabel();
            var methodEnd = il.DefineLabel();

            /*Declare and init result*/
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_0);

            /*Declare and init i*/
            il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc_1);

            /*Label LoopStart*/
            il.MarkLabel(loopStart);
            /*Test Continue or goto label MethodEnd*/
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldc_I4_S, 3);
            il.Emit(OpCodes.Bge_S, methodEnd);

            /*body*/
            //i*x
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Mul);
            // result +=
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_0);
            /*body*/

            /*i++*/
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_1);

            /*goto label loop*/
            il.Emit(OpCodes.Br, loopStart);

            /*Label methodEnd*/
            il.MarkLabel(methodEnd);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var d = (Func<int, int>)dynamicSomeClassCalculateMethod.CreateDelegate(typeof(Func<int, int>));
            Assert.AreEqual(d(1000),3000);
        }

        public static string Print(string s)
        {
            return $"the value passed to Print is {s}";
        }
        [TestMethod]
        public void Assert_Print_Method_Is_called_By_Dynamic_Method()
        {
            var dynamicCallPrintMethod = new DynamicMethod(
                                "CallPrintMethod",
                                typeof(string),
                                new[] { typeof(string) },
                                typeof(MsilTest).Module);

            var il = dynamicCallPrintMethod.GetILGenerator();
            MethodInfo methodInfo = typeof(MsilTest).GetMethod("Print", new Type[1] { typeof(string) });
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Ret);
            var callPrintMethodDelegate = (Func<string,string>)dynamicCallPrintMethod.CreateDelegate(typeof(Func<string,string>));
            Assert.AreEqual(callPrintMethodDelegate("jim"), "the value passed to Print is jim");
        }
    }
}
