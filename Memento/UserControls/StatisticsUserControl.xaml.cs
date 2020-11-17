using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Memento.BLL;
using System.Diagnostics;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class StatisticsUserControl : UserControl
    {
        public StatisticsUserControl(Statistics statistics, Settings settings)
        {
            InitializeComponent();
            DataContext = this;

            AppStatistics = statistics;

            Timer = new DispatcherTimer();
            Timer.Tick += UpdatePage;

            TimeAdd += AppStatistics.AddSpentTimeToday;
            TTSProgress.Maximum = settings.HoursPerDay;
            CardsLearned.Maximum = settings.CardsPerDay;
            CardsSlider.Value = statistics.CardsLearnedToday.Count;
            //TTSslider.Value = todayTimeSpent;
            //CardLearned.Text = Convert.ToString(cardsperday);
            //TodayTimeSpent.Text = Convert.ToString(Math.Round(todayspent, 2));
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            SecondsText.Text = Convert.ToString(AppStatistics.TimeSpentToday.TotalSeconds);
            Timer.Interval = new TimeSpan(0, 0, 5);
            Timer.Start();
            GoBackButton.Focus();
            
        }

        static private readonly DependencyProperty AppStatisticsProperty = DependencyProperty.Register(nameof(AppStatistics), typeof(Statistics),
            typeof(StatisticsUserControl), new PropertyMetadata(new Statistics()));


        public DispatcherTimer Timer { get; }

        public Statistics AppStatistics
        {
            get => (Statistics)GetValue(AppStatisticsProperty);
            private set => SetValue(AppStatisticsProperty, value);
        }

        private void UpdatePage(object source, EventArgs e)
        {
            //TTSslider.Value = AppStatistics.TimeSpentToday.TotalHours;
            TimeAdd?.Invoke(this, new StatAddSpentTimeEventArgs(new TimeSpan(0, 0, 5)));
            //TodayTimeSpent.Text = Convert.ToString(Math.Round(todaytimespent, 2));
            Debug.WriteLine(AppStatistics.TimeSpentToday.TotalSeconds);
        }


        public event EventHandler MakeMainPageVisible;
        public event EventHandler<StatAddSpentTimeEventArgs> TimeAdd;

        private void GoBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }
        //private void HandleEsc(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //        Window.GetWindow(this).Close();
        //}
    }
}
