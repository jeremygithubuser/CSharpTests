using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpTests.Oop
{
    public abstract class Alpha : object
    {
        //accessible from derivedClasses
        protected string AlphaField = "alphaFieldHasProtectedAccesLevel";
        public string Speak()
        {
            return "alpha";
        }

        public string Speak_a()
        {
            return "alpha";
        }

        public virtual string Speak_b()
        {
            return "alpha";
        }
        public virtual string Speak_c()
        {
            return "alpha";
        }
        public virtual string Speak_d()
        {
            return "alpha";
        }

        public virtual string Speak_e()
        {
            return "alpha";
        }
        protected virtual string Speak_f()
        {
            return AlphaField;
        }
        public virtual string Speak_g()
        {
            return "alpha";
        }

        public abstract string Speak_h();
    }
    public class Beta : Alpha
    {
        public new string Speak()
        {
            return "beta";
        }
        public new string Speak_a()
        {
            return "beta";
        }

        public override string Speak_c()
        {
            return "beta";
        }

        public override string Speak_d()
        {
            return "beta";
        }

        public new string Speak_e()
        {
            return "beta";
        }
        protected override string Speak_f()
        {
            return $"{base.Speak_f()}beta{AlphaField}";
        }

        public override string Speak_g()
        {
            return Speak_f();
        }

        public override string Speak_h()
        {
            return "When the parents declare an abstract method it must be implemented by the child";
        }
    }
    public class Gamma : Beta
    {
        public new string Speak_a()
        {
            return "gamma";
        }

        public override string Speak_c()
        {
            return "gamma";
        }

        public new string Speak_d()
        {
            return "gamma";
        }
    }
    [TestClass]
    public class InheritanceTest
    {
        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Class_Runs_Parents_Method()
        {
            Beta b = new Beta();
            Assert.AreEqual(b.Speak(), "beta");
        }

        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Class_Runs_Parents_Method_When_Missing()
        {
            Alpha l = new Beta();
            Assert.AreEqual(l.Speak_b(), "alpha");

            Alpha l2 = new Gamma();
            Assert.AreEqual(l2.Speak_b(), "alpha");
        }

        [TestMethod]
        public void Sub_Class_Runs_Firts_Method_In_Hierarchy_When_Missing()
        {
            Gamma g = new Gamma();
            Assert.AreEqual(g.Speak_e(), "beta");
        }

        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Runs_Firts_Override_Method_In_Hierarchy_When_Missing()
        {
            Alpha l = new Gamma();
            Assert.AreEqual(l.Speak_e(), "alpha");
        }

        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Class_Run_Parents_Method_When_New_Is_Used()
        {
            Alpha l = new Beta();
            Assert.AreEqual(l.Speak_a(), "alpha");

            Alpha l2 = new Gamma();
            Assert.AreEqual(l2.Speak_a(), "alpha");
        }

        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Class_Run_Current_Class_Method_When_Override_Is_Used()
        {
            Alpha l = new Beta();
            Assert.AreEqual(l.Speak_c(), "beta");
            Alpha l2 = new Gamma();
            Assert.AreEqual(l2.Speak_c(), "gamma");
        }

        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Class_Call_Base_And_Run_Current_Class_Method_When_Override_Is_Used()
        {
            Alpha l = new Beta();
            //Compile error because we are calling the protected method Speak_f() outside the hierarchy.
            //Assert.AreEqual(l.Speak_f(), "alphaFieldHasProtectedAccesLevelbetaalphaFieldHasProtectedAccesLevel");
            Assert.AreEqual(l.Speak_g(), "alphaFieldHasProtectedAccesLevelbetaalphaFieldHasProtectedAccesLevel");
        }

        [TestMethod]
        public void Sub_Class_Holded_By_Parent_Class_Run_First_Overrided_Method_When_New_Is_Used()
        {
            Alpha l = new Beta();
            Assert.AreEqual(l.Speak_d(), "beta");
            Alpha l2 = new Gamma();
            Assert.AreEqual(l2.Speak_d(), "beta");
        }
    }
}
