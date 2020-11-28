// <copyright file="DeckEditor.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Memento.BLL.DeckEditorEventArgs;
using Memento.DAL;

namespace Memento.BLL
{
    /// <summary>
    /// The main logic for deck editing.
    /// </summary>
    public class DeckEditor : INotifyPropertyChanged
    {
        private Deck deck;
        private Card currentCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class with an empty deck.
        /// </summary>
        public DeckEditor()
        {
            Deck = new Deck();
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks()
                                                                .OrderBy(x => x.DeckName));
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class that a deck that is identical to the parameter.
        /// </summary>
        /// <param name="deck">The deck that will get copied.</param>
        public DeckEditor(Deck deck)
        {
            Deck = new Deck(deck);
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks()
                                                                .OrderBy(x => x.DeckName));
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class with a deck of the given name.
        /// </summary>
        /// <param name="deckName">The name to be given to the deck.</param>
        public DeckEditor(string deckName)
        {
            Deck = Repository.FetchDeck(deckName);
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks()
                                                                .OrderBy(x => x.DeckName));
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeckEditor"/> class with a deck of the given id.
        /// </summary>
        /// <param name="deckId">The id to be given to the deck.</param>
        public DeckEditor(int deckId)
        {
            Deck = Repository.FetchDeck(deckId);
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks()
                                                                .OrderBy(x => x.DeckName));
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
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
            get => deck;
            private set
            {
                deck = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the current card that is being edited.
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
            if (!Deck.Contains(e.Card))
            {
                if (e.Card.Id == Card.DefaultId)
                {
                    Repository.AddCard(e.Card);

                    foreach (var item in e.Card.Tags)
                    {
                        Repository.AddTagToCard(e.Card.Id, item.Trim());
                    }
                }

                Repository.AddCardToDeck(e.Deck.Id, e.Card.Id);

                foreach (var tag in e.Card.Tags)
                {
                    if (!Tags.Contains(tag.Trim()))
                    {
                        Tags.Add(tag.Trim());
                    }
                }

                Deck.Add(e.Card);
                CurrentCard.Id = e.Card.Id;
                return;
            }

            throw new ArgumentException("The card is already in the deck");
        }

        /// <summary>
        /// Updates the card. Updates only if the card is already in the database.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to be updated.</param>
        public void UpdateCard(object sender, UpdateCardDeckEditorEventArgs e)
        {
            if (e.Card.Id != Card.DefaultId)
            {
                Deck[Deck.IndexOf(e.Card)] = new Card(e.NewCard);

                foreach (var tag in e.Card.Tags)
                {
                    if (!Tags.Contains(tag.Trim()))
                    {
                        Tags.Add(tag.Trim());
                    }
                }

                Repository.UpdateCard(e.Card.Id, e.NewCard, UpdateCardOptions.UpdateAll);
            }
        }

        /// <summary>
        /// Removes a card from the current deck.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to be removed.</param>
        public void RemoveCard(object sender, DeckEditorRemoveCardEventArgs e)
        {
            e.CardRemoved = Deck.Remove(e.Card);

            if (e.Card.Id != Card.DefaultId && e.Deck.Id != Deck.DefaultId)
            {
                Repository.RemoveCardFromDeck(e.Deck.Id, e.Card.Id);
            }

            CurrentCard = new Card();
        }

        /// <summary>
        /// Chnages the current deck.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The deck to change to.</param>
        public void ChangeDeck(object sender, DeckEditorDeckEventArgs e)
        {
            Deck = new Deck(e.Deck);
            CurrentCard = new Card();
        }

        /// <summary>
        /// Edits the current deck.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The name and the tag name to change to.</param>
        public void EditDeck(object sender, EditDeckDeckEditorEventArgs e)
        {
            int index = AllDecks.IndexOf(new Deck(Deck.DeckName));
            Deck.DeckName = e.DeckName;
            Deck.TagName = e.TagName;
            Repository.UpdateDeck(Deck.Id, Deck, UpdateDeckOptions.UpdateContent);
            AllDecks[index].DeckName = Deck.DeckName;
            AllDecks[index].TagName = Deck.TagName;
        }

        /// <summary>
        /// Removes the deck from the database.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The deck to reomve.</param>
        public void RemoveDeck(object sender, RemoveDeckEditorDeckEventArgs e)
        {
            if (e.Deck.Id != Deck.DefaultId)
            {
                Repository.RemoveDeck(e.Deck.Id);
                AllDecks.Remove(e.Deck);
                e.Removed = true;
                Deck = new Deck();
                CurrentCard = new Card();
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
            if (e.Deck.Id == Deck.DefaultId)
            {
                Repository.AddDeck(e.Deck);
                AllDecks.Add(e.Deck);
                ChangeDeck(this, new DeckEditorDeckEventArgs(e.Deck));
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
            CurrentCard = new Card();
        }

        /// <summary>
        /// Changes the current card.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">The card to change to.</param>
        public void ChangeCard(object sender, DeckEditorCardEventArgs e)
        {
            CurrentCard = new Card(e.Card);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
