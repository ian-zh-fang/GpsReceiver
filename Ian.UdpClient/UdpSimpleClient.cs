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
        private static readonly System.Net.IPAddress[] ADDRESSES;
        /// <summary>
        /// 本地 IPv4 地址
        /// </summary>
        private static readonly System.Net.IPAddress LOCALIPV4ADDRESS;
        /// <summary>
        /// 网络数据报，本地接收端口
        /// </summary>
        private readonly int _Port;

        //static _actor.
        static UdpSimpleClient()
        {
            string hostname = System.Net.Dns.GetHostName();
            ADDRESSES = System.Net.Dns.GetHostAddresses(hostname);
            LOCALIPV4ADDRESS = ADDRESSES[3];
        }
        
        //_actor.
        public UdpSimpleClient(int port)
        {
            this._Port = port;
        }

        protected sealed override System.Net.Sockets.UdpClient CreateUdpClient()
        {
            System.Net.IPEndPoint localIpEndPoint = new System.Net.IPEndPoint(LOCALIPV4ADDRESS, _Port);
            System.Net.Sockets.UdpClient client = new System.Net.Sockets.UdpClient(localIpEndPoint);
            return client;
        }
    }
}
