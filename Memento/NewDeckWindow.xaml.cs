// <copyright file="NewDeckWindow.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Interaction logic for NewDeckWindow.xaml.
    /// </summary>
    public partial class NewDeckWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewDeckWindow"/> class.
        /// </summary>
        public NewDeckWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the name of the deck.
        /// </summary>
        public string DeckName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tag name of the deck.
        /// </summary>
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
