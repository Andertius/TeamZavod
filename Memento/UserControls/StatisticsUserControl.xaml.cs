// <copyright file="StatisticsUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private static readonly DependencyProperty AppStatisticsProperty = DependencyProperty.Register(
            nameof(AppStats),
            typeof(Statistics),
            typeof(StatisticsUserControl),
            new PropertyMetadata(new Statistics()));

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsUserControl"/> class.
        /// </summary>
        /// <param name="stats">statistics object.</param>
        /// <param name="settings">settings object.</param>
        public StatisticsUserControl(Statistics stats, Settings settings)
        {
            InitializeComponent();
            DataContext = this;

            AppStats = stats;
            TTSProgress.Maximum = settings.HoursPerDay;
            CardsLearned.Maximum = settings.CardsPerDay;
            GoBackButton.Focus();
        }

        /// <summary>
        /// Event handler for making page visible.
        /// </summary>
        public event EventHandler MakeMainPageVisible;

        /// <summary>
        /// Gets event handler for time addition.
        /// </summary>
        // public event EventHandler<StatAddSpentTimeEventArgs> TimeAdd;

        /// <summary>
        /// Gets time.
        /// </summary>
        // public DispatcherTimer Timer { get; }

        /// <summary>
        /// Gets sets AppStatistic value.
        /// </summary>
        public Statistics AppStats
        {
            get => (Statistics)GetValue(AppStatisticsProperty);
            private set => SetValue(AppStatisticsProperty, value);
        }

        private void GoBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }
    }
}
