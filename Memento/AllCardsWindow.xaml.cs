﻿// <copyright file="AllCardsWindow.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Memento.DAL;

namespace Memento
{
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
            DataContext = this;
            InitializeComponent();
            SelectedCard = new Card();

            if (MainWindow.AppSettings.Theme == BLL.Theme.Dark)
            {
                AllCardsWindowGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2c303a");
            }

            Cards = new List<Card>(cards);
            RenderCards(this, EventArgs.Empty);

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp.AddValueChanged(SearchTextBox, RenderCards);
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
            CardsPanel.Children.Clear();

            foreach (var item in Cards)
            {
                if (item.Word.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()))
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

                    chooseButton.Click += Select;

                    CardsPanel.Children.Add(chooseButton);
                }
            }
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            SelectedCard = (sender as Button).Tag as Card;
            SelectedTextBlock.Text = $"{SelectedCard.Word}: {SelectedCard.Description}";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            okPressed = true;
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!okPressed)
            {
                SelectedCard = null;
            }
        }
    }
}
