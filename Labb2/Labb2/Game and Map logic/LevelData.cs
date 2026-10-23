// LevelElement
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

