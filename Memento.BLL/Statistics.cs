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

namespace Memento.BLL
{
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
            TimeSpentToday = default;
            AverageTime = default;
            CardsLearnedToday = default;
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
            get => timeSpentToday;
            set
            {
                timeSpentToday = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets property that shows average time spent per day today.
        /// </summary>
        public TimeSpan AverageTime
        {
            get => averageTimePerDay;
            set
            {
                averageTimePerDay = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets property that shows how many cards were learned today.
        /// </summary>
        public int CardsLearnedToday
        {
            get => cardsLearnedToday;
            set
            {
                cardsLearnedToday = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Adds time to time counter.
        /// </summary>
        /// <param name="sender">time counter.</param>
        /// <param name="e">event args for event.</param>
        public void AddSpentTimeToday(object sender, StatAddSpentTimeEventArgs e)
        {
            TimeSpentToday = TimeSpentToday.Add(e.TimePassed);
        }

        /// <summary>
        /// Add cards tocards learned list.
        /// </summary>
        /// <param name="sender">cards list.</param>
        /// <param name="e">event args for event.</param>
        public void AddCardLearned()
        {
            CardsLearnedToday++;
        }

        /// <summary>
        /// Gets data from XML.
        /// </summary>
        /// <param name="stat">XML file param.</param>
        public void GetFromXML(string stat)
        {
            string pathtext = Path.GetFullPath("TimeSpent.txt");
            List<string> timeSpentPerDay = File.ReadAllLines(pathtext).ToList();

            XDocument xdoc;

            // string stat = "Statistics";
            string filepath = Path.GetFullPath($"{stat}.xml");

            if (System.IO.File.Exists(filepath))
            {
                xdoc = XDocument.Load(filepath);
            }
            else
            {
                xdoc = new XDocument(new XElement(
                    $"{stat}",
                    new XElement(
                    "GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")),
                    new XElement("SecondsToday", "0"),
                    new XElement("AverageSecondsToday", "0"),
                    new XElement("CardsToday", "0"),
                    new XElement("Day", DateTime.Now.Day.ToString()),
                    new XElement("CheckFirst", "0")));

                xdoc.Save(filepath);

                xdoc = XDocument.Load(filepath);
            }

            XElement member = xdoc
                .Descendants(stat).First();

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

                var forAverageCount = timeSpentPerDay.Select(Int32.Parse).ToList();

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

            // xdoc.Save(filepath);
        }

        /// <summary>
        /// Writes data in XML.
        /// </summary>
        /// <param name="stat">XML name.</param>
        public void WriteInXML(string stat)
        {
            XDocument xdoc;

            // string stat = "Statistics";
            string filepath = Path.GetFullPath($"{stat}.xml");

            if (File.Exists(filepath))
            {
                xdoc = XDocument.Load(filepath);
            }
            else
            {
                xdoc = new XDocument(new XElement(
                    $"{stat}",
                    new XElement(
                    "GeneralInfo",
                    new XElement("FirstLogin", "0"),
                    new XElement("LastLogin", "0"),
                    new XElement("TotalHours", "0")),
                    new XElement("SecondsToday", "0"),
                    new XElement("AverageSecondsToday", "0"),
                    new XElement("CardsToday", "0"),
                    new XElement("Day", DateTime.Now.Day.ToString()),
                    new XElement("CheckFirst", "0")));

                xdoc.Save(filepath);

                xdoc = XDocument.Load(filepath);
            }

            XElement member = xdoc
                .Descendants($"{stat}").First();

            DateTime localDate = DateTime.Now;

            if (member != null)
            {
                member.Element("SecondsToday").Value = Convert.ToString(TimeSpentToday.TotalSeconds);
                member.Element("CardsToday").Value = Convert.ToString(CardsLearnedToday);

                member.Element("Day").Value = Convert.ToString(localDate.Day);
                member.Element("GeneralInfo").Element("LastLogin").Value = localDate.ToString();

                var time = Convert.ToDouble(member.Element("GeneralInfo").Element("TotalHours").Value);

                time += 5.0 / 3600;
                member.Element("GeneralInfo").Element("TotalHours").Value = Convert.ToString(time);
            }

            xdoc.Save(filepath);
        }

        /// <summary>
        /// Event that launches when property is changed.
        /// </summary>
        /// <param name="propertyName">property name whish is under check.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
