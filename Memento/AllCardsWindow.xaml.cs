using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Memento.DAL;

namespace Memento
{
    public partial class AllCardsWindow : Window
    {
        private bool okPressed = false;

        public AllCardsWindow(IEnumerable<Card> cards)
        {
            DataContext = this;
            InitializeComponent();
            SelectedCard = new Card();

            Cards = new List<Card>(cards);
            RenderCards(this, EventArgs.Empty);

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp.AddValueChanged(SearchTextBox, RenderCards);
        }

        public List<Card> Cards { get; }
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
