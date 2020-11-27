// <copyright file="EditDeckDeckEditorEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

namespace Memento.BLL.DeckEditorEventArgs
{
    /// <summary>
    /// Event arguments for editing a deck.
    /// </summary>
    public class EditDeckDeckEditorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditDeckDeckEditorEventArgs"/> class.
        /// </summary>
        /// <param name="deckName">The name of the deck that is to be edited.</param>
        /// <param name="tagName">The tag name of the deck that is to be edited.</param>
        public EditDeckDeckEditorEventArgs(string deckName, string tagName)
        {
            DeckName = deckName;
            TagName = tagName;
        }

        /// <summary>
        /// Gets the name of the deck.
        /// </summary>
        public string DeckName { get; }

        /// <summary>
        /// Gets the tag name of the deck.
        /// </summary>
        public string TagName { get; }
    }
}
