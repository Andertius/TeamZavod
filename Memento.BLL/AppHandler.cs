//StatEventArgs (TimeSpan t);
// <copyright file="Statistics.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Memento.BLL
{
    using System;
    using Memento.BLL.AppHandlerEventArgs;
    using Memento.DAL;

    /// <summary>
    /// Class to handle learning process
    /// </summary>
    public class AppHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        /// <param name="deck">Deck instance.</param>
        public AppHandler(Deck deck)
        {
            this.Deck = new Deck(deck);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        /// <param name="deckId">Some deck id.</param>
        public AppHandler(int deckId)
        {
            this.Deck = new Deck(Repository.FetchDeck(deckId));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        /// <param name="deckName">Some deck title.</param>
        public AppHandler(string deckName)
        {
            this.Deck = new Deck(Repository.FetchDeck(deckName));
        }

        /// <summary>
        /// Property for getting Deck instance.
        /// </summary>
        public Deck Deck { get; }

        /// <summary>
        /// Funtion to run handler.
        /// </summary>
        /// <param name="order">Sort order for cards.</param>
        /// <param name="showImages">Enable or Disable images.</param>
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

        /// <summary>
        /// Randomize card order.
        /// </summary>
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

        /// <summary>
        /// Order cards by ascending.
        /// </summary>
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

        /// <summary>
        /// Order cards by descending.
        /// </summary>
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

        /// <summary>
        ///Flip card event.
        /// </summary>
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

        /// <summary>
        /// Move card to special position.
        /// </summary>
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
