using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Memento.BLL;
using Memento.BLL.DeckEditorEventArgs;
using Memento.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Memento.UnitTests
{
    [TestClass]
    public class BLLDeckEditorTests
    {
        private DeckEditor deckEditor;
        private Deck deck;
        private Card utilityCard;

        private Card RandomizeCard()
        {
            var rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string word = new string(Enumerable
                .Repeat(chars, rand.Next(1, 15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            string description = new string(Enumerable
                .Repeat(chars, rand.Next(1, 15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            string transcription = new string(Enumerable
                .Repeat(chars, rand.Next(1, 15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            Difficulty diff = Difficulty.None;

            string imagePath = new string(Enumerable
                .Repeat(chars, rand.Next(1, 15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            return new Card(word, description, transcription, imagePath, diff);
        }

        private Deck RandomizeDeck()
        {
            var rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string deckName = new string(Enumerable
                .Repeat(chars, rand.Next(1, 15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            string tagName = new string(Enumerable
                .Repeat(chars, rand.Next(1, 15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            var card1 = RandomizeCard();
            var card2 = RandomizeCard();
            var deck = new Deck(deckName, tagName);
            deck.AddRange(new Card[] { card1, card2 });

            return deck;
        }

        [TestInitialize]
        public void Initialize()
        {
            deck = RandomizeDeck();
            utilityCard = RandomizeCard();
            deckEditor = new DeckEditor(deck);
        }

        [TestMethod]
        public void AddCardTest_CardAddedToDatabase()
        {
            Repository.AddDeck(deck);
            deckEditor.AddCard(this, new DeckEditorCardEventArgs(deck, utilityCard));
            Assert.IsNotNull(Repository.FetchCard(deckEditor.Deck.Cards[^1].Id));
        }

        [TestMethod]
        public void AddCardTest_CardAddedToDeck()
        {
            Repository.AddDeck(deck);
            deckEditor.AddCard(this, new DeckEditorCardEventArgs(deck, utilityCard));

            Assert.AreEqual(deckEditor.Deck.Count, 3);

            Assert.AreEqual(deckEditor.Deck[2].Word, utilityCard.Word);
            Assert.AreEqual(deckEditor.Deck[2].Description, utilityCard.Description);
            Assert.AreEqual(deckEditor.Deck[2].Transcription, utilityCard.Transcription);
            Assert.AreEqual(deckEditor.Deck[2].ImagePath, utilityCard.ImagePath);
            Assert.AreEqual(deckEditor.Deck[2].Difficulty, utilityCard.Difficulty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCardTest_ThrowsArgumentException()
        {
            deckEditor.AddCard(this, new DeckEditorCardEventArgs(deck, deck[0]));
        }

        [TestMethod]
        [Ignore]
        public void UpdateCardTest_UpdatesCard()
        {
            //TODO
            deckEditor.AddCard(this, new DeckEditorCardEventArgs(new Deck(), utilityCard));
            var newCard = RandomizeCard();
            deckEditor.UpdateCard(this, new UpdateCardDeckEditorEventArgs(utilityCard.Id, newCard));

            Assert.AreEqual(deckEditor.Deck[2].Word, newCard.Word);
            Assert.AreEqual(deckEditor.Deck[2].Description, newCard.Description);
            Assert.AreEqual(deckEditor.Deck[2].Transcription, newCard.Transcription);
            Assert.AreEqual(deckEditor.Deck[2].ImagePath, newCard.ImagePath);
            Assert.AreEqual(deckEditor.Deck[2].Difficulty, newCard.Difficulty);
        }

        [TestMethod]
        public void SaveChangesTest_DeckNotInDatabase_AddsDeckToDatabase()
        {
            deckEditor.SaveChanges(this, new DeckEditorDeckEventArgs(deck));
            var dbDeck = Repository.FetchDeck(deckEditor.Deck.DeckName);

            Assert.IsNotNull(dbDeck);

            Assert.IsNotNull(dbDeck[0]);
            Assert.IsNotNull(dbDeck[1]);

            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[0].Id);
            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[1].Id);
            Repository.RemoveDeck(deckEditor.Deck.Id);
            Repository.RemoveCard(deckEditor.Deck[0].Id);
            Repository.RemoveCard(deckEditor.Deck[1].Id);
        }

        [TestMethod]
        public void SaveChangesTest_DeckPresentInDatabase_UpdatesDeck()
        {
            deckEditor.SaveChanges(this, new DeckEditorDeckEventArgs(deck));

            var deck1 = RandomizeDeck();
            deckEditor.Deck.DeckName = deck1.DeckName;
            deckEditor.Deck.TagName = deck1.TagName;

            deckEditor.Deck[0] = deck1[0];
            deckEditor.Deck[1] = deck1[1];

            deckEditor.SaveChanges(this, new DeckEditorDeckEventArgs(deckEditor.Deck));
            var dbDeck = Repository.FetchDeck(deckEditor.Deck.DeckName);

            Assert.IsNotNull(dbDeck);

            Assert.IsNotNull(dbDeck[0]);
            Assert.IsNotNull(dbDeck[1]);

            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[0].Id);
            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[1].Id);
            Repository.RemoveDeck(deckEditor.Deck.Id);
            Repository.RemoveCard(deckEditor.Deck[0].Id);
            Repository.RemoveCard(deckEditor.Deck[1].Id);
        }

        [TestMethod]
        public void ChangeDeckTest_ChangesDeck()
        {
            var newDeck = RandomizeDeck();
            deckEditor.ChangeDeck(this, new DeckEditorDeckEventArgs(newDeck));

            Assert.AreEqual(deckEditor.Deck.Count, 2);
            Assert.AreEqual(deckEditor.Deck.Id, newDeck.Id);
            Assert.AreEqual(deckEditor.Deck.DeckName, newDeck.DeckName);
            Assert.AreEqual(deckEditor.Deck.TagName, newDeck.TagName);
            Assert.AreEqual(deckEditor.Deck[0], newDeck[0]);
            Assert.AreEqual(deckEditor.Deck[1], newDeck[1]);
        }

        [TestMethod]
        public void EditDeckTest_EditsDatabaseDeck()
        {
            var deck = RandomizeDeck();

            deckEditor.SaveChanges(this, new DeckEditorDeckEventArgs(this.deck));
            deckEditor.EditDeck(this, new EditDeckDeckEditorEventArgs(deck.DeckName, deck.TagName));

            var result = Repository.FetchDeck(deckEditor.Deck.DeckName);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.TagName, deck.TagName);

            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[0].Id);
            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[1].Id);
            Repository.RemoveDeck(deckEditor.Deck.Id);
            Repository.RemoveCard(deckEditor.Deck[0].Id);
            Repository.RemoveCard(deckEditor.Deck[1].Id);
        }

        [TestMethod]
        public void EditDeckTest_EditsLocalDeck()
        {
            var deck = RandomizeDeck();

            deckEditor.SaveChanges(this, new DeckEditorDeckEventArgs(this.deck));
            deckEditor.EditDeck(this, new EditDeckDeckEditorEventArgs(deck.DeckName, deck.TagName));

            Assert.AreEqual(deckEditor.Deck.DeckName, deck.DeckName);
            Assert.AreEqual(deckEditor.Deck.TagName, deck.TagName);

            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[0].Id);
            Repository.RemoveCardFromDeck(deckEditor.Deck.Id, deckEditor.Deck[1].Id);
            Repository.RemoveDeck(deckEditor.Deck.Id);
            Repository.RemoveCard(deckEditor.Deck[0].Id);
            Repository.RemoveCard(deckEditor.Deck[1].Id);
        }

        [TestMethod]
        public void RemoveDeckTest_RemovesTheDeck()
        {
            deckEditor.SaveChanges(this, new DeckEditorDeckEventArgs(deck));
            deckEditor.RemoveDeck(this, new RemoveDeckEditorDeckEventArgs(deckEditor.Deck));
        }

        [TestMethod]
        [ExpectedException(typeof(DeckNotFoundException))]
        public void RemoveDeckTest_ThrowsDeckNotFoundException()
        {
            deckEditor.RemoveDeck(this, new RemoveDeckEditorDeckEventArgs(deck));
        }

        [TestMethod]
        public void ChangeCardTest()
        {
            deckEditor.ChangeDeck(this, new DeckEditorDeckEventArgs(deck));
            deckEditor.ChangeCard(this, new DeckEditorCardEventArgs(new Deck(), deck[1]));

            Assert.AreEqual(deckEditor.CurrentCard.Id, deck[1].Id);
            Assert.AreEqual(deckEditor.CurrentCard.Word, deck[1].Word);
            Assert.AreEqual(deckEditor.CurrentCard.Description, deck[1].Description);
            Assert.AreEqual(deckEditor.CurrentCard.Transcription, deck[1].Transcription);
            Assert.AreEqual(deckEditor.CurrentCard.ImagePath, deck[1].ImagePath);
            Assert.AreEqual(deckEditor.CurrentCard.Difficulty, deck[1].Difficulty);
        }
    }
}
