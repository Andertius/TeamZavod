// <copyright file="UpdateCardDeckEditorEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

using Memento.DAL;

namespace Memento.BLL.DeckEditorEventArgs
{
    /// <summary>
    /// Event arguments for updating a card.
    /// </summary>
    public class UpdateCardDeckEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCardDeckEditorEventArgs"/> class.
        /// </summary>
        /// <param name="card">The card that should be updated.</param>
        /// <param name="newCard">Card information that should be transferred to the card.</param>
        public UpdateCardDeckEditorEventArgs(Card card, Card newCard)
        {
            Card = new Card(card);
            NewCard = new Card(newCard);
        }

        /// <summary>
        /// Gets the the card that should be updated.
        /// </summary>
        public Card Card { get; }

        /// <summary>
        /// Gets the card information that should be transferred to the card.
        /// </summary>
        public Card NewCard { get; }
    }
}
