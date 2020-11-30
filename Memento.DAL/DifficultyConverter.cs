// <copyright file="DifficultyConverter.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;

namespace Memento.DAL
{
    /// <summary>
    /// Converts Difficulty enum to difficulty string and vice versa.
    /// </summary>
    internal static class DifficultyConverter
    {
        /// <summary>
        /// Converts from string to enum.
        /// </summary>
        /// <param name="stringrepr">string representative of difficulty.</param>
        /// <returns>Difficulty enum.</returns>
        public static Difficulty ToDifficultyConverter(string stringrepr)
        {
            Difficulty diff;
            diff = stringrepr switch
            {
                "none" => Difficulty.None,
                "beginner" => Difficulty.Beginner,
                "advanced" => Difficulty.Advanced,
                "intermediate" => Difficulty.Intermediate,
                _ => throw new NotSupportedException("Not valid difficulty level"),
            };

            return diff;
        }

        /// <summary>
        /// Converts from string to enum.
        /// </summary>
        /// <param name="diff">enum representative of difficulty.</param>
        /// <returns>difficulty as a string.</returns>
        public static string ToStringConverter(Difficulty diff)
        {
            string stringrepr;

            stringrepr = diff switch
            {
                Difficulty.None => "none",
                Difficulty.Beginner => "beginner",
                Difficulty.Intermediate => "intermediate",
                Difficulty.Advanced => "advanced",
                _ => throw new NotSupportedException("Not valid difficulty level"),
            };

            return stringrepr;
        }
    }
}
