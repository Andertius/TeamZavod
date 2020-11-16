using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.ObjectModel;

namespace Memento.DAL
{
    public static class Repository
    {
        public static Deck FetchDeck(string deckName)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            if (context.Database.CanConnect())
            {
                var cards = new List<Card>();

                Card crd = new Card();

                var help = context.Cards.FromSqlInterpolated($"SELECT Card_Table.card_id, description, difficulty_level, image_path, transcription, word FROM Card_Table, Deck_To_Card_Table WHERE Deck_To_Card_Table.card_id = Card_Table.card_id AND Deck_To_Card_Table.deck_id = (SELECT deck_id FROM Deck_Table  WHERE deck_name = {deckName})").ToList();
                foreach (var item in help)
                {
                    crd = new Card
                    {
                        Id = item.Id,
                        Word = item.Word,
                        Description = item.Description,
                        ImagePath = item.ImagePath,
                        Transcription = item.Transcription,

                        Difficulty = DifficultyConverter.ToDifficultyConverter(item.Difficulty)
                    };

                    cards.Add(crd);
                }

                for (int i = 0; i < cards.Count; i++)
                {
                    var tags = context.Tags.Where(x => x.CardID == cards[i].Id).ToList();
                    List<string> answerTags = new List<string>();

                    foreach (var item in tags)
                    {
                        //перевірка чи пустий
                        answerTags.Add(item.TagName);
                    }

                    cards[i].Tags = new ObservableCollection<string>(answerTags);
                }

                var deckdetails = context.Decks.Where(x => x.DeckName == deckName).ToList();

                var deck = new Deck();

                foreach (var item in deckdetails)
                {
                    deck.TagName = item.TagName;
                    deck.Id = item.Id;
                    deck.DeckName = item.DeckName;
                    deck.AddRange(cards);
                }

                return deck;
            }
            return new Deck();
        }

        public static Deck FetchDeck(int id)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            var cards = new List<Card>();

            Card crd = new Card();

            var help = context.Cards.FromSqlInterpolated($"SELECT Card_Table.card_id, description, difficulty_level, image_path, transcription, word FROM Card_Table, Deck_To_Card_Table WHERE Deck_To_Card_Table.card_id = Card_Table.card_id AND Deck_To_Card_Table.deck_id = (SELECT deck_id FROM Deck_Table  WHERE deck_id = {id})").ToList();

            foreach (var item in help)
            {
                crd = new Card
                {
                    Id = item.Id,
                    Word = item.Word,
                    Description = item.Description,
                    ImagePath = item.ImagePath,
                    Transcription = item.Transcription,

                    Difficulty = DifficultyConverter.ToDifficultyConverter(item.Difficulty)
                };

                cards.Add(crd);
            }

            for (int i = 0; i < cards.Count; i++)
            {
                var tags = context.Tags.Where(x => x.CardID == cards[i].Id).ToList();
                List<string> answerTags = new List<string>();

                foreach (var item in tags)
                {
                    //перевірка чи пустий
                    answerTags.Add(item.TagName);
                }

                cards[i].Tags = new ObservableCollection<string>(answerTags);

            }

            var deckdetails = context.Decks.Where(x => x.Id == id).ToList();

            var deck = new Deck();

            foreach (var item in deckdetails)
            {
                deck.TagName = item.TagName;
                deck.Id = item.Id;
                deck.DeckName = item.DeckName;
                deck.AddRange(cards);
            }

            return deck;
        }

        public static Card FetchCard(int id)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            Card crd = new Card();

            var help = context.Cards.FromSqlInterpolated($"SELECT * FROM \"Card_Table\" WHERE card_id = {id}").ToList();

            foreach (var item in help)
            {
                crd = new Card
                {
                    Id = item.Id,
                    Word = item.Word,
                    Description = item.Description,
                    ImagePath = item.ImagePath,
                    Transcription = item.Transcription,

                    Difficulty = DifficultyConverter.ToDifficultyConverter(item.Difficulty)
                };
            }

            var tags = context.Tags.FromSqlInterpolated($"SELECT * FROM \"Tag_To_Card_Table\" WHERE card_id = {id}").ToList();

            List<string> answerTags = new List<string>();

            foreach(var item in tags)
            {
                //перевірка чи пустий
                answerTags.Add(item.TagName);
            }

            crd.Tags = new ObservableCollection<string>(answerTags);

            return crd;
        }

        public static IEnumerable<Deck> FetchAllDecks()
        {
            using var context = new CardsContext();

            var decks = context.Decks.ToList();

            List<Deck> received_data = new List<Deck>();
            Deck deck;

            foreach (var item in decks)
            {
                deck = new Deck
                {
                    Id = item.Id,
                    DeckName = item.DeckName,
                    TagName = item.TagName
                };

                received_data.Add(deck);
            }

            return received_data;
        }

        public static IEnumerable<Card> FetchAllCards()
        {
            using var context = new CardsContext();
            //context.Database.Migrate();

            var cards = new List<Card>();

            Card crd = new Card();

            var help = context.Cards.FromSqlInterpolated($"SELECT * FROM Card_Table").ToList();

            foreach (var item in help)
            {
                crd = new Card
                {
                    Id = item.Id,
                    Word = item.Word,
                    Description = item.Description,
                    ImagePath = item.ImagePath,
                    Transcription = item.Transcription,

                    Difficulty = DifficultyConverter.ToDifficultyConverter(item.Difficulty)
                };

                cards.Add(crd);
            }

            for (int i = 0; i < cards.Count; i++)
            {
                var tags = context.Tags.FromSqlInterpolated($"SELECT * FROM \"Tag_To_Card_Table\" WHERE card_id = {cards[i].Id}").ToList();
                List<string> answerTags = new List<string>();


                foreach (var item in tags)
                {
                    //перевірка чи пустий
                    answerTags.Add(item.TagName);
                }

                cards[i].Tags = new ObservableCollection<string>(answerTags);
            }

            return cards;
        }

        public static void AddCard(Card card)
        {
            using var context = new CardsContext();
            var com = context.Cards.FromSqlRaw("SELECT * FROM \"Card_Table\" ORDER BY card_id DESC LIMIT 1").ToList();

            string diff = "";

            foreach (var item in com)
            {
                diff = DifficultyConverter.ToStringConverter(card.Difficulty);
            }

            context.Database.ExecuteSqlInterpolated($"INSERT INTO Card_Table (image_path, word, transcription, description, difficulty_level) VALUES({card.ImagePath}, {card.Word}, {card.Transcription}, {card.Description}, {diff})");

            com = context.Cards.FromSqlRaw("SELECT * FROM \"Card_Table\" ORDER BY card_id DESC LIMIT 1").ToList();

            card.Id = com[0].Id;

            for (int i = 0; i < card.Tags.Count; i++)
            {
                AddTagToCard(card.Id, card.Tags[i].Trim());
            }

            context.SaveChanges();
        }

        public static void RemoveCard(int id)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            context.Database.ExecuteSqlInterpolated($"DELETE FROM Tag_To_Card_Table WHERE card_id = {id}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM Deck_To_Card_Table WHERE card_id = {id}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM Card_Table WHERE card_id = {id}");

            var check = context.Cards.ToList();

            context.SaveChanges();
        }

        public static void UpdateCard(int id, Card card, UpdateCardOptions options)
        {
            using var context = new CardsContext();

            var cardForUpdate = context.Cards.Find(id);

            switch (options)
            {
                case UpdateCardOptions.UpdateContent:
                    
                    if (cardForUpdate != null)
                    {
                        cardForUpdate.Description = card.Description;
                        cardForUpdate.ImagePath = card.ImagePath;
                        cardForUpdate.Transcription = card.Transcription;
                        cardForUpdate.Word = card.Word;

                        cardForUpdate.Difficulty = DifficultyConverter.ToStringConverter(card.Difficulty);

                        context.SaveChanges();
                    }

                    break;

                case UpdateCardOptions.UpdateTags:
                    if(cardForUpdate != null)
                    {
                        for (int i = 0; i < card.Tags.Count; i++)
                        {
                            AddTagToCard(cardForUpdate.Id, card.Tags[i]);
                        }

                        context.SaveChanges();
                    }

                    break;

                case UpdateCardOptions.UpdateAll:
                    if (cardForUpdate != null)
                    {
                        cardForUpdate.Description = card.Description;
                        cardForUpdate.ImagePath = card.ImagePath;
                        cardForUpdate.Transcription = card.Transcription;
                        cardForUpdate.Word = card.Word;

                        cardForUpdate.Difficulty = DifficultyConverter.ToStringConverter(card.Difficulty);

                        for (int i = 0; i < card.Tags.Count; i++)
                        {
                            AddTagToCard(cardForUpdate.Id, card.Tags[i]);
                        }

                        context.SaveChanges();
                    }

                    break;
            }

            ////TODO: create enum Update card options (update content, update tags, update all)
        }

        public static void AddTagToCard(int cardId, string tagName)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            context.Database.ExecuteSqlInterpolated($"INSERT INTO Tag_To_Card_Table (tag_name, card_id) VALUES ('{tagName}', {cardId})");

            context.SaveChanges();
        }

        public static void RemoveTagFromCard(int cardId, string tagName)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            //var tag = context.Tags.Where(x => x.CardID == cardId && x.TagName == tagName).FirstOrDefault();

            //context.Tags.Remove(tag);
            //var test = context.Tags.Find(tagName, cardId);

            //Console.WriteLine($"{test.CardID}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM Tag_To_Card_Table WHERE card_id = {cardId} AND tag_name = {tagName}");

            context.SaveChanges();
        }

        public static void AddDeck(Deck deck)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            var deckToAdd = new DeckModel { DeckName = deck.DeckName, TagName = deck.TagName, NumberOfCards = deck.Count };
            context.Add(deckToAdd);
            context.SaveChanges();

            deck.Id = context.Decks.OrderByDescending(x => x.Id).FirstOrDefault().Id;

            for (int i = 0; i < deck.Cards.Count; i++)
            {
                AddCardToDeck(deck.Id, deck.Cards[i].Id);
            }

            //TODO: write loop that adds relationship between cards and decks

            //Console.WriteLine($"{deck.Id}");
        }

        public static void RemoveDeck(int id)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            Console.WriteLine(context.Database.CanConnect());

            context.Database.ExecuteSqlInterpolated($"DELETE FROM Deck_To_Card_Table WHERE deck_id = {id}");
            context.Database.ExecuteSqlInterpolated($"DELETE FROM Deck_Table WHERE deck_id = {id}");

            context.SaveChanges();
        }

        public static void UpdateDeck(int id, Deck deck)
        {
            using var context = new CardsContext();

            var deckForUpdate = context.Decks.Find(id);

            if (deckForUpdate != null)
            {
                deckForUpdate.DeckName = deck.DeckName;
                deckForUpdate.TagName = deck.TagName;
                context.SaveChanges();
            }


            //TODO: create enum Update deck options (update name and tag, update cards, update all)

        }

        public static void AddCardToDeck(int deckId, int cardId)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            Console.WriteLine(context.Database.CanConnect());

            context.Database.ExecuteSqlInterpolated($"INSERT INTO Deck_To_Card_Table (card_id, deck_id), VALUES('{cardId}', '{deckId}'");

            //вернути нову деку


            context.SaveChanges();
        }

        public static void RemoveCardFromDeck(int deckId, int cardId)
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            context.Database.ExecuteSqlInterpolated($"DELETE FROM Deck_To_Card_Table WHERE card_id = {cardId} AND deck_id = {deckId}");

            context.SaveChanges();
        }

        public static IEnumerable<string> FetchAllTags()
        {
            using var context = new CardsContext();
            context.Database.Migrate();

            var cont = context.Tags.Select(x => x.TagName).ToList();

            return cont;
        }
    }
}
