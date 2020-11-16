using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Memento.DAL
{
    class DeckToCardModel
    {
        public DeckToCardModel()
        { }

        public DeckToCardModel(int deckId, int cardId)
        {
            DeckID = deckId;
            CardID = cardId;
        }

        [Column("deck_id")]
        public int DeckID { get; set; }

        [Column("card_id")]
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
