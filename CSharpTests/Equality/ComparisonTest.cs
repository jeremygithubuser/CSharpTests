using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSharpTests.Equality
{
    [TestClass]
    public class ImplementingComparisonForValueTypes
    {
        private string apple;
        private string pear;

        private int five;
        private int seven;


        public struct ComparableStruct : IComparable, IComparable<ComparableStruct>, IEquatable<ComparableStruct>
        {

            private int _counter;
            public int Counter { get { return _counter; } }
            public ComparableStruct(int counter)
            {
                _counter = counter;
            }
            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException();
                }
                if (obj.GetType() != this.GetType())
                {
                    throw new ArgumentException();
                }
                return CompareTo((ComparableStruct)obj);
            }
            public int CompareTo(ComparableStruct other)
            {
                return Counter.CompareTo(other.Counter);
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj.GetType() != this.GetType())
                {
                    return false;
                }
                ComparableStruct other = (ComparableStruct)obj;
                return Equals(other);
            }
            public bool Equals(ComparableStruct other)
            {
                return Counter == other.Counter;
            }
            public static bool operator ==(ComparableStruct lhs, ComparableStruct rhs)
            {
                return lhs.Equals(rhs);
            }
            public static bool operator !=(ComparableStruct lhs, ComparableStruct rhs)
            {
                return !lhs.Equals(rhs);
            }
            public static bool operator >(ComparableStruct lhs, ComparableStruct rhs)
            {

                var result = false;
                switch (lhs.CompareTo(rhs))
                {
                    case 1:
                        result = true;
                        break;
                    case 0:
                        result = false;
                        break;
                    case -1:
                        result = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }
            public static bool operator >=(ComparableStruct lhs, ComparableStruct rhs)
            {

                var result = false;
                switch (lhs.CompareTo(rhs))
                {
                    case 1:
                        result = true;
                        break;
                    case 0:
                        result = true;
                        break;
                    case -1:
                        result = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }
            public static bool operator <(ComparableStruct lhs, ComparableStruct rhs)
            {
                var result = false;
                switch (lhs.CompareTo(rhs))
                {
                    case 1:
                        result = false;
                        break;
                    case 0:
                        result = false;
                        break;
                    case -1:
                        result = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }
            public static bool operator <=(ComparableStruct lhs, ComparableStruct rhs)
            {
                var result = false;
                switch (lhs.CompareTo(rhs))
                {
                    case 1:
                        result = false;
                        break;
                    case 0:
                        result = true;
                        break;
                    case -1:
                        result = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return result;
            }
            public override int GetHashCode()
            {
                return Counter.GetHashCode();
            }

        }

        [TestInitialize]
        public void TestInitialize()
        {
            apple = "apple";
            pear = "pear";
            five = 5;
            seven = 7;
        }

        public static void DisplayOrder<T>(T x, T y) where T : IComparable<T>
        {
            switch (x.CompareTo(y))
            {
                case -1:
                    Console.WriteLine($"{x} < {y}");
                    break;
                case 0:
                    Console.WriteLine($"{x} = {y}");
                    break;
                case 1:
                    Console.WriteLine($"{x} > {y}");
                    break;
                default:
                    break;
            }
        }

        [TestMethod]
        public void Compare_string_with_display_order()
        {
            DisplayOrder(apple, pear);
            DisplayOrder(pear, apple);
            DisplayOrder(apple, apple);
        }

        [TestMethod]
        public void Compare_int_with_display_order()
        {
            DisplayOrder(five, seven);
            DisplayOrder(seven, five);
            DisplayOrder(five, five);
        }

        [TestMethod]
        public void Compare_int_with_operator()
        {
            Assert.IsTrue(five < seven);
            Assert.IsTrue(seven > five);
        }

        [TestMethod]
        public void Compare_ComparableStruct_with_display_order()
        {

            var c1 = new ComparableStruct(5);
            var c2 = new ComparableStruct(15);
            DisplayOrder(c1, c2);
        }

        [TestMethod]
        public void Compare_ComparableStruct_with_operators()
        {

            var c1 = new ComparableStruct(5);
            var c2 = new ComparableStruct(15);
            var c3 = new ComparableStruct(5);

            Assert.IsTrue(c1 < c2);
            Assert.IsTrue(c1 <= c2);
            Assert.IsFalse(c2 < c1);
            Assert.IsFalse(c2 <= c1);
            Assert.IsTrue(c2 > c1);
            Assert.IsTrue(c2 >= c1);
            Assert.IsTrue(c1 <= c3);
            Assert.IsTrue(c1 >= c3);
            Assert.IsTrue(c1 == c3);
            Assert.IsFalse(c1 == c2);
        }

    }

    [TestClass]
    public class ImplementingComparisonForStrings
    {
        [TestMethod]
        public void OrdinalCompareIgnoreCase()
        {
            int r = string.Compare("lemon", "Lime", StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(r < 0);
        }
        [TestMethod]
        public void OrdinalCompareCaseSensitive()
        {
            int r = string.Compare("lemon", "Lime", StringComparison.Ordinal);
            Assert.IsTrue(r > 0);
        }
    }
}
