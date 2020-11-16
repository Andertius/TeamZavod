using System;
using System.Collections.Generic;
using System.Text;

using Memento.DAL;

namespace Memento.BLL
{
    public class Settings
    {
        public double HoursPerDay { get; set; }
        public int CardsPerDay { get; set; }
        public Theme Theme { get; set; }
        public CardOrder CardOrder { get; set; }
        public bool ShowImages { get; set; }

        public Settings()
        {
            HoursPerDay = 3.5;
            CardsPerDay = 25;
            Theme = Theme.Light;
            CardOrder = CardOrder.Random;
            ShowImages = true;
        }

        public Settings(double hrs, int cards, Theme theme, CardOrder order, bool showImages)
        {
            HoursPerDay = hrs;
            CardsPerDay = cards;
            Theme = theme;
            CardOrder = order;
            ShowImages = showImages;
        }
    }
}
