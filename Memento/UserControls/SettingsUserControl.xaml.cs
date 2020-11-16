using Memento.BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public SettingsUserControl(Settings settings)
        {
            DataContext = this;
            InitializeComponent();
            AppSettings = settings;
            ThemeCombox.SelectedIndex = (int)AppSettings.Theme;
            CardOrderCombox.SelectedIndex = (int)AppSettings.CardOrder;
        }
        static private readonly DependencyProperty AppSettingsProperty = DependencyProperty.Register(nameof(AppSettings), typeof(Settings),
            typeof(SettingsUserControl), new PropertyMetadata(new Settings()));

        public event EventHandler MakeMainPageVisible;

        public Settings AppSettings
        {
            get => (Settings)GetValue(AppSettingsProperty);
            private set => SetValue(AppSettingsProperty, value);
        }
        public Theme AppTheme { get; set; }
        private bool handle = false;

        public void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            bool isOkHrs = Double.TryParse(HrsTextBox.Text, out double hoursPerDay);
            if (hoursPerDay < 0 || hoursPerDay > 24)
            {
                isOkHrs = false;
            }

            bool isOkCardsNum = Int32.TryParse(CardsTextBox.Text, out int cardsPerDay);
            if (cardsPerDay < 0 || cardsPerDay > 1000)
            {
                isOkCardsNum = false;
            }

            if (isOkHrs && isOkCardsNum)
            {
                bool showImages = (bool)ImagesCheckBox.IsChecked;

                string val = CardOrderCombox.Text;
                CardOrder cardsOrder = (val == "Ascending") ? CardOrder.Ascending : (val == "Descending") ? CardOrder.Descending : CardOrder.Random;

                AppSettings.HoursPerDay = hoursPerDay;
                AppSettings.CardsPerDay = cardsPerDay;
                AppSettings.CardOrder = cardsOrder;
                AppSettings.ShowImages = showImages;
                AppSettings.Theme = AppTheme;
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
