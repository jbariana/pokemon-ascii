namespace turn_based_game;

public class Character
{
    public string Name { get; set; }
    public Pokemon Pokemon { get; set; }
    public bool AttackingFirst { get; set; }
    public Character(string name, string pokemon, bool isAttackingFirst)
    {
        Name = name;
        Pokemon = new Pokemon(pokemon);
        AttackingFirst = isAttackingFirst;
    }
}
