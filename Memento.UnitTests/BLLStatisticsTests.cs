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
        public void WriteInXML_Test()
        {
            stats = new Statistics();

            stats.TimeSpentToday = new TimeSpan(0, 0, 2);
            stats.CardsLearnedToday = 2;

            string stat = "Statistics";
            string statPath = $"{stat}.xml";

            stats.WriteInXML(stat);
            stats.GetFromXML(stat);

            Assert.AreEqual(stats.CardsLearnedToday.ToString(), "2");
            Assert.AreEqual(stats.TimeSpentToday.TotalSeconds.ToString(), "4");
        }
    }
}
