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
        public static readonly RoutedCommand NewCardCommand = new RoutedUICommand(
            "New Card",
            nameof(NewCardCommand),
            typeof(DeckEditorUserControl));

        public static readonly RoutedCommand NewExistingCardCommand = new RoutedUICommand(
            "New Existing Card",
            nameof(NewExistingCardCommand),
            typeof(DeckEditorUserControl));

        public static readonly RoutedCommand RemoveCardCommand = new RoutedUICommand(
            "Remove Card",
            nameof(RemoveCardCommand),
            typeof(DeckEditorUserControl));

        public static readonly RoutedCommand SaveCardCommand = new RoutedUICommand(
            "Save Card",
            nameof(SaveCardCommand),
            typeof(DeckEditorUserControl));

        public static readonly RoutedCommand RemoveDeckCommand = new RoutedUICommand(
            "Remove Deck",
            nameof(RemoveDeckCommand),
            typeof(DeckEditorUserControl));

        public static readonly RoutedCommand GoBackCommand = new RoutedUICommand(
            "Back",
            nameof(GoBackCommand),
            typeof(StatisticsUserControl),
            new InputGestureCollection(new InputGesture[]
            {
                new KeyGesture(Key.Escape),
            }));

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
