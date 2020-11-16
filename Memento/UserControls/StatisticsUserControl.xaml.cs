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

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class StatisticsUserControl : UserControl
    {
        private double averagetime;
        private double todaytimespent;
        private double cardslearnedtoday;

        private double milestonetodaytimespent;
        private double milestonecardslearnedtoday;
        private bool check = true;

        DispatcherTimer timer;

        public StatisticsUserControl(double avgtime, double todayspent, double cardsperday)
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += UpdatePage;
            averagetime = avgtime;
            todaytimespent = todayspent;
            cardslearnedtoday = cardsperday;
            AverageTimePerDay.Text = Convert.ToString(Math.Round(avgtime, 2));
            TTSProgress.Maximum = Memento.UserControls.SettingsUserControl.
            //CardLearned.Text = Convert.ToString(cardsperday);
            //TodayTimeSpent.Text = Convert.ToString(Math.Round(todayspent, 2));
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            GoBackButton.Focus();
        }

        private void UpdatePage(object source, EventArgs e)
        {
            todaytimespent += 0.0013;
            //TodayTimeSpent.Text = Convert.ToString(Math.Round(todaytimespent, 2));
        }


        public event EventHandler MakeMainPageVisible;

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
