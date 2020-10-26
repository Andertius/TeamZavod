using System;
using System.Collections.Generic;
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
        }

        public DeckEditor(Deck deck)
        {
            Deck = new Deck(deck);
        }

        public DeckEditor(string deckName)
        {
            Deck = Repository.FetchDeck(deckName);
        }

        public DeckEditor(int deckId)
        {
            Deck = Repository.FetchDeck(deckId);
        }

        public Deck Deck { get; private set; }

        public void AddCard(object sender, DeckEditorAddCardEventArgs e)
        {
            if (e.Card.Id == -1)
            {
                e.Card.Id = Repository.AddCard(e.Card);
            }

            Deck.Add(e.Card);
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

        public void ExitEditor(object sender, ExitDeckEditorEventArgs e)
        {
             
        }
    }
}
