using Memento.DAL;
using System;

namespace Memento.BLL
{
    class AppHandlerEventArgs : EventArgs
    {
        public AppHandlerEventArgs(Card card)
        {
            Card = new Card(card);
        }
        public Card Card { get; set; }
        public bool isFlipped = false;
    }
    
}
