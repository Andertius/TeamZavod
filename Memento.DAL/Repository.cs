using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            using var context = new CardsContext();
            context.Database.Migrate();

            if (context.Database.CanConnect())
            {
                Card crd = new Card();

                var help = context.Cards.FromSqlRaw($"SELECT * FROM \"Card_Table\" WHERE card_id = {id}").ToList();

                foreach (var item in help)
                {
                    //Difficulty diff

                    crd = new Card
                    {
                        Id = item.Id,
                        Word = item.Word,
                        Description = item.Description,
                        ImagePath = item.ImagePath,
                        Transcription = item.Transcription,
                        Difficulty = item.Difficulty switch
                        {
                            "none" => Difficulty.None,
                            "beginner" => Difficulty.Beginner,
                            "advanced" => Difficulty.Advanced,
                            "intermediate" => Difficulty.Intermediate,
                            _ => throw new NotSupportedException("Not valid difficulty level")
                        }

                    };

                }

                return crd;
            }

            return new Card();
        }

        public static IEnumerable<Deck> FetchAllDecks()
        {
            return new List<Deck>();
        }

        public static IEnumerable<Card> FetchAllCards(string deckName = "")
        {
            return new List<Card>();
        }

        public static int AddCard(Card card)
        {
            //returns the id of the newly created card
            using var context = new CardsContext();
            context.Database.Migrate();

            var com = context.Cards.FromSqlRaw("SELECT * FROM \"Card_Table\" ORDER BY card_id DESC LIMIT 1").ToList();

            string diff = "";

            foreach(var item in com)
            {

                diff = card.Difficulty switch
                {
                    Difficulty.None => "none",
                    Difficulty.Beginner => "beginner",
                    Difficulty.Intermediate => "intermediate",
                    Difficulty.Advanced => "advanced",
                    _ => throw new NotSupportedException("Not valid difficulty level"),
                };
            }

            context.Database.ExecuteSqlRaw($"INSERT INTO \"Card_Table\" (image_path, word, transcription, description, difficulty_level) " +
                $"VALUES('{card.ImagePath}', '{card.Word}', '{card.Transcription}', '{card.Description}', '{diff}')");

            //context.SaveChanges();

            com = context.Cards.FromSqlRaw("SELECT * FROM \"Card_Table\" ORDER BY card_id DESC LIMIT 1").ToList();

            foreach(var item in com)
            {
                return item.Id;
            }

            return -1;
        }

        public static void RemoveCard(int id)
        {
            using var context = new CardsContext();
            context.Database.Migrate();
            
            context.Database.ExecuteSqlRaw($"DELETE FROM \"Tag_To_Card_Table\" WHERE card_id = {id}");
            context.Database.ExecuteSqlRaw($"DELETE FROM \"Deck_To_Card_Table\" WHERE card_id = {id}");
            context.Database.ExecuteSqlRaw($"DELETE FROM \"Card_Table\" WHERE card_id = {id}");

            //context.SaveChanges();
        }

        public static void UpdateCard(int id, Card card)
        {

        }

        public static void AddTagToCard(int cardId, string tagName)
        {

        }

        public static void RemoveTagFromCard(int cardId, string tagName)
        {

        }

        public static int AddDeck(Deck deck)
        {
            //returns the id of the newly created deck
            return -1;
        }

        public static void RemoveDeck(int id)
        {

        }

        public static void UpdateDeck(int id, Deck deck)
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
