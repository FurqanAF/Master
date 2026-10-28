// LevelElement
public static class Combat
{
    public static void Attack(Enemy initiator, Enemy defender)
    {
        int attackRoll = initiator.AttackDice.Throw();
        int defenceRoll = defender.DefenceDice.Throw();
        int damage =  attackRoll - defenceRoll;

        if (damage > 0)
        {
            defender.HP -= damage;
        }
    }

    public static void OneTurnOfCombat(Enemy initiator, Enemy defender)
    {
        Attack(initiator, defender);

        if (defender.HP > 0)
        {
            Attack(defender, initiator);
        }
    }
}

