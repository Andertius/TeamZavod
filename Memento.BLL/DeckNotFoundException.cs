using System;

namespace Memento.BLL
{
    class DeckNotFoundException : Exception
    {
        public DeckNotFoundException(string message = "No such deck could be found in the database")
        {
            Message = message;
        }

        public override string Message { get; }
    }
}
