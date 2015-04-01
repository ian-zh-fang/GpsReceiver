using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ian.UdpClient.UdpDatagramAnalyzer
{
    /// <summary>
    /// 网络数据报协议（UDP）服务数据解析器
    /// </summary>
    public abstract class DatagramAnalyzer
    {
        /// <summary>
        /// 缓存已接收到网络数据报
        /// </summary>
        private readonly List<byte> _CacheBuffer = new List<byte>();
        /// <summary>
        /// 网络数据报协议（UDP）服务数据转发器
        /// </summary>
        private UdpClient.Client _Client;

        /// <summary>
        /// Udp 打开指定网络端口，并开始接收数据工作，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnOpened
        {
            add { _Client.OnOpened += value; }
            remove { _Client.OnOpened -= value; }
        }

        /// <summary>
        /// Udp 网络数据端口被关闭，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnClosed
        {
            add { _Client.OnClosed += value; }
            remove { _Client.OnClosed -= value; }
        }

        /// <summary>
        /// 程序接收到网络数据，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnReceived
        {
            add { _Client.OnReceived += value; }
            remove { _Client.OnReceived -= value; }
        }

        /// <summary>
        /// 程序执行错误，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnException
        {
            add { _Client.OnException += value; }
            remove { _Client.OnException -= value; }
        }

        /// <summary>
        /// 创建 UdpClient.Client 的一个实例
        /// </summary>
        /// <param name="port">网络数据报本地接收端口</param>
        /// <returns></returns>
        protected abstract UdpClient.Client CreateUdpClient(int port);

        /// <summary>
        /// 数据解析函数，调用本地函数解析当前数据；
        /// 返回尚未解析的字节数组
        /// </summary>
        /// <param name="buffer">程序接收到的所有数据报</param>
        /// <returns></returns>
        protected abstract byte[] Analyze(byte[] buffer);

        /// <summary>
        /// 打开网络数据报协议（UDP）服务；
        /// 开始接收网络数据报
        /// </summary>
        /// <param name="port">本地接收网络数据报文端口（0~65535）</param>
        public void Open(int port)
        {
            _Client = _Client ?? CreateUdpClient(port);
            _Client.OnReceived += _Client_OnReceived;

            _Client.Open();
        }

        //此处开始解析数据
        private void _Client_OnReceived(object sender, UdpEventArg e)
        {
            //缓存接收到的网络数据报；
            //并解析所有收到的数据报
            _CacheBuffer.AddRange(e.Data);
            byte[] buffer = Analyze(_CacheBuffer.ToArray());

            //解析数据完成；
            //移除解析完成的数据报
            int cachelen = _CacheBuffer.Count;
            int len = buffer.Length;
            _CacheBuffer.InsertRange(0, buffer);
            _CacheBuffer.RemoveRange(len, cachelen);
        }

        /// <summary>
        /// 关闭网络数据报协议（UDP）服务；
        /// 停止接收网络数据报
        /// </summary>
        public void Close()
        {
            if (null != _Client)
                _Client.Close();
        }
    }
}
