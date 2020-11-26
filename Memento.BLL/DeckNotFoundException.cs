// <copyright file="DeckNotFoundException.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

namespace Memento.BLL
{
    /// <summary>
    /// This exception is thrown when the deck could not be found.
    /// </summary>
    internal class DeckNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeckNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public DeckNotFoundException(string message = "No such deck could be found in the database")
        {
            Message = message;
        }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        public override string Message { get; }
    }
}
