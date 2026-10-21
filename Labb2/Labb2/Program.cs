// LevelElement
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;


Game game = new Game();
game.runGame();

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
public class LevelData
{
    private List<LevelElement> elements = new();
    public List<LevelElement> Elements => elements;
    public void Load(string filename, Level level)
    {
        
        string[] lines = File.ReadAllLines(filename);
        Player player = new Player(level);

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char currentChar = lines[y][x];
                switch(currentChar)
                {
                    case '#':
                        Wall wall = new Wall();
                        wall.X = x; wall.Y = y;
                        elements.Add(wall);
                        break;
                    case '@':
                        player.X = x; player.Y = y;
                        elements.Add(player);
                        break;
                    case 'r':
                        Rat rat = new Rat(level);
                        rat.X = x; rat.Y = y; 
                        elements.Add(rat);
                        break;
                    case 's':
                        Snake snake = new Snake(player, level);
                        snake.X = x; snake.Y = y;
                        elements.Add(snake);
                        break;
                }
            }
        }
    }

}
public static class Combat
{
    public static void Attack(Enemy initiator, Enemy defender)
    {
        int attackRoll = initiator.AttackDice.Throw();
        int defenceRoll = defender.DefenceDice.Throw();
        int damage =  attackRoll - defenceRoll;

        if (damage > 0)
        {
            defender.HP -= damage;
        }
    }

    public static void OneTurnOfCombat(Enemy initiator, Enemy defender)
    {
        Attack(initiator, defender);

        if (defender.HP > 0)
        {
            Attack(defender, initiator);
        }
    }
}

public class Level
{
    private LevelData data;
    private Game game;

    public List<LevelElement> Elements => data.Elements;
    public bool ValidMoveCheck(LevelElement currentObject, int x, int y)
    {

        foreach (LevelElement element in Elements)
        {
            if (element == currentObject)
            {
                continue;
            }
            if (element.X == x && element.Y == y)
            {
                if (element is Wall)
                    return false;

                else if ((element is Player || currentObject is Player) 
                    &&   (element is Enemy || currentObject is Enemy))
                {
                    game.EnterCombat((Enemy)currentObject,(Enemy)element);
                }
            }

        }
        return true;
    }

    public Level(Game game)
    {
        data = new LevelData();
        data.Load("Level1.txt", this);
        this.game = game;
    }
}

public class Wall : LevelElement
{
    public bool Discovered { get; set; } = false;
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
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public abstract void Update();
    
    
}

public class Rat : Enemy
{
    private Level level;
    public Rat(Level level)
    {
        this.level = level;
        HP = 10;
        Name = "Rat";
        Symbol = 'r';
        Color = ConsoleColor.Red;
        AttackDice = new Dice(NumberOfDice: 1, SidesPerDice: 6, flat_modifier: 3);
        DefenceDice = new Dice(NumberOfDice: 1, SidesPerDice: 6, flat_modifier: 1);
    }
    public override void Update()
    {

        int direction = Random.Shared.Next(0, 4);
        int testX = X;
        int testY = Y;
        switch (direction) 
        {
            case 0:
                testY = Y+1;
                break;
            case 1:
                testY = Y-1;
                break;
            case 2:
                testX = X-1;
                break;
            case 3:
                testX = X+1;
                break;

        }
        if (level.ValidMoveCheck(this, testX, testY))
        {
            X = testX;
            Y = testY;

        }
    }
}

public class Dice
{
    private int Highroll;
    private int Lowroll;
    private int NumberOfDice;
    private int ThrowResult;
    private int flat_modifier;
    public Dice(int NumberOfDice, int SidesPerDice, int flat_modifier)
    {
        Highroll = SidesPerDice;
        Lowroll = 1;
        this.NumberOfDice = NumberOfDice;
        this.flat_modifier = flat_modifier;
    }
    public override string ToString()
    {
        return $"{NumberOfDice}d{Highroll}+{flat_modifier}";
    }
    public int Throw()
    {
        ThrowResult = 0;
        for (int i = 0; i < NumberOfDice; i++)
        {
            ThrowResult += Random.Shared.Next(Lowroll, Highroll + 1);
        }
        ThrowResult += flat_modifier;
        return ThrowResult;
    }
}
public class Snake : Enemy
{
    private Player player;
    private Level level;
    private Dice dice;
    public Snake(Player player, Level level)
    {
        this.player = player;
        this.level = level;
        HP = 25;
        Name = "Snake";
        Symbol = 's';
        Color = ConsoleColor.Green;
        AttackDice = new Dice(NumberOfDice: 3,SidesPerDice: 4,flat_modifier: 2);
        DefenceDice = new Dice(NumberOfDice: 1,SidesPerDice: 8,flat_modifier: 5);
        //MovementPattern = GeneSnakeeMovementPattern(x) placeholder for a case based movement pattern
    }
    public override void Update()
    {
        int testX = X; //testvariabler x och y
        int testY = Y;
        int deltaX = X - player.X;
        int deltaY = Y - player.Y;
        int distance = (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY); //valde int med mening - kändes bättre ingame
        int snakeFreezeDistance = 2;
        int direction = Random.Shared.Next(0, 2);
        if (deltaX == 0)
            direction = 1;
        else if (deltaY == 0)
            direction = 0; 

        if (distance <= snakeFreezeDistance)
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
            if (!level.ValidMoveCheck(this, testX, testY))
            {
                HP = (int)(0.95 * HP - 1); //anti crowding/tactical mechanic, int is intentional functionally creating a floor()
            }
            else
            {
                X = testX;
                Y = testY;
            }

        }

    }
}

public class Player : Enemy
{
    private Level level;
    public Player(Level level)
    {
        this.level = level;
        HP = 100;
        AttackDice = new Dice(NumberOfDice: 2, SidesPerDice: 6, flat_modifier: 2);
        DefenceDice = new Dice(NumberOfDice: 2, SidesPerDice: 6, flat_modifier: 0);
        Name = "Player";
        Symbol = '@';
        Color = ConsoleColor.DarkMagenta;
    }
    public override void Update()
    {
        int testX = X;
        int testY = Y;
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
        }
        while ( key is not (ConsoleKey.W or ConsoleKey.A or ConsoleKey.S or ConsoleKey.D or ConsoleKey.Spacebar));

        switch (key)
        {
            case ConsoleKey.W:
                testY--;
                break;
            case ConsoleKey.S:
                testY++;
                break;
            case ConsoleKey.A:
                testX--;
                break;
            case ConsoleKey.D:
                testX++;
                break;
            case ConsoleKey.Spacebar:
                break;
        }
        if (level.ValidMoveCheck(this, testX, testY))
            {
                X = testX;
                Y = testY;
            }
    }
}
public enum GameState
{
    OutOfCombat,
    InCombat,
    GameOver

}

public class Game
{
    private Level level;
    private Player player;
    private LevelData leveldata;
    private Enemy initiator;
    private Enemy defender;
    private Wall wall;
    int sightRadius = 5;
    private GameState currentState = GameState.OutOfCombat;
    bool gameisRunning = true;
    public void ExitCombat(string cause)
    {
        if (cause == "death")
        {
            currentState = GameState.GameOver;
            Console.Clear();
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("You died loser");
            }
            gameisRunning = false;
        }
        else
        {
            currentState = GameState.OutOfCombat;
        }
    }

    public double DistanceToPlayer(Player player, LevelElement element)
    {
        int deltaX = player.X - element.X;
        int deltaY = player.Y - element.Y;
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        return (distance);
    }

    public void EnterCombat(Enemy initiator, Enemy defender)
    {
        this.initiator = initiator;
        this.defender = defender;
        currentState = GameState.InCombat;
    }
    public void runGame()
    {
        level = new Level(this);
        player = level.Elements.OfType<Player>().FirstOrDefault();
        while (gameisRunning)
        {
            while (currentState == GameState.OutOfCombat)
            {
                Console.Clear();

                foreach (LevelElement element in level.Elements)
                {
                    if (DistanceToPlayer(player, element) <= sightRadius)
                    {
                        element.Draw();
                        if (element is Wall wall)
                        {
                            wall.Discovered = true;
                        }
                    }
                    else
                    {
                        if (element is Wall wall && wall.Discovered)
                        {
                            element.Draw();
                        }
                    }
                }
                player.Update();
                foreach (Enemy enemy in level.Elements.OfType<Enemy>())
                {
                    if (enemy is Player)
                        continue;
                    enemy.Update();
                }
            }

            while (currentState == GameState.InCombat
                && initiator.HP > 0 && defender.HP > 0)
            {
                ConsoleKey key;
                do
                {
                    key = Console.ReadKey(true).Key;
                }
                while (key != ConsoleKey.Spacebar);

                Combat.OneTurnOfCombat(initiator, defender);

                if (player.HP <= 0)
                {
                    ExitCombat("death");

                }
                else if (initiator.HP <= 0 ||  defender.HP <= 0)
                {
                    Enemy defeatedEnemy = initiator is Player ? defender : initiator;
                    level.Elements.Remove(defeatedEnemy);
                    ExitCombat("survived");
                }
            }
        }
    }
}

