using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExtensions;
using static Krig.Card;

namespace Krig
{
    public class CardDeck
    {
        private readonly List<Card> _cards;
        public List<Card> Cards { get { return _cards; } }

        public int RemainingCards {
            get {
                return _cards.Count;
            }
        }

        

        public CardDeck()
        {
            _cards = new List<Card>();
            GenerateCards();
        }

        public void GenerateCards()
        {
            _cards.Clear();
            foreach ( CardSuit cardSuit in Enum.GetValues(typeof(CardSuit))) {

                foreach(int value in Card.AvailableValues)
                {
                    _cards.Add(new Card(cardSuit, value));
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            _cards.Shuffle();
        }

        public Card? DealCard()
        {
            if (_cards.Count > 0) {
                int lastIndex = _cards.Count - 1;
                Card returnCard = _cards[lastIndex];
                _cards.RemoveAt(lastIndex);
                return returnCard;
            }
            return null;
        }

    }
}
