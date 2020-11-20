// <copyright file="DeckToCard.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    /// <summary>
    /// Model of DeckToCard table.
    /// </summary>
    internal class DeckToCard
    {
        /// <summary>
        /// Gets or sets deck Id of a DeckToCard row.
        /// </summary>
        [Column("card_id")]
        public int DeckID { get; set; }

        /// <summary>
        /// Gets or sets card Id of DeckToCard row.
        /// </summary>
        [Column("deck_id")]
        public int CardID { get; set; }

        // [ForeignKey("CardModel")]

        /// <summary>
        /// Gets or sets foreign key for a card.
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// Gets or sets card model of a card.
        /// </summary>
        public CardModel Card { get; set; }
    }
}
