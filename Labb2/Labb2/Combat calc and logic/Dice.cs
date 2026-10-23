// LevelElement
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

