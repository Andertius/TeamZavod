// <copyright file="AllCardsWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    using Memento.DAL;

    /// <summary>
    /// Interaction logic for AllCardsWindow.xaml.
    /// </summary>
    public partial class AllCardsWindow : Window
    {
        private bool okPressed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllCardsWindow"/> class.
        /// </summary>
        /// <param name="cards">All the cards that exist.</param>
        public AllCardsWindow(IEnumerable<Card> cards)
        {
            this.DataContext = this;
            this.InitializeComponent();
            this.SelectedCard = new Card();

            this.Cards = new List<Card>(cards);
            this.RenderCards(this, EventArgs.Empty);

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp.AddValueChanged(this.SearchTextBox, this.RenderCards);
        }

        /// <summary>
        /// Gets all the cards that exist.
        /// </summary>
        public List<Card> Cards { get; }

        /// <summary>
        /// Gets or sets the card that the user selected.
        /// </summary>
        public Card SelectedCard { get; set; }

        private void RenderCards(object sender, EventArgs e)
        {
            this.CardsPanel.Children.Clear();

            foreach (var item in this.Cards)
            {
                if (item.Word.ToLower().Contains(this.SearchTextBox.Text.ToLower().Trim()))
                {
                    var word = new TextBlock()
                    {
                        Text = "Word: " + item.Word + "\nDescription: " + item.Description,
                        TextWrapping = TextWrapping.Wrap,
                    };

                    var chooseButton = new Button()
                    {
                        Content = word,
                        Tag = item,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = 100,
                        Margin = new Thickness(0, 0, 7, 7),
                    };

                    chooseButton.Click += this.Select;

                    this.CardsPanel.Children.Add(chooseButton);
                }
            }
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            this.SelectedCard = (sender as Button).Tag as Card;
            this.SelectedTextBlock.Text = $"{this.SelectedCard.Word}: {this.SelectedCard.Description}";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.okPressed = true;
            this.Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!this.okPressed)
            {
                this.SelectedCard = null;
            }
        }
    }
}
