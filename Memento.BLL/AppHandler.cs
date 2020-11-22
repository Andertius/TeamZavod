namespace Memento.BLL
{
    using System;
    using Memento.BLL.AppHandlerEventArgs;
    using Memento.DAL;

    public class AppHandler
    {
        public AppHandler(Deck deck)
        {
            this.Deck = new Deck(deck);
        }

        public AppHandler(int deckId)
        {
            this.Deck = new Deck(Repository.FetchDeck(deckId));
        }

        public AppHandler(string deckName)
        {
            this.Deck = new Deck(Repository.FetchDeck(deckName));
        }

        public Deck Deck { get; }

        public void Start(CardOrder order, bool showImages)
        {
            if (order == CardOrder.Random)
            {
                this.RadndomizeDeck();
            }
            else if (order == CardOrder.Ascending)
            {
                this.SortDeckByAscendingDifficulty();
            }
            else
            {
                this.SortDeckByDescendingDifficulty();
            }
        }

        public void Stop()
        {
           
        }

        public void RadndomizeDeck()
        {
            Random rand = new Random();
            for (int i = 0; i < this.Deck.Count - 1; i++)
            {
                int j = rand.Next(i, Deck.Count - 1);
                Card card = this.Deck[i];
                this.Deck[i] = this.Deck[j];
                this.Deck[j] = card;
            }
        }

        public void SortDeckByAscendingDifficulty()
        {
            for (int i = 0; i < this.Deck.Count - 1; i++)
            {
                for (int j = 0; j < this.Deck.Count - i - 1; j++)
                {
                    if (this.Deck[j].Difficulty > this.Deck[j + 1].Difficulty)
                    {
                        Card card = this.Deck[j];
                        this.Deck[j] = this.Deck[j + 1];
                        this.Deck[j + 1] = card;
                    }
                }
            }
        }

        public void SortDeckByDescendingDifficulty()
        {
            for (int i = 0; i < this.Deck.Count - 1; i++)
            {
                for (int j = 0; j < this.Deck.Count - i - 1; j++)
                {
                    if (this.Deck[j].Difficulty < this.Deck[j + 1].Difficulty)
                    {
                        Card card = this.Deck[j];
                        this.Deck[j] = this.Deck[j + 1];
                        this.Deck[j + 1] = card;
                    }
                }
            }
        }

        public void FlipCard(object sender, AppHandlerFlipEventArgs e)
        {
            e.IsFlipped = !e.IsFlipped;
        }

        //public void NextCard(object sender, AppHandlerMoveCardEventArgs e)
        //{
        //    if (e.Card.Id != -1 && Deck.Cards.IndexOf(e.Card) < Deck.Count)
        //    {
        //        this.Deck.Remove(e.Card);
        //        e.Card = Deck[0];
        //    }
        //}

        public void MoveCardIntoDeck(object sender, AppHandlerMoveCardEventArgs e)
        {
            if (e.Card.Id != -1 && 
                this.Deck.Cards.IndexOf(e.Card) < this.Deck.Count - (this.Deck.Count / 10))
            {
                if (e.RememberValue == RememberingLevels.Trivial)
                {
                    this.Deck.MoveCard(this.Deck.IndexOf(e.Card), this.Deck.Count - (this.Deck.Count / 5));
                }
                else if (e.RememberValue == RememberingLevels.GotIt)
                {
                    this.Deck.MoveCard(this.Deck.IndexOf(e.Card), this.Deck.Count - (this.Deck.Count / 3));
                }
                else if (e.RememberValue == RememberingLevels.Again)
                {
                    this.Deck.MoveCard(this.Deck.IndexOf(e.Card), this.Deck.Count - (2 * this.Deck.Count / 3));
                }
            }
            else
            {
                this.Deck.MoveCard(this.Deck.IndexOf(e.Card), this.Deck.Count - 1);
            }

            e.Card = this.Deck[0];
        }



    }
}
