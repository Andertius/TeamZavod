using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Memento.BLL;
using Memento.DAL;

namespace Memento.UserControls
{
    public partial class DeckEditorUserControl : UserControl
    {
        private static readonly DependencyProperty DeckEditorProperty = DependencyProperty.Register(nameof(DeckEditor), typeof(DeckEditor), typeof(DeckEditorUserControl),
            new PropertyMetadata(new DeckEditor()));

        private static readonly DependencyProperty CurrentCardProperty = DependencyProperty.Register(nameof(CurrentCard), typeof(Card), typeof(DeckEditorUserControl),
            new PropertyMetadata(new Card()));

        private bool handle = true;
        private bool isCardEdited = false;
        private bool isDeckEdited = false;

        private string lastSavedWord = "";
        private string lastSavedDescription = "";
        private string lastSavedTranscription = "";
        private ImageSource lastSavedImageSource;
        private Difficulty lastSavedDifficulty = Difficulty.None;

        public DeckEditorUserControl(int deckId)
        {
            DataContext = this;
            InitializeComponent();

            if (deckId == -1)
            {
                DeckEditor = new DeckEditor();
            }
            else
            {
                DeckEditor = new DeckEditor(deckId);
            }

            CardAdded += DeckEditor.AddCard;
            CardUpdated += DeckEditor.UpdateCard;
            CardRemoved += DeckEditor.RemoveCard;
            DeckChanged += DeckEditor.ChangeDeck;
            DeckChanged += ChangeTitleOnChangedDeck;
            DeckRemoved += DeckEditor.RemoveDeck;
            ChangesSaved += DeckEditor.SaveChanges;

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));

            dp.AddValueChanged(DescriptionTextBox, (sender, args) =>
            {
                DescriptionLimit.Text = DescriptionTextBox.Text.Length + "/500";

                if (DescriptionTextBox.Text != lastSavedDescription)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            dp.AddValueChanged(WordTextBox, (sender, args) =>
            {
                if (WordTextBox.Text != lastSavedWord)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            dp.AddValueChanged(TranscriptionTextBox, (sender, args) =>
            {
                if (TranscriptionTextBox.Text != lastSavedTranscription)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            var dpRadioButton = DependencyPropertyDescriptor.FromProperty(ToggleButton.IsCheckedProperty, typeof(TextBox));

            dpRadioButton.AddValueChanged(BeginnerButton, (sender, args) =>
            {
                if ((bool)BeginnerButton.IsChecked && CurrentCard.Difficulty != lastSavedDifficulty)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            dpRadioButton.AddValueChanged(IntermediateButton, (sender, args) =>
            {
                if ((bool)IntermediateButton.IsChecked && CurrentCard.Difficulty != lastSavedDifficulty)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            dpRadioButton.AddValueChanged(AdvancedButton, (sender, args) =>
            {
                if ((bool)AdvancedButton.IsChecked && CurrentCard.Difficulty != lastSavedDifficulty)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            dpRadioButton.AddValueChanged(NoneButton, (sender, args) =>
            {
                if ((bool)NoneButton.IsChecked && CurrentCard.Difficulty != lastSavedDifficulty)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            DecksCombox.SelectedValue = DeckEditor.Deck;
            Cards = Repository.FetchAllCards().ToList();
        }

        public List<Card> Cards { get; set; }

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

        public bool IsCardEdited
        {
            get => isCardEdited;
            private set
            {
                if (isCardEdited != value)
                {
                    isCardEdited = value;

                    if (isCardEdited == true)
                    {
                        TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {DeckEditor.Deck.DeckName}*"));
                    }
                    else
                    {
                        TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {DeckEditor.Deck.DeckName}"));
                    }
                }
            }
        }

        public bool IsDeckEdited
        {
            get => isDeckEdited;
            private set
            {
                if (isDeckEdited != value)
                {
                    isDeckEdited = value;

                    if (isDeckEdited == true)
                    {
                        TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {DeckEditor.Deck.DeckName}*"));
                    }
                    else
                    {
                        TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {DeckEditor.Deck.DeckName}"));
                    }
                }
            }
        }

        public event EventHandler<DeckEditorCardEventArgs> CardAdded;
        public event EventHandler<DeckEditorCardEventArgs> CardUpdated;
        public event EventHandler<DeckEditorRemoveCardEventArgs> CardRemoved;
        public event EventHandler<DeckEditorDeckEventArgs> DeckChanged;
        public event EventHandler<DeckEditorDeckEventArgs> ChangesSaved;
        public event EventHandler<RemoveDeckEditorDeckEventArgs> DeckRemoved;

        public event EventHandler<ChangeTitleEventArgs> TitleChanged;
        public event EventHandler MakeMainPageVisible;

        public void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited || IsDeckEdited)
            {
                var result = MessageBox.Show("You have unsaved changes. Do you wish to save them?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveDeck(this, new RoutedEventArgs());
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }

        public void NewCard(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning(false))
            {
                return;
            }

            ClearCard();
            ChangeLastSaved();
        }

        public void NewExistingCard(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning(false))
            {
                return;
            }

            var cardsWindow = new AllCardsWindow(Cards);
            cardsWindow.ShowDialog();

            if (!(cardsWindow.SelectedCard is null))
            {
                ChangeCard(cardsWindow.SelectedCard);
                IsCardEdited = true;
                IsDeckEdited = true;
            }
        }

        public void SaveCard(object sender, RoutedEventArgs e)
        {
            if (DeckEditor.Deck.Contains(CurrentCard))
            {
                CardUpdated?.Invoke(this, new DeckEditorCardEventArgs(CurrentCard));
                Cards[Cards.IndexOf(CurrentCard)] = new Card(CurrentCard);
            }
            else
            {
                CardAdded?.Invoke(this, new DeckEditorCardEventArgs(CurrentCard));
                Cards.Add(CurrentCard);
            }

            ChangeLastSaved();
            IsDeckEdited = true;
        }

        public void SaveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CurrentCard.Word != String.Empty && CurrentCard.Description != String.Empty && IsCardEdited;
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
            if ((IsCardEdited || IsDeckEdited) && ShowSaveWarning(true))
            {
                return;
            }

            if (DecksCombox.SelectedItem is Deck deck)
            {
                DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(Repository.FetchDeck(deck.DeckName)));
            }
        }

        public void RemoveCard(object sender, RoutedEventArgs e)
        {
            CardRemoved?.Invoke(this, new DeckEditorRemoveCardEventArgs(CurrentCard));
            IsDeckEdited = true;
        }

        public void RemoveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.Deck.Contains(CurrentCard);
        }

        public void SaveDeck(object sender, RoutedEventArgs e)
        {
            if (DeckEditor.Deck.DeckName == String.Empty)
            {
                var cards = new ObservableCollection<Card>(DeckEditor.Deck.Cards);
                IsDeckEdited = false;
                NewDeckButton_Click(this, new RoutedEventArgs());
                DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(DeckEditor.Deck));
                DeckEditor.Deck.Clear();
                DeckEditor.Deck.AddRange(cards);
            }

            ChangesSaved?.Invoke(this, new DeckEditorDeckEventArgs(DeckEditor.Deck));
        }

        public void SaveDeckCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = IsDeckEdited;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning(false))
            {
                return;
            }

            ChangeCard(DeckEditor.Deck[DeckEditor.Deck.IndexOf((Card)CardsDataGrid.SelectedItem)]);
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var imageDialog = new ImagesWindow(CardImage.Source);
            imageDialog.ShowDialog();

            if (!CompareImages(imageDialog.CurrentImage))
            {
                IsCardEdited = true;
            }
            else if (CheckForEdition())
            {
                IsCardEdited = false;
            }

            CardImage.Source = imageDialog.ImageSource;
        }

        private bool CheckForEdition()
        {
            return CurrentCard.Word != lastSavedWord || CurrentCard.Description != lastSavedDescription || CurrentCard.Transcription != lastSavedTranscription ||
                CurrentCard.Difficulty != lastSavedDifficulty || !CompareImages(CardImage);
        }

        private void ChangeLastSaved()
        {
            lastSavedWord = CurrentCard.Word;
            lastSavedDescription = CurrentCard.Description;
            lastSavedTranscription = CurrentCard.Transcription;
            lastSavedImageSource = CardImage.Source;
            lastSavedDifficulty = CurrentCard.Difficulty;
            IsCardEdited = false;
        }

        private bool ShowSaveWarning(bool saveDeck)
        {
            var result = MessageBox.Show("You have unsaved changes. Do you wish to save them?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                SaveCard(this, new RoutedEventArgs());

                if (saveDeck)
                {
                    SaveDeck(this, new RoutedEventArgs());
                }

                return false;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CompareImages(Image img)
        {
            return img.Source is null && lastSavedImageSource is null || (img.Source == lastSavedImageSource);
        }

        private void NewDeckButton_Click(object sender, RoutedEventArgs e)
        {
            if ((IsCardEdited || IsDeckEdited) && ShowSaveWarning(true))
            {
                return;
            }

            var newDeckWindow = new NewDeckWindow();
            newDeckWindow.ShowDialog();

            if (newDeckWindow.DeckName != String.Empty)
            {
                var newDeck = new Deck() { DeckName = newDeckWindow.DeckName, TagName = newDeckWindow.TagName };

                if (DeckEditor.AllDecks.Contains(newDeck))
                {
                    MessageBox.Show($"The Deck {newDeck.DeckName} already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(newDeck));
                ChangesSaved?.Invoke(this, new DeckEditorDeckEventArgs(newDeck));
                ClearCard();
                ChangeLastSaved();
                DecksCombox.SelectedItem = DeckEditor.AllDecks.ElementAt(DeckEditor.AllDecks.IndexOf(newDeck));
            }
        }

        private void RemoveDeck(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {DeckEditor.Deck.DeckName}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                var args = new RemoveDeckEditorDeckEventArgs(DeckEditor.Deck);
                DeckRemoved?.Invoke(this, args);
                
                if (args.Removed)
                {
                    DeckEditor.AllDecks.Remove(DeckEditor.Deck);
                    DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(new Deck()));
                    ClearCard();
                    ChangeLastSaved();
                }
            }
        }

        public void RemoveDeckCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.Deck.DeckName != String.Empty;
        }

        private void ClearCard()
        {
            CurrentCard = new Card();
            CardImage.Source = null;
        }

        private void ChangeCard(Card card)
        {
            CurrentCard = new Card(card);

            try
            {
                CardImage.Source = String.IsNullOrWhiteSpace(CurrentCard.ImagePath)
                    ? null
                    : (ImageSource)new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), $"{CurrentCard.ImagePath}")));

                ChangeLastSaved();
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Image path does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeTitleOnChangedDeck(object sender, DeckEditorDeckEventArgs e)
        {
            TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {DeckEditor.Deck.DeckName}"));
        }
    }
}
