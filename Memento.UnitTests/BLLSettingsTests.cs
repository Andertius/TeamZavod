using System.IO;

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
