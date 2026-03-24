namespace WarGame.Core
{
    //enum for the 4 suits in a standard deck
    public enum Suit
    {
        Hearts,
        Diamonds,
		
        Clubs,
        Spades
    }

    //ranks from 2 (low) to Ace (high) - int values allow direct comparison
    public enum Rank
    {
        Two = 2, Three, Four, Five, Six, Seven,
		
		
        Eight, Nine, Ten, Jack, Queen, King, Ace
    }

    //represents a single card with suit and rank - comparable by rank
    public class Card : IComparable<Card>
    {
        public Suit Suit { get; }
		
		
        public Rank Rank { get; }

        //constructor sets suit and rank
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        //compares cards by rank only - suits ignored in War
        public int CompareTo(Card? other)
        {
            if (other == null) return 1;
            return Rank.CompareTo(other.Rank);
        }

        //returns short display str like "A♠" or "10♥"
        public override string ToString()
        {
            //map rank to short str
            string rankStr = Rank switch
            {
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
				
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "10",
				
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                Rank.Ace => "A",
                _ => "?"
            };

            //map suit to symbol
            string suitStr = Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => "?"
            };

            return rankStr + suitStr;
        }
    }
}