using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ian.Configuration;
using Ian.HttpClient.Configuration;

namespace Ian.HttpClient
{
    public class SimpleClient : Client
    {
        protected static readonly string DEFAULTSECTIONNAME;
        protected static readonly IConfigurationReader CONFIGURATIONREADER;

        protected readonly string _basic_uri;

        static SimpleClient()
        {
            DEFAULTSECTIONNAME = "defaultclientconf";
            CONFIGURATIONREADER = new SimpleConfigurationReader();
        }

        public SimpleClient()
        {
            SectionConfigCollection configs = CONFIGURATIONREADER.GetSection<SectionConfigCollection>(DEFAULTSECTIONNAME);
            SectionConfigModel config = configs[DEFAULTSECTIONNAME];
            _basic_uri = string.Join("/", "http:/", config.ServerIp, config.ApiUri);
        }

        public SimpleClient(string requestBasicUri)
        {
            _basic_uri = requestBasicUri.TrimEnd('/');
        }

        protected string Request<T>(
            string actionName,
            string controllerName,
            MethodType method, 
            T t)
        {
            string url = string.Join("/", _basic_uri, controllerName, actionName);
            return Request<T>(url, method, t);
        }


        protected ExecuteResult<T> Request<T>(
            string actionName, 
            string controllerName, 
            MethodType method, 
            ParamCollection parameters)
        {
            string url = string.Join("/", _basic_uri, controllerName, actionName);
            string responseText = Request(url, method, parameters);
            ExecuteResult<T> result = Newtonsoft.Json.JsonConvert.DeserializeObject<ExecuteResult<T>>(responseText);
            return result;
        }

        protected ExecuteResult<T> Request<T, T1>(
            string actionName, 
            string controllerName, 
            MethodType method, T1 t)
        {
            string responseText = Request<T1>(actionName, controllerName, method, t);
            ExecuteResult<T> result = Newtonsoft.Json.JsonConvert.DeserializeObject<ExecuteResult<T>>(responseText);
            return result;
        }

        public ExecuteResult<T> Post<T>(
            string actionName, 
            string controllerName, 
            ParamCollection parameters)
        {
            return Request<T>(actionName, controllerName, MethodType.Post, parameters);
        }

        /// <summary>
        /// 以 POST 方式向远程服务器发送请求，并返回响应结果
        /// </summary>
        /// <typeparam name="T">标识返回响应数据的运行时类型</typeparam>
        /// <typeparam name="T1">标识指定请求数据参数的运行时类型</typeparam>
        /// <param name="actionName">服务器方法名称</param>
        /// <param name="controllerName">服务器控制器名称</param>
        /// <param name="t">制定类型的实例，它标识请求数据的参数信息</param>
        /// <returns>返回 ExecuteResult&lt;T> 的一个实例</returns>
        public ExecuteResult<T> Post<T, T1>(
            string actionName, 
            string controllerName, 
            T1 t)
        {
            return Request<T, T1>(actionName, controllerName, MethodType.Post, t);
        }

        public ExecuteResult<T> Get<T>(
            string actionName,
            string controllerName,
            ParamCollection parameters)
        {
            return Request<T>(actionName, controllerName, MethodType.Get, parameters);
        }

        /// <summary>
        /// 以 GET 方式向远程服务器发送请求，并返回响应结果
        /// </summary>
        /// <typeparam name="T">标识返回响应数据的运行时类型</typeparam>
        /// <typeparam name="T1">标识指定请求数据参数的运行时类型</typeparam>
        /// <param name="actionName">服务器方法名称</param>
        /// <param name="controllerName">服务器控制器名称</param>
        /// <param name="t">制定类型的实例，它标识请求数据的参数信息</param>
        /// <returns>返回 ExecuteResult&lt;T> 的一个实例</returns>
        public ExecuteResult<T> Get<T, T1>(
            string actionName, 
            string controllerName, 
            T1 t)
        {
            return Request<T, T1>(actionName, controllerName, MethodType.Get, t);
        }

        /// <summary>
        /// 执行远程请求响应结果模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public sealed class ExecuteResult<T>
        {
            /// <summary>
            /// 执行结果
            /// </summary>
            public T Result { get; set; }

            /// <summary>
            /// 响应消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 响应代码
            /// <para>0 -- 标识响应数据成功</para>
            /// </summary>
            public int Status { get; set; }
        }
    }
}
