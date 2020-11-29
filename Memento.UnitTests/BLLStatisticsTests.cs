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
        public void GetFromXML_Test()
        {
            string result = Path.GetTempPath();
            string stat = "Stats";
            string statPath = result + $"{stat}.xml";

            XDocument xdoc;

            xdoc = new XDocument(new XElement("Stats",
                new XElement("GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")
                ),

               new XElement("SecondsToday", "122"),
               new XElement("AverageSecondsToday", "122"),
               new XElement("CardsToday", "2"),
               new XElement("Day", DateTime.Now.Day.ToString()),
               new XElement("CheckFirst", "0")
               )
            );

            xdoc.Save(statPath);

            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "0");
            // Assert.AreEqual(stats.AverageTime, "");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "0");

            if (File.Exists(statPath))
            {
                File.Delete(statPath);
            }

        }

        [TestMethod]
        public void WriteToXML_Test()
        {
            string result = Path.GetTempPath();
            string stat = "Statistics";
            string statPath = result + $"{stat}.xml";

            XDocument xdoc;

            xdoc = new XDocument(new XElement("Statistics",
                new XElement("GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")
                ),

               new XElement("SecondsToday", "122"),
               new XElement("AverageSecondsToday", "122"),
               new XElement("CardsToday", "2"),
               new XElement("Day", DateTime.Now.Day.ToString()),
               new XElement("CheckFirst", "0")
               )
            );

            xdoc.Save(statPath);

            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "0");
            // Assert.AreEqual(stats.AverageTime, "");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "0");

            if (File.Exists(statPath))
            {
                File.Delete(statPath);
            }

        }
    }
}
