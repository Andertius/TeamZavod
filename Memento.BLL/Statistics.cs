//StatEventArgs (TimeSpan t);
﻿// <copyright file="Statistics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
    /// <summary>
    /// All statistics.
    /// </summary>
    public class Statistics : INotifyPropertyChanged
    {
        private TimeSpan timeSpentToday;
        private TimeSpan averageTimePerDay;
        private List<Card> cardsLearnedToday;

        /// <summary>
        /// Initializes a new instance of the <see cref="Statistics"/> class.
        /// </summary>
        public Statistics()
        {
            TimeSpentToday = default;
            AvarageTimePerDay = default;
            CardsLearnedToday = new List<Card>();
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
        public TimeSpan AvarageTimePerDay
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
        public List<Card> CardsLearnedToday
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
        public void AddCardLearned(object sender, StatCardLearnedEventArgs e)
        {
            CardsLearnedToday.Add(e.LearnedCard);
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
        /// Event that launches when property is changed.
        /// </summary>
        /// <param name="propertyName">property name whish is under check.</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
