// <copyright file="DeckEditorCardEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

using Memento.DAL;

namespace Memento.BLL.DeckEditorEventArgs
{
    /// <summary>
    /// Event arguments for anything related to a card.
    /// </summary>
    public class DeckEditorCardEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditorCardEventArgs"/> class.
        /// </summary>
        /// <param name="deck">The deck.</param>
        /// <param name="card">The card.</param>
        public DeckEditorCardEventArgs(Deck deck, Card card)
        {
            Card = new Card(card);
            Deck = new Deck(deck);
        }

        /// <summary>
        /// Gets the current card.
        /// </summary>
        public Card Card { get; }

        /// <summary>
        /// Gets the current deck.
        /// </summary>
        public Deck Deck { get; }
    }
}
