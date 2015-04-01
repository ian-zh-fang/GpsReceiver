using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ian.UdpClient.UdpDatagramAnalyzer
{
    /// <summary>
    /// 简单的网络数据报协议（UDP）数据解析程序
    /// </summary>
    public class SimpleDatagramAnalyzer : DatagramAnalyzer
    {
        protected sealed override Client CreateUdpClient(int port)
        {
            Client client = new UdpSimpleClient(port);
            return client;
        }

        protected override byte[] Analyze(byte[] buffer)
        {
            return buffer;
        }
    }
}
