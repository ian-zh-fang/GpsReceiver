using System;

namespace Tiger.GpsAnalyzer.Configuration
{
    /// <summary>
    /// GPS 数据解析配置模型
    /// </summary>
    public class ConfigSectionModel
    {
        [Ian.Configuration.ConfigurationSection(Name = "encoding", IsAttribute = true, AttributeName = "value")]
        public string EncodingText { get; set; }
    }
}
