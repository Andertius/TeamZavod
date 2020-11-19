// <copyright file="DeckEditorCardEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.BLL.DeckEditorEventArgs
{
    using System;

    using Memento.DAL;

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
            this.Card = new Card(card);
            this.Deck = new Deck(deck);
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
