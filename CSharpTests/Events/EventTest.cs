using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CSharpTests.Events
{

    [TestClass]
    public class EventTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public static List<string> SharedList = new List<string>();

        public event SutDelegate SutEvent;
        public event SutDelegateStandard SutEventStandard;

        public List<SutDelegate> DelegateList = new List<SutDelegate>();

        [TestMethod]
        public void Assert_That_Suscribers_Received_The_Publishers_Message()
        {
            var l = new Lambda();
            var b = new Beta();
            var g = new Gamma();

            var ld = new SutDelegate(l.LambdaMethod);
            SutDelegate bd = b.BetaMethod;
            Action<string> gd = g.GammaMethod;

            const string publisherMessage = "this is the publisher's message";
            ld(publisherMessage);
            bd(publisherMessage);
            gd(publisherMessage);

            Assert.AreEqual(SharedList[0], $"Lambda method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[1], $"Beta method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[2], $"Gamma method has been called and recived the following message : {publisherMessage}");
        }

        [TestMethod]
        public void Assert_That_Suscribers_Received_The_Publishers_Message_Raised_By_An_Event()
        {
            var l = new Lambda();
            var b = new Beta();
            var g = new Gamma();

            SutEvent += l.LambdaMethod;
            SutEvent += b.BetaMethod;
            SutEvent += g.GammaMethod;

            const string publisherMessage = "this is the publisher's message";
            SutEvent?.Invoke(publisherMessage);

            Assert.AreEqual(SharedList[0], $"Lambda method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[1], $"Beta method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[2], $"Gamma method has been called and recived the following message : {publisherMessage}");
        }

        [TestMethod]
        public void Assert_That_Suscribers_Received_The_Publishers_Message_Raised_By_A_List()
        {
            var l = new Lambda();
            var b = new Beta();
            var g = new Gamma();

            DelegateList.Add(l.LambdaMethod);
            DelegateList.Add(b.BetaMethod);
            DelegateList.Add(g.GammaMethod);

            const string publisherMessage = "this is the publisher's message";

            foreach (var d in DelegateList)
            {
                d(publisherMessage);
            }

            Assert.AreEqual(SharedList[0], $"Lambda method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[1], $"Beta method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[2], $"Gamma method has been called and recived the following message : {publisherMessage}");
        }

        [TestMethod]
        public void Assert_That_Suscribers_Received_The_Publishers_Message_Raised_By_An_Event_Standard()
        {
            var l = new Lambda();
            var b = new Beta();
            var g = new Gamma();

            SutEventStandard += l.LambdaMethodStandard;
            SutEventStandard += b.BetaMethodStandard;
            SutEventStandard += g.GammaMethodStandard;

            var publisherMessage = "this is the publisher's message";
            var eventArgs = new SutEventArgs { Message = publisherMessage };

            SutEventStandard?.Invoke(this, eventArgs);

            Assert.AreEqual(SharedList[0], $"Lambda method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[1], $"Beta method has been called and recived the following message : {publisherMessage}");
            Assert.AreEqual(SharedList[2], $"Gamma method has been called and recived the following message : {publisherMessage}");
        }
    }

    /// <summary>
    /// Lambda represent one external suscriber
    /// </summary>
    public class Lambda
    {
        public void LambdaMethod(string s)
        {
            EventTest.SharedList.Add($"Lambda method has been called and recived the following message : {s}");
        }
        public void LambdaMethodStandard(object sender, SutEventArgs e)
        {
            EventTest.SharedList.Add($"Lambda method has been called and recived the following message : {e.Message}");
        }
    }

    /// <summary>
    /// Beta represent one external suscriber
    /// </summary>
    public class Beta
    {
        public void BetaMethod(string s)
        {
            EventTest.SharedList.Add($"Beta method has been called and recived the following message : {s}");
        }
        public void BetaMethodStandard(object sender, SutEventArgs e)
        {
            EventTest.SharedList.Add($"Beta method has been called and recived the following message : {e.Message}");
        }
    }

    /// <summary>
    /// Gamma represent one external suscriber
    /// </summary>
    public class Gamma
    {
        public void GammaMethod(string s)
        {
            EventTest.SharedList.Add($"Gamma method has been called and recived the following message : {s}");
        }
        public void GammaMethodStandard(object sender, SutEventArgs e)
        {
            EventTest.SharedList.Add($"Gamma method has been called and recived the following message : {e.Message}");
        }
    }
    public delegate void SutDelegate(string s);
    public delegate void SutDelegateStandard(object o, SutEventArgs e);
    public class SutEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
