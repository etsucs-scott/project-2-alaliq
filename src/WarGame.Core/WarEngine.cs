namespace WarGame.Core
{
		//this is the main engine/backend of all the code
    public class WarEngine
    {
        //dict of player name to hand  holds all player hands
        public Dictionary<string, Hand> PlayerHands { get; private set; }

        //list of player names in play order
        private List<string> playerNames;

        //max rounds before forced end
        private const int RoundLimit = 10000;

        //tracks total rounds played
        public int RoundCount { get; private set; }

        //stores log output for each round so console can print it
        public List<string> RoundLog { get; private set; }

        //constructor takes number of players and sets up the game
        public WarEngine(int numPlayers)
        {
            if (numPlayers < 2 || numPlayers > 4)
                throw new ArgumentException("Must have 2-4 players");

            PlayerHands = new Dictionary<string, Hand>();
            playerNames = new List<string>();
            RoundLog = new List<string>();
            RoundCount = 0;

            //create player names and empty hands
            for (int i = 1; i <= numPlayers; i++)
            {
                string name = "Player " + i;
                playerNames.Add(name);
                PlayerHands[name] = new Hand();
            }

            //create deck and deal cards round-robin
            Deck deck = new Deck();
            int dealIndex = 0;
            while (deck.Count > 0)
            {
                //deal to each player in order - first players get extras if uneven
                PlayerHands[playerNames[dealIndex % numPlayers]].AddCard(deck.Deal());
                dealIndex++;
            }
        }

        //runs the full game and returns the winners name
        public string PlayGame()
        {
            while (RoundCount < RoundLimit)
            {
                //get list of players still in the game (have cards)
                List<string> activePlayers = GetActivePlayers();

                //if only one player left they win
                if (activePlayers.Count == 1)
                {
                    RoundLog.Add(activePlayers[0] + " wins the game!");
                    return activePlayers[0];
                }

                //if somehow no one has cards its a draw
                if (activePlayers.Count == 0)
                {
                    RoundLog.Add("No players left - draw!");
                    return "Draw";
                }

                RoundCount++;
                PlayRound(activePlayers);
            }

            //round limit hit - player with most cards wins
            return DetermineRoundLimitWinner();
        }

        //plays a single round including any tiebreakers
        private void PlayRound(List<string> activePlayers)
        {
            //pot holds all cards played this round and any tiebreaker rounds
            List<Card> pot = new List<Card>();

            //each active player plays their top card
            Dictionary<string, Card> playedCards = new Dictionary<string, Card>();
            foreach (string name in activePlayers)
            {
                Card card = PlayerHands[name].PlayCard();
                playedCards[name] = card;
                pot.Add(card);
            }

            //log what everyone played
            string roundHeader = "Round " + RoundCount;
            RoundLog.Add(roundHeader);
            foreach (var kvp in playedCards)
            {
                RoundLog.Add("  " + kvp.Key + ": " + kvp.Value);
            }

            //find highest rank played
            Rank highestRank = GetHighestRank(playedCards);

            //find who tied for highest
            List<string> tiedPlayers = new List<string>();
            foreach (var kvp in playedCards)
            {
                if (kvp.Value.Rank == highestRank)
                    tiedPlayers.Add(kvp.Key);
            }

            //if no tie the single winner takes the pot
            if (tiedPlayers.Count == 1)
            {
                string winner = tiedPlayers[0];
                AddPotToWinner(winner, pot);
                RoundLog.Add("  Winner: " + winner + " " + GetCardCounts());
            }
            else
            {
                //tie - log it and go to tiebreaker
                RoundLog.Add("  Tie between " + string.Join(" and ", tiedPlayers) + "!");
                RoundLog.Add("  Pot includes: " + string.Join(", ", pot));
                ResolveTie(tiedPlayers, pot);
            }
        }

        //resolves ties recursively until one player wins or all tied players eliminated
        private void ResolveTie(List<string> tiedPlayers, List<Card> pot)
        {
            //remove any tied players who have no cards left
            List<string> canPlay = new List<string>();
            foreach (string name in tiedPlayers)
            {
                if (PlayerHands[name].HasCards)
                    canPlay.Add(name);
                else
                    RoundLog.Add("  " + name + " eliminated (no cards for tiebreaker)");
            }

            //if one player left they win the pot
            if (canPlay.Count == 1)
            {
                AddPotToWinner(canPlay[0], pot);
                RoundLog.Add("  Tiebreaker winner: " + canPlay[0] + " " + GetCardCounts());
                return;
            }

            //if nobody can play the pot is just lost (edge case)
            if (canPlay.Count == 0)
            {
                RoundLog.Add("  All tied players eliminated - pot is discarded");
                return;
            }

            //each tied player plays one face-up tiebreaker card
            Dictionary<string, Card> tieCards = new Dictionary<string, Card>();
            foreach (string name in canPlay)
            {
                Card card = PlayerHands[name].PlayCard();
                tieCards[name] = card;
                pot.Add(card);
            }

            //log tiebreaker cards
            List<string> tieEntries = new List<string>();
            foreach (var kvp in tieCards)
            {
                tieEntries.Add(kvp.Key + ": " + kvp.Value);
            }
            RoundLog.Add("  Tiebreaker: " + string.Join(" | ", tieEntries));

            //check for another tie
            Rank highestRank = GetHighestRank(tieCards);
            List<string> newTied = new List<string>();
            foreach (var kvp in tieCards)
            {
                if (kvp.Value.Rank == highestRank)
                    newTied.Add(kvp.Key);
            }

            if (newTied.Count == 1)
            {
                //single winner takes entire pot
                AddPotToWinner(newTied[0], pot);
                RoundLog.Add("  Tiebreaker winner: " + newTied[0] + " " + GetCardCounts());
            }
            else
            {
                //still tied - recurse
                RoundLog.Add("  Still tied between " + string.Join(" and ", newTied) + "!");
                ResolveTie(newTied, pot);
            }
        }

        //gives all cards in pot to the winner (added to back of their queue)
        private void AddPotToWinner(string winner, List<Card> pot)
        {
            foreach (Card card in pot)
            {
                PlayerHands[winner].AddCard(card);
            }
        }

        //finds highest rank from played cards dict
        private Rank GetHighestRank(Dictionary<string, Card> playedCards)
        {
            Rank highest = Rank.Two;
            foreach (var kvp in playedCards)
            {
                if (kvp.Value.Rank > highest)
                    highest = kvp.Value.Rank;
            }
            return highest;
        }

        //returns list of players who still have cards
        private List<string> GetActivePlayers()
        {
            List<string> active = new List<string>();
            foreach (string name in playerNames)
            {
                if (PlayerHands[name].HasCards)
                    active.Add(name);
            }
            return active;
        }

        //builds a string showing each players card count
        private string GetCardCounts()
        {
            List<string> counts = new List<string>();
            foreach (string name in playerNames)
            {
                //use short label P1 P2 etc
                string shortName = "P" + name.Split(' ')[1];
                counts.Add(shortName + "=" + PlayerHands[name].Count);
            }
            return "(Cards: " + string.Join(", ", counts) + ")";
        }

        //determines winner when round limit is reached - most cards wins
        private string DetermineRoundLimitWinner()
        {
            int maxCards = 0;
            List<string> leaders = new List<string>();

            foreach (string name in playerNames)
            {
                int count = PlayerHands[name].Count;
                if (count > maxCards)
                {
                    maxCards = count;
                    leaders = new List<string> { name };
                }
                else if (count == maxCards)
                {
                    leaders.Add(name);
                }
            }

            //if tied at round limit its a draw
            if (leaders.Count > 1)
            {
                RoundLog.Add("Round limit reached - draw between " + string.Join(" and ", leaders));
                return "Draw";
            }

            RoundLog.Add("Round limit reached - " + leaders[0] + " wins with most cards!");
            return leaders[0];
        }
    }
}