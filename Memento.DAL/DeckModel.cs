// <copyright file="DeckModel.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    /// <summary>
    /// model that takes data from Deck table.
    /// </summary>
    internal class DeckModel
    {
        /// <summary>
        /// Gets or sets Id of a Deck.
        /// </summary>
        [Key]
        [Column("deck_id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Tag name of a Deck.
        /// </summary>
        [Column("tag_name")]
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets Name of a Decks.
        /// </summary>
        [Column("deck_name")]
        public string DeckName { get; set; }

        /// <summary>
        /// Gets or sets number of cards of a Deck.
        /// </summary>
        [Column("number_of_cards")]
        public int NumberOfCards { get; set; }

        /// <summary>
        /// Gets or sets List of cards in a Deck.
        /// </summary>
        public List<DeckToCardModel> Cards { get; set; }
    }
}
