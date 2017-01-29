using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpTests.Oop
{
    public class Instruments<T> : IEnumerable<T>
    {
        private T[] _instrumentsArray;

        public Instruments(T[] instrumentsArray)
        {
            _instrumentsArray = instrumentsArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new GenericTypeEnumerator(this);
        }

        //Explicit implementation because IEnumerable<T> : IEnumerable
        //Then we return an Object Iterator
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ObjectIterator(this);
        }

        private class ObjectIterator : IEnumerator
        {
            private Instruments<T> _instrument;
            private int _currentIndex = -1;
            public ObjectIterator(Instruments<T> instrument)
            {
                _instrument = instrument;
            }
            public object Current
            {
                get
                {
                    return _instrument._instrumentsArray[_currentIndex];
                }
            }

            public bool MoveNext()
            {
                if (_currentIndex < _instrument._instrumentsArray.Length - 1)
                {
                    _currentIndex++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }

        }

        private class GenericTypeEnumerator : IEnumerator<T>
        {
            private Instruments<T> _instrument;
            private int _currentIndex = -1;
            public GenericTypeEnumerator(Instruments<T> instrument)
            {
                _instrument = instrument;
            }
            public T Current
            {
                get
                {
                    return _instrument._instrumentsArray[_currentIndex];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    //This code will never be reached
                    throw new NotImplementedException();
                }
            }

            public bool MoveNext()
            {
                if (_currentIndex < _instrument._instrumentsArray.Length - 1)
                {
                    _currentIndex++;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }

            public void Dispose()
            {

            }
        }
    }
    [TestClass]
    public class InterfaceTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void IenumerableTest()
        {
            var resulta = string.Empty;
            var resultb = string.Empty;
            var resultc = string.Empty;
            var expectedResult = "ViolonPianoGuitarePipo";

            string[] array = { "Violon", "Piano", "Guitare", "Pipo" };


            IEnumerable instrumentsa = new Instruments<string>(array);
            TestContext.WriteLine($"type of instrumenta {instrumentsa.GetType()}");
            foreach (var a in instrumentsa)
            {
                TestContext.WriteLine($"type of a {a.GetType()}");
                resulta += a;
            }

            IEnumerable<string> instrumentsb = new Instruments<string>(array);
            TestContext.WriteLine($"type of instrumentb {instrumentsb.GetType()}");
            foreach (var b in instrumentsb)
            {
                TestContext.WriteLine($"type of b {b.GetType()}");
                resultb += b;
            }


            var instrumentsc = new Instruments<string>(array);
            TestContext.WriteLine($"type of instrumentc {instrumentsc.GetType()}");
            var enumeratorc = instrumentsc.GetEnumerator();
            while (enumeratorc.MoveNext())
            {
                resultc += enumeratorc.Current;
            }

            var instrumentsd = new Instruments<string>(array);
            TestContext.WriteLine($"type of instrumentd {instrumentsd.GetType()}");
            IEnumerator enumeratord = instrumentsc.GetEnumerator();
            //An Exeception wull be throwned because the enumerator is handled by a variable
            //of type IEnumerator 
            var hasAnExceptionBeenThrowned = false;
            try
            {
                while (enumeratord.MoveNext())
                {
                    resultc += enumeratord.Current;
                }
            }
            catch (Exception e)
            {

                if (e.GetType() == typeof(NotImplementedException))
                {
                    hasAnExceptionBeenThrowned = true;
                }
            }
            Assert.AreEqual(hasAnExceptionBeenThrowned, true);
            Assert.AreEqual(resulta, expectedResult);
            Assert.AreEqual(resultb, expectedResult);
            Assert.AreEqual(resultc, expectedResult);
        }
    }
}
