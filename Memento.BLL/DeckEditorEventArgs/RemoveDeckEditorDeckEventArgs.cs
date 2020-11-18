// <copyright file="RemoveDeckEditorDeckEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.BLL.DeckEditorEventArgs
{
    using System;

    using Memento.DAL;

    public class RemoveDeckEditorDeckEventArgs : EventArgs
    {
        public RemoveDeckEditorDeckEventArgs(Deck deck)
        {
            this.Deck = new Deck(deck);
            this.Removed = false;
        }

        public Deck Deck { get; }

        public bool Removed { get; set; }
    }
}
