// <copyright file="DeckEditorUserControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.UserControls
{
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

        private string lastSavedWord = string.Empty;
        private string lastSavedDescription = string.Empty;
        private string lastSavedTranscription = string.Empty;
        private string lastSavedImagePath;
        private Difficulty lastSavedDifficulty = Difficulty.None;
        private List<string> lastSavedTags = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditorUserControl"/> class.
        /// </summary>
        /// <param name="deckId">The id of the deck thats going to be shown when the page loads.</param>
        public DeckEditorUserControl(int deckId)
        {
            this.DataContext = this;
            this.InitializeComponent();

            if (deckId == -1)
            {
                this.DeckEditor = new DeckEditor();
            }
            else
            {
                this.DeckEditor = new DeckEditor(deckId);
            }

            this.CardAdded += this.DeckEditor.AddCard;
            this.CardUpdated += this.DeckEditor.UpdateCard;
            this.CardRemoved += this.DeckEditor.RemoveCard;
            this.CardChanged += this.DeckEditor.ChangeCard;
            this.CardCleared += this.DeckEditor.ClearCard;
            this.CardCleared += this.ClearImageAndTags;
            this.DeckChanged += this.DeckEditor.ChangeDeck;
            this.DeckChanged += this.ChangeTitleOnChangedDeck;
            this.DeckChanged += this.ClearImageAndTags;
            this.DeckRemoved += this.DeckEditor.RemoveDeck;
            this.ChangesSaved += this.DeckEditor.SaveChanges;

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));

            dp.AddValueChanged(this.DescriptionTextBox, (sender, args) =>
            {
                this.DescriptionLimit.Text = this.DescriptionTextBox.Text.Length + "/500";

                if (this.DescriptionTextBox.Text != this.lastSavedDescription)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            dp.AddValueChanged(this.WordTextBox, (sender, args) =>
            {
                if (this.WordTextBox.Text != this.lastSavedWord)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            dp.AddValueChanged(this.TranscriptionTextBox, (sender, args) =>
            {
                if (this.TranscriptionTextBox.Text != this.lastSavedTranscription)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            var dpRadioButton = DependencyPropertyDescriptor.FromProperty(ToggleButton.IsCheckedProperty, typeof(TextBox));

            dpRadioButton.AddValueChanged(this.BeginnerButton, (sender, args) =>
            {
                if ((bool)this.BeginnerButton.IsChecked && this.DeckEditor.CurrentCard.Difficulty != this.lastSavedDifficulty)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            dpRadioButton.AddValueChanged(this.IntermediateButton, (sender, args) =>
            {
                if ((bool)this.IntermediateButton.IsChecked && this.DeckEditor.CurrentCard.Difficulty != this.lastSavedDifficulty)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            dpRadioButton.AddValueChanged(this.AdvancedButton, (sender, args) =>
            {
                if ((bool)this.AdvancedButton.IsChecked && this.DeckEditor.CurrentCard.Difficulty != this.lastSavedDifficulty)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            dpRadioButton.AddValueChanged(this.NoneButton, (sender, args) =>
            {
                if ((bool)this.NoneButton.IsChecked && this.DeckEditor.CurrentCard.Difficulty != this.lastSavedDifficulty)
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }
            });

            this.DecksCombox.SelectedValue = this.DeckEditor.Deck;
            this.RenderTags(this, EventArgs.Empty);

            var dp2 = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp2.AddValueChanged(this.SearchTextBox, this.RenderTags);
        }

        /// <summary>
        /// An event that handles the addition of new cards to a deck.
        /// </summary>
        public event EventHandler<DeckEditorCardEventArgs> CardAdded;

        /// <summary>
        /// An event that handles the updating of a card.
        /// </summary>
        public event EventHandler<DeckEditorCardEventArgs> CardUpdated;

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
            get => (DeckEditor)this.GetValue(DeckEditorProperty);
            set => this.SetValue(DeckEditorProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the current card is edited od not.
        /// </summary>
        public bool IsCardEdited
        {
            get => this.isCardEdited;
            private set
            {
                if (this.isCardEdited != value)
                {
                    this.isCardEdited = value;

                    if (this.isCardEdited == true)
                    {
                        this.TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {this.DeckEditor.Deck.DeckName}*"));
                    }
                    else
                    {
                        this.TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {this.DeckEditor.Deck.DeckName}"));
                    }
                }
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsCardEdited)
            {
                var result = MessageBox.Show("You have unsaved changes. Do you wish to save them?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    this.SaveCard(this, new RoutedEventArgs());
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            this.MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }

        private void NewCard(object sender, RoutedEventArgs e)
        {
            if (this.IsCardEdited && this.ShowSaveWarning())
            {
                return;
            }

            this.CardCleared?.Invoke(this, EventArgs.Empty);
            this.ChangeLastSaved();
        }

        private void NewExistingCard(object sender, RoutedEventArgs e)
        {
            if (this.IsCardEdited && this.ShowSaveWarning())
            {
                return;
            }

            var cardsWindow = new AllCardsWindow(this.DeckEditor.Cards);
            cardsWindow.ShowDialog();

            if (!(cardsWindow.SelectedCard is null))
            {
                this.ChangeCard(cardsWindow.SelectedCard);
                this.IsCardEdited = true;
            }
        }

        private void SaveCard(object sender, RoutedEventArgs e)
        {
            if (this.DeckEditor.Deck.Contains(this.DeckEditor.CurrentCard))
            {
                this.CardUpdated?.Invoke(this, new DeckEditorCardEventArgs(this.DeckEditor.Deck, this.DeckEditor.CurrentCard));
                this.DeckEditor.Cards[this.DeckEditor.Cards.IndexOf(this.DeckEditor.CurrentCard)] = new Card(this.DeckEditor.CurrentCard);
            }
            else
            {
                this.CardAdded?.Invoke(this, new DeckEditorCardEventArgs(this.DeckEditor.Deck, this.DeckEditor.CurrentCard));
                this.DeckEditor.Cards.Add(this.DeckEditor.CurrentCard);
            }

            foreach (var tag in this.DeckEditor.CurrentCard.Tags)
            {
                if (!this.DeckEditor.Tags.Contains(tag))
                {
                    this.DeckEditor.Tags.Add(tag);
                }
            }

            this.RenderCurrentCardTags(sender, e);
            this.ChangeLastSaved();
        }

        private void SaveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.DeckEditor.CurrentCard.Word != string.Empty && this.DeckEditor.CurrentCard.Description != string.Empty && this.IsCardEdited;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (this.handle)
            {
                this.ChangeDeck();
            }

            // handle = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            this.handle = !cmb.IsDropDownOpen;
            this.ChangeDeck();
        }

        private void ChangeDeck()
        {
            if (this.IsCardEdited && this.ShowSaveWarning())
            {
                return;
            }

            if (this.DecksCombox.SelectedItem is Deck deck)
            {
                this.DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(Repository.FetchDeck(deck.DeckName)));
                this.IsCardEdited = false;
            }
        }

        private void RemoveCard(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {this.DeckEditor.CurrentCard.Word}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                this.CardRemoved?.Invoke(this, new DeckEditorRemoveCardEventArgs(this.DeckEditor.Deck, this.DeckEditor.CurrentCard));
                this.RenderCurrentCardTags(sender, e);
                this.CardImage.Source = null;
                this.IsCardEdited = false;
            }
        }

        private void RemoveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.DeckEditor.Deck.Contains(this.DeckEditor.CurrentCard);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsCardEdited && this.ShowSaveWarning())
            {
                return;
            }

            this.ChangeCard(this.DeckEditor.Deck[this.DeckEditor.Deck.IndexOf((Card)this.CardsDataGrid.SelectedItem)]);
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            var imageDialog = new ImagesWindow(this.CardImage.Source);
            imageDialog.ShowDialog();

            if (imageDialog.SelectedPath != this.lastSavedImagePath)
            {
                this.IsCardEdited = true;
            }
            else if (!this.CheckForEdition())
            {
                this.IsCardEdited = false;
            }

            this.CardImage.Source = imageDialog.ImageSource;
            this.DeckEditor.CurrentCard.ImagePath = imageDialog.SelectedPath;
        }

        private bool CheckForEdition()
        {
            return this.DeckEditor.CurrentCard.Word != this.lastSavedWord || this.DeckEditor.CurrentCard.Description != this.lastSavedDescription ||
                this.DeckEditor.CurrentCard.Transcription != this.lastSavedTranscription || this.DeckEditor.CurrentCard.Difficulty != this.lastSavedDifficulty ||
                this.DeckEditor.CurrentCard.ImagePath != this.lastSavedImagePath || !this.DeckEditor.CurrentCard.Tags.SequenceEqual(this.lastSavedTags);
        }

        private void ChangeLastSaved()
        {
            this.lastSavedWord = this.DeckEditor.CurrentCard.Word;
            this.lastSavedDescription = this.DeckEditor.CurrentCard.Description;
            this.lastSavedTranscription = this.DeckEditor.CurrentCard.Transcription;
            this.lastSavedImagePath = this.DeckEditor.CurrentCard.ImagePath;
            this.lastSavedDifficulty = this.DeckEditor.CurrentCard.Difficulty;
            this.lastSavedTags = new List<string>(this.DeckEditor.CurrentCard.Tags);
            this.IsCardEdited = false;
        }

        private bool ShowSaveWarning()
        {
            var result = MessageBox.Show("You have unsaved changes. Do you wish to save them?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.SaveCard(this, new RoutedEventArgs());
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
            if (this.IsCardEdited && this.ShowSaveWarning())
            {
                return;
            }

            var newDeckWindow = new NewDeckWindow();
            newDeckWindow.ShowDialog();

            if (newDeckWindow.DeckName != string.Empty)
            {
                var newDeck = new Deck() { DeckName = newDeckWindow.DeckName, TagName = newDeckWindow.TagName };

                if (this.DeckEditor.AllDecks.Contains(newDeck))
                {
                    MessageBox.Show($"The Deck {newDeck.DeckName} already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(newDeck));
                this.ChangesSaved?.Invoke(this, new DeckEditorDeckEventArgs(newDeck));
                this.CardCleared?.Invoke(this, EventArgs.Empty);
                this.ChangeLastSaved();
                this.DecksCombox.SelectedItem = this.DeckEditor.AllDecks.ElementAt(this.DeckEditor.AllDecks.IndexOf(newDeck));
            }
        }

        private void RemoveDeck(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {this.DeckEditor.Deck.DeckName}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                var args = new RemoveDeckEditorDeckEventArgs(this.DeckEditor.Deck);
                this.DeckRemoved?.Invoke(this, args);

                if (args.Removed)
                {
                    this.DeckEditor.AllDecks.Remove(this.DeckEditor.Deck);
                    this.DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(new Deck()));
                    this.CardCleared?.Invoke(this, EventArgs.Empty);
                    this.ChangeLastSaved();
                }
            }
        }

        private void RemoveDeckCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.DeckEditor.Deck.DeckName != string.Empty;
        }

        private void ClearImageAndTags(object sender, EventArgs e)
        {
            this.CardImage.Source = null;
            this.RenderCurrentCardTags(sender, e);
        }

        private void ChangeCard(Card card)
        {
            this.CardChanged?.Invoke(this, new DeckEditorCardEventArgs(this.DeckEditor.Deck, card));
            this.RenderCurrentCardTags(this, EventArgs.Empty);

            try
            {
                this.CardImage.Source = string.IsNullOrWhiteSpace(this.DeckEditor.CurrentCard.ImagePath)
                    ? null
                    : (ImageSource)new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), $"{this.DeckEditor.CurrentCard.ImagePath}")));

                this.ChangeLastSaved();
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Image path does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeTitleOnChangedDeck(object sender, DeckEditorDeckEventArgs e)
        {
            this.TitleChanged?.Invoke(this, new ChangeTitleEventArgs($"Memento - Deck Editor - {this.DeckEditor.Deck.DeckName}"));
        }

        private void RenderTags(object sender, EventArgs e)
        {
            this.TagsPanel.Children.Clear();

            foreach (var item in this.DeckEditor.Tags)
            {
                if (item.ToLower().Contains(this.SearchTextBox.Text.ToLower().Trim()))
                {
                    var chooseButton = new Button()
                    {
                        Content = item,
                        Tag = item,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = 65,
                        Margin = new Thickness(0, 0, 7, 7),
                    };

                    chooseButton.Click += this.Select;

                    this.TagsPanel.Children.Add(chooseButton);
                }
            }
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            string tag = (string)((Button)sender).Content;

            if (!this.DeckEditor.CurrentCard.Tags.Contains(tag))
            {
                this.DeckEditor.CurrentCard.Tags.Add(tag);
                this.DeckEditor.CurrentCard.Tags = new ObservableCollection<string>(this.DeckEditor.CurrentCard.Tags.OrderBy(x => x));
                this.RenderCurrentCardTags(this, EventArgs.Empty);

                if (!this.DeckEditor.CurrentCard.Tags.SequenceEqual(this.lastSavedTags))
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }

                if (this.DeckEditor.CurrentCard.Id != -1)
                {
                    this.DeckEditor.Tags.Add(tag);
                }
            }
        }

        private void NewTagButton_Click(object sender, RoutedEventArgs e)
        {
            string tag = this.NewTagTextBox.Text.Trim();

            if (tag != string.Empty && !this.DeckEditor.CurrentCard.Tags.Contains(tag))
            {
                this.DeckEditor.CurrentCard.Tags.Add(tag);
                this.DeckEditor.CurrentCard.Tags = new ObservableCollection<string>(this.DeckEditor.CurrentCard.Tags.OrderBy(x => x));
                this.RenderCurrentCardTags(this, EventArgs.Empty);

                if (!this.DeckEditor.CurrentCard.Tags.SequenceEqual(this.lastSavedTags))
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }

                if (this.DeckEditor.CurrentCard.Id != -1)
                {
                    this.DeckEditor.Tags.Add(tag);
                }
            }
        }

        private void RenderCurrentCardTags(object sender, EventArgs e)
        {
            this.TagsWrapPanel.Children.Clear();
            var tagsTextBlock = new TextBlock() { Text = "Tags: " };
            this.TagsWrapPanel.Children.Add(tagsTextBlock);

            foreach (var tag in this.DeckEditor.CurrentCard.Tags)
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

                button.Click += this.RemoveTag;

                stack.Children.Add(textblock);
                stack.Children.Add(button);

                border.Child = stack;
                this.TagsWrapPanel.Children.Add(border);
            }
        }

        private void RemoveTag(object sender, RoutedEventArgs e)
        {
            var tag = (string)((Button)sender).Tag;

            if (this.DeckEditor.CurrentCard.Tags.Remove(tag))
            {
                if (!this.DeckEditor.CurrentCard.Tags.SequenceEqual(this.lastSavedTags))
                {
                    this.IsCardEdited = true;
                }
                else if (!this.CheckForEdition())
                {
                    this.IsCardEdited = false;
                }

                this.RenderCurrentCardTags(this, EventArgs.Empty);

                if (this.DeckEditor.CurrentCard.Id != -1)
                {
                    this.DeckEditor.Tags.Remove(tag);
                }
            }
        }
    }
}
