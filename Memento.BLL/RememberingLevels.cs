// <copyright file="CardOrder.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.BLL
{
    /// <summary>
    /// Levels of remembering for cards.
    /// </summary>
    public enum RememberingLevels
    {
        /// <summary>
        /// Worts level of remembering.
        /// </summary>
        Again,

        /// <summary>
        /// Middle level of remebering.
        /// </summary>
        GotIt,

        /// <summary>
        /// Maximal level of remembering.
        /// </summary>
        Trivial,
    }
}
