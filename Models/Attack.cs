/*
 * i decided to make a class for attacks, it seems like well have a lot of them in the future so
 * i figured it would be nice to have a "framework" for them
 */

namespace turn_based_game.Models;

public class Attack
{
    public string Name { get; }
    public int Damage { get; }
    public int Cost { get; }

    public Attack(string name, int damage, int cost)
    {
        Name = name;
        Damage = damage;
        Cost = cost;
    }
}
