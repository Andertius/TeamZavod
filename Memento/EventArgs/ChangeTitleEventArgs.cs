// <copyright file="ChangeTitleEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento
{
    using System;

    /// <summary>
    /// Event arguments for changing the window title.
    /// </summary>
    public class ChangeTitleEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTitleEventArgs"/> class.
        /// </summary>
        /// <param name="title">The new window title.</param>
        public ChangeTitleEventArgs(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Gets the window title.
        /// </summary>
        public string Title { get; }
    }
}
