using System;
using System.Configuration;

namespace Ian.Configuration
{
    public class SimpleConfigurationReader : IConfigurationReader
    {
        public object GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }

        public T GetSection<T>(string sectionName) where T : class, new()
        {
            return GetSection(sectionName) as T;
        }
    }
}
