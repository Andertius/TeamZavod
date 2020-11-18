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

    public class DeckEditor : INotifyPropertyChanged
    {
        private Deck deck;
        private Card currentCard;

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

        public Deck Deck
        {
            get => this.deck;
            private set
            {
                this.deck = value;
                this.OnPropertyChanged();
            }
        }

        public Card CurrentCard
        {
            get => this.currentCard;
            private set
            {
                this.currentCard = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Tags { get; }

        public ObservableCollection<Card> Cards { get; }

        public ObservableCollection<Deck> AllDecks { get; }

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

        public void RemoveCard(object sender, DeckEditorRemoveCardEventArgs e)
        {
            e.CardRemoved = this.Deck.Remove(e.Card);

            if (e.Card.Id != -1 && e.Deck.Id != -1)
            {
                Repository.RemoveCardFromDeck(e.Deck.Id, e.Card.Id);
            }

            this.CurrentCard = new Card();
        }

        public void ChangeDeck(object sender, DeckEditorDeckEventArgs e)
        {
            this.Deck = new Deck(e.Deck);
            this.CurrentCard = new Card();
        }

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

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearCard(object sender, EventArgs e)
        {
            this.CurrentCard = new Card();
        }

        public void ChangeCard(object sender, DeckEditorCardEventArgs e)
        {
            this.CurrentCard = new Card(e.Card);
        }
    }
}
