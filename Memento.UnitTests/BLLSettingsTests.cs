using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Memento.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Memento.UnitTests
{
    [TestClass]
    public class BLLSettingsTests
    {
        private Settings settings;
        private string filePath;

        [TestInitialize]
        public void Initialize()
        {
            settings = new Settings();
            filePath = "SettingsTest.xml";
        }

        [TestMethod]
        [DataRow(5)]
        [DataRow(3.5)]
        [DataRow(13)]
        [DataRow(18.6)]
        [DataRow(0)]
        public void CheckHoursPerDayTest_ReturnsTrue(double hours)
        {
            settings.HoursPerDay = hours;

            bool result = settings.CheckHoursPerDay();

            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow(-4)]
        [DataRow(-3.5)]
        [DataRow(24.8)]
        [DataRow(25)]
        public void CheckHoursPerDayTest_ReturnsFalse(double hours)
        {
            settings.HoursPerDay = hours;

            bool result = settings.CheckHoursPerDay();

            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(3)]
        [DataRow(13)]
        [DataRow(100)]
        [DataRow(500)]
        [DataRow(1000)]
        public void CheckCardsPerDayTest_ReturnsTrue(int cards)
        {
            settings.CardsPerDay = cards;

            bool result = settings.CheckCardsPerDay();

            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow(-5)]
        [DataRow(1001)]
        [DataRow(37248)]
        public void CheckCardsPerDayTest_ReturnsFalse(int cards)
        {
            settings.CardsPerDay = cards;

            bool result = settings.CheckCardsPerDay();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WriteToXMLFileTest_WritesToFile()
        {
            settings = new Settings(5, 20, Theme.Dark, CardOrder.Ascending, false);

            settings.WriteToXMLFile(filePath);

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            string text = doc.DocumentElement.SelectSingleNode("//HoursPerDay").InnerText;
            StringAssert.Equals(text, "5");

            text = doc.DocumentElement.SelectSingleNode("//CardsPerDay").InnerText;
            StringAssert.Equals(text, "20");

            text = doc.DocumentElement.SelectSingleNode("//Theme").InnerText;
            StringAssert.Equals(text, "Dark");

            text = doc.DocumentElement.SelectSingleNode("//CardOrder").InnerText;
            StringAssert.Equals(text, "Ascending");

            text = doc.DocumentElement.SelectSingleNode("//ShowImages").InnerText;
            StringAssert.Equals(text, "false");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("egiufiu.csv")]
        [DataRow("rwnwe")]
        [ExpectedException(typeof(ArgumentException), "Invalid file name")]
        public void WriteToXMLFileTest_ThrowsArgumentException(string filePath)
        {
            settings.WriteToXMLFile(filePath);
        }

        [TestMethod]
        public void ReadFromXMLFileTest_ReadsFromFile()
        {
            XDocument doc = new XDocument(new XElement("Settings",
               new XElement("HoursPerDay", "3.5"),
               new XElement("CardsPerDay", "19"),
               new XElement("Theme", "Dark"),
               new XElement("CardOrder", "Random"),
               new XElement("ShowImages", "true")
               )
            );

            doc.Save(filePath);

            settings.ReadFromXMLFile(filePath);

            Assert.AreEqual(settings.HoursPerDay, 3.5);
            Assert.AreEqual(settings.CardsPerDay, 19);
            Assert.AreEqual(settings.Theme, Theme.Dark);
            Assert.AreEqual(settings.CardOrder, CardOrder.Random);
            Assert.IsTrue(settings.ShowImages);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("efwub.txt")]
        [DataRow("neneokow")]
        [ExpectedException(typeof(ArgumentException), "Invalid file name")]
        public void ReadFromXMLFileTest_ThrowsArgumentException(string filePath)
        {
            settings.ReadFromXMLFile(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException), "No such file")]
        public void ReadFromXMLFileTest_ThrowsDirectoryNotFoundException()
        {
            settings.ReadFromXMLFile("NotExistingFile.xml");
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
