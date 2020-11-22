// <copyright file="DeckEditorDeckEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

using Memento.DAL;

namespace Memento.BLL.DeckEditorEventArgs
{
    /// <summary>
    /// Event arguments for anything related to a deck.
    /// </summary>
    public class DeckEditorDeckEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditorDeckEventArgs"/> class.
        /// </summary>
        /// <param name="deck">The deck, i guess.</param>
        public DeckEditorDeckEventArgs(Deck deck)
        {
            Deck = new Deck(deck);
        }

        /// <summary>
        /// Gets the deck.
        /// </summary>
        public Deck Deck { get; }
    }
}
