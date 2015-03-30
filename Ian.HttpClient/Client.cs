using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Ian.Utility;
using Ian.HttpClient.Configuration;

namespace Ian.HttpClient
{
    public abstract class Client
    {
        private static readonly string DEFAULTUSERAGENT = "defaultuseragent";
        static Client()
        {
            DEFAULTUSERAGENT = System.Configuration.ConfigurationManager.AppSettings[DEFAULTUSERAGENT];
        }

        /// <summary>
        /// 发送 HttpWebRequest 请求，返回服务器响应数据
        /// </summary>
        /// <param name="url">远程服务器数据请求地址</param>
        /// <param name="method">请求方式，枚举类型 MethodType 的值之一。</param>
        /// <param name="parameters">请求参数信息。</param>
        /// <returns>返回服务器响应数据</returns>
        public string Request(string url, MethodType method, ParamCollection parameters)
        {            
            HttpWebRequest request = Create(url, method, parameters);
            string responseText = Receive(request).Result;
            return responseText;
        }

        public string Request<T>(string url, MethodType method, T t)
        {
            ParamCollection parameters = CreateParamCollection<T>(t);
            return Request(url, method, parameters);
        }

        /// <summary>
        /// 根据指定类型实例的参数形式，并返回 ParamCollection 的一个实例
        /// </summary>
        /// <typeparam name="T">指定的运行时类型</typeparam>
        /// <param name="t">指定运行时类型的一个实例</param>
        /// <returns>返回 ParamCollection 的一个实例</returns>
        protected virtual ParamCollection CreateParamCollection<T>(T t)
        { 
            Type tp = typeof(T);
            PropertyInfo[] properties = tp.GetProperties();
            ParamCollection parameters = new ParamCollection();
            int len = properties.Length;
            PropertyInfo property = null;
            for (int i = 0; i < len; i++)
            {
                if ((property = properties[i]).CanRead)
                {
                    object val = property.GetValue(t, null);
                    parameters.Add(property.Name, string.Format("{0}", val));
                }
            }
            return parameters;
        }

        protected virtual async Task<string> Receive(HttpWebRequest request)
        {
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            string val = null;

            try
            {
                //此处等待请求响应
                response = await Task.Factory.StartNew(() => (HttpWebResponse)request.GetResponse());
                //从此处开始读取请求的数据
                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                val = reader.ReadToEnd();
            }
            catch (Exception e) { throw e; }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }

                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }

            return val;
        }

        /// <summary>
        /// 创建 HttpWebRequest 请求实例。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式，枚举类型 MethodType 的值之一。</param>
        /// <param name="parameters">请求参数信息。</param>
        /// <returns>返回 HttpWebRequest 的一个实例</returns>
        protected virtual HttpWebRequest Create(string url, MethodType method, ParamCollection parameters)
        {
            HttpWebRequest request = null;
            parameters = parameters ?? new ParamCollection();
            switch (method)
            { 
                case MethodType.Get:
                    url = string.Format("{0}?token={1}&{2}", url, Guid.NewGuid().ToInt64(), parameters.ToString());
                    request = HttpWebRequest.CreateHttp(url);
                    request.ContentType = "text/html";
                    request.Method = "GET";
                    request.ContentLength = 0;
                    break;
                case MethodType.Post:
                default:
                    request = HttpWebRequest.CreateHttp(url);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    WriteParamToStream(parameters, ref request);
                    break;
            }
            request.CookieContainer = new CookieContainer();
            request.UserAgent = DEFAULTUSERAGENT;

            return request;
        }

        /// <summary>
        /// 向 HttpWebRequest 请求正文中添加参数信息
        /// </summary>
        /// <param name="parameters">参数集合信息</param>
        /// <param name="request">HttpWebRequest 请求实例</param>
        protected virtual void WriteParamToStream(ParamCollection parameters, ref HttpWebRequest request)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(parameters.ToString());
            int len = buffer.Length;

            request.ContentLength = len;
            using(Stream stream = request.GetRequestStream())
            {
                stream.Write(buffer, 0, len);
                stream.Close();
            }
        }
    }
}
