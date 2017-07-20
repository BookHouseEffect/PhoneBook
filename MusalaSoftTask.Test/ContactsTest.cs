using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.IO;
using MusalaSoftTask;

namespace MusalaSoftTask.Test
{
    [TestClass]
    public class ContactsTest
    {
        private string TestData { get; set; }
        private string TestPath { get; set; }
        private int ExpedtedEntriesCount { get; set; }

        [TestInitialize]
        public void InitTest()
        {
            StringBuilder builder = new StringBuilder("");
            /*01*/ builder.Append("TestA +359878123456").Append('\n');
            /*02*/ builder.Append("TestB +359878123456 ").Append('\n');
            /*03*/ builder.Append("tesTC +359 87 8 123 456").Append('\n');
            /****/ builder.Append("testC + 3 5 9 8 7 8 1 2 3 4 5 6 ").Append('\n'); //Invalid. Same name
            /****/ builder.Append("TESTD 35 987 8123 456").Append('\n'); //Invalid format
            /*04*/ builder.Append("testE 0878123456").Append('\n');
            /*05*/ builder.Append("tEsTF 0 87 8123 45 6 ").Append('\n');
            /****/ builder.Append("test test 0 0 87 812 34 56").Append('\n'); //Invalid format
            /*06*/ builder.Append("  test  ing 00359878123456").Append('\n');
            /*07*/ builder.Append("testting   00  359  87  8  123   456   ").Append('\n');
            /*08*/ builder.Append("testJ +359 87 8123456").Append('\n');
            /*09*/ builder.Append("testK +359 88 8123456").Append('\n');
            /*10*/ builder.Append("tEStL +359 89 8123456").Append('\n');
            /****/ builder.Append("tEStM +359 86 8123456").Append('\n'); //Invalid Operator
            /****/ builder.Append("tEStN +35989 0 123456").Append('\n'); //Invalid format
            /****/ builder.Append("tEStO +35989 1 456789").Append('\n'); //Invalid format
            /*11*/ builder.Append("tEStP +35989 2 789012").Append('\n');
            /*12*/ builder.Append("tEStQ +35989 3 345678").Append('\n');
            /*13*/ builder.Append("tEStR +35989 4 901234").Append('\n');
            /*14*/ builder.Append("tEStS +35989 5 567890").Append('\n');
            /*15*/ builder.Append("tEStT +35989 6 123456").Append('\n');
            /*16*/ builder.Append("tEStU +35989 7 111222").Append('\n');
            /*17*/ builder.Append("tEStV +35989 9 333444").Append('\n'); 
            /****/ builder.Append("tEStW +35989 8 55g666").Append('\n'); //Invalid format. Has letters
            /****/ builder.Append("tEStX +359d9 6 123456").Append('\n'); //Invalid format. Has letters
            /****/ builder.Append("+35989 6 123456").Append('\n'); //Invalid format. No name
            /****/ builder.Append("TestY +389878123456").Append('\n'); //Invalid country
            /****/ builder.Append("TestZ 00389878123456").Append('\n'); //Invalid country

            TestData = builder.ToString();
            TestPath = Path.Combine(Environment.CurrentDirectory, "test.data");
            ExpedtedEntriesCount = 17;

            File.WriteAllText(TestPath, TestData);
        }

        [TestMethod]
        public void TestingConstructorAndPrinting()
        {
            BulgarianPhoneBook phoneBook = new BulgarianPhoneBook(TestPath);

            string[] entries = phoneBook.ListWholePhoneBook().Split('\n');
            // (entries.Length - 1) because last printed row is empty row
            Assert.AreEqual(ExpedtedEntriesCount, entries.Length - 1);
        }

        [TestMethod]
        public void TestingNewPair()
        {
            BulgarianPhoneBook phoneBook = new BulgarianPhoneBook(TestPath);

            string name = "Musala Soft";
            string phoneNumber = "+359895112233";

            phoneBook.AddNewPair(name, phoneNumber);

            string[] entries = phoneBook.ListWholePhoneBook().Split('\n');
            Assert.AreEqual(ExpedtedEntriesCount + 1, entries.Length - 1);
        }

        [TestMethod]
        public void DeletingPairByName()
        {
            BulgarianPhoneBook phoneBook = new BulgarianPhoneBook(TestPath);

            string name = "TestA";

            phoneBook.DeletePairByName(name);

            string[] entries = phoneBook.ListWholePhoneBook().Split('\n');
            Assert.AreEqual(ExpedtedEntriesCount - 1, entries.Length - 1);
        }

        [TestMethod]
        public void GettingPhoneNumber()
        {
            BulgarianPhoneBook phoneBook = new BulgarianPhoneBook(TestPath);

            string name = "TestA";

            string phoneNumber = phoneBook.GetPhoneNumberByName(name);

            Assert.AreEqual("+359878123456", phoneNumber);
        }

        [TestCleanup]
        public void CleanEnvironment()
        {
            if (File.Exists(TestPath))
                File.Delete(TestPath);
        }
    }
}
