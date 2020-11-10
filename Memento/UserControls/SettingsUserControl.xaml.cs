using Memento.DAL;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            InitializeComponent();
            AppSettings = new Settings();
            AppTheme = Theme.Light;
        }

        public event EventHandler MakeMainPageVisible;

        public Theme AppTheme { get; set; }
        public Settings AppSettings { get; set; }
        private bool handle = false;

        public void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            double hoursPerDay;
            bool isOkHrs = Double.TryParse(HrsTextBox.Text, out hoursPerDay);
            if (hoursPerDay < 0 || hoursPerDay > 24)
                isOkHrs = false;

            int cardsPerDay;
            bool isOkCardsNum = Int32.TryParse(CardsTextBox.Text, out cardsPerDay);
            if (cardsPerDay < 0 || cardsPerDay > 1000)
                isOkCardsNum = false;

            if (isOkHrs && isOkCardsNum)
            {
                bool showImages = (bool)ImagesCheckBox.IsChecked;

                string val = CardOrderCombox.Text;
                CardOrder cardsOrder = (val == "Ascending") ? CardOrder.Ascending : (val == "Descending") ? CardOrder.Descending : CardOrder.Random;

                AppSettings = new Settings(hoursPerDay, cardsPerDay, cardsOrder, showImages);
                MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if (!isOkHrs)
                {
                    MessageBox.Show("Invalid Daily Milestone (hrs)");
                }
                else
                {
                    MessageBox.Show("Invalid Daily Milestone (cards)");
                }
            }
        }

        private void Handle()
        {
            string val = ThemeCombox.Text;
            AppTheme = (val == "Light") ? Theme.Light : Theme.Dark;
        }

        public void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            if (handle)
            {
                Handle();
            }

            handle = true;
        }

        public void Theme_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                Handle();
            }
        }
    }
}
