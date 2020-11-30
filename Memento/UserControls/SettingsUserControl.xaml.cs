// <copyright file="SettingsUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

using Memento.BLL;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml.
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        private static readonly DependencyProperty AppSettingsProperty = DependencyProperty.Register(
            nameof(AppSettings),
            typeof(Settings),
            typeof(SettingsUserControl),
            new PropertyMetadata(new Settings()));

        private bool handle = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsUserControl"/> class.
        /// </summary>
        /// <param name="settings">Application settings that will be displayed when the page loads.</param>
        public SettingsUserControl(Settings settings)
        {
            DataContext = this;
            InitializeComponent();
            AppSettings = settings;
            HrsTextBox.Text = AppSettings.HoursPerDay.ToString();
            ThemeCombox.SelectedIndex = (int)AppSettings.Theme;
            CardOrderCombox.SelectedIndex = (int)AppSettings.CardOrder;

            ChangeTheme();
        }

        /// <summary>
        /// An event that handles the return to the main page.
        /// </summary>
        public event EventHandler MakeMainPageVisible;

        /// <summary>
        /// Gets or sets the settings that the user works with.
        /// </summary>
        public Settings AppSettings
        {
            get => (Settings)GetValue(AppSettingsProperty);
            set => SetValue(AppSettingsProperty, value);
        }

        /// <summary>
        /// Gets or sets current application theme.
        /// </summary>
        public Theme AppTheme { get; set; }

        /// <summary>
        /// Checks hoursPerDay for validity.
        /// </summary>
        /// <param name="hoursPerDay">hoursPerDay to check.</param>
        /// <returns>true if hoursPerDay is valid.</returns>
        public bool CheckHoursPerDay(double hoursPerDay)
        {
            return hoursPerDay >= 0 && hoursPerDay <= 24;
        }

        /// <summary>
        /// Checks cardsPerDay for validity.
        /// </summary>
        /// <param name="cardsPerDay">cardsPerDay to check.</param>
        /// <returns>true if cardsPerDay is valid.</returns>
        public bool CheckCardsPerDay(int cardsPerDay)
        {
            return cardsPerDay >= 0 && cardsPerDay <= 1000;
        }

        /// <summary>
        /// Writes settings to file.
        /// </summary>
        /// <param name="settings">settings to write.</param>
        /// <param name="filePath">file path to write to.</param>
        public void WriteSettingsToFile(Settings settings, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            StreamWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, settings);

            writer.Close();
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            bool isOkHrs = true;
            double hoursPerDay = 0;
            try
            {
                hoursPerDay = Double.Parse(HrsTextBox.Text, CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                isOkHrs = false;
            }

            isOkHrs &= CheckHoursPerDay(hoursPerDay);

            bool isOkCardsNum = Int32.TryParse(CardsTextBox.Text, out int cardsPerDay);
            isOkCardsNum &= CheckCardsPerDay(cardsPerDay);

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

                string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
                string path = Path.Combine(dir, @"Memento.BLL\Settings.xml");

                WriteSettingsToFile(AppSettings, path);

                MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
            }
            else if (!isOkHrs)
            {
                MessageBox.Show("Invalid Daily Milestone(hrs)");
            }
            else
            {
                MessageBox.Show("Invalid Daily Milestone (cards)");
            }
        }

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            if (handle)
            {
                ChangeTheme();
            }

            handle = true;
        }

        private void Theme_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                ChangeTheme();
            }

            handle = true;
        }

        private void ChangeTheme()
        {
            string val = ThemeCombox.Text;
            AppTheme = (val == "Light") ? Theme.Light : Theme.Dark;
            if (AppTheme == Theme.Dark)
            {
                SettingsGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2c303a");
            }
            else
            {
                SettingsGrid.Background = Brushes.White;
            }
        }
    }
}
