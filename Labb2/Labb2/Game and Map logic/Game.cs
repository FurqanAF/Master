// LevelElement
public class Game
{
    private Level level;
    private Player player;
    private Enemy initiator;
    private Enemy defender;
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

