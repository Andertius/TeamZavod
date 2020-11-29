using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Memento.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Memento.UnitTests
{
    [TestClass]
    public class BLLStatisticsTests
    {
        private Statistics stats;

        [TestInitialize]
        public void Initialize()
        {
            stats = new Statistics();
        }

        [TestMethod]
        public void GetFromXMLTest_GetsDataFromXML()
        {
            string stat = "Stats";
            string filepath = Path.GetFullPath($"{stat}.xml");
            string statPath = $"{stat}.xml";

            XDocument xdoc;

            xdoc = new XDocument(new XElement($"{stat}",
                new XElement("GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")
                ),

               new XElement("SecondsToday", "0"),
               new XElement("AverageSecondsToday", "0"),
               new XElement("CardsToday", "1"),
               new XElement("Day", DateTime.Now.Day.ToString()),
               new XElement("CheckFirst", "0")
               )
            );

            xdoc.Save(statPath);

            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "1");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "0");

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        [TestMethod]
        public void GetFromXMLTest_CheckFirstEntry_GetsDataFromXML()
        {
            string stat = "Stats";
            string filepath = Path.GetFullPath($"{stat}.xml");
            string statPath = $"{stat}.xml";

            XDocument xdoc;

            xdoc = new XDocument(new XElement($"{stat}",
                new XElement("GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")
                ),

               new XElement("SecondsToday", "0"),
               new XElement("AverageSecondsToday", "0"),
               new XElement("CardsToday", "1"),
               new XElement("Day", (DateTime.Now.Day - 1).ToString()),
               new XElement("CheckFirst", "1")
               )
            );

            xdoc.Save(statPath);

            stats.GetFromXML(stat);

            xdoc = XDocument.Load(filepath);

            Assert.AreEqual(xdoc.Descendants(stat).First().Element("CheckFirst").Value.ToString(), "0");

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        [TestMethod]
        public void GetFromXMLTest_NewDay_GetsDataFromXML()
        {
            string stat = "Stats";
            string filepath = Path.GetFullPath($"{stat}.xml");
            string statPath = $"{stat}.xml";

            XDocument xdoc;

            xdoc = new XDocument(new XElement($"{stat}",
                new XElement("GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")
                ),

               new XElement("SecondsToday", "0"),
               new XElement("AverageSecondsToday", "0"),
               new XElement("CardsToday", "1"),
               new XElement("Day", (DateTime.Now.Day - 1).ToString()),
               new XElement("CheckFirst", "0")
               )
            );

            xdoc.Save(statPath);

            stats.GetFromXML(stat);

            xdoc = XDocument.Load(filepath);

            Assert.AreEqual(xdoc.Descendants(stat).First().Element("Day").Value.ToString(), DateTime.Now.Day.ToString());

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        [TestMethod]
        public void WriteInXMLTest_WritesInXML()
        {
            stats = new Statistics();

            stats.TimeSpentToday = new TimeSpan(0, 0, 2);
            stats.CardsLearnedToday = 2;

            string stat = "Stats";
            string statPath = $"{stat}.xml";
            string filepath = Path.GetFullPath($"{stat}.xml");

            stats.WriteInXML(stat);
            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "2");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "4");

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        [TestMethod]
        public void GetFromXMLTest_NoSuchXML_GetsDataFromXML()
        {
            string stat = "Statis";
            string filepath = Path.GetFullPath($"{stat}.xml");
            string statPath = $"{stat}.xml";

            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "0");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "0");

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }

        [TestMethod]
        public void WriteInXMLTest_NoSuchXMLFile_WritesInXML()
        {
            stats = new Statistics();

            stats.TimeSpentToday = new TimeSpan(0, 0, 2);
            stats.CardsLearnedToday = 2;

            string stat = "Statts";
            string statPath = $"{stat}.xml";
            string filepath = Path.GetFullPath($"{stat}.xml");

            stats.WriteInXML(stat);
            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "2");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "4");

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
