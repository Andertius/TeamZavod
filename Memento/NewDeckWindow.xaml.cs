// <copyright file="NewDeckWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    public partial class NewDeckWindow : Window
    {
        public NewDeckWindow()
        {
            this.InitializeComponent();
        }

        public string DeckName { get; set; } = string.Empty;

        public string TagName { get; set; } = string.Empty;

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DeckNameTextBox.Text.Trim() == string.Empty)
            {
                ColorAnimation ca = new ColorAnimation(Colors.Black, Colors.Red, new Duration(TimeSpan.FromMilliseconds(500)));
                this.DeckNameTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                this.DeckNameTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ca);

                await Task.Delay(500);

                ColorAnimation ca1 = new ColorAnimation(Colors.Red, Colors.Black, new Duration(TimeSpan.FromMilliseconds(500)));
                this.DeckNameTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                this.DeckNameTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ca1);

                return;
            }

            this.DeckName = this.DeckNameTextBox.Text.Trim();
            this.TagName = this.TagNameTextBox.Text.Trim();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
