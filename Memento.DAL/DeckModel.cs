using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Memento.DAL
{
    class DeckModel
    {
        [Key]
        [Column("deck_id")]
        public int Id { get; set; }

        [Column("tag_name")]
        public string TagName { get; set; }
        
        [Column("deck_name")]
        public string DeckName { get; set; }
        
        [Column("number_of_cards")]
        public int NumberOfCards { get; set; }

        public List<DeckToCardModel> Cards { get; set; }

    }
}
