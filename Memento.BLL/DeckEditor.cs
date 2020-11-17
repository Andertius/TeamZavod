using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Memento.DAL;

namespace Memento.BLL
{
    public class DeckEditor : INotifyPropertyChanged
    {
        private Deck deck;
        private Card currentCard;

        public DeckEditor()
        {
            Deck = new Deck();
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        public DeckEditor(Deck deck)
        {
            Deck = new Deck(deck);
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        public DeckEditor(string deckName)
        {
            Deck = Repository.FetchDeck(deckName);
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        public DeckEditor(int deckId)
        {
            Deck = Repository.FetchDeck(deckId);
            CurrentCard = new Card();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
            Cards = new ObservableCollection<Card>(Repository.FetchAllCards());
            Tags = new ObservableCollection<string>(Repository.FetchAllTags()
                                                              .Distinct()
                                                              .OrderBy(x => x));
        }

        public Deck Deck
        {
            get => deck;
            private set
            {
                deck = value;
                OnPropertyChanged();
            }
        }

        public Card CurrentCard
        {
            get => currentCard;
            private set
            {
                currentCard = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Tags { get; }
        public ObservableCollection<Card> Cards { get; }
        public ObservableCollection<Deck> AllDecks { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddCard(object sender, DeckEditorCardEventArgs e)
        {
            if (!Deck.Contains(e.Card))
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
                    if (!Tags.Contains(tag.Trim()))
                    {
                        Tags.Add(tag.Trim());
                    }
                }

                Deck.Add(e.Card);
                return;
            }

            throw new ArgumentException("The card is already in the deck");
        }

        public void UpdateCard(object sender, DeckEditorCardEventArgs e)
        {
            Deck[Deck.IndexOf(e.Card)] = new Card(e.Card);

            foreach (var tag in e.Card.Tags)
            {
                if (!Tags.Contains(tag.Trim()))
                {
                    Tags.Add(tag.Trim());
                }
            }

            Repository.UpdateCard(e.Card.Id, e.Card, UpdateCardOptions.UpdateAll);
        }

        public void RemoveCard(object sender, DeckEditorRemoveCardEventArgs e)
        {
            e.CardRemoved = Deck.Remove(e.Card);

            if (e.Card.Id != -1 && e.Deck.Id != -1)
            {
                Repository.RemoveCardFromDeck(e.Deck.Id, e.Card.Id);
            }
            
            CurrentCard = new Card();
        }

        public void ChangeDeck(object sender, DeckEditorDeckEventArgs e)
        {
            Deck = new Deck(e.Deck);
            CurrentCard = new Card();
        }

        public void RemoveDeck(object sender, RemoveDeckEditorDeckEventArgs e)
        {
            if (e.Deck.Id != -1)
            {
                Repository.RemoveDeck(e.Deck.Id);
                e.Removed = true;
                CurrentCard = new Card();
                return;
            }

            throw new DeckNotFoundException();
        }

        public void SaveChanges(object sender, DeckEditorDeckEventArgs e)
        {
            if (e.Deck.Id == -1)
            {
                Repository.AddDeck(e.Deck);
                AllDecks.Add(e.Deck);
            }
            else
            {
                Repository.UpdateDeck(e.Deck.Id, e.Deck);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearCard(object sender, EventArgs e)
        {
            CurrentCard = new Card();
        }

        public void ChangeCard(object sender, DeckEditorCardEventArgs e)
        {
            CurrentCard = new Card(e.Card);
        }
    }
}
