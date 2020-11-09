using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.DAL
{
    public class Settings
    {
        public double HoursPerDay { get; set; }
        public int CardsPerDay { get; set; }
        public CardOrder CardsOrder { get; set; }
        public bool ShowImages { get; set; }

        public Settings()
        {
            HoursPerDay = 3.5;
            CardsPerDay = 25;
            CardsOrder = CardOrder.Random;
            ShowImages = true;
        }

        public Settings(double hrs, int cards, CardOrder order, bool showImages)
        {
            HoursPerDay = hrs;
            CardsPerDay = cards;
            CardsOrder = order;
            ShowImages = showImages;
        }
    }
}
