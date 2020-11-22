// <copyright file="Settings.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

namespace Memento.BLL
{
    /// <summary>
    /// The main logic for settings.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            HoursPerDay = 3.5;
            CardsPerDay = 25;
            Theme = Theme.Light;
            CardOrder = CardOrder.Random;
            ShowImages = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="hours">Hours per day.</param>
        /// <param name="cards">Cards per day.</param>
        /// <param name="theme">The theme.</param>
        /// <param name="order">Cards order.</param>
        /// <param name="showImages">.</param>
        public Settings(double hours, int cards, Theme theme, CardOrder order, bool showImages)
        {
            HoursPerDay = hours;
            CardsPerDay = cards;
            Theme = theme;
            CardOrder = order;
            ShowImages = showImages;
        }

        /// <summary>
        /// Gets or sets hoursPerDay.
        /// </summary>
        public double HoursPerDay { get; set; }

        /// <summary>
        /// Gets or sets cardsPerDay.
        /// </summary>
        public int CardsPerDay { get; set; }

        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// Gets or sets cardOrder.
        /// </summary>
        public CardOrder CardOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether showImages.
        /// </summary>

        public bool ShowImages { get; set; }
    }
}
