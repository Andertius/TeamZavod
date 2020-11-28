// <copyright file="DeckEditorUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

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
using Memento.BLL.DeckEditorEventArgs;
using Memento.DAL;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for DeckEditorUserControl.xaml.
    /// </summary>
    public partial class DeckEditorUserControl : UserControl
    {
        private static readonly DependencyProperty DeckEditorProperty = DependencyProperty.Register(
            nameof(DeckEditor),
            typeof(DeckEditor),
            typeof(DeckEditorUserControl),
            new PropertyMetadata(new DeckEditor()));

        private bool handle = true;
        private bool isCardEdited = false;
        private bool isDeckChosen = false;

        private string lastSavedWord = String.Empty;
        private string lastSavedDescription = String.Empty;
        private string lastSavedTranscription = String.Empty;
        private string lastSavedImagePath;
        private Difficulty lastSavedDifficulty = Difficulty.None;
        private List<string> lastSavedTags = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditorUserControl"/> class.
        /// </summary>
        /// <param name="deckId">The id of the deck thats going to be shown when the page loads.</param>
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
                isDeckChosen = true;
            }

            CardAdded += DeckEditor.AddCard;
            CardUpdated += DeckEditor.UpdateCard;
            CardRemoved += DeckEditor.RemoveCard;
            CardChanged += DeckEditor.ChangeCard;
            CardCleared += DeckEditor.ClearCard;
            CardCleared += ClearImageAndTags;
            DeckChanged += DeckEditor.ChangeDeck;
            DeckChanged += ChangeTitleOnChangedDeck;
            DeckChanged += ClearImageAndTags;
            DeckEdited += DeckEditor.EditDeck;
            DeckEdited += ChangeTitleOnEditedDeck;
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
                if ((bool)BeginnerButton.IsChecked && DeckEditor.CurrentCard.Difficulty != lastSavedDifficulty)
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
                if ((bool)IntermediateButton.IsChecked && DeckEditor.CurrentCard.Difficulty != lastSavedDifficulty)
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
                if ((bool)AdvancedButton.IsChecked && DeckEditor.CurrentCard.Difficulty != lastSavedDifficulty)
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
                if ((bool)NoneButton.IsChecked && DeckEditor.CurrentCard.Difficulty != lastSavedDifficulty)
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }
            });

            DecksCombox.SelectedValue = DeckEditor.Deck;
            RenderTags(this, EventArgs.Empty);

            var dp2 = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp2.AddValueChanged(SearchTextBox, RenderTags);
        }

        /// <summary>
        /// An event that handles the addition of new cards to a deck.
        /// </summary>
        public event EventHandler<DeckEditorCardEventArgs> CardAdded;

        /// <summary>
        /// An event that handles the updating of a card.
        /// </summary>
        public event EventHandler<UpdateCardDeckEditorEventArgs> CardUpdated;

        /// <summary>
        /// An event that handles the removal of a card from a deck.
        /// </summary>
        public event EventHandler<DeckEditorRemoveCardEventArgs> CardRemoved;

        /// <summary>
        /// An event that handles the change of the current card
        /// </summary>
        public event EventHandler<DeckEditorCardEventArgs> CardChanged;

        /// <summary>
        /// An event that handles the cleaning of a card
        /// </summary>
        public event EventHandler CardCleared;

        /// <summary>
        /// An event that handles the change of the current deck
        /// </summary>
        public event EventHandler<DeckEditorDeckEventArgs> DeckChanged;

        /// <summary>
        /// An event that handles saving
        /// </summary>
        public event EventHandler<DeckEditorDeckEventArgs> ChangesSaved;

        /// <summary>
        /// An event that handles the removal of a deck from the database.
        /// </summary>
        public event EventHandler<RemoveDeckEditorDeckEventArgs> DeckRemoved;

        /// <summary>
        /// An event that handles the editing of a deck.
        /// </summary>
        public event EventHandler<EditDeckDeckEditorEventArgs> DeckEdited;

        /// <summary>
        /// An event that handles the change in the title of the window.
        /// </summary>
        public event EventHandler<ChangeTitleEventArgs> TitleChanged;

        /// <summary>
        /// An event that handles the return to the main page.
        /// </summary>
        public event EventHandler MakeMainPageVisible;

        /// <summary>
        /// Gets or sets the deck that the user works with.
        /// </summary>
        public DeckEditor DeckEditor
        {
            get => (DeckEditor)GetValue(DeckEditorProperty);
            set => SetValue(DeckEditorProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the current card is edited od not.
        /// </summary>
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

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited)
            {
                var result = MessageBox.Show("You have unsaved changes. Do you wish to save them?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    SaveCard(this, new RoutedEventArgs());
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }

        private void NewCard(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning())
            {
                return;
            }

            CardCleared?.Invoke(this, EventArgs.Empty);
            ChangeLastSaved();
        }

        private void NewExistingCard(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning())
            {
                return;
            }

            var cardsWindow = new AllCardsWindow(DeckEditor.Cards);
            cardsWindow.ShowDialog();

            if (!(cardsWindow.SelectedCard is null))
            {
                ChangeCard(cardsWindow.SelectedCard);
                IsCardEdited = true;
            }
        }

        private void SaveCard(object sender, RoutedEventArgs e)
        {
            if (DeckEditor.Deck.Contains(DeckEditor.CurrentCard))
            {
                CardUpdated?.Invoke(this, new UpdateCardDeckEditorEventArgs(DeckEditor.CurrentCard.Id, DeckEditor.CurrentCard));
                DeckEditor.Cards[DeckEditor.Cards.IndexOf(DeckEditor.CurrentCard)] = new Card(DeckEditor.CurrentCard);
            }
            else
            {
                CardAdded?.Invoke(this, new DeckEditorCardEventArgs(DeckEditor.Deck, DeckEditor.CurrentCard));
                DeckEditor.Cards.Add(DeckEditor.CurrentCard);
            }

            foreach (var tag in DeckEditor.CurrentCard.Tags)
            {
                if (!DeckEditor.Tags.Contains(tag))
                {
                    DeckEditor.Tags.Add(tag);
                }
            }

            RenderCurrentCardTags(sender, e);
            CardsDataGrid.Items.Refresh();
            ChangeLastSaved();
        }

        private void SaveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.CurrentCard.Word != String.Empty && DeckEditor.CurrentCard.Description != String.Empty && IsCardEdited && isDeckChosen;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                ChangeDeck();
            }

            // handle = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            ChangeDeck();
        }

        private void ChangeDeck()
        {
            if (IsCardEdited && isDeckChosen && ShowSaveWarning())
            {
                return;
            }

            if (DecksCombox.SelectedItem is Deck deck)
            {
                DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(Repository.FetchDeck(deck.DeckName)));
                IsCardEdited = false;
                isDeckChosen = true;
            }
        }

        private void RemoveCard(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {DeckEditor.CurrentCard.Word}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                CardRemoved?.Invoke(this, new DeckEditorRemoveCardEventArgs(DeckEditor.Deck, DeckEditor.CurrentCard));
                RenderCurrentCardTags(sender, e);
                CardImage.Source = null;
                IsCardEdited = false;
            }
        }

        private void RemoveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.Deck.Contains(DeckEditor.CurrentCard);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning())
            {
                return;
            }

            ChangeCard(DeckEditor.Deck[DeckEditor.Deck.IndexOf((Card)CardsDataGrid.SelectedItem)]);
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var imageDialog = new ImagesWindow(CardImage.Source);
            imageDialog.ShowDialog();

            if (imageDialog.SelectedPath != lastSavedImagePath)
            {
                IsCardEdited = true;
            }
            else if (!CheckForEdition())
            {
                IsCardEdited = false;
            }

            CardImage.Source = imageDialog.ImageSource;
            DeckEditor.CurrentCard.ImagePath = imageDialog.SelectedPath;
        }

        private bool CheckForEdition()
        {
            return DeckEditor.CurrentCard.Word != lastSavedWord || DeckEditor.CurrentCard.Description != lastSavedDescription ||
                DeckEditor.CurrentCard.Transcription != lastSavedTranscription || DeckEditor.CurrentCard.Difficulty != lastSavedDifficulty ||
                DeckEditor.CurrentCard.ImagePath != lastSavedImagePath || !DeckEditor.CurrentCard.Tags.SequenceEqual(lastSavedTags);
        }

        private void ChangeLastSaved()
        {
            lastSavedWord = DeckEditor.CurrentCard.Word;
            lastSavedDescription = DeckEditor.CurrentCard.Description;
            lastSavedTranscription = DeckEditor.CurrentCard.Transcription;
            lastSavedImagePath = DeckEditor.CurrentCard.ImagePath;
            lastSavedDifficulty = DeckEditor.CurrentCard.Difficulty;
            lastSavedTags = new List<string>(DeckEditor.CurrentCard.Tags);
            IsCardEdited = false;
        }

        private bool ShowSaveWarning()
        {
            var result = MessageBox.Show("You have unsaved changes. Do you wish to save them?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                SaveCard(this, new RoutedEventArgs());
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

        private void NewDeckButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning())
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
                CardCleared?.Invoke(this, EventArgs.Empty);
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
                    CardCleared?.Invoke(this, EventArgs.Empty);
                    ChangeLastSaved();
                }
            }
        }

        private void RemoveDeckCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.Deck.DeckName != String.Empty;
        }

        private void EditDeck(object sender, RoutedEventArgs e)
        {
            var editDeckWindow = new EditDeckWindow(DeckEditor.Deck.DeckName, DeckEditor.Deck.TagName);
            editDeckWindow.ShowDialog();

            if (editDeckWindow.DeckName != DeckEditor.Deck.DeckName || editDeckWindow.TagName != DeckEditor.Deck.TagName)
            {
                DeckEdited?.Invoke(this, new EditDeckDeckEditorEventArgs(editDeckWindow.DeckName, editDeckWindow.TagName));

                DecksCombox.Items.Refresh();
                DecksCombox.SelectedIndex = DeckEditor.AllDecks.IndexOf(DeckEditor.Deck);
            }
        }

        private void EditDeckCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDeckChosen;
        }

        private void ClearImageAndTags(object sender, EventArgs e)
        {
            CardImage.Source = null;
            RenderCurrentCardTags(sender, e);
        }

        private void ChangeCard(Card card)
        {
            CardChanged?.Invoke(this, new DeckEditorCardEventArgs(DeckEditor.Deck, card));
            RenderCurrentCardTags(this, EventArgs.Empty);

            try
            {
                CardImage.Source = String.IsNullOrWhiteSpace(DeckEditor.CurrentCard.ImagePath)
                    ? null
                    : (ImageSource)new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), $"{DeckEditor.CurrentCard.ImagePath}")));

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

        private void ChangeTitleOnEditedDeck(object sender, EditDeckDeckEditorEventArgs e)
        {
            TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {DeckEditor.Deck.DeckName}"));
        }

        private void RenderTags(object sender, EventArgs e)
        {
            TagsPanel.Children.Clear();

            foreach (var item in DeckEditor.Tags)
            {
                if (item.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()))
                {
                    var chooseButton = new Button()
                    {
                        Content = item,
                        Tag = item,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = 65,
                        Margin = new Thickness(0, 0, 7, 7),
                    };

                    chooseButton.Click += Select;

                    TagsPanel.Children.Add(chooseButton);
                }
            }
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            string tag = (string)((Button)sender).Content;

            if (!DeckEditor.CurrentCard.Tags.Contains(tag))
            {
                DeckEditor.CurrentCard.Tags.Add(tag);
                DeckEditor.CurrentCard.Tags = new ObservableCollection<string>(DeckEditor.CurrentCard.Tags.OrderBy(x => x));
                RenderCurrentCardTags(this, EventArgs.Empty);

                if (!DeckEditor.CurrentCard.Tags.SequenceEqual(lastSavedTags))
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }

                if (DeckEditor.CurrentCard.Id != -1)
                {
                    DeckEditor.Tags.Add(tag);
                }
            }
        }

        private void NewTagButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = NewTagTextBox.Text.Trim();

            if (tag != String.Empty && !DeckEditor.CurrentCard.Tags.Contains(tag))
            {
                DeckEditor.CurrentCard.Tags.Add(tag);
                DeckEditor.CurrentCard.Tags = new ObservableCollection<string>(DeckEditor.CurrentCard.Tags.OrderBy(x => x));
                RenderCurrentCardTags(this, EventArgs.Empty);
                NewTagTextBox.Text = String.Empty;

                if (!DeckEditor.CurrentCard.Tags.SequenceEqual(lastSavedTags))
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }

                if (DeckEditor.CurrentCard.Id != -1)
                {
                    DeckEditor.Tags.Add(tag);
                }
            }
        }

        private void RenderCurrentCardTags(object sender, EventArgs e)
        {
            TagsWrapPanel.Children.Clear();
            var tagsTextBlock = new TextBlock() { Text = "Tags: " };
            TagsWrapPanel.Children.Add(tagsTextBlock);

            foreach (var tag in DeckEditor.CurrentCard.Tags)
            {
                var border = new Border()
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0, 0, 7, 7),
                    Padding = new Thickness(3, 1, 3, 1),
                    CornerRadius = new CornerRadius(2),
                };

                var stack = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                };

                var textblock = new TextBlock()
                {
                    Text = tag,
                    Margin = new Thickness(0, 0, 4, 0),
                };

                var backgourndSetter = new Setter()
                {
                    Property = BackgroundProperty,
                    Value = Brushes.Transparent,
                };

                var borderSetter = new Setter()
                {
                    Property = BorderBrushProperty,
                    Value = Brushes.Transparent,
                };

                var trigger = new Trigger()
                {
                    Property = IsMouseOverProperty,
                    Value = true,
                    Setters = { backgourndSetter, borderSetter },
                };

                var style = new Style()
                {
                    Triggers = { trigger },
                };

                var button = new Button()
                {
                    Tag = tag,
                    Content = "\u2A09",
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Style = style,
                };

                button.Click += RemoveTag;

                stack.Children.Add(textblock);
                stack.Children.Add(button);

                border.Child = stack;
                TagsWrapPanel.Children.Add(border);
            }
        }

        private void RemoveTag(object sender, RoutedEventArgs e)
        {
            var tag = (string)((Button)sender).Tag;

            if (DeckEditor.CurrentCard.Tags.Remove(tag))
            {
                if (!DeckEditor.CurrentCard.Tags.SequenceEqual(lastSavedTags))
                {
                    IsCardEdited = true;
                }
                else if (!CheckForEdition())
                {
                    IsCardEdited = false;
                }

                RenderCurrentCardTags(this, EventArgs.Empty);

                if (DeckEditor.CurrentCard.Id != -1)
                {
                    DeckEditor.Tags.Remove(tag);
                }
            }
        }
    }
}
