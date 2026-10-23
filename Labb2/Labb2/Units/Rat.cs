// LevelElement
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

