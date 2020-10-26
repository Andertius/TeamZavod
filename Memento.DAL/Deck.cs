using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Memento.DAL
{
    public class Deck : IEquatable<Deck>, IEnumerable<Card>, ICollection<Card>
    {
        public Deck()
        {
            Id = -1;
            Cards = new List<Card>();
            DeckName = "";
            TagName = "";
        }

        public Deck(Deck deck)
        {
            Id = deck.Id;
            Cards = new List<Card>(deck.Cards.Select(card => new Card(card)));
            DeckName = deck.DeckName;
            TagName = deck.TagName;
        }

        public int Id { get; set; }
        public List<Card> Cards { get; }
        public string DeckName { get; set; }
        public int Count { get => Cards.Count; }
        public string TagName { get; set; }
        public bool IsReadOnly => false;

        public Card this[int index]
        {
            get => Cards[index];
            set => Cards[index] = value;
        }

        public void Add(Card card)
            => Cards.Add(card);

        public void AddRange(IEnumerable<Card> cards)
            => Cards.AddRange(cards);

        public void InsertCard(int index, Card card)
            => Cards.Insert(index, card);

        public void InsertRange(int index, IEnumerable<Card> cards)
            => Cards.InsertRange(index, cards);

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
                throw new ArgumentNullException("The array cannot be null.");
            }
            else if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
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
        {
            string result = "";

            foreach (var card in Cards)
            {
                result += $"{card}\n";
            }

            return result;
        }

        public override int GetHashCode()
            => HashCode.Combine(Id, Cards, DeckName, TagName);

        public override bool Equals(object obj)
            => obj is Deck deck && Equals(deck);

        public bool Equals(Deck deck)
            => Id == deck.Id;

        public IEnumerator<Card> GetEnumerator()
            => Cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
