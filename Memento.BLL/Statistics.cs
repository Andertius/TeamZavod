namespace Memento.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Linq;
    using Memento.DAL;

    //StatEventArgs (TimeSpan t);
    public class Statistics : INotifyPropertyChanged
    {
        private TimeSpan timeSpentToday;
        private TimeSpan averageTimePerDay;
        private List<Card> cardsLearnedToday;

        public Statistics()
        {
            this.TimeSpentToday = new TimeSpan();
            this.AvarageTimePerDay = new TimeSpan();
            this.CardsLearnedToday = new List<Card>();
        }

        public TimeSpan TimeSpentToday 
        {
            get => timeSpentToday; 
            set
            {
                timeSpentToday = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan AvarageTimePerDay 
        { 
            get => averageTimePerDay; 
            set
            {
                averageTimePerDay = value;
                OnPropertyChanged();
            }
        }

        public List<Card> CardsLearnedToday 
        { 
            get => cardsLearnedToday; 
            set
            {
                cardsLearnedToday = value;
                OnPropertyChanged();
            }
        }

        public void AddSpentTimeToday(object sender, StatAddSpentTimeEventArgs e)
        {
            this.TimeSpentToday = TimeSpentToday.Add(e.TimePassed);
        }

        public void AddCardLearned(object sender, StatCardLearnedEventArgs e)
        {
            this.CardsLearnedToday.Add(e.LearnedCard);
        }

        public void ManageStatistics()
        {
            var filename = "Statistics.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory, filename);

            XElement purchaseOrder = XElement.Load(purchaseOrderFilepath);

            IEnumerable<string> partNos = purchaseOrder.Descendants("Item").Select(x => (string)x);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
