using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Memento.DAL
{
    class DeckToCardModel
    {
        [Column("card_id")]
        public int DeckID { get; set; }

        [Column("deck_id")]
        public int CardID { get; set; }

        //[ForeignKey("CardModel")]
        [NotMapped]
        public int CardId { get; set; }

        public CardModel Card { get; set; }
        
        [NotMapped]
        public int DeckId { get; set; }

        public DeckModel Deck { get; set; }
    }
}
