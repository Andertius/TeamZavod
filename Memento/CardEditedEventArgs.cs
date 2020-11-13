using System;

namespace Memento
{
    public class CardEditedEventArgs : EventArgs
    {
        public CardEditedEventArgs(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
