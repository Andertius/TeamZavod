// <copyright file="StartEditingEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

namespace Memento
{
    /// <summary>
    /// Event arguments for the deck editing page.
    /// </summary>
    public class StartEditingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartEditingEventArgs"/> class.
        /// </summary>
        /// <param name="deckId">The id of the deck that should be open whrn the page opens.</param>
        public StartEditingEventArgs(int deckId = -1)
        {
            DeckId = deckId;
        }

        /// <summary>
        /// Gets the deck id.
        /// </summary>
        public int DeckId { get; }
    }
}
