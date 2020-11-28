// <copyright file="SettingsUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            ThemeCombox.SelectedIndex = (int)AppSettings.Theme;
            CardOrderCombox.SelectedIndex = (int)AppSettings.CardOrder;
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

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
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

                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
                string path = Path.Combine(dir, @"Memento.BLL\Settings.xml");
                StreamWriter writer = new StreamWriter(path);
                serializer.Serialize(writer, AppSettings);
                writer.Close();

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

        private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            if (handle)
            {
                Handle();
            }

            handle = true;
        }

        private void Theme_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                Handle();
            }
        }

        private void Handle()
        {
            string val = ThemeCombox.Text;
            AppTheme = (val == "Light") ? Theme.Light : Theme.Dark;
        }
    }
}
