using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Krig
{
    public class Card
    {
        private readonly CardSuit _cardSuit;
        public CardSuit CardSuit { get { return _cardSuit; } }
        private readonly int _rankValue;
        public int RankValue { get { return _rankValue; }}

        public Card(CardSuit cardSuit, int rankValue)
        {
            CheckCardType(cardSuit, rankValue);
            _cardSuit = cardSuit;
            _rankValue = rankValue;
        }

        private static void CheckCardType(CardSuit cardSuit, int rankValue)
        {
            bool rankFound = false;
            foreach (var rank in AvailableValues) 
            {
                if(rank == rankValue)
                {
                    rankFound = true;
                }
            }

            if(!rankFound) { throw new Exception("Invalid rank value!"); }
        }

        public override string ToString()
        {
            return _cardSuit.ToString() + " " + GetCardRankString();
        }

        public static readonly int[] AvailableValues = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

        public string GetCardRankString()
        {
            switch (_rankValue)
            {
                case 2: return "2";
                case 3: return "3";
                case 4: return "4";
                case 5: return "5";
                case 6: return "6";
                case 7: return "7";
                case 8: return "8";
                case 9: return "9";
                case 10: return "10";
                case 11: return "Jack";
                case 12: return "Queen";
                case 13: return "King";
                case 14: return "Ace";
                default: throw new Exception("Invalid card rank!");
            }
        }

        public string GetCardCode()
        {            
            // Format is: [rankCode][first letter of suit]
      
            
            string cardRankString = GetCardRankString();
            string rankCode;
            // If number it can be more than one character
            if (int.TryParse(cardRankString, out _))
            {
                rankCode = cardRankString;
            }
            else
            {
                rankCode = cardRankString.Substring(0, 1);
            }

            string cardSuitString = _cardSuit.ToString();
            string suitFirstLetter = cardSuitString.Substring(0, 1);            
            
            return rankCode + suitFirstLetter;

        }
    }

    public enum CardSuit
    {
        Spade,
        Heart,
        Clover,
        Diamond
    }
}
