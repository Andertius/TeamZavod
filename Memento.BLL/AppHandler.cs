using System;
using System.Linq;
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

        public void Start(CardOrder order, bool showImages)
        {
            if (order == CardOrder.Random)
            {
                RadndomizeDeck();
            }
            else if (order == CardOrder.Ascending)
            {
                SortDeckByAscendingDifficulty();
            }
            else
            {
                SortDeckByDescendingDifficulty();
            }
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

        public void SortDeckByAscendingDifficulty()
        {
            for (int i = 0; i < Deck.Count - 1; i++)
            {
                for (int j = 0; j < Deck.Count - i - 1; j++)
                {
                    if (Deck[j].Difficulty > Deck[j + 1].Difficulty)
                    {
                        Card card = Deck[j];
                        Deck[j] = Deck[j + 1];
                        Deck[j + 1] = card;
                    }
                }
            }
        }

        public void SortDeckByDescendingDifficulty()
        {
            for (int i = 0; i < Deck.Count - 1; i++)
            {
                for (int j = 0; j < Deck.Count - i - 1; j++)
                {
                    if (Deck[j].Difficulty < Deck[j + 1].Difficulty)
                    {
                        Card card = Deck[j];
                        Deck[j] = Deck[j + 1];
                        Deck[j + 1] = card;
                    }
                }
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
