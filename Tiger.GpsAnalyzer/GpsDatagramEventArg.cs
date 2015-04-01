using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiger.GpsAnalyzer
{
    /// <summary>
    /// GPS 数据报文解析事件参数
    /// </summary>
    public class GpsDatagramEventArg:EventArgs
    {
        /// <summary>
        /// 解析完成的数据报文数据模型实例
        /// </summary>
        public GpsDatagramModel Data { get; set; }

        public GpsDatagramEventArg(GpsDatagramModel data)
        {
            Data = data;
        }
    }
}
