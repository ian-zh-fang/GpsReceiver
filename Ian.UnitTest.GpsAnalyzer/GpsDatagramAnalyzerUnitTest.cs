using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ian.UnitTest.GpsAnalyzerUnitTest
{
    [TestClass]
    public class GpsDatagramAnalyzerUnitTest
    {
        private const int PORT = 46668;

        [TestMethod]
        public void GpsDatagramAnalyzerTest()
        {
            Tiger.GpsAnalyzer.GpsDatagramAnalyzer analyzer = new Tiger.GpsAnalyzer.GpsDatagramAnalyzer();
            analyzer.OnAnalyzed += analyzer_OnAnalyzed;

            analyzer.Open(PORT);

            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(60000);
                analyzer.Close();
            });
            task.Wait();
        }

        void analyzer_OnAnalyzed(object sender, Tiger.GpsAnalyzer.GpsDatagramEventArg e)
        {
            
        }
    }
}
