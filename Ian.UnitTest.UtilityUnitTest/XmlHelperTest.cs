using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ian.Utility.XmlUtil;

namespace Ian.UnitTest.UtilityUnitTest
{
    [TestClass]
    public class XmlHelperTest
    {
        private static readonly string Xml;
        static XmlHelperTest()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<PPVSPMessage>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Version>0.1</Version>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Sequence>1</Sequence>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<CommandType>REQUEST</CommandType>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Command>GPS</Command>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Params>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<DeviceID>13876687687</DeviceID>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Time>2009-02-24 16:59:00</Time>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<DivisionEW>E</DivisionEW>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Longitude>13034500</Longitude>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<DivisionNS>N</DivisionNS>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Latitude>28650000</Latitude>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Direction>2898</Direction>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Speed>1239900</Speed>");
            stringBuilder.Append("\n");
            stringBuilder.Append("<Remark></Remark>");
            stringBuilder.Append("\n");
            stringBuilder.Append("</Params>");
            stringBuilder.Append("\n");
            stringBuilder.Append("</PPVSPMessage>");
            Xml = stringBuilder.ToString();
        }

        [TestMethod]
        public void XmlElementToEntityTest()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(Xml);

            object obj = doc.DocumentElement.XmlElementToEntity<Tiger.GpsAnalyzer.GpsDatagramModel>();
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(Tiger.GpsAnalyzer.GpsDatagramModel));

            Tiger.GpsAnalyzer.GpsDatagramModel t = obj as Tiger.GpsAnalyzer.GpsDatagramModel;
            Assert.IsNotNull(t);
            Assert.IsNotNull(t.Params);
        }
    }
}
