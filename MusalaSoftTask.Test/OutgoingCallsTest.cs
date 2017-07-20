using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.IO;

namespace MusalaSoftTask.Test
{
    [TestClass]
    public class OutgoingCallsTest
    {
        private string TestData { get; set; }
        private string TestPath { get; set; }

        private BulgarianPhoneBook phoneBook;

        [TestInitialize]
        public void InitializeTest()
        {
            StringBuilder builder = new StringBuilder("");
            builder.Append("TestA +359 87 2 000000").Append('\n');
            builder.Append("TestB +359 87 3 111111").Append('\n'); 
            builder.Append("TestC +359 87 4 222222").Append('\n');
            builder.Append("TestD +359 88 4 333333").Append('\n');
            builder.Append("TestE +359 88 5 444444").Append('\n');
            builder.Append("TestF +359 88 6 555555").Append('\n');
            builder.Append("TestG +359 88 7 666666").Append('\n');
            builder.Append("TestH +359 89 7 777777").Append('\n');
            builder.Append("TestI +359 89 8 888888").Append('\n');
            builder.Append("TestJ +359 89 9 999999").Append('\n');

            TestData = builder.ToString();
            TestPath = Path.Combine(Environment.CurrentDirectory, "test.data");

            File.WriteAllText(TestPath, TestData);

            phoneBook = new BulgarianPhoneBook(TestPath);
        }

        [TestMethod]
        public void AddOutgoingCall()
        {
            string name = "TestA";
            phoneBook.AddOutgoingCall(name);

            string[] listOfOutgoingCalls = phoneBook.ListTopCalledCalls().Split('\n');

            Assert.AreEqual(true, listOfOutgoingCalls[0].ToUpper().Contains(name.ToUpper()));
        }

        [TestMethod]
        public void TestingSorting()
        {
            string[] names = {
                "TestG", //1
                "TestD", //2
                "TestJ", //3
                "TestB"  //4
            };

            for (int i=0; i<names.Length; i++)
            {
                for (int j = 0; j < i + 1; j++)
                    phoneBook.AddOutgoingCall(names[i]);
            }

            string[] listOfOutgoingCalls = phoneBook.ListTopCalledCalls().Split('\n');

            Assert.AreEqual(5, listOfOutgoingCalls.Length - 1);
                   
            Assert.AreEqual(true, listOfOutgoingCalls[0].ToUpper().Contains(names[3].ToUpper()));
            Assert.AreEqual(true, listOfOutgoingCalls[1].ToUpper().Contains(names[2].ToUpper()));
            Assert.AreEqual(true, listOfOutgoingCalls[2].ToUpper().Contains(names[1].ToUpper()));
            Assert.AreEqual(true, listOfOutgoingCalls[3].ToUpper().Contains(names[0].ToUpper()));
            Assert.AreEqual(true, listOfOutgoingCalls[4].ToUpper().Contains("TestA".ToUpper()));
        }

        [TestCleanup]
        public void CleanEnvironment()
        {
            if (File.Exists(TestPath))
                File.Delete(TestPath);
        }
    }
}
