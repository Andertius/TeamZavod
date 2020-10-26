using System;

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
            RadndomizeDeck();
        }

        public void Stop()
        {
           
        }

        public void RadndomizeDeck()
        {
            Random rand = new Random();
            for (int i = 0; i < Deck.Count - 1; i++)
            {
                int j = rand.Next(i, Deck.Count - 1);
                Card card = Deck[i];
                Deck[i] = Deck[j];
                Deck[j] = card;
            }
        }
        public void FlipCard(object sender, AppHandlerFlipEventArgs e)
        {
            e.IsFlipped = !e.IsFlipped;
        }


        public void NextCard(object sender, AppHandlerNextCardEventArgs e)
        {
            if (e.Card.Id != -1 && Deck.Cards.IndexOf(e.Card) < Deck.Count)
            {
                Deck.Remove(e.Card);
                e.Card = Deck[0];
            }
        }


        public void MoveCardIntoDeck(object sender, AppHandlerMoveCardEventArgs e)
        {
            if (e.Card.Id != -1 && Deck.Cards.IndexOf(e.Card) < Deck.Count - Deck.Count / 10)
            {
                if (e.RememberValue == (int)RememberingLevels.Trivial)
                {
                    Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - Deck.Count / 5);
                }
                else if (e.RememberValue == (int)RememberingLevels.GotIt)
                {
                    Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - Deck.Count / 3);
                }
                else if (e.RememberValue == (int)RememberingLevels.Again)
                {
                    Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - 2 * Deck.Count / 3);
                }
            }
            else
            {
                Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - 1);
            }
        }



    }
}
