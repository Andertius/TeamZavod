// <copyright file="DeckToCardModel.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Memento.DAL
{
    /// <summary>
    /// Model of DeckToCard table.
    /// </summary>
    internal class DeckToCardModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckToCardModel"/> class.
        /// Model of DeckToCard table.
        /// </summary>
        public DeckToCardModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckToCardModel"/> class.
        /// Model of DeckToCard table.
        /// </summary>
        /// <param name="cardId">id of a card.</param>
        /// <param name="deckId">id o a deck.</param>
        public DeckToCardModel(int deckId, int cardId)
        {
            this.DeckID = deckId;
            this.CardID = cardId;
        }

        /// <summary>
        /// Gets or sets deck id.
        /// </summary>
        [Column("deck_id")]
        public int DeckID { get; set; }

        /// <summary>
        /// Gets or sets card id.
        /// </summary>
        [Column("card_id")]
        public int CardID { get; set; }

        // [ForeignKey("CardModel")]

        /// <summary>
        /// Gets or sets foreign key to a card model.
        /// </summary>
        [NotMapped]
        public int CardId { get; set; }

        /// <summary>
        /// Gets or sets a card model.
        /// </summary>
        public CardModel Card { get; set; }

        /// <summary>
        /// Gets or sets foreign key to a deck model.
        /// </summary>
        [NotMapped]
        public int DeckId { get; set; }

        /// <summary>
        /// Gets or sets a deck model.
        /// </summary>
        public DeckModel Deck { get; set; }
    }
}
