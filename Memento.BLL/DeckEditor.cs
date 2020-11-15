using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Memento.DAL;

namespace Memento.BLL
{
    public class DeckEditor : INotifyPropertyChanged
    {
        private Deck deck;

        public DeckEditor()
        {
            Deck = new Deck();
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
        }

        public DeckEditor(Deck deck)
        {
            Deck = new Deck(deck);
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
        }

        public DeckEditor(string deckName)
        {
            Deck = Repository.FetchDeck(deckName);
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
        }

        public DeckEditor(int deckId)
        {
            Deck = Repository.FetchDeck(deckId);
            AllDecks = new ObservableCollection<Deck>(Repository.FetchAllDecks());
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

        public ObservableCollection<Deck> AllDecks { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddCard(object sender, DeckEditorCardEventArgs e)
        {
            if (!Deck.Contains(e.Card))
            {
                if (e.Card.Id == -1)
                {
                    Repository.AddCard(e.Card);
                }

                Deck.Add(e.Card);
                return;
            }

            throw new ArgumentException("The card is already in the deck");
        }

        public void UpdateCard(object sender, DeckEditorCardEventArgs e)
        {
            Deck[Deck.IndexOf(e.Card)] = new Card(e.Card);
        }

        public void RemoveCard(object sender, DeckEditorRemoveCardEventArgs e)
        {
            e.CardRemoved = Deck.Remove(e.Card);
        }

        public void ChangeDeck(object sender, DeckEditorDeckEventArgs e)
        {
            Deck = new Deck(e.Deck);
        }

        public void RemoveDeck(object sender, RemoveDeckEditorDeckEventArgs e)
        {
            if (e.Deck.Id != -1)
            {
                Repository.RemoveDeck(e.Deck.Id);
                e.Removed = true;
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
    }
}
