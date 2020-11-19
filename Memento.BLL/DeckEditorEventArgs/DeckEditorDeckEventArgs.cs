// <copyright file="DeckEditorDeckEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.BLL.DeckEditorEventArgs
{
    using System;

    using Memento.DAL;

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
            this.Deck = new Deck(deck);
        }

        /// <summary>
        /// Gets the deck.
        /// </summary>
        public Deck Deck { get; }
    }
}
