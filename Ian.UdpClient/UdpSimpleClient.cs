using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ian.UdpClient
{
    public class UdpSimpleClient : Client
    {
        /// <summary>
        /// 本地地址簇
        /// </summary>
        private static readonly System.Net.IPAddress[] Addresses;
        /// <summary>
        /// 本地 IPv4 地址
        /// </summary>
        private static readonly System.Net.IPAddress LocalIpv4Address;
        /// <summary>
        /// 网络数据报，本地接收端口
        /// </summary>
        private readonly int _Port;

        //static _actor.
        static UdpSimpleClient()
        {
            string hostname = System.Net.Dns.GetHostName();
            Addresses = System.Net.Dns.GetHostAddresses(hostname);
            LocalIpv4Address = Addresses.FirstOrDefault(t => t.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }
        
        //_actor.
        public UdpSimpleClient(int port)
        {
            this._Port = port;
        }

        protected sealed override System.Net.Sockets.UdpClient CreateUdpClient()
        {
            System.Net.IPEndPoint localIpEndPoint = new System.Net.IPEndPoint(LocalIpv4Address, _Port);
            System.Net.Sockets.UdpClient client = new System.Net.Sockets.UdpClient(localIpEndPoint);
            return client;
        }
    }
}
