using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Memento
{
    internal class Commands
    {
        public static readonly RoutedCommand SaveCardCommand = new RoutedUICommand("Save Card", nameof(SaveCardCommand), typeof(MainWindow));
        public static readonly RoutedCommand RemoveCardCommand = new RoutedUICommand("Remove Card", nameof(RemoveCardCommand), typeof(MainWindow));

        public static readonly RoutedCommand SaveDeckCommand = new RoutedUICommand("Save Deck", nameof(SaveDeckCommand), typeof(MainWindow),
        new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.S, ModifierKeys.Control)
        }));

        public static readonly RoutedCommand ExitCommand = new RoutedUICommand("Exit", nameof(ExitCommand), typeof(MainWindow),
        new InputGestureCollection(new InputGesture[]
        {
            new KeyGesture(Key.Escape)
        }));

        public static readonly RoutedCommand StartLearningCommand = new RoutedUICommand("Start", nameof(StartLearningCommand), typeof(MainWindow));
    }
}
