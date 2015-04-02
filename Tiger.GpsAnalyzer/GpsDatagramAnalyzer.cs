using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiger.GpsAnalyzer.Configuration;
using Ian.Utility.XmlUtil;

namespace Tiger.GpsAnalyzer
{
    /// <summary>
    /// GPS 网络数据报协议（UDP）数据解析程序
    /// </summary>
    public sealed class GpsDatagramAnalyzer:Ian.UdpClient.UdpDatagramAnalyzer.SimpleDatagramAnalyzer
    {
        /// <summary>
        /// 配置节名称
        /// </summary>
        private const string SECTIONNAME = "gpsanalyzer";
        /// <summary>
        /// 协议文档开始声明
        /// </summary>
        private const string HEADERCONTEXT = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
        /// <summary>
        /// 协议头
        /// </summary>
        private const string PROTOCOLSTART = "<PPVSPMessage>";
        /// <summary>
        /// 协议尾
        /// </summary>
        private const string PROTOCOLEND = "</PPVSPMessage>";
        /// <summary>
        /// 数据编码方式
        /// </summary>
        private static readonly Encoding Encoding;

        public event EventHandler<GpsDatagramEventArg> OnAnalyzed;

        static GpsDatagramAnalyzer()
        {
            Ian.Configuration.SimpleConfigurationReader r = new Ian.Configuration.SimpleConfigurationReader();
            ConfigSectionModel t = r.GetSection<ConfigSectionModel>(SECTIONNAME);
            if (string.IsNullOrWhiteSpace(t.EncodingText))
                Encoding = System.Text.Encoding.ASCII;
            else
                Encoding = System.Text.Encoding.GetEncoding(t.EncodingText);
        }

        protected override byte[] Analyze(byte[] buffer)
        {
            try
            {
                string context = Encoding.GetString(buffer, 0, buffer.Length);
                //此处开始解析代码
                //采用正则表达式获取匹配项
                int offset = Analyze(context);
                context = context.Remove(0, offset);
                Array.Clear(buffer, 0, buffer.Length);
                buffer = Encoding.GetBytes(context);
            }
            catch(Exception) 
            {
                Array.Clear(buffer, 0, buffer.Length);
                buffer = new byte[0];
            }

            return base.Analyze(buffer);
        }

        //解析文档信息
        //返回当前数据报的
        private int Analyze(string context, int offset = 0)
        {
            //查找数据报开头
            int index = context.IndexOf(HEADERCONTEXT, offset);
            if (-1 == index)
                return offset;

            index += HEADERCONTEXT.Length;
            //查找协议头
            index = context.IndexOf(PROTOCOLSTART, index);
            if (-1 == index)
                return offset;

            index += PROTOCOLSTART.Length;
            //查找第二个数据报开头
            int sindex = context.IndexOf(HEADERCONTEXT, index);

            //查找协议尾
            int indexe = context.IndexOf(PROTOCOLEND, index);
            if (-1 == indexe)
                return offset;

            //出现粘包；
            //并且上一包数据断尾
            if (sindex >= 0 && indexe > sindex)
                return Analyze(context, sindex);
            //重新计算当前
            indexe += PROTOCOLEND.Length;

            //截取完全的数据报
            int len = indexe - offset;
            string xmldoc = context.Substring(offset, len).Trim(' ', '\n', '\t');
            
            //解析数据报
            AnalyzeXmlDatagram(xmldoc);

            //重新赋值下一个数据报开始位置
            //并递归调用解析数据报
            offset = indexe + 1;
            return Analyze(context, offset);
        }

        //解析获取得到的数据包
        private void AnalyzeXmlDatagram(string xmlcontext)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xmlcontext);

            GpsDatagramModel t = doc.DocumentElement.XmlElementToEntity<GpsDatagramModel>() as GpsDatagramModel;
            if (null != OnAnalyzed)
            {
                GpsDatagramEventArg m = new GpsDatagramEventArg(t);
                OnAnalyzed(this, m);
            }
        }
    }
}
