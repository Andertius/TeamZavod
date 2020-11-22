// <copyright file="Deck.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Deck : IEquatable<Deck>, IEnumerable<Card>, ICollection<Card>, INotifyPropertyChanged
    {
        private int id;
        private string deckName;
        private string tagName;

        public Deck()
        {
            this.Id = -1;
            this.Cards = new ObservableCollection<Card>();
            this.DeckName = string.Empty;
            this.TagName = string.Empty;
            this.Cards.CollectionChanged += (sender, e) => this.OnPropertyChanged(nameof(this.Count));
        }

        public Deck(Deck deck)
        {
            this.Id = deck.Id;
            this.Cards = new ObservableCollection<Card>(deck.Cards.Select(card => new Card(card)));
            this.DeckName = deck.DeckName;
            this.TagName = deck.TagName;
            this.Cards.CollectionChanged += (sender, e) => this.OnPropertyChanged(nameof(this.Count));
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

        public int Count { get => this.Cards.Count; }

        public string DeckName
        {
            get => this.deckName;
            set
            {
                this.deckName = value;
                this.OnPropertyChanged();
            }
        }

        public string TagName
        {
            get => this.tagName;
            set
            {
                this.tagName = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<Card> Cards { get; }

        public bool IsReadOnly => false;

        public Card this[int index]
        {
            get => this.Cards[index];
            set => this.Cards[index] = value;
        }

        public void Add(Card card)
            => this.Cards.Add(card);

        public void AddRange(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                this.Add(card);
            }
        }

        public void InsertCard(int index, Card card)
            => this.Cards.Insert(index, card);

        public bool Remove(Card card)
            => this.Cards.Remove(card);

        public void RemoveAt(int index)
            => this.Cards.RemoveAt(index);

        public void MoveCard(int oldIndex, int newIndex)
        {
            var card = this.Cards[oldIndex];
            this.Cards.RemoveAt(oldIndex);
            this.Cards.Insert(newIndex, card);
        }

        public int IndexOf(Card card)
        {
            return this.Cards.IndexOf(card);
        }

        public void Clear()
            => this.Cards.Clear();

        public bool Contains(Card card)
            => this.Cards.Contains(card);

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

        public override string ToString()
            => this.DeckName;

        public override int GetHashCode()
            => HashCode.Combine(this.Id, this.Cards, this.DeckName, this.TagName);

        public override bool Equals(object obj)
            => obj is Deck deck && this.Equals(deck);

        public bool Equals(Deck deck)
            => this.DeckName == deck.DeckName;

        public IEnumerator<Card> GetEnumerator()
            => this.Cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
