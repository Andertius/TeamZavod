// <copyright file="CardModel.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Card Model for Database that is linked to Card tablefrom DB.
    /// </summary>
    internal class CardModel
    {
        /// <summary>
        /// Gets or sets Id of a Card.
        /// </summary>
        [Key]
        [Column("card_id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets word of a Card.
        /// </summary>
        [Column("word")]
        public string Word { get; set; }

        /// <summary>
        /// Gets or sets description of a Card.
        /// </summary>
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets transcription of a Card.
        /// </summary>
        [Column("transcription")]
        public string Transcription { get; set; }

        /// <summary>
        /// Gets or sets difficulty level of a Card.
        /// </summary>
        [Column("difficulty_level")]
        public string Difficulty { get; set; }

        /// <summary>
        /// Gets or sets image path of Card.
        /// </summary>
        [Column("image_path")]
        public string ImagePath { get; set; }

        // [ForeignKey("CardModelFK")]

        /// <summary>
        /// Gets or sets list of tags of a Card.
        /// </summary>
        public List<TagToCardModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets list of decks where Card is.
        /// </summary>
        public List<DeckToCardModel> Decks { get; set; }
    }
}
