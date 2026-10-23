// LevelElement
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

