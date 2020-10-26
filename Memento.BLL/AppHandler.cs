using System;
using System.Collections.Generic;
using System.Text;

using Memento.DAL;

namespace Memento.BLL
{
    public class AppHandler
    {
        public AppHandler(Deck deck)
        {
            Deck = new Deck(deck);
        }

        public AppHandler(int deckId)
        {
            Deck = new Deck(Repository.FetchDeck(deckId));
        }

        public AppHandler(string deckName)
        {
            Deck = new Deck(Repository.FetchDeck(deckName));
        }

        public Deck Deck { get; }

        public void Start()
        {
            //шафл деки
            //відлік часу
            //while(canelationtoken){}
        }

        //flip card

        //nextCard

        //MoveCardIntoDeck

        //Cancel
        //{canelationtoken=stop}


    }
}
