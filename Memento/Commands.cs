using System.Windows.Input;
using Memento.UserControls;

namespace Memento
{
    internal class Commands
    {
        public static readonly RoutedCommand NewCardCommand = new RoutedUICommand("New Card", nameof(NewCardCommand), typeof(DeckEditorUserControl));
        public static readonly RoutedCommand SaveCardCommand = new RoutedUICommand("Save Card", nameof(SaveCardCommand), typeof(DeckEditorUserControl));
        public static readonly RoutedCommand RemoveCardCommand = new RoutedUICommand("Remove Card", nameof(RemoveCardCommand), typeof(DeckEditorUserControl));

        public static readonly RoutedCommand SaveDeckCommand = new RoutedUICommand("Save Deck", nameof(SaveDeckCommand), typeof(DeckEditorUserControl),
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
