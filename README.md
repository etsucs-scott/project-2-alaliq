# WarGame - Card Game Simulation

A consolebased simulation of the card game War built in C#. Supports 2, 3, or 4 players.

## Build and Run


```
dotnet build
dotnet run --project src/WarGame.Console
```

The game will prompt you to enter the number of players (2-4).



You can also pass the player count as a command line argument to skip the prompt

```


dotnet run --project src/WarGame.Console -- 2
dotnet run --project src/WarGame.Console -- 3
dotnet run --project src/WarGame.Console -- 4
```

##  How Player Count is chosen

The program first checks for a command line argument, If a valid number (2-4) is provided it uses that. Otherwise it prompts the user to type a number and press enter.

## Project Structure

- src/WarGame.Core/ Core game library with all game logic
    - Card.cs Card class with Suit and Rank enums. Implements IComparable for rank comparison
    - Deck.cs standard 52-card deck stored as a Stack. Shuffled on creation using Fisher-Yates
    - Hand.cs Player hand stored as a Queue (play from front add to back)
    WarEngine.cs Main game engine handling rounds ties pot management and win conditions
    - src/WarGame.Console/ Console app that runs the game and prints output
    - Program.cs Entry point that gets player count runs the engine and prints round log
- WarGame_UML.png UML class diagram showing all classes and relationships


## Submission

Submitted via GitHub Classroom repository at https://github.com/etsucs-scott/project-2-alaliq
