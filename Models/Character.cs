namespace turn_based_game.Models;

public class Character
{
    // removed setters to make it immutable after construction (data safety)
    public string Name { get; }
    public Pokemon Pokemon { get; set; }
    public bool AttackingFirst { get; }
    public Character(string name, string pokemon, bool isAttackingFirst)
    {
        Name = name;
        Pokemon = new Pokemon(pokemon);
        AttackingFirst = isAttackingFirst;
    }
}
