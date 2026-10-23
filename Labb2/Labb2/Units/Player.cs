// LevelElement
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

