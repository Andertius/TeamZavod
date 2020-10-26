using Memento.DAL;
using System;

namespace Memento.BLL
{
    public class AppHandlerFlipEventArgs : EventArgs
    {
        public AppHandlerFlipEventArgs(Card card)
        {
            Card = new Card(card);
        }
        public Card Card { get; set; }
        public bool isFlipped { get; set; }
    }

    public class AppHandlerMoveCardEventArgs : EventArgs
    {
        public AppHandlerMoveCardEventArgs(Card card)
        {
            Card = new Card(card);
        }
        public Card Card { get; set; }
        public int RememberValue { get; }
    }
}
