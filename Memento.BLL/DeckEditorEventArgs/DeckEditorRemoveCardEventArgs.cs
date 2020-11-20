// <copyright file="DeckEditorRemoveCardEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

using Memento.DAL;

namespace Memento.BLL.DeckEditorEventArgs
{
    /// <summary>
    /// Event arguments for deleting a card.
    /// </summary>
    public class DeckEditorRemoveCardEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditorRemoveCardEventArgs"/> class.
        /// </summary>
        /// <param name="deck">The deck from which the card will removed.</param>
        /// <param name="card">The card to be removed.</param>
        public DeckEditorRemoveCardEventArgs(Deck deck, Card card)
        {
            Card = new Card(card);
            Deck = new Deck(deck);
        }

        /// <summary>
        /// Gets the card to be removed.
        /// </summary>
        public Card Card { get; }

        /// <summary>
        /// Gets the deck from which the card should be removed.
        /// </summary>
        public Deck Deck { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the card was removed or not.
        /// </summary>
        public bool CardRemoved { get; set; }
    }
}
