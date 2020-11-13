using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Memento.DAL
{
    public class Deck : IEquatable<Deck>, IEnumerable<Card>, ICollection<Card>, INotifyPropertyChanged
    {
        private int id;
        private string deckName;
        private string tagName;

        public Deck()
        {
            Id = -1;
            Cards = new ObservableCollection<Card>();
            DeckName = "";
            TagName = "";
            Cards.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(Count));
        }

        public Deck(Deck deck)
        {
            Id = deck.Id;
            Cards = new ObservableCollection<Card>(deck.Cards.Select(card => new Card(card)));
            DeckName = deck.DeckName;
            TagName = deck.TagName;
            Cards.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(Count));
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Card> Cards { get; }
        public int Count { get => Cards.Count; }

        public string DeckName
        {
            get => deckName;
            set
            {
                deckName = value;
                OnPropertyChanged();
            }
        }

        public string TagName
        {
            get => tagName;
            set
            {
                tagName = value;
                OnPropertyChanged();
            }
        }

        public bool IsReadOnly => false;

        public Card this[int index]
        {
            get => Cards[index];
            set => Cards[index] = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(Card card)
            => Cards.Add(card);

        public void AddRange(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                Add(card);
            }
        }

        public void InsertCard(int index, Card card)
            => Cards.Insert(index, card);

        public bool Remove(Card card)
            => Cards.Remove(card);

        public void RemoveAt(int index)
            => Cards.RemoveAt(index);

        public void MoveCard(int oldIndex, int newIndex)
        {
            var card = Cards[oldIndex];
            Cards.RemoveAt(oldIndex);
            Cards.Insert(newIndex, card);
        }

        public int IndexOf(Card card)
        {
            return Cards.IndexOf(card);
        }

        public void Clear()
            => Cards.Clear();

        public bool Contains(Card card)
            => Cards.Contains(card);

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
            else if (Count > array.Length - arrayIndex + 1)
            {
                throw new ArgumentException("The destination array has fewer elements than the collection.");
            }

            for (int i = 0; i < Cards.Count; i++)
            {
                array[i + arrayIndex] = Cards[i];
            }
        }

        public override string ToString()
            => DeckName;

        public override int GetHashCode()
            => HashCode.Combine(Id, Cards, DeckName, TagName);

        public override bool Equals(object obj)
            => obj is Deck deck && Equals(deck);

        public bool Equals(Deck deck)
            => DeckName == deck.DeckName;

        public IEnumerator<Card> GetEnumerator()
            => Cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
