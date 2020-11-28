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
        /// <param name="cardId">The id of the card that should be updated.</param>
        /// <param name="card">Card information that should be transferred to the card.</param>
        public UpdateCardDeckEditorEventArgs(int cardId, Card card)
        {
            CardId = cardId;
            Card = new Card(card);
        }

        /// <summary>
        /// Gets the id of the card that should be updated.
        /// </summary>
        public int CardId { get; }

        /// <summary>
        /// Gets the card information that should be transferred to the card.
        /// </summary>
        public Card Card { get; }
    }
}
