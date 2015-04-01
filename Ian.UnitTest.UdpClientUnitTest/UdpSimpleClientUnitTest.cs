using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ian.UdpClient;

namespace Ian.UnitTest.UdpClientUnitTest
{
    [TestClass]
    public class UdpSimpleClientUnitTest
    {
        private const int PORT = 46668;

        [TestMethod]
        public void ClientTest()
        {
            UdpSimpleClient client = new UdpSimpleClient(PORT);
            client.OnOpened += client_OnOpened;
            client.OnClosed += client_OnClosed;
            client.OnReceived += client_OnReceived;
            client.OnException += client_OnException;

            client.Open();

            System.Threading.Thread.Sleep(100000);
            client.Close();
            System.Threading.Thread.Sleep(10000);
        }

        void client_OnReceived(object sender, UdpEventArg e)
        {
            string msg = System.Text.Encoding.ASCII.GetString(e.Data);
        }

        void client_OnOpened(object sender, UdpEventArg e)
        {
            
        }

        void client_OnException(object sender, UdpEventArg e)
        {
            string msg = e.Exception.Message;
        }

        void client_OnClosed(object sender, UdpEventArg e)
        {
            
        }
    }
}
