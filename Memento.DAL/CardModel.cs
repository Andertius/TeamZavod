using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memento.DAL
{
    class CardModel
    {
        [Key]
        [Column("card_id")]
        public int Id { get; set; }

        [Column("word")]
        public string Word { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("transcription")]
        public string Transcription { get; set; }

        [Column("difficulty_level")]
        public string Difficulty { get; set; }

        [Column("image_path")]
        public string ImagePath { get; set; }

        //[ForeignKey("CardModelFK")]
        public List<TagToCardModel> Tags { get; set; }
        public List<DeckToCardModel> Decks { get; set; }
    }
}
