using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.BLL
{
    public class Card
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

        public int Id { get; set; }
        public string Word { get; set; }
        public string Description { get; set; }
        public string Transcription { get; set; }
        public Difficulty Difficulty { get; set; }
        public string ImagePath { get; set; }

        public override string ToString()
        {
            return $"{Word} [{Transcription}] - {Description}";
        }


    }
}
