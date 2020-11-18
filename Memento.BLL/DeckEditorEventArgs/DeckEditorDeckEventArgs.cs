// <copyright file="DeckEditorDeckEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.BLL.DeckEditorEventArgs
{
    using System;

    using Memento.DAL;

    public class DeckEditorDeckEventArgs : EventArgs
    {
        public DeckEditorDeckEventArgs(Deck deck)
        {
            this.Deck = new Deck(deck);
        }

        public Deck Deck { get; }
    }
}
