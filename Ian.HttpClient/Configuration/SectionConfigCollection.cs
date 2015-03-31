using System;
using System.Collections.Generic;
using System.Linq;

namespace Ian.HttpClient.Configuration
{
    public class SectionConfigCollection
    {
        [Ian.Configuration.ConfigurationSection(Name = "conf", IsCollection = true)]
        public List<SectionConfigModel> Configs { get; set; }

        public int Count
        {
            get { return (Configs = Configs ?? new List<SectionConfigModel>()).Count; }
        }

        public SectionConfigModel this[int index]
        {
            get { return (Configs = Configs ?? new List<SectionConfigModel>())[index]; }
        }

        public SectionConfigModel this[string name]
        {
            get { return (Configs = Configs ?? new List<SectionConfigModel>()).FirstOrDefault(t => t.Name == name); }
        }
    }

    public class SectionConfigModel
    {
        [Ian.Configuration.ConfigurationSection(Name = "name", IsAttribute = true, AttributeName = "value")]
        public string Name { get; set; }

        [Ian.Configuration.ConfigurationSection(Name = "serverip", IsAttribute = true, AttributeName = "value")]
        public string ServerIp { get; set; }

        [Ian.Configuration.ConfigurationSection(Name = "apiuri", IsAttribute = true, AttributeName = "value")]
        public string ApiUri { get; set; }
    }
}
