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

    /// <summary>
    /// The main logic for the cards.
    /// </summary>
    public class Card : IEquatable<Card>, INotifyPropertyChanged
    {
        private int id;
        private string word;
        private string description;
        private string transcription;
        private Difficulty difficulty;
        private string imagePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        public Card()
        {
            this.Id = -1;
            this.Word = string.Empty;
            this.Description = string.Empty;
            this.Transcription = string.Empty;
            this.Difficulty = Difficulty.None;
            this.ImagePath = string.Empty;
            this.Tags = new ObservableCollection<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="description">The description.</param>
        /// <param name="transcription">The transcription.</param>
        /// <param name="imagePath">The image path.</param>
        /// <param name="difficulty">The difficulty.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="card">The card that should be copied.</param>
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

        /// <summary>
        /// Event that notifies the appropriate objects when a certain propery is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the card id.
        /// </summary>
        public int Id
        {
            get => this.id;
            set
            {
                this.id = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        public string Word
        {
            get => this.word;
            set
            {
                this.word = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the transcription.
        /// </summary>
        public string Transcription
        {
            get => this.transcription;
            set
            {
                this.transcription = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => this.difficulty;
            set
            {
                this.difficulty = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath
        {
            get => this.imagePath;
            set
            {
                this.imagePath = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public ObservableCollection<string> Tags { get; set; }

        /// <summary>
        /// Overrides the default ToString method.
        /// </summary>
        /// <returns>The string that represents the object.</returns>
        public override string ToString()
        {
            return $"{this.Word} [{this.Transcription}] - {this.Description}";
        }

        /// <summary>
        /// Overrides the default ToString method.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Word, this.Description);
        }

        /// <summary>
        /// Deternines whether the card equals to the object passed.
        /// </summary>
        /// <param name="obj">The object to compare the card with.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj is Card card && this.Equals(card);
        }

        /// <summary>
        /// Deternines whether the card equals to the object passed.
        /// </summary>
        /// <param name="card">The card to compare the card with.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
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
