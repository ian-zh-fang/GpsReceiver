using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiger.GpsAnalyzer
{
    /// <summary>
    /// GPS 数据报协议数据模型
    /// </summary>
    public class GpsDatagramModel
    {
        //<?xml version="1.0" encoding="UTF-8" ?>
        //<PPVSPMessage>
        //<Version>0.1</Version>
        //<Sequence>1</Sequence>
        //<CommandType>REQUEST</CommandType>
        //<Command>GPS</Command>
        //<Params>
        //<DeviceID>13876687687</DeviceID>
        //<Time>2009-02-24 16:59:00</Time>
        //<DivisionEW>E</DivisionEW>
        //<Longitude>13034500</Longitude>
        //<DivisionNS>N</DivisionNS>
        //<Latitude>28650000</Latitude>
        //<Direction>2898</Direction>
        //<Speed>1239900</Speed >
        //<Remark></Remark>
        //</Params>
        //</PPVSPMessage>

        /// <summary>
        /// 版本说明
        /// </summary>
        [Ian.Utility.XmlUtil.Model(Name = "Version", IsLeaf=true)]
        public string Version { get; set; }

        /// <summary>
        /// 队列数
        /// </summary>
        [Ian.Utility.XmlUtil.Model(Name = "Sequence", IsLeaf = true)]
        public int Sequence { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        [Ian.Utility.XmlUtil.Model(Name = "CommandType", IsLeaf = true)]
        public string CommandType { get; set; }

        /// <summary>
        /// 命令种类
        /// </summary>
        [Ian.Utility.XmlUtil.Model(Name = "Command", IsLeaf = true)]
        public string Command { get; set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        [Ian.Utility.XmlUtil.Model(Name = "Params")]
        public GpsParamsDatagramModel Params { get; set; }

        /// <summary>
        /// GPS 数据报协议参数数据模型
        /// </summary>
        public class GpsParamsDatagramModel
        {
            /// <summary>
            /// 设备ID,设备在平台上添加时的编号
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "DeviceID", IsLeaf = true)]
            public string DeviceID { get; set; }

            /// <summary>
            /// 时间
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "Time", IsLeaf = true)]
            public DateTime Time { get; set; }

            /// <summary>
            /// 经度方向 E:东  W：西
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "DivisionEW", IsLeaf = true)]
            public string DivisionEW { get; set; }

            /// <summary>
            /// 经度值 实际经度需要除以360000
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "Longitude", IsLeaf = true)]
            public long Longitude { get; set; }

            /// <summary>
            /// 纬度方向 N:北  S：南
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "DivisionNS", IsLeaf = true)]
            public string DivisionNS { get; set; }

            /// <summary>
            /// 纬度值 实际经度需要除以360000
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "Latitude", IsLeaf = true)]
            public long Latitude { get; set; }

            /// <summary>
            /// //方向角 实际方向(正北方向为0度,顺时针方向计算)需要除以100
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "Direction", IsLeaf = true)]
            public long Direction { get; set; }

            /// <summary>
            /// 速度  厘米/小时
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "Speed", IsLeaf = true)]
            public long Speed { get; set; }

            /// <summary>
            /// 备注信息
            /// </summary>
            [Ian.Utility.XmlUtil.Model(Name = "Remark", IsLeaf = true)]
            public string Remark { get; set; }
        }
    }
}
