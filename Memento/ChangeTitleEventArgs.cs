using System;

namespace Memento
{
    public class ChangeTitleEventArgs : EventArgs
    {
        public ChangeTitleEventArgs(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
