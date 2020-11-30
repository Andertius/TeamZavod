// <copyright file="AppHandlerMoveCardEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using Memento.DAL;

namespace Memento.BLL.AppHandlerEventArgs
{
    /// <summary>
    /// Event for moving card to specific deck place.
    /// </summary>
    public class AppHandlerMoveCardEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppHandlerMoveCardEventArgs"/> class.
        /// </summary>
        /// <param name="card">Current card.</param>
        /// <param name="level">Remeber Level (trivial, got it, again).</param>
        public AppHandlerMoveCardEventArgs(Card card, RememberingLevels level)
        {
            this.Card = new Card(card);
            this.RememberValue = level;
        }

        /// <summary>
        /// Gets or Sets card.
        /// </summary>
        public Card Card { get; set; }

        /// <summary>
        /// Gets user remember level for card.
        /// </summary>
        public RememberingLevels RememberValue { get; }
    }
}
