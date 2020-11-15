using System;
using System.Collections.Generic;
using System.Text;

namespace Memento
{
    public class StartEditingEventArgs : EventArgs
    {
        public StartEditingEventArgs(int deckId = -1)
        {
            DeckId = deckId;
        }

        public int DeckId { get; }
    }
}
