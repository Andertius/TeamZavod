namespace Memento.BLL
{
    public class Settings
    {
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

        public double HoursPerDay { get; set; }

        public int CardsPerDay { get; set; }

        public Theme Theme { get; set; }

        public CardOrder CardOrder { get; set; }

        public bool ShowImages { get; set; }
    }
}
