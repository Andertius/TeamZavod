// <copyright file="AppHandler.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Memento.BLL.AppHandlerEventArgs;
using Memento.DAL;

namespace Memento.BLL
{
    /// <summary>
    /// Class to handle learning process.
    /// </summary>
    public class AppHandler : INotifyPropertyChanged
    {
        /// <summary>
        /// Property for getting Deck instance.
        /// </summary>
        private Card currentCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        public AppHandler()
        {
            Deck = new Deck();
            CurrentCard = new Card();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        /// <param name="deck">Deck instance.</param>
        public AppHandler(Deck deck)
        {
            Deck = new Deck(deck);
            CurrentCard = Deck[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        /// <param name="deckId">Some deck id.</param>
        public AppHandler(int deckId)
        {
            Deck = new Deck(Repository.FetchDeck(deckId));
            CurrentCard = Deck[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandler"/> class.
        /// </summary>
        /// <param name="deckName">Some deck title.</param>
        public AppHandler(string deckName)
        {
            Deck = new Deck(Repository.FetchDeck(deckName));
            CurrentCard = Deck[0];
        }

        /// <summary>
        /// Event for property changing.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets current deck for app.
        /// </summary>
        public Deck Deck { get; }

        /// <summary>
        /// Gets or set current card.
        /// </summary>
        public Card CurrentCard
        {
            get => currentCard;
            private set
            {
                currentCard = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Funtion to run handler.
        /// </summary>
        /// <param name="order">Sort order for cards.</param>
        public void Start(CardOrder order)
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

            CurrentCard = Deck[0];
        }

        /// <summary>
        /// Randomize card order.
        /// </summary>
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

        /// <summary>
        /// Order cards by ascending.
        /// </summary>
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

        /// <summary>
        /// Order cards by descending.
        /// </summary>
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

        /// <summary>
        /// Move card to special position.
        /// </summary>
        /// <param name="sender">Who calls this event.</param>
        /// <param name="e">AppHandlerMoveCardEventArgs object.</param>
        public void MoveCardIntoDeck(object sender, AppHandlerMoveCardEventArgs e)
        {
            if (e.Card.Id != -1 && Deck.Count > 3)
            {
                if (e.RememberValue == RememberingLevels.Trivial)
                {
                    Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - (Deck.Count / 5));
                }
                else if (e.RememberValue == RememberingLevels.GotIt)
                {
                    Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - (Deck.Count / 3));
                }
                else if (e.RememberValue == RememberingLevels.Again)
                {
                    Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - (2 * Deck.Count / 3));
                }
            }
            else
            {
                Deck.MoveCard(Deck.IndexOf(e.Card), Deck.Count - 1);
            }

            CurrentCard = Deck[0];
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
