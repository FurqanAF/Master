// LevelElement
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

