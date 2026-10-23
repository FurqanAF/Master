// LevelElement
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

