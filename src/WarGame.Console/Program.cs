using WarGame.Core;

//get player count from command line args or prompt user


int numPlayers = 0;


if (args.Length > 0)
{
    //try to parse command line arg
    int.TryParse(args[0], out numPlayers);
}

//if no valid arg then prompt
if (numPlayers < 2 || numPlayers > 4)
{
    Console.Write("enter number of players (2-4): ");
    string? input = Console.ReadLine();
	
	
    int.TryParse(input, out numPlayers);

    //validate input
    if (numPlayers < 2 || numPlayers > 4)
    {
        Console.WriteLine("Invalid number of players Must be 2-4");
		
		
        return;
    }
}

Console.WriteLine("starting War with " + numPlayers + " players!!!...");
Console.WriteLine(new string('-', 40));

//create engine and run the game
WarEngine engine = new WarEngine(numPlayers);
string winner = engine.PlayGame();

//print every round from the log


foreach (string line in engine.RoundLog)
{
	
	
	
    Console.WriteLine(line);
}

//print final result


Console.WriteLine(new string('-', 40));
if (winner == "Draw")
{
    Console.WriteLine("Game ended in a draw!");
}
else
{
    Console.WriteLine("Winner: " + winner);
}
Console.WriteLine("Total rounds: " + engine.RoundCount);