// <copyright file="StartEditingEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento
{
    using System;

    public class StartEditingEventArgs : EventArgs
    {
        public StartEditingEventArgs(int deckId = -1)
        {
            this.DeckId = deckId;
        }

        public int DeckId { get; }
    }
}
