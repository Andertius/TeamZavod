using System;
using System.Collections.Generic;

namespace Memento.DAL
{
    public static class Repository
    {
        public static Deck FetchDeck(string deckName)
        {
            return new Deck();
        }

        public static Deck FetchDeck(int id)
        {
            return new Deck();
        }

        public static Card FetchCard(int id)
        {
            return new Card();
        }

        public static int AddCard(string word, string description, string transcription = "", string difficulty = "None", string imagePath = "")
        {
            //returns the id of the newly created card
            return -1;
        }

        public static void RemoveCard(int id)
        {

        }

        public static void UpdateCard(int id, string word = "", string description = "", string transcription = "", string Difficulty = "None", string imagePath = "")
        {

        }

        public static void AddTagToCard(int cardId, string tagName)
        {

        }

        public static void RemoveTagFromCard(int cardId, string tagName)
        {

        }

        public static int AddDeck(string deckName, string tagName = "")
        {
            //returns the id of the newly created deck
            return -1;
        }

        public static void RemoveDeck(int id)
        {

        }

        public static void UpdateDeck(int id, string newDeckName = "", string newTagName = "")
        {

        }

        public static void AddCardToDeck(int deckId, int cardId)
        {

        }

        public static void RemoveCardFromDeck(int deckId, int cardId)
        {

        }
    }
}
