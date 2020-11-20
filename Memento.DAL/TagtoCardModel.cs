// <copyright file="TagtoCardModel.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// model representative of TagToCard table.
    /// </summary>
    internal class TagToCardModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagToCardModel"/> class.
        /// </summary>
        public TagToCardModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagToCardModel"/> class.
        /// </summary>
        /// <param name="cardId">card if.</param>
        /// <param name="tag">tag name.</param>
        public TagToCardModel(int cardId, string tag)
        {
            CardID = cardId;
            TagName = tag;
        }

        /// <summary>
        /// Gets or sets tag name of TagtoCard table.
        /// </summary>
        [Column("tag_name")]
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets card id of TagtoCard table.
        /// </summary>
        [Column("card_id")]
        public int CardID { get; set; }

        /// <summary>
        /// Gets or sets foreign key to Card model.
        /// </summary>
        [NotMapped]
        public int CardId { get; set; }

        /// <summary>
        /// Gets or sets Card model.
        /// </summary>
        public CardModel Card { get; set; }

    }
}
