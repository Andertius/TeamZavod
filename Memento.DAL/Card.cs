using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Memento.DAL
{
    public class Card : IEquatable<Card>
    {
        public Card()
        {
            Id = -1;
            Word = "";
            Description = "";
            Transcription = "";
            Difficulty = Difficulty.None;
            ImagePath = "";
        }

        public Card(string word, string description, string transcription = "", string imagePath = "", Difficulty difficulty = Difficulty.None)
        {
            Id = -1;
            Word = word;
            Description = description;
            Transcription = transcription;
            Difficulty = difficulty;
            ImagePath = imagePath;
        }

        public Card(Card card)
        {
            Id = card.Id;
            Word = card.Word;
            Description = card.Description;
            Transcription = card.Transcription;
            Difficulty = card.Difficulty;
            ImagePath = card.ImagePath;
        }

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
        public Difficulty Difficulty { get; set; }

        [Column("image_path")]
        public string ImagePath { get; set; }

        public List<string> Tags { get; set; }

        //добавити масив тегів

        public override string ToString()
        {
            return $"{Word} [{Transcription}] - {Description}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Word, Description);
        }

        public override bool Equals(object obj)
        {
            return obj is Card card && Equals(card);
        }

        public bool Equals(Card card)
        {
            return Id == card.Id;
        }
    }
}
