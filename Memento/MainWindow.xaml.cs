﻿// <copyright file="MainWindow.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Serialization;

using Memento.BLL;
using Memento.UserControls;

namespace Memento
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly DependencyProperty AppStatisticsProperty = DependencyProperty.Register(
            nameof(AppStatistics),
            typeof(Statistics),
            typeof(StatisticsUserControl),
            new PropertyMetadata(new Statistics()));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            StreamReader reader = new StreamReader("Settings.xml");
            AppSettings = (Settings)deserializer.Deserialize(reader);
            reader.Close();

            Content = MainPage = new MainPageUserControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            MainPage.StartEditingEvent += StartEditing;
            MainPage.OpenSettingsEvent += OpenSettings;
            MainPage.OpenStatisticsEvent += OpenStatistics;
            MainPage.OpenLearningEvent += StartLearning;

            AppStatistics = new Statistics();
            AppStatistics.GetFromXML();

            this.Timer = new DispatcherTimer();
            this.Timer.Tick += this.UpdatePage;
            this.TimeAdd += this.AppStatistics.AddSpentTimeToday;

            IsInEditor = false;
            IsInLearningProcess = false;

            this.Timer.Interval = new TimeSpan(0, 0, 5);
            this.Timer.Start();
        }

        /// <summary>
        /// An event that handles time incrementing.
        /// </summary>
        public event EventHandler<StatAddSpentTimeEventArgs> TimeAdd;

        /// <summary>
        /// Gets or sets AppSettings.
        /// </summary>
        public static Settings AppSettings { get; set; }

        /// <summary>
        /// Gets sets AppStatistic value.
        /// </summary>
        public Statistics AppStatistics
        {
            get => (Statistics)this.GetValue(AppStatisticsProperty);
            private set => this.SetValue(AppStatisticsProperty, value);
        }

        /// <summary>
        /// Gets Timer.
        /// </summary>
        public DispatcherTimer Timer { get; }

        /// <summary>
        /// Gets a value indicating whether IsInEditor.
        /// </summary>
        public bool IsInEditor { get; private set; }

        /// <summary>
        /// Gets a value indicating whether IsInLearningProcess.
        /// </summary>
        public bool IsInLearningProcess { get; private set; }

        /// <summary>
        /// Gets LearningProcess.
        /// </summary>
        public AppHandler LearningProcess { get; private set; }

        /// <summary>
        /// Gets or sets DeckEditor.
        /// </summary>
        public DeckEditor DeckEditor { get; set; }

        /// <summary>
        /// Gets or sets MainPage.
        /// </summary>
        public MainPageUserControl MainPage { get; set; }

        /// <summary>
        /// Gets or sets DeckEditorPage.
        /// </summary>
        public DeckEditorUserControl DeckEditorPage { get; set; }

        /// <summary>
        /// Gets or sets SettingsPage.
        /// </summary>
        public SettingsUserControl SettingsPage { get; set; }

        /// <summary>
        /// Gets or sets StatisticsPage.
        /// </summary>
        public StatisticsUserControl StatisticsPage { get; set; }

        /// <summary>
        /// Gets or sets LearningPage.
        /// </summary>
        public LearningUserControl LearningPage { get; set; }

        private void StartLearning(object sender, StartLearningEventArgs e)
        {
            if (LearningProcess is null)
            {
                LearningProcess = new AppHandler(e.DeckId);
            }

            Content = LearningPage = new LearningUserControl(e.DeckId, this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            Title = $"Memento - {LearningProcess.Deck.DeckName}";
            LearningPage.MakeMainPageVisible += GoToMainPageFromLearning;
            IsInLearningProcess = true;
        }

        private void GoToMainPageFromLearning(object sender, EventArgs e)
        {
            LearningPage.MakeMainPageVisible -= GoToMainPageFromLearning;
            Content = MainPage;
            Title = "Memento";
        }

        private void StartEditing(object sender, StartEditingEventArgs e)
        {
            Content = DeckEditorPage = new DeckEditorUserControl(e.DeckId)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            DeckEditorPage.MakeMainPageVisible += GoToMainPageFromDeckEditor;
            DeckEditorPage.TitleChanged += ChangeMainTitle;
            MainPage.Decks = DeckEditorPage.DeckEditor.AllDecks.ToList();

            if (e.DeckId == -1)
            {
                Title = "Memento - Deck Editor -";
            }
            else
            {
                Title = $"Memento - Deck Editor - {DeckEditorPage.DeckEditor.Deck.DeckName}";
            }
        }

        private void GoToMainPageFromDeckEditor(object sender, EventArgs e)
        {
            DeckEditorPage.MakeMainPageVisible -= GoToMainPageFromDeckEditor;
            DeckEditorPage.TitleChanged -= ChangeMainTitle;
            Content = MainPage;
            Title = "Memento";
        }

        private void ChangeMainTitle(object sender, ChangeTitleEventArgs e)
        {
            Title = e.Title;
        }

        private void UpdatePage(object source, EventArgs e)
        {
            // TTSslider.Value = AppStatistics.TimeSpentToday.TotalHours;
            this.TimeAdd?.Invoke(this, new StatAddSpentTimeEventArgs(new TimeSpan(0, 0, 5)));
            AppStatistics.WriteInXML();
            // TodayTimeSpent.Text = Convert.ToString(Math.Round(todaytimespent, 2));
        }

        private void OpenSettings(object sender, EventArgs e)
        {
            Content = SettingsPage = new SettingsUserControl(AppSettings)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            SettingsPage.MakeMainPageVisible += GoToMainPageFromSettings;
            Title = "Memento - Settings";
        }

        private void GoToMainPageFromSettings(object sender, EventArgs e)
        {
            SettingsPage.MakeMainPageVisible -= GoToMainPageFromSettings;
            Content = MainPage;
            MainPage.ChangeTheme();
            Title = "Memento";
        }

        private void OpenStatistics(object sender, EventArgs e)
        {
            if (AppStatistics is null)
            {
                AppStatistics = new Statistics();
            }

            if (SettingsPage is null)
            {
                Content = StatisticsPage = new StatisticsUserControl(AppStatistics, AppSettings)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
            }
            else
            {
                Content = StatisticsPage = new StatisticsUserControl(AppStatistics, SettingsPage.AppSettings)
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                };
            }

            StatisticsPage.MakeMainPageVisible += GoToMainPageFromStatistics;
            Title = "Memento - Statistics";
        }

        private void GoToMainPageFromStatistics(object sender, EventArgs e)
        {
            StatisticsPage.MakeMainPageVisible -= GoToMainPageFromStatistics;
            Content = MainPage;
            Title = "Memento";
        }
    }
}
