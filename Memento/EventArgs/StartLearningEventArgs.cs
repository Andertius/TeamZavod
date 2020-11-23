// <copyright file="StartEditingEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

namespace Memento
{ 
    /// <summary>
    /// Event arguments for the learning page.
    /// </summary>
    public class StartLearningEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartLearningEventArgs"/> class.
        /// </summary>
        /// <param name="deckId">The id of the deck that should be open whrn the page opens.</param>
        public StartLearningEventArgs(int deckId = -1)
        {
            DeckId = deckId;
        }

        /// <summary>
        /// Gets the deck id.
        /// </summary>
        public int DeckId { get; }
    }
}
