using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Memento.DAL;

namespace Memento.BLL
{
    public class DeckEditor
    {
        public DeckEditor()
        {
            Deck = new Deck();
            AllDecks = Repository.FetchAllDecks().ToList();
        }

        public DeckEditor(Deck deck)
        {
            Deck = new Deck(deck);
            AllDecks = Repository.FetchAllDecks().ToList();
        }

        public DeckEditor(string deckName)
        {
            Deck = Repository.FetchDeck(deckName);
            AllDecks = Repository.FetchAllDecks().ToList();
        }

        public DeckEditor(int deckId)
        {
            Deck = Repository.FetchDeck(deckId);
            AllDecks = Repository.FetchAllDecks().ToList();
        }

        public Deck Deck { get; private set; }
        public List<Deck> AllDecks { get; private set; }

        public void AddCard(object sender, DeckEditorCardEventArgs e)
        {
            if (!Deck.Contains(e.Card))
            {
                if (e.Card.Id == -1)
                {
                    Repository.AddCard(e.Card);
                }

                Deck.Add(e.Card);
                return;
            }

            throw new ArgumentException("The card is already in the deck");
        }

        public void UpdateCard(object sender, DeckEditorCardEventArgs e)
        {
            Deck[Deck.IndexOf(e.Card)] = new Card(e.Card);
        }

        public void RemoveCard(object sender, DeckEditorRemoveCardEventArgs e)
        {
            e.CardRemoved = Deck.Remove(e.Card);
        }

        public void ChangeDeck(object sender, DeckEditorDeckEventArgs e)
        {
            Deck = new Deck(e.Deck);
        }

        public void SaveChanges(object sender, DeckEditorDeckEventArgs e)
        {
            Repository.UpdateDeck(e.Deck.Id, e.Deck);
        }
    }
}
