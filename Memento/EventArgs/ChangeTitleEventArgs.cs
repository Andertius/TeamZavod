// <copyright file="ChangeTitleEventArgs.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

namespace Memento
{
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
            Title = title;
        }

        /// <summary>
        /// Gets the window title.
        /// </summary>
        public string Title { get; }
    }
}
