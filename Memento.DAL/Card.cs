// <copyright file="Card.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Card : IEquatable<Card>, INotifyPropertyChanged
    {
        private int id;
        private string word;
        private string description;
        private string transcription;
        private Difficulty difficulty;
        private string imagePath;

        public Card()
        {
            this.Id = -1;
            this.Word = "";
            this.Description = "";
            this.Transcription = "";
            this.Difficulty = Difficulty.None;
            this.ImagePath = "";
            this.Tags = new ObservableCollection<string>();
        }

        public Card(string word, string description, string transcription = "", string imagePath = "", Difficulty difficulty = Difficulty.None)
        {
            this.Id = -1;
            this.Word = word;
            this.Description = description;
            this.Transcription = transcription;
            this.Difficulty = difficulty;
            this.ImagePath = imagePath;
            this.Tags = new ObservableCollection<string>();
        }

        public Card(Card card)
        {
            this.Id = card.Id;
            this.Word = card.Word;
            this.Description = card.Description;
            this.Transcription = card.Transcription;
            this.Difficulty = card.Difficulty;
            this.ImagePath = card.ImagePath;
            this.Tags = new ObservableCollection<string>(card.Tags.OrderBy(x => x));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id
        {
            get => this.id;
            set
            {
                this.id = value;
                this.OnPropertyChanged();
            }
        }

        public string Word
        {
            get => this.word;
            set
            {
                this.word = value;
                this.OnPropertyChanged();
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.OnPropertyChanged();
            }
        }

        public string Transcription
        {
            get => this.transcription;
            set
            {
                this.transcription = value;
                this.OnPropertyChanged();
            }
        }

        public Difficulty Difficulty
        {
            get => this.difficulty;
            set
            {
                this.difficulty = value;
                this.OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => this.imagePath;
            set
            {
                this.imagePath = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Tags { get; set; }

        public override string ToString()
        {
            return $"{this.Word} [{this.Transcription}] - {this.Description}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Word, this.Description);
        }

        public override bool Equals(object obj)
        {
            return obj is Card card && this.Equals(card);
        }

        public bool Equals(Card card)
        {
            return this.Id == card.Id || (this.Word == card.Word &&
                                          this.Description == card.Description &&
                                          this.Transcription == card.Transcription &&
                                          this.Difficulty == card.Difficulty &&
                                          this.ImagePath == card.ImagePath);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
