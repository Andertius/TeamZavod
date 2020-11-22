using System;
using Memento.DAL;

namespace Memento.BLL.AppHandlerEventArgs
{
    public class AppHandlerFlipEventArgs : EventArgs
    {
        public AppHandlerFlipEventArgs(Card card)
        {
            Card = new Card(card);
        }

        public Card Card { get; set; }
        public bool IsFlipped { get; set; }
    }
}
