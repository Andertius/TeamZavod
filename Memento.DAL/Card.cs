// <copyright file="Card.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Memento.DAL
{
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
            Id = DefaultId;
            Word = String.Empty;
            Description = String.Empty;
            Transcription = String.Empty;
            Difficulty = Difficulty.None;
            ImagePath = String.Empty;
            Tags = new ObservableCollection<string>();
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
            Id = DefaultId;
            Word = word;
            Description = description;
            Transcription = transcription;
            Difficulty = difficulty;
            ImagePath = imagePath;
            Tags = new ObservableCollection<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="card">The card that should be copied.</param>
        public Card(Card card)
        {
            Id = card.Id;
            Word = card.Word;
            Description = card.Description;
            Transcription = card.Transcription;
            Difficulty = card.Difficulty;
            ImagePath = card.ImagePath;
            Tags = new ObservableCollection<string>(card.Tags.OrderBy(x => x));
        }

        /// <summary>
        /// Event that notifies the appropriate objects when a certain propery is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the default id of the card that is not in the database.
        /// </summary>
        public static int DefaultId => -1;

        /// <summary>
        /// Gets or sets the card id.
        /// </summary>
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        public string Word
        {
            get => word;
            set
            {
                word = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the transcription.
        /// </summary>
        public string Transcription
        {
            get => transcription;
            set
            {
                transcription = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        public Difficulty Difficulty
        {
            get => difficulty;
            set
            {
                difficulty = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the image path.
        /// </summary>
        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
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
            return $"{Word} [{Transcription}] - {Description}";
        }

        /// <summary>
        /// Overrides the default ToString method.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Word, Description);
        }

        /// <summary>
        /// Deternines whether the card equals to the object passed.
        /// </summary>
        /// <param name="obj">The object to compare the card with.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
        public override bool Equals(object obj)
        {
            return obj is Card card && Equals(card);
        }

        /// <summary>
        /// Deternines whether the card equals to the object passed.
        /// </summary>
        /// <param name="card">The card to compare the card with.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
        public bool Equals(Card card)
        {
            return Id == card.Id ||
                (Word == card.Word &&
                Description == card.Description &&
                Transcription == card.Transcription &&
                Difficulty == card.Difficulty &&
                ImagePath == card.ImagePath);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
