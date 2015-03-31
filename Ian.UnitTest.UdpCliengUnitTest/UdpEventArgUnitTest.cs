using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ian.UdpClient;

namespace Ian.UnitTest.UdpCliengUnitTest
{
    [TestClass]
    public class UdpEventArgUnitTest
    {
        public delegate UdpEventArg UdpEventHandler();

        [TestMethod]
        public void UdpEventArgTest()
        {
            UdpEventArg a = new UdpEventHandler(delegate() { return UdpEventArg.CreateOpen(); })();
            Assert.AreEqual(a.Code, EventCode.Opened);

            UdpEventArg b = new UdpEventHandler(delegate() { return UdpEventArg.CreateClose(); })();
            Assert.AreEqual(b.Code, EventCode.Closed);

            Assert.AreNotEqual(a.Code, b.Code);
        }
    }
}
