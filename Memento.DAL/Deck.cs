// <copyright file="Deck.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Memento.DAL
{
    /// <summary>
    /// The main logic for the deck.
    /// </summary>
    public class Deck : IEquatable<Deck>, IEnumerable<Card>, ICollection<Card>, INotifyPropertyChanged
    {
        private int id;
        private string deckName;
        private string tagName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        public Deck()
        {
            this.Id = -1;
            this.Cards = new ObservableCollection<Card>();
            this.DeckName = string.Empty;
            this.TagName = string.Empty;
            this.Cards.CollectionChanged += (sender, e) => this.OnPropertyChanged(nameof(this.Count));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="deck">The deck that shold be copied.</param>
        public Deck(Deck deck)
        {
            this.Id = deck.Id;
            this.Cards = new ObservableCollection<Card>(deck.Cards.Select(card => new Card(card)));
            this.DeckName = deck.DeckName;
            this.TagName = deck.TagName;
            this.Cards.CollectionChanged += (sender, e) => this.OnPropertyChanged(nameof(this.Count));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event that notifies the appropriate objects when a certain propery is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the id of the deck.
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
        /// Gets the count of cards in the deck.
        /// </summary>
        public int Count { get => this.Cards.Count; }

        /// <summary>
        /// Gets or sets the name of the deck.
        /// </summary>
        public string DeckName
        {
            get => this.deckName;
            set
            {
                this.deckName = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the tag name of the deck.
        /// </summary>
        public string TagName
        {
            get => this.tagName;
            set
            {
                this.tagName = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the cards in the deck.
        /// </summary>
        public ObservableCollection<Card> Cards { get; }

        /// <summary>
        /// Gets a value indicating whether the collection is read only or not.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the card with the given index.
        /// </summary>
        /// <param name="index">The index of a card to be returned.</param>
        /// <returns>A card with the given index.</returns>
        public Card this[int index]
        {
            get => this.Cards[index];
            set => this.Cards[index] = value;
        }

        /// <summary>
        /// Adds a card to the end of the deck.
        /// </summary>
        /// <param name="card">The card to be added.</param>
        public void Add(Card card)
            => this.Cards.Add(card);

        /// <summary>
        /// Adds a range of cards to the end of the deck.
        /// </summary>
        /// <param name="cards">Ther range of cards to be added.</param>
        public void AddRange(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                this.Add(card);
            }
        }

        /// <summary>
        /// Inserts a card at the given index.
        /// </summary>
        /// <param name="index">The index where the card should be inserted.</param>
        /// <param name="card">The card to be inserted.</param>
        public void InsertCard(int index, Card card)
            => this.Cards.Insert(index, card);

        /// <summary>
        /// Removes the first occurance of the card.
        /// </summary>
        /// <param name="card">The card to be removed.</param>
        /// <returns><see cref="true"/> if the card is successfully removed, otherwise <see cref="false"/>. Also returns <see cref="false"/> if the card was not found in the deck.</returns>
        public bool Remove(Card card)
            => this.Cards.Remove(card);

        /// <summary>
        /// Removes a card located at the given index.
        /// </summary>
        /// <param name="index">The index at which the card should be removed.</param>
        public void RemoveAt(int index)
            => this.Cards.RemoveAt(index);

        /// <summary>
        /// Moves a card from one index to another.
        /// </summary>
        /// <param name="oldIndex">The old location of a card.</param>
        /// <param name="newIndex">The new location of a card.</param>
        public void MoveCard(int oldIndex, int newIndex)
        {
            var card = this.Cards[oldIndex];
            this.Cards.RemoveAt(oldIndex);
            this.Cards.Insert(newIndex, card);
        }

        /// <summary>
        /// Gets the index of a card.
        /// </summary>
        /// <param name="card">The card, index of which should be returned.</param>
        /// <returns>The index of the given card.</returns>
        public int IndexOf(Card card)
        {
            return this.Cards.IndexOf(card);
        }

        /// <summary>
        /// Clears the deck.
        /// </summary>
        public void Clear()
            => this.Cards.Clear();

        /// <summary>
        /// Determines whether the deck contains the given card.
        /// </summary>
        /// <returns>A value indicating whether the deck contains the given card.</returns>
        /// <param name="card">The card to check.</param>
        public bool Contains(Card card)
            => this.Cards.Contains(card);

        /// <summary>
        /// Copies the deck into an array at the given index.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The index at which the copying should start.</param>
        public void CopyTo(Card[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            else if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(array), "The starting array index cannot be negative.");
            }
            else if (this.Count > array.Length - arrayIndex + 1)
            {
                throw new ArgumentException("The destination array has fewer elements than the collection.");
            }

            for (int i = 0; i < this.Cards.Count; i++)
            {
                array[i + arrayIndex] = this.Cards[i];
            }
        }

        /// <summary>
        /// Overrides the default ToString method.
        /// </summary>
        /// <returns>The string that represents the object.</returns>
        public override string ToString()
            => this.DeckName;

        /// <summary>
        /// Overrides the default ToString method.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
            => HashCode.Combine(this.Id, this.Cards, this.DeckName, this.TagName);

        /// <summary>
        /// Deternines whether the card equals to the object passed.
        /// </summary>
        /// <param name="obj">The object to compare the card with.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
        public override bool Equals(object obj)
            => obj is Deck deck && this.Equals(deck);

        /// <summary>
        /// Deternines whether the card equals to the object passed.
        /// </summary>
        /// <param name="deck">The deck to compare the card with.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
        public bool Equals(Deck deck)
            => this.DeckName == deck.DeckName;

        /// <summary>
        /// Returns an enumerator that iterates through cards.
        /// </summary>
        /// <returns>An <see cref="IEnumerator$lt;out Card$gt;"/> for the <see cref="Deck"/>.</returns>
        public IEnumerator<Card> GetEnumerator()
            => this.Cards.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through cards.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that enumerates through a <see cref="Deck"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
