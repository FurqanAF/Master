// LevelElement
public abstract class Enemy : LevelElement
{
    public int HP { get; set; }
    public string Name { get; set; }
    public Dice AttackDice { get; set; }
    public Dice DefenceDice { get; set; }

    public abstract void Update();
    
    
}

