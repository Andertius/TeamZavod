// <copyright file="Statistics.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

using Memento.DAL;

namespace Memento.BLL
{
    // StatEventArgs (TimeSpan t);

    /// <summary>
    /// All statistics.
    /// </summary>
    public class Statistics : INotifyPropertyChanged
    {
        private TimeSpan timeSpentToday;
        private TimeSpan averageTimePerDay;
        private int cardsLearnedToday;

        /// <summary>
        /// Initializes a new instance of the <see cref="Statistics"/> class.
        /// </summary>
        public Statistics()
        {
            this.TimeSpentToday = default;
            this.AverageTime = default;
            this.CardsLearnedToday = default;
        }

        /// <summary>
        /// Property changed event handler.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets property that shows how much time was spent today.
        /// </summary>
        public TimeSpan TimeSpentToday
        {
            get => this.timeSpentToday;
            set
            {
                this.timeSpentToday = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets property that shows average time spent per day today.
        /// </summary>
        public TimeSpan AverageTime
        {
            get => this.averageTimePerDay;
            set
            {
                this.averageTimePerDay = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets property that shows how many cards were learned today.
        /// </summary>
        public int CardsLearnedToday
        {
            get => this.cardsLearnedToday;
            set
            {
                this.cardsLearnedToday = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Adds time to time counter.
        /// </summary>
        /// <param name="sender">time counter.</param>
        /// <param name="e">event args for event.</param>
        public void AddSpentTimeToday(object sender, StatAddSpentTimeEventArgs e)
        {
            this.TimeSpentToday = this.TimeSpentToday.Add(e.TimePassed);
        }

        /// <summary>
        /// Add cards tocards learned list.
        /// </summary>
        /// <param name="sender">cards list.</param>
        /// <param name="e">event args for event.</param>
        public void AddCardLearned(object sender, StatCardLearnedEventArgs e)
        {
            this.CardsLearnedToday += 1;
        }

        /// <summary>
        /// Gets statistic.
        /// </summary>
        public void ManageStatistics()
        {
            var filename = "Statistics.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory, filename);

            XElement purchaseOrder = XElement.Load(purchaseOrderFilepath);

            IEnumerable<string> partNos = purchaseOrder.Descendants("Item").Select(x => (string)x);
        }

        /// <summary>
        /// fills the data from xml.
        /// </summary>
        public void GetFromXML()
        {
            string pathtext = Path.GetFullPath("TimeSpent.txt");
            List<string> timeSpentPerDay = File.ReadAllLines(pathtext).ToList();

            XDocument xdoc;

            string filepath = Path.GetFullPath("Statistics.xml");

            if (System.IO.File.Exists(filepath))
            {
                xdoc = XDocument.Load(filepath);
            }
            else
            {
                xdoc = new XDocument(new XElement("Stistics"));
            }

            XElement member = xdoc
                .Descendants("Statistics").First();

            DateTime localDate = DateTime.Now;

            if (Convert.ToString(localDate.Day) != member.Element("Day").Value)
            {
                if (timeSpentPerDay.Count <= 100)
                {
                    timeSpentPerDay.Add(member.Element("SecondsToday").Value);
                }
                else
                {
                    for (int i = 0; i < timeSpentPerDay.Count; i++)
                    {
                        if (i == timeSpentPerDay.Count - 1)
                        {
                            timeSpentPerDay[i] = member.Element("SecondsToday").Value;
                        }

                        timeSpentPerDay[i] = timeSpentPerDay[i + 1];
                    }
                }

                File.WriteAllLines(pathtext, timeSpentPerDay);

                var forAverageCount = timeSpentPerDay.Select(int.Parse).ToList();

                member.Element("SecondsToday").Value = "0";
                member.Element("CardsToday").Value = "0";
                member.Element("AverageSecondsToday").Value = Convert.ToString(forAverageCount.Average());
                member.Element("Day").Value = localDate.Day.ToString();

                if (member.Element("CheckFirst").Value == "1")
                {
                    member.Element("CheckFirst").Value = "0";
                    member.Element("GeneralInfo").Element("FirstLogin").Value = localDate.ToString();
                }

                member.Element("GeneralInfo").Element("LastLogin").Value = localDate.ToString();

                xdoc.Save(filepath);
            }

            if (member != null)
            {
                TimeSpentToday = TimeSpentToday.Add(new TimeSpan(0, 0, Convert.ToInt32(member.Element("SecondsToday").Value)));
                CardsLearnedToday = Convert.ToInt32(member.Element("CardsToday").Value);

                Double.TryParse(member.Element("AverageSecondsToday").Value, out double number);

                AverageTime = new TimeSpan(0, 0, Convert.ToInt32(number));
            }
            else
            {
                // XElement newMember = new XElement("Statistics",
                //    new XElement("HoursToday", "0"),
                //    new XElement("AverageHoursToday", "0"),
                //    new XElement("CardsToday", "0"),
                //    new XElement("Day", "1"),
                //    );

                // Xdoc.Descendants("Statistics").First().Add(newMember);
            }

            // xdoc.Save(filepath);
        }

        /// <summary>
        /// writes updated data in xml.
        /// </summary>
        public void WriteInXML()
        {
            XDocument xdoc;

            string filepath = Path.GetFullPath("Statistics.xml");

            if (System.IO.File.Exists(filepath))
            {
                xdoc = XDocument.Load(filepath);
            }
            else
            {
                xdoc = new XDocument(new XElement("Statistics"));
            }

            XElement member = xdoc
                .Descendants("Statistics").First();

            DateTime localDate = DateTime.Now;

            if (member != null)
            {
                member.Element("SecondsToday").Value = Convert.ToString(TimeSpentToday.TotalSeconds);
                member.Element("CardsToday").Value = Convert.ToString(CardsLearnedToday);
                // member.Element("AverageHoursToday").Value = Convert.ToString(AverageTime.TotalSeconds);
                member.Element("Day").Value = Convert.ToString(localDate.Day);
                member.Element("GeneralInfo").Element("LastLogin").Value = localDate.ToString();

                var time = Convert.ToDouble(member.Element("GeneralInfo").Element("TotalHours").Value);

                time += 5.0 / 3600;
                member.Element("GeneralInfo").Element("TotalHours").Value = Convert.ToString(time);
            }
            else
            {
                // XElement newMember = new XElement("Statistics",
                //    new XElement("HoursToday", "0"),
                //    new XElement("AverageHoursToday", "0"),
                //    new XElement("CardsToday", "0"),
                //    new XElement("Day", "1"),
                //    );

                // Xdoc.Descendants("Statistics").First().Add(newMember);
            }

            xdoc.Save(filepath);
        }

        /// <summary>
        /// Event that launches when property is changed.
        /// </summary>
        /// <param name="propertyName">property name whish is under check.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
