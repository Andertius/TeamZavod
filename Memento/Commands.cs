// <copyright file="Commands.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System.Windows.Input;

using Memento.UserControls;

namespace Memento
{
    /// <summary>
    /// Command definitions.
    /// </summary>
    internal class Commands
    {
        /// <summary>
        /// Command to clear the current card.
        /// </summary>
        public static readonly RoutedCommand NewCardCommand = new RoutedUICommand(
            "New Card",
            nameof(NewCardCommand),
            typeof(DeckEditorUserControl));

        /// <summary>
        /// Command to add an existing card from the database.
        /// </summary>
        public static readonly RoutedCommand NewExistingCardCommand = new RoutedUICommand(
            "New Existing Card",
            nameof(NewExistingCardCommand),
            typeof(DeckEditorUserControl));

        /// <summary>
        /// Command to remove the current card from the deck.
        /// </summary>
        public static readonly RoutedCommand RemoveCardCommand = new RoutedUICommand(
            "Remove Card",
            nameof(RemoveCardCommand),
            typeof(DeckEditorUserControl));

        /// <summary>
        /// Command to save the card to the deck.
        /// </summary>
        public static readonly RoutedCommand SaveCardCommand = new RoutedUICommand(
            "Save Card",
            nameof(SaveCardCommand),
            typeof(DeckEditorUserControl),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.S, ModifierKeys.Control) }));

        /// <summary>
        /// Command to remove the current deck from the database.
        /// </summary>
        public static readonly RoutedCommand RemoveDeckCommand = new RoutedUICommand(
            "Remove Deck",
            nameof(RemoveDeckCommand),
            typeof(DeckEditorUserControl));

        /// <summary>
        /// Command to edit the name and/or the tag name of the current deck.
        /// </summary>
        public static readonly RoutedCommand EditDeckCommand = new RoutedUICommand(
            "Edit Deck",
            nameof(EditDeckCommand),
            typeof(DeckEditorUserControl));

        /// <summary>
        /// Command to exit to the main menu from the deck editor.
        /// </summary>
        public static readonly RoutedCommand GoBackCommand = new RoutedUICommand(
            "Back",
            nameof(GoBackCommand),
            typeof(StatisticsUserControl),
            new InputGestureCollection(new InputGesture[] { new KeyGesture(Key.Escape) }));

        /// <summary>
        /// Command to exit programm.
        /// </summary>
        public static readonly RoutedCommand ExitCommand = new RoutedUICommand(
            "Exit",
            nameof(ExitCommand),
            typeof(MainPageUserControl));

        /// <summary>
        /// Command to start learning.
        /// </summary>
        public static readonly RoutedCommand StartLearningCommand = new RoutedUICommand(
            "Start",
            nameof(StartLearningCommand),
            typeof(MainPageUserControl));

        /// <summary>
        /// Command to open help menu.
        /// </summary>
        public static readonly RoutedCommand HelpCommand = new RoutedUICommand(
            "Help",
            nameof(HelpCommand),
            typeof(MainPageUserControl));
    }
}
