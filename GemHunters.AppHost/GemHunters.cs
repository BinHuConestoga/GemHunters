/*
Summary: Game 'Gem Hunters'
Description: Two players are playing this game to collect Gems. There are 15 rounds for each player. Who gets more Gems will win this game.
Author: Bin Hu
Create Date: 2024/6/6
*/

using System;

/*  Define a class 'Position'. Two varibles are introduced in this class to identicate the location.*/
public class Position
{
    public int x;
    public int y;

    public int X
    {
        get { return x; }
        set { x = value; }     
    }

    public int Y
    {
        get { return y; }
        set { y = value; }
    }

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

/*  Define a class 'Player'.  */
/*  Add 3 attributes to a play: Name, Position and Gems*/
public class Player
{
    public string Name;
    public Position Position;
    public int Gems;

    public string name
    {
        get { return Name; }
    }

    public Position position
    {
        get { return Position; }
        set { Position = value; }
    }

    public int gems
    {
        get { return Gems; }
        set { Gems = value; }
    }

/*  Initiate the quantity of Gems for the players to 0. */
    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        Gems = 0;
    }

/*  Add a method 'Move': U for up, D for down, L for left and R for right.*/
    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                Position.y--;
                break;
            case 'D':
                Position.y++;
                break;
            case 'L':
                Position.x--;
                break;
            case 'R':
                Position.x++;
                break;
        }
    }
}

/*  Define the basic values(G/O/-) value and make them random.*/
public class Cell
{
    Random rnd = new Random();
    List<string> fixedNumbers = new List<string> { "G", "O", "-" };
    public string Occupant;

    public string occupant
    {
        get { return Occupant; }
        set { Occupant = value; }
    }
    public Cell()
    {
        Occupant = fixedNumbers[rnd.Next(0, fixedNumbers.Count)];
    }
}

/*  Initiate the board with 6*6*/
public class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell();
            }
        }
/*  Player 1 starts from top left and Player 2 starts from bottom right.*/
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";
    }

/*  Insert value the board with G/O/- and set colors to them*/
    public void Display()
    {

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (Grid[i, j].Occupant == "G")
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (Grid[i, j].Occupant == "O")
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.White;
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

/*  Define the valid move. Handle the obstacles.*/
    public bool CountMove(Player player, char direction)
    {
        int X = player.Position.x;
        int Y = player.Position.y;
        switch (direction)
        {
            case 'U':
                Y--;
                break;
            case 'D':
                Y++;
                break;
            case 'L':
                X--;
                break;
            case 'R':
                X++;
                break;
        }
        if (X < 0 || X >= 6 || Y < 0 || Y >= 6)
            return false;
        if (Grid[Y, X].Occupant == "O")
        {
            Console.WriteLine("Oops! It's an obstacle!");
            return false;
        }
        return true;
    }

/*  Add 1 if finding a gem/No change if it's '-'*/
    public bool FoundGem(Player player)
    {
        if (Grid[player.Position.y, player.Position.x].Occupant == "G")
        {
            player.Gems++;
            Grid[player.Position.y, player.Position.x].Occupant = "-";
            return true;
        }
        return false;
    }
}

/*  A methond shows the whole process of the game. will put this into Main method.*/
public class GemHunters
{
    private Board board;
    private Player player1;
    private Player player2;
    private Player currentTurn;
    private int totalTurns;

    public GemHunters()
    {
        board = new Board();
        player1 = new Player("P1", new Position(0, 0));
        player2 = new Player("P2", new Position(5, 5));
        currentTurn = player1;
        totalTurns = 0;
    }

    public void Start()
    {
        Console.WriteLine("Let's start the game!\n");
        while (!IsGameOver())
        {
            Console.WriteLine($"Round {totalTurns + 1}: {currentTurn.Name}'s turn");
            board.Display();
            char direction;
            do
            {
                Console.WriteLine("Input direction (U/D/L/R): ");
                direction = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
            } while (!board.CountMove(currentTurn, direction));

            currentTurn.Move(direction);
            Console.CursorVisible = true;
            if (board.FoundGem(currentTurn))
                Console.WriteLine($"{currentTurn.Name} detected a gem!");

            totalTurns++;
            SwitchTurn();
        }

        GameResults();
    }

    private void SwitchTurn()
    {
        currentTurn = currentTurn == player1 ? player2 : player1;   /* Switch turns*/
    }

    private bool IsGameOver()
    {
        return totalTurns >= 30;    /* 30 turns in total for the game.*/
    }

/*  Results of the game: Player 1 wins/Player 2 wins/Tie*/
    private void GameResults()
    {
        Console.WriteLine("Game Over!");
        Console.WriteLine($"Player1's Gems: {player1.Gems}");
        Console.WriteLine($"Player2's Gems: {player2.Gems}");
        if (player1.Gems > player2.Gems)
            Console.WriteLine("Player1 Wins! Congratuation to Player1!");
        else if (player1.Gems < player2.Gems)
            Console.WriteLine("Player2 Wins! Congratuation to Player2!");
        else
            Console.WriteLine("It's a Tie!");
    }
}

class Project
{
    static void Main(string[] args) /*Main method.*/
    {
        GemHunters gemHunters = new GemHunters();
        gemHunters.Start();
    }
}