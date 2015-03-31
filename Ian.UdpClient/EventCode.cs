using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ian.UdpClient
{
    /// <summary>
    /// （枚举类）事件编码类型
    /// </summary>
    public enum EventCode : byte
    {
        /// <summary>
        /// 以开始执行
        /// </summary>
        Opened = 0x00,
        /// <summary>
        /// 以关闭执行
        /// </summary>
        Closed = 0x01,
        /// <summary>
        /// 以收到数据
        /// </summary>
        Received = 0x02,
        /// <summary>
        /// 执行异常
        /// </summary>
        Exception = 0x04,
        /// <summary>
        /// 等待超时
        /// </summary>
        TimeOut = 0x08,
    }
}
