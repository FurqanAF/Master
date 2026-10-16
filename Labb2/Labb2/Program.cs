// LevelElement
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

public abstract class LevelElement
{

    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
    public ConsoleColor Color {  get; set; }
    public void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(X, Y);
        Console.Write(Symbol);
        Console.ResetColor(); // ifall jag vill lägga till textpopups osv
    }
}

public class Level
{
    public List<LevelElement> Elements { get; } = new();
    public bool ValidMoveCheck(LevelElement currentObject, int x, int y)
    {

        foreach (LevelElement element in Elements)
        {
            if (element == currentObject) //prevents self blocking in movement
                continue;
            if (element.X == x && element.Y == y) //condition for invalid move
                return false;

        }
        return true;
    }

    public Level()
    {
        Player player = new Player();

        Elements.Add(player);
        Elements.Add(new Wall());
        Elements.Add(new Rat());
        Elements.Add(new Snake(player, this));
    }
}

public class Wall : LevelElement
{
    public Wall() 
    {
        Color = ConsoleColor.DarkGray;
        Symbol = '#';
    }


}

public abstract class Enemy : LevelElement
{
    public int HP { get; set; }
    public string Name { get; set; }
    public int AttackDice { get; set; }
    public int DefenceDice { get; set; }

    public abstract void Update();
    
    
}

public class Rat : Enemy
{
    public Rat()
    {
        HP = 10;
        Name = "Rat";
        Symbol = 'r';
        Color = ConsoleColor.Red;
        //Attack = dice(a,b,c) placeholder until i make a dice function
        //defence = dice(a2, b2, c2) same here
        //MovementPattern = GenerateMovementPattern(x) placeholder for a case based movement pattern
    }
    public override void Update()
    {
        int direction = Random.Shared.Next(0, 4);
        //if-elseif logic scanning for collision and changing the direction value if collision is inbound
        switch (direction) 
        {
            case 0:
                Y--;
                break;
            case 1:
                Y++;
                break;
            case 2:
                X--;
                break;
            case 3:
                X++;
                break;

        }
    }
}

public class Snake : Enemy
{
    private Player player;
    private Level level;
    public Snake(Player player, Level level)
    {
        this.player = player;
        this.level = level;
        HP = 25;
        Name = "Snake";
        Symbol = 'r';
        Color = ConsoleColor.Green;
        //Attack = dice(a,b,c) placeholder until i make a dice function
        //defence = dice(a2, b2, c2) same here
        //MovementPattern = GeneSnakeeMovementPattern(x) placeholder for a case based movement pattern
    }
    public override void Update()
    {
        int testX = X; //testvariabler x och y
        int testY = Y;
        int deltaX = X - player.X;
        int deltaY = Y - player.Y;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY); //2d distans ges av pythagoras
        int direction = Random.Shared.Next(0, 2);
        if (deltaX == 0)
            direction = 1; //tvinga rörelse i Y-led
        else if (deltaY == 0)
            direction = 0; //reciprokt för X-led - för att undvika edge cases där random genererar ett case där Math.sign = 0 

        if (distance <= 2)
        {
            switch (direction)
            {
                case 0:
                    testX += Math.Sign(deltaX); //smidigt sätt att flytta ett steg i 'rätt' riktning
                    break;
                case 1:
                    testY += Math.Sign(deltaY);
                    break;
            }
            for (int attempt_to_move = 0 )
                if (level.ValidMoveCheck(this, testX, testY))
                {
                    X = testX;
                    Y = testY;
                    break;
                }

        }

    }
}

public class Player : Enemy
{
    public Player()
    {
        HP = 100;
        //atkdice
        //defdice
        Name = "Player";
        Symbol = 'O';
        Color = ConsoleColor.DarkMagenta;
    }
    public override void Update()
    {
        throw new NotImplementedException();
    }
}