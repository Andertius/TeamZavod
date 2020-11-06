using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Memento.DAL;
using Memento.BLL;
using System.ComponentModel;
using System.Linq;

namespace Memento.UserControls
{
    public partial class DeckEditorUserControl : UserControl
    {
        private readonly DependencyProperty DeckEditorProperty = DependencyProperty.Register(nameof(DeckEditor), typeof(DeckEditor), typeof(DeckEditorUserControl),
            new PropertyMetadata(new DeckEditor()));

        private readonly DependencyProperty CurrentCardProperty = DependencyProperty.Register(nameof(CurrentCard), typeof(Card), typeof(DeckEditorUserControl),
            new PropertyMetadata(new Card()));

        private bool handle = true;

        public DeckEditorUserControl(int deckId)
        {
            DataContext = this;
            InitializeComponent();

            DeckEditor = new DeckEditor(deckId);

            CardAdded += DeckEditor.AddCard;
            CardAdded += RefreshDataGrid;

            CardRemoved += DeckEditor.RemoveCard;
            CardRemoved += RefreshDataGrid;

            DeckChanged += DeckEditor.ChangeDeck;
            DeckChanged += RefreshDataGrid;

            ChangesSaved += DeckEditor.SaveChanges;
            EditorExited += DeckEditor.ExitEditor;

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));

            dp.AddValueChanged(DescriptionTextBox, (sender, args) =>
            {
                DescriptionLimit.Text = DescriptionTextBox.Text.Length + "/200";
            });

            DecksCombox.SelectedValue = DeckEditor.Deck;
        }

        public DeckEditor DeckEditor
        {
            get => (DeckEditor)GetValue(DeckEditorProperty);
            set => SetValue(DeckEditorProperty, value);
        }

        public Card CurrentCard
        {
            get => (Card)GetValue(CurrentCardProperty);
            set => SetValue(CurrentCardProperty, value);
        }

        //Deck editor events
        public event EventHandler<DeckEditorAddCardEventArgs> CardAdded;
        public event EventHandler<DeckEditorRemoveCardEventArgs> CardRemoved;
        public event EventHandler<DeckEditorDeckEventArgs> DeckChanged;
        public event EventHandler<DeckEditorDeckEventArgs> ChangesSaved;
        public event EventHandler<ExitDeckEditorEventArgs> EditorExited;

        public event EventHandler MakeMainPageVisible;

        public void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }

        public void SaveCard(object sender, RoutedEventArgs e)
        {
            CardAdded?.Invoke(this, new DeckEditorAddCardEventArgs(CurrentCard));
        }

        public void SaveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CurrentCard.Word != String.Empty && CurrentCard.Description != String.Empty;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                Handle();
            }

            handle = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            Handle();
        }

        private void Handle()
        {
            var deck = DecksCombox.SelectedItem as Deck;
            DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(Repository.FetchDeck(deck.DeckName)));
        }

        public void RemoveCard(object sender, RoutedEventArgs e)
        {
            CardRemoved?.Invoke(this, new DeckEditorRemoveCardEventArgs(CurrentCard));
        }

        public void RemoveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.Deck.Contains(CurrentCard);
        }

        public void SaveDeck(object sender, RoutedEventArgs e)
        {
            ChangesSaved?.Invoke(this, new DeckEditorDeckEventArgs(DeckEditor.Deck));
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentCard = DeckEditor.Deck[DeckEditor.Deck.IndexOf((Card)CardsDataGrid.SelectedItem)];
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshDataGrid(object sender, EventArgs e)
        {
            CardsDataGrid.ItemsSource = null;
            CardsDataGrid.ItemsSource = DeckEditor.Deck.Cards;
        }
    }
}
