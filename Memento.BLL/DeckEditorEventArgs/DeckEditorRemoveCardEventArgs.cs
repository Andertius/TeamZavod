// <copyright file="DeckEditorRemoveCardEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.BLL.DeckEditorEventArgs
{
    using System;

    using Memento.DAL;

    public class DeckEditorRemoveCardEventArgs : EventArgs
    {
        public DeckEditorRemoveCardEventArgs(Deck deck, Card card)
        {
            this.Card = new Card(card);
            this.Deck = new Deck(deck);
        }

        public Card Card { get; }

        public Deck Deck { get; }

        public bool CardRemoved { get; set; }
    }
}
