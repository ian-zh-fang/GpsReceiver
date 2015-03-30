using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ian.Configuration;
using Ian.HttpClient.Configuration;

namespace Ian.UnitTest.ConfigurationTest
{
    [TestClass]
    public class SimpleConfigurationReaderUnitTest
    {
        private static readonly string SECTION = "def.config";
        private static readonly string SECTION1 = "def.config1";
        private static readonly string SECTION2 = "def.config2";

        private readonly IConfigurationReader _reader = new SimpleConfigurationReader();

        [TestMethod]
        public void GetSection()
        {
            object conf = _reader.GetSection(SECTION);
            Assert.IsNull(conf);

            object obj = _reader.GetSection(SECTION1);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(SectionConfigCollection));

            SectionConfigCollection configs = obj as SectionConfigCollection;
            Assert.IsNotNull(configs);
            Assert.IsTrue(configs.Count == 2);

            SectionConfigModel config1 = configs[0];
            Assert.IsNotNull(config1);
            Assert.AreEqual(config1.ApiUri, "api");
            Assert.AreEqual(config1.Name, "test-config1");
            Assert.AreEqual(config1.ServerIp, "localhost:23586");

            SectionConfigModel config2 = configs[1];
            Assert.IsNotNull(config2);
            Assert.AreEqual(config2.ApiUri, "api");
            Assert.AreEqual(config2.Name, "test-config2");
            Assert.AreEqual(config2.ServerIp, "localhost:23586");

            SectionConfigCollection obj2 = _reader.GetSection<SectionConfigCollection>(SECTION2);
            Assert.IsNotNull(obj2);
            Assert.AreEqual(obj2.Count, 1);

            SectionConfigModel config3 = obj2[0];
            Assert.IsNotNull(config3);
            Assert.AreEqual(config3.ApiUri, "api");
            Assert.AreEqual(config3.Name, "test-config3");
            Assert.AreEqual(config3.ServerIp, "localhost:23586");
        }
    }
}
