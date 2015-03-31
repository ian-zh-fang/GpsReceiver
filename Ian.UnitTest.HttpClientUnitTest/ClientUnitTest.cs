using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ian.HttpClient;
using Ian.UnitTest.Model;

namespace Ian.UnitTest.HttpClientUnitTest
{
    [TestClass]
    public class ClientUnitTest
    {
        private static readonly string CONTROLLERNAME = "TestHandler";
        private static readonly SimpleClient CLIENT = new SimpleClient();
        private static readonly ParamCollection PARAMCOLLECTION = new ParamCollection();

        [TestMethod]
        public void RequestTest1()
        {
            DateTime now = DateTime.Now;
            PARAMCOLLECTION.Clear();
            PARAMCOLLECTION.Add("time", now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            SimpleClient.ExecuteResult<DateTime> result = CLIENT.Get<DateTime>("Action1", CONTROLLERNAME, PARAMCOLLECTION);
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(result.Status, 0);
            //Assert.AreEqual(now.Ticks, result.Result.Ticks);
        }

        [TestMethod]
        public void RequestTest2()
        {
            DateTime now = DateTime.Now;
            SimpleClient.ExecuteResult<DateTime> result = CLIENT.Get<DateTime>("Action2", CONTROLLERNAME, null);
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(result.Status, 0);
            //Assert.AreNotEqual(now.Ticks, result.Result.Ticks);
        }

        [TestMethod]
        public void RequestTest3()
        {
            TestModel t = new TestModel()
            {
                Msg = "this message is from client.",
                Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0),
                Time = DateTime.Now
            };
            SimpleClient.ExecuteResult<TestModel> result = CLIENT.Get<TestModel, TestModel>("Action3", CONTROLLERNAME, t);
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(result.Status, 0);
            Assert.AreEqual(result.Result.Msg, "this message is from service.");
            Assert.AreEqual(result.Result.Id, t.Id);
            Assert.AreNotEqual(result.Result.Msg, t.Msg);
            //Assert.AreEqual(result.Result.Time.Ticks, t.Time.Ticks);
        }

        [TestMethod]
        public void RequestTest4()
        {
            TestModel t = new TestModel()
            {
                Msg = "this message is from client.",
                Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0),
                Time = DateTime.Now
            };
            SimpleClient.ExecuteResult<TestModel> result = CLIENT.Post<TestModel, TestModel>("Action4", CONTROLLERNAME, t);
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(result.Status, 0);
            Assert.AreEqual(result.Result.Msg, "this message is from service.");
            Assert.AreEqual(result.Result.Id, t.Id);
            Assert.AreNotEqual(result.Result.Msg, t.Msg);
            //Assert.AreNotEqual(result.Result.Time.Ticks, t.Time.Ticks);
        }

        [TestMethod]
        public void RequestTest5()
        {
            TestModel t = new TestModel()
            {
                Msg = "this message is from client.",
                Id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0),
                Time = DateTime.Now
            };
            SimpleClient.ExecuteResult<TestModel> result = CLIENT.Get<TestModel, TestModel>("Action5", CONTROLLERNAME, t);
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(result.Status, 0);
            Assert.AreEqual(result.Result.Msg, "this message is from service.");
            Assert.AreNotEqual(result.Result.Id, t.Id);
            Assert.AreNotEqual(result.Result.Msg, t.Msg);
            //Assert.AreNotEqual(result.Result.Time.Ticks, t.Time.Ticks);
        }
    }
}
