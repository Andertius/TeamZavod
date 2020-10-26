using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Memento.DAL
{
    class DeckToCard
    {
        [Column("card_id")]
        public int DeckID { get; set; }

        [Column("deck_id")]
        public int CardID { get; set; }

        //[ForeignKey("CardModel")]
        public int CardId { get; set; }
        public CardModel Card { get; set; }
    }
}
