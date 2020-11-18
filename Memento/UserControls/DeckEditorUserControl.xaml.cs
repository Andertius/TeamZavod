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

        public event EventHandler<DeckEditorCardEventArgs> CardAdded;
        public event EventHandler<DeckEditorCardEventArgs> CardUpdated;
        public event EventHandler<DeckEditorRemoveCardEventArgs> CardRemoved;
        public event EventHandler<DeckEditorCardEventArgs> CardChanged;
        public event EventHandler CardCleared;
        public event EventHandler<DeckEditorDeckEventArgs> DeckChanged;
        public event EventHandler<DeckEditorDeckEventArgs> ChangesSaved;
        public event EventHandler<RemoveDeckEditorDeckEventArgs> DeckRemoved;

        public event EventHandler<ChangeTitleEventArgs> TitleChanged;
        public event EventHandler MakeMainPageVisible;

        public void GoBackButton_Click(object sender, RoutedEventArgs e)
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

        public void NewCard(object sender, RoutedEventArgs e)
        {
            if (IsCardEdited && ShowSaveWarning())
            {
                return;
            }

            CardCleared?.Invoke(this, EventArgs.Empty);
            ChangeLastSaved();
        }

        public void NewExistingCard(object sender, RoutedEventArgs e)
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

        public void SaveCard(object sender, RoutedEventArgs e)
        {
            if (DeckEditor.Deck.Contains(DeckEditor.CurrentCard))
            {
                CardUpdated?.Invoke(this, new DeckEditorCardEventArgs(DeckEditor.Deck, DeckEditor.CurrentCard));
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
            ChangeLastSaved();
        }

        public void SaveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.CurrentCard.Word != String.Empty && DeckEditor.CurrentCard.Description != String.Empty && IsCardEdited;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle)
            {
                ChangeDeck();
            }

            //handle = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            ChangeDeck();
        }

        private void ChangeDeck()
        {
            if ((IsCardEdited) && ShowSaveWarning())
            {
                return;
            }

            if (DecksCombox.SelectedItem is Deck deck)
            {
                DeckChanged?.Invoke(this, new DeckEditorDeckEventArgs(Repository.FetchDeck(deck.DeckName)));
                IsCardEdited = false;
            }
        }

        public void RemoveCard(object sender, RoutedEventArgs e)
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

        public void RemoveCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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

        public void RemoveDeckCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeckEditor.Deck.DeckName != String.Empty;
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
