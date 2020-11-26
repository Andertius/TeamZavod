using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Memento.BLL;
using Memento.BLL.DeckEditorEventArgs;
using Memento.DAL;
using System.Linq;

namespace Memento.UnitTests
{
    [TestClass]
    public class BLLDeckEditorTests
    {
        private DeckEditor deckEditor;

        private Deck deck;

        [TestInitialize]
        public void Initialize()
        {
            deckEditor = new DeckEditor();
            var rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string deckName = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            string tagName = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            deck = new Deck(deckName, tagName);

            string word = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            string description = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            string transcription = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            Difficulty diff = (Difficulty)rand.Next(3);

            string imagePath = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            var card1 = new Card(word, description, transcription, imagePath, diff);

            word = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            description = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            transcription = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            diff = (Difficulty)rand.Next(3);

            imagePath = new string(Enumerable
                .Repeat(chars, rand.Next(15))
                .Select(s => s[rand.Next(s.Length)])
                .ToArray());

            var card2 = new Card(word, description, transcription, imagePath, diff);

            deck.AddRange(new Card[] { card1, card2 });
        }

        [TestMethod]
        public void AddCardTest()
        {
            // deckEditor.AddCard(this, new DeckEditorCardEventArgs(deck, deck[0]));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddCardTest_ThrowsArgumentException()
        {
            void act() => deckEditor.AddCard(this, new DeckEditorCardEventArgs(deck, deck[0]));

            Assert.ThrowsException<ArgumentException>(act);
        }

        [TestMethod]
        public void UpdateCardTest()
        {

        }

        [TestMethod]
        public void RemoveCardTest()
        {

        }

        [TestMethod]
        public void ChangeDeckTest()
        {
            deckEditor.ChangeDeck(this, new DeckEditorDeckEventArgs(deck));

            Assert.AreEqual(deckEditor.Deck.Id, deck.Id);
            Assert.AreEqual(deckEditor.Deck.DeckName, deck.DeckName);
            Assert.AreEqual(deckEditor.Deck.TagName, deck.TagName);
            Assert.AreEqual(deckEditor.Deck[0], deck[0]);
            Assert.AreEqual(deckEditor.Deck[1], deck[1]);
        }

        [TestMethod]
        public void EditDeckTest()
        {

        }

        [TestMethod]
        public void RemoveDeckTest()
        {

        }

        [TestMethod]
        public void SaveChangesTest()
        {

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
