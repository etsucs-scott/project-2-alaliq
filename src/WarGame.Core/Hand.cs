namespace WarGame.Core
{
    public class Hand
    {
        //queue holds the players carrds
        public Queue<Card> Cards { get; private set; }

        //creates an empty hand
        public Hand()
        {
            Cards = new Queue<Card>();
        }

        //adds a card to back o hand
        public void AddCard(Card card)
        {
			
			
            Cards.Enqueue(card);
        }

        //plays (removes) card from front of hand
        public Card PlayCard()
        {
            return Cards.Dequeue();
        }

        
        public int Count => Cards.Count;

        
        public bool HasCards => Cards.Count > 0;
    }
}