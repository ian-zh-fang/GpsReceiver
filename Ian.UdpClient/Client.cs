using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Ian.UdpClient
{
    public abstract class Client
    {
        private const int MAXWAITTIME = 10000;

        /// <summary>
        /// System.Net.Sockets.UdpClient 数据报协议（Udp）服务
        /// </summary>
        private System.Net.Sockets.UdpClient _UdpClient;
        /// <summary>
        /// 通知 System.Threading.CancellationToken，告知其应被取消。
        /// </summary>
        private System.Threading.CancellationTokenSource _CancelToken;

        /// <summary>
        /// 本地Ipv4地址
        /// </summary>
        public string LocalEndPoint
        {
            get { return (null == _UdpClient) ? string.Empty : _UdpClient.Client.LocalEndPoint.ToString(); }
        }

        /// <summary>
        /// Udp 打开指定网络端口，并开始接收数据工作，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnOpened;
        
        /// <summary>
        /// Udp 网络数据端口被关闭，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnClosed;
        
        /// <summary>
        /// 程序接收到网络数据，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnReceived;

        /// <summary>
        /// 程序执行错误，触发当前事件
        /// </summary>
        public event EventHandler<UdpEventArg> OnException;

        /// <summary>
        /// 触发 OnOpened 事件
        /// </summary>
        /// <param name="sender">触发当前事件对象</param>
        protected virtual void OpenTrigger(object sender = null)
        {
            if (OnOpened != null)
                OnOpened(sender ?? this, UdpEventArg.CreateOpen());
        }

        /// <summary>
        /// 触发 OnClosed 事件
        /// </summary>
        /// <param name="sender">触发当前事件对象</param>
        protected virtual void CloseTrigger(object sender = null)
        {
            if (OnClosed != null)
                OnClosed(sender ?? this, UdpEventArg.CreateClose());
        }

        /// <summary>
        /// 触发 OnReceived 事件
        /// </summary>
        /// <param name="sender">触发当前事件对象</param>
        /// <param name="data">接收到的数据正文信息</param>
        protected virtual void ReceiveTrigger(byte[] data, object sender = null)
        {
            if (OnReceived != null)
                OnReceived(sender ?? this, UdpEventArg.CreateReceive(data));
        }

        /// <summary>
        /// 触发 OnException 事件
        /// </summary>
        /// <param name="sender">触发当前事件对象</param>
        /// <param name="e">程序执行时，发生的错误信息对象</param>
        protected virtual void ExceptionTrigger(Exception e, object sender = null)
        {
            if (OnException != null)
                OnException(sender ?? this, UdpEventArg.CreateException(e));
        }

        /// <summary>
        /// 此处创建底层数据报协议（UDP）网络服务；
        /// 并开启数据接收线程
        /// </summary>
        public void Open()
        {
            //如果缓存存在，那么不在继续调用程序执行端口再次打开
            if (_UdpClient != null) return;

            //创建底层数据报协议（UDP）网络服务，并缓存当前的服务
            _UdpClient = CreateUdpClient();

            //此处开始接收网络数据报
            ExecuteAsync();
        }

        /// <summary>
        /// 此处关闭当前端口，取消数据接收工作；
        /// 并释放占用的系统资源，包括内存和套接字
        /// </summary>
        public void Close()
        {
            if (_CancelToken == null) return;

            //此处异步调用请求停止接收网络数据报
            _CancelToken.CancelAfter(1000);
        }

        /// <summary>
        /// 创建 System.Net.Sockets.UdpClient 的实例
        /// </summary>
        /// <returns>System.Net.Sockets.UdpClient 的一个实例</returns>
        protected abstract System.Net.Sockets.UdpClient CreateUdpClient();

        private async void ExecuteAsync()
        {
            await Task.Delay(1000);
            //程序开始执行
            OpenTrigger();
            //创建包含取消操作的闭包
            _CancelToken = new System.Threading.CancellationTokenSource();

            //程序执行中...
            try 
            {
                await Task.Factory.StartNew(new Action(ExecuteCoreAsync), _CancelToken.Token);
            }
            catch (Exception e)
            {
                ExceptionTrigger(e);
            }
            finally
            {
                //释放相关资源
                _CancelToken.Dispose();

                _UdpClient.Client.Close();
                _UdpClient.Close();
            }
            _CancelToken = null;
            _UdpClient = null;
            GC.Collect();//显示调用GC，强制垃圾回收

            //程序执行结束
            CloseTrigger();
        }

        /// <summary>
        /// 此处用来循环接收网络数据报
        /// </summary>
        private void ExecuteCoreAsync()
        {
            //异步取消，停止接收数据报
            if (_CancelToken.IsCancellationRequested) return;

            try
            {
                Task<System.Net.Sockets.UdpReceiveResult> task = _UdpClient.ReceiveAsync();
                //等待指定时间
                if (task.Wait(MAXWAITTIME))
                {
                    System.Net.Sockets.UdpReceiveResult result = task.Result;
                    ReceiveTrigger(result.Buffer);
                }

                //递归调用，循环获取数据
                ExecuteCoreAsync();
            }
            catch (AggregateException e)
            {
                e.Flatten().Handle(t => true);
                ExceptionTrigger(e.InnerException);
            }
            catch (Exception e)
            {
                ExceptionTrigger(e);
            }
            finally 
            {
                
            }
        }
    }
}
