namespace turn_based_game;

public class Character
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public string Pokemon { get; set; }
    public List<string> Attacks { get; set; } = [];
    public Character(string name, int hP, int maxHP, string pokemon)
    {
        Name = name;
        HP = hP;
        MaxHP = maxHP;
        Pokemon = pokemon;
        var attacks = new List<string>();

        // find attack list, skip the first line (header)
        if (File.Exists("weapons.csv"))
        {
            var lines = File.ReadAllLines("weapons.csv").Skip(1);
            foreach (var line in lines)
            {
                var columns = line.Split(',');
                if (columns.Length > 0 && columns[0].Trim().Equals(pokemon, StringComparison.OrdinalIgnoreCase))
                {
                    if (columns.Length > 4 && !string.IsNullOrWhiteSpace(columns[4]))
                        attacks.Add(columns[4].Trim());
                    else
                    {
                        attacks.Add("wha");
                    }
                    if (columns.Length > 7 && !string.IsNullOrWhiteSpace(columns[7]))
                        attacks.Add(columns[7].Trim());
                    else
                    {
                        attacks.Add("not an attack");
                    }
                    break;
                }
            }
        }
        Attacks = attacks;
    }
}
