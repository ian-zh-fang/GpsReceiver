using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ian.UdpClient
{
    /// <summary>
    /// Udp 网络数据接收事件参数模型
    /// </summary>
    public class UdpEventArg:EventArgs
    {
        private EventCode _Code;
        /// <summary>
        /// 标识当前事件参数信息类型
        /// </summary>
        public EventCode Code 
        {
            get { return _Code; }
            private set { _Code = value; }
        }

        private Exception _Exception;
        /// <summary>
        /// 异常信息
        /// <para>当 Code 的值为 EventCode.Exception 时，当前属性启用；否则为 NULL</para>
        /// </summary>
        public Exception Exception 
        {
            get { return _Exception; }
            private set { _Exception = value; }
        }

        private byte[] _Data;
        /// <summary>
        /// 数据正文内容
        /// </summary>
        public byte[] Data 
        { 
            get 
            {
                int len = (_Data = _Data ?? new byte[0]).Length;
                byte[] buffer = new byte[len];
                Array.Copy(_Data, buffer, len);
                return buffer;
            }
            private set { _Data = value; }
        }

        private DateTime _CreateTime = DateTime.Now;
        /// <summary>
        /// 当前实例创建的时间
        /// </summary>
        public DateTime CreateTime 
        {
            get { return _CreateTime; }
            private set { _CreateTime = value; }
        }

        /// <summary>
        /// _actor.
        /// </summary>
        protected UdpEventArg() { }

        /// <summary>
        /// 创建一个 Opened 类型的事件模型实例
        /// <para>标识组件已经打开数据接收端口，并且开始工作</para>
        /// </summary>
        /// <returns></returns>
        public static UdpEventArg CreateOpen()
        {
            return new UdpEventArg() 
            {
                Code = EventCode.Opened
            };
        }

        /// <summary>
        /// 创建一个 Closed 类型的事件模型实例
        /// <para>标识组件已经关闭数据接收端口，并且释放相关资源</para>
        /// </summary>
        /// <returns></returns>
        public static UdpEventArg CreateClose()
        {
            return new UdpEventArg()
            {
                Code = EventCode.Closed
            };
        }

        /// <summary>
        /// 创建一个 Received 类型的事件模型实例
        /// <para>标识接收数据超时</para>
        /// </summary>
        /// <param name="data">数据正文，它包含了接收到的网络流数据</param>
        /// <returns></returns>
        public static UdpEventArg CreateReceive(byte[] data)
        {
            return new UdpEventArg()
            {
                Code = EventCode.Received,
                Data = data
            };
        }

        /// <summary>
        /// 创建一个 Exception 类型的事件模型实例
        /// <para>标识组建执行发生错误</para>
        /// </summary>
        /// <param name="e">异常模型实例，它包含了发生的异常信息</param>
        /// <returns></returns>
        public static UdpEventArg CreateException(Exception e)
        {
            return new UdpEventArg()
            {
                Code = EventCode.Exception,
                Exception = e
            };
        }
    }
}
