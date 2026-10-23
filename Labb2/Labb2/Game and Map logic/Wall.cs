// LevelElement
public class Wall : LevelElement
{
    public bool Discovered { get; set; } = false;
    public Wall() 
    {
        Color = ConsoleColor.DarkGray;
        Symbol = '#';
    }


}

