// <copyright file="UpdateDeckOptions.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.DAL
{
    /// <summary>
    /// Enum for easier usage of update options.
    /// </summary>
    public enum UpdateDeckOptions
    {
        /// <summary>
        /// Update content options.
        /// </summary>
        UpdateContent,

        /// <summary>
        /// Update cards option.
        /// </summary>
        UpdateCards,

        /// <summary>
        /// Update all(fk ths stff).
        /// </summary>
        UpdateAll,
    }
}
