using Memento.DAL;
using System;

namespace Memento.BLL
{
    public class DeckEditorCardEventArgs : EventArgs
    {
        public DeckEditorCardEventArgs(Deck deck, Card card)
        {
            Card = new Card(card);
            Deck = new Deck(deck);
        }

        public Card Card { get; }
        public Deck Deck { get; }
    }

    public class DeckEditorRemoveCardEventArgs : EventArgs
    {
        public DeckEditorRemoveCardEventArgs(Deck deck, Card card)
        {
            Card = new Card(card);
            Deck = new Deck(deck);
        }

        public Card Card { get; }
        public Deck Deck { get; }
        public bool CardRemoved { get; set; }
    }

    public class DeckEditorDeckEventArgs : EventArgs
    {
        public DeckEditorDeckEventArgs(Deck deck)
        {
            Deck = new Deck(deck);
        }

        public Deck Deck { get; }
    }

    public class RemoveDeckEditorDeckEventArgs : EventArgs
    {
        public RemoveDeckEditorDeckEventArgs(Deck deck)
        {
            Deck = new Deck(deck);
            Removed = false;
        }

        public Deck Deck { get; }
        public bool Removed { get; set; }
    }
}
