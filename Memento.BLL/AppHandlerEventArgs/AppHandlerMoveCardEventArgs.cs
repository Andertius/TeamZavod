using System;
using Memento.DAL;

namespace Memento.BLL.AppHandlerEventArgs
{
    public class AppHandlerMoveCardEventArgs
    {
        public AppHandlerMoveCardEventArgs(Card card, RememberingLevels level)
        {
            this.Card = new Card(card);
            this.RememberValue = level;
        }

        public Card Card { get; set; }
        public RememberingLevels RememberValue { get; }
    }
}
