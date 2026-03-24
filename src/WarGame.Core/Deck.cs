namespace WarGame.Core
{
	
	//commentin begins
    public class Deck
    {
        //cards stored as stack - pop from top when dealing
        public Stack<Card> Cards { get; private set; }

        public Deck()
        {
            //build ordered list of all 52 cards
            List<Card> allCards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    allCards.Add(new Card(suit, rank));
                }
            }

            //shuffle then push onto stack
            Shuffle(allCards);

            Cards = new Stack<Card>();
            foreach (Card card in allCards)
            {
                Cards.Push(card);
            }
        }

        //fisher-yates shuffle - randomizes card order n place
        private void Shuffle(List<Card> cards)
        {
            Random rng = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                //swap
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        //deals (pops) one card of top of deck
        public Card Deal()
		
		
		
        {
            return Cards.Pop();
        }

        //returns how many cards left in deck
        public int Count => Cards.Count;
    }
}