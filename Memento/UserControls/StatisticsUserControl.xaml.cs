// <copyright file="StatisticsUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using Memento.BLL;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for Statistics.xaml.
    /// </summary>
    public partial class StatisticsUserControl : UserControl
    {
        /// <summary>
        /// property for statistics.
        /// </summary>
        private static readonly DependencyProperty AppStatisticsProperty = DependencyProperty.Register(nameof(AppStats), typeof(Statistics), typeof(StatisticsUserControl), new PropertyMetadata(new Statistics()));

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsUserControl"/> class.
        /// </summary>
        /// <param name="stats">statistics object.</param>
        /// <param name="settings">settings object.</param>
        public StatisticsUserControl(Statistics stats, Settings settings)
        {
            this.InitializeComponent();
            this.DataContext = this;

            if (MainWindow.AppSettings.Theme == Theme.Dark)
            {
                StatisticsGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2c303a");
            }

            this.AppStats = stats;

            //this.Timer = new DispatcherTimer();
            //this.Timer.Tick += this.UpdatePage;

            //this.TimeAdd += this.AppStats.AddSpentTimeToday;
            this.TTSProgress.Maximum = settings.HoursPerDay;
            this.CardsLearned.Maximum = settings.CardsPerDay;
            this.CardsSlider.Value = AppStats.CardsLearnedToday;

            // TTSslider.Value = todayTimeSpent;
            // CardLearned.Text = Convert.ToString(cardsperday);
            // TodayTimeSpent.Text = Convert.ToString(Math.Round(todayspent, 2));
            // this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            // SecondsText.Text = Convert.ToString(AppStatistics.TimeSpentToday.TotalSeconds);
            //this.Timer.Interval = new TimeSpan(0, 0, 5);
            //this.Timer.Start();
            this.GoBackButton.Focus();
        }

        /// <summary>
        /// Event handler for making page visible.
        /// </summary>
        public event EventHandler MakeMainPageVisible;

        /// <summary>
        /// Gets event handler for time addition.
        /// </summary>
        //public event EventHandler<StatAddSpentTimeEventArgs> TimeAdd;

        /// <summary>
        /// Gets time.
        /// </summary>
        //public DispatcherTimer Timer { get; }

        /// <summary>
        /// Gets sets AppStatistic value.
        /// </summary>
        public Statistics AppStats
        {
            get => (Statistics)this.GetValue(AppStatisticsProperty);
            private set => this.SetValue(AppStatisticsProperty, value);
        }

        //private void UpdatePage(object source, EventArgs e)
        //{
        //    // TTSslider.Value = AppStatistics.TimeSpentToday.TotalHours;
        //    this.TimeAdd?.Invoke(this, new StatAddSpentTimeEventArgs(new TimeSpan(0, 0, 5)));

        //    // TodayTimeSpent.Text = Convert.ToString(Math.Round(todaytimespent, 2));
        //}

        private void GoBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }

        // private void HandleEsc(object sender, KeyEventArgs e)
        // {
        //    if (e.Key == Key.Escape)
        //        Window.GetWindow(this).Close();
        // }
    }
}
