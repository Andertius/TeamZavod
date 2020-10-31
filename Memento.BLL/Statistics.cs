using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Memento.DAL;
using System.Linq;

namespace Memento.BLL
{
    //StatEventArgs (TimeSpan t);
    public class Statistics
    {
        public Statistics()
        {
            TimeSpentToday = new TimeSpan();
            AvarageTimePerDay = new TimeSpan();
            CardsLearnedToday = new List<Card>();
        }

        public TimeSpan TimeSpentToday { get; set; }
        public TimeSpan AvarageTimePerDay { get; set; }
        public List<Card> CardsLearnedToday { get; set; }

        public void AddSpentTimeToday(object sender, StatAddSpentTimeEventArgs e)
        {
            TimeSpentToday += e.TimePassed;
        }

        public void AddCardLearned(object sender, StatCardLearnedEventArgs e)
        {
            CardsLearnedToday.Add(e.LearnedCard);
        }

        public void ManageStatistics()
        {
            var filename = "Statistics.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory, filename);

            XElement purchaseOrder = XElement.Load(purchaseOrderFilepath);

            IEnumerable<string> partNos = purchaseOrder.Descendants("Item").Select(x => (string)x);
        }
    }
}
