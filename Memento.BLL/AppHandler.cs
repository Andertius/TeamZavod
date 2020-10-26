using System;
using System.Collections.Generic;
using System.Text;

using Memento.DAL;

namespace Memento.BLL
{
    public class AppHandler
    {
        public AppHandler(Deck deck)
        {
            Deck = new Deck(deck);
        }

        public AppHandler(int deckId)
        {
            Deck = new Deck(Repository.FetchDeck(deckId));
        }

        public AppHandler(string deckName)
        {
            Deck = new Deck(Repository.FetchDeck(deckName));
        }

        public Deck Deck { get; }

        public void Start()
        {
            //шафл деки

        }

        //flip card
        public void FlipCard(object sender, AppHandlerFlipEventArgs e)
        {
            e.isFlipped = !e.isFlipped;
        }

        //nextCard
        public void NextCard(object sender, AppHandlerMoveCardEventArgs e)
        {
            if (e.Card.Id != -1 && Deck.Cards.IndexOf(e.Card) < Deck.Count)
            {
                e.Card = Deck[e.Card.Id + 1];
            }
        }

        //MoveCardIntoDeck
        public void MoveCardIntoDeck(object sender, AppHandlerMoveCardEventArgs e)
        {
            if (e.Card.Id != -1 && Deck.Cards.IndexOf(e.Card) < Deck.Count- Deck.Count/10)
            {
                if (e.RememberValue == 3)
                {
                    Deck.Remove(e.Card);
                    Deck.InsertCard(Deck.Count - Deck.Count / 5, e.Card);
                }
                else if (e.RememberValue == 2)
                {
                    Deck.Remove(e.Card);
                    Deck.InsertCard(Deck.Count - Deck.Count / 3, e.Card);
                }
                else if (e.RememberValue == 1)
                {
                    Deck.Remove(e.Card);
                    Deck.InsertCard(Deck.Count - 2 * Deck.Count / 3, e.Card);
                }
            }
            else
            {
                Deck.Remove(e.Card);
                Deck.Add(e.Card);
            }
        }

        //Cancel
        //{canelationtoken=stop}


    }
}
