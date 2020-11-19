// <copyright file="DeckEditor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.BLL
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Memento.BLL.DeckEditorEventArgs;
    using Memento.DAL;

    /// <summary>
    /// The main logic for deck editing.
    /// </summary>
    public class DeckEditor : INotifyPropertyChanged
    {
        private Deck deck;
        private Card currentCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class.
        /// </summary>
        public DeckEditor()
        {
            this.Deck = new Deck();
            this.CurrentCard = new Card();
            this.AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            this.Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            this.Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                                   .Distinct()
                                                                   .OrderBy(x => x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class that is identical to the parameter.
        /// </summary>
        /// <param name="deck">The deck that will get copied.</param>
        public DeckEditor(Deck deck)
        {
            this.Deck = new Deck(deck);
            this.CurrentCard = new Card();
            this.AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            this.Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            this.Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                                   .Distinct()
                                                                   .OrderBy(x => x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class with the given name.
        /// </summary>
        /// <param name="deckName">The name to be given to the deck.</param>
        public DeckEditor(string deckName)
        {
            this.Deck = Repository.FetchDeck(deckName);
            this.CurrentCard = new Card();
            this.AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            this.Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            this.Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                                   .Distinct()
                                                                   .OrderBy(x => x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class with the given id.
        /// </summary>
        /// <param name="deckId">The id to be given to the deck.</param>
        public DeckEditor(int deckId)
        {
            this.Deck = Repository.FetchDeck(deckId);
            this.CurrentCard = new Card();
            this.AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            this.Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            this.Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                                   .Distinct()
                                                                   .OrderBy(x => x));
        }

        /// <summary>
        /// Event that notifies the appropriate objects when a certain propery is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the deck that is being edited.
        /// </summary>
        public Deck Deck
        {
            get => this.deck;
            private set
            {
                this.deck = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the current card that is being edited.
        /// </summary>
        public Card CurrentCard
        {
            get => this.currentCard;
            private set
            {
                this.currentCard = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets all the tags that exist.
        /// </summary>
        public ObservableCollection<string> Tags { get; }

        /// <summary>
        /// Gets all the cards that exist.
        /// </summary>
        public ObservableCollection<Card> Cards { get; }

        /// <summary>
        /// Gets all the decks that exist.
        /// </summary>
        public ObservableCollection<Deck> AllDecks { get; }

        /// <summary>
        /// Adds a card to the current deck.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to be added.</param>
        public void AddCard(object sender, DeckEditorCardEventArgs e)
        {
            if (!this.Deck.Contains(e.Card))
            {
                if (e.Card.Id == -1)
                {
                    Repository.AddCard(e.Card);

                    foreach (var item in e.Card.Tags)
                    {
                        Repository.AddTagToCard(e.Card.Id, item.Trim());
                    }
                }
                else
                {
                    Repository.AddCardToDeck(e.Deck.Id, e.Card.Id);
                }

                foreach (var tag in e.Card.Tags)
                {
                    if (!this.Tags.Contains(tag.Trim()))
                    {
                        this.Tags.Add(tag.Trim());
                    }
                }

                this.Deck.Add(e.Card);
                return;
            }

            throw new ArgumentException("The card is already in the deck");
        }

        /// <summary>
        /// Updates the card.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to be updated.</param>
        public void UpdateCard(object sender, DeckEditorCardEventArgs e)
        {
            this.Deck[this.Deck.IndexOf(e.Card)] = new Card(e.Card);

            foreach (var tag in e.Card.Tags)
            {
                if (!this.Tags.Contains(tag.Trim()))
                {
                    this.Tags.Add(tag.Trim());
                }
            }

            Repository.UpdateCard(e.Card.Id, e.Card, UpdateCardOptions.UpdateAll);
        }

        /// <summary>
        /// Removes a card from the current deck.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to be removed.</param>
        public void RemoveCard(object sender, DeckEditorRemoveCardEventArgs e)
        {
            e.CardRemoved = this.Deck.Remove(e.Card);

            if (e.Card.Id != -1 && e.Deck.Id != -1)
            {
                Repository.RemoveCardFromDeck(e.Deck.Id, e.Card.Id);
            }

            this.CurrentCard = new Card();
        }

        /// <summary>
        /// Chnages the current deck.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The deck to change to.</param>
        public void ChangeDeck(object sender, DeckEditorDeckEventArgs e)
        {
            this.Deck = new Deck(e.Deck);
            this.CurrentCard = new Card();
        }

        /// <summary>
        /// Removes the deck from the database.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The deck to reomve.</param>
        public void RemoveDeck(object sender, RemoveDeckEditorDeckEventArgs e)
        {
            if (e.Deck.Id != -1)
            {
                Repository.RemoveDeck(e.Deck.Id);
                e.Removed = true;
                this.CurrentCard = new Card();
                return;
            }

            throw new DeckNotFoundException();
        }

        /// <summary>
        /// Saves all the changes to database.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">All the information to be saved.</param>
        public void SaveChanges(object sender, DeckEditorDeckEventArgs e)
        {
            if (e.Deck.Id == -1)
            {
                Repository.AddDeck(e.Deck);
                this.AllDecks.Add(e.Deck);
            }
            else
            {
                Repository.UpdateDeck(e.Deck.Id, e.Deck);
            }
        }

        /// <summary>
        /// Clears the current card.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">No parameters.</param>
        public void ClearCard(object sender, EventArgs e)
        {
            this.CurrentCard = new Card();
        }

        /// <summary>
        /// Changes the current card.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to change to.</param>
        public void ChangeCard(object sender, DeckEditorCardEventArgs e)
        {
            this.CurrentCard = new Card(e.Card);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
