namespace turn_based_game;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Pokemon
{
    public string Name { get; set; } //
    public int HP { get; set; } //
    public int MaxHP { get; set; } //
    public int energyAttached { get; set; } //
    public List<string> Attacks { get; set; } = new List<string>(); //
    public Pokemon(string name)
    {
        //name is passed in
        Name = name;
        energyAttached = 0;
        MaxHP = 0; // default max HP
        var lines = File.ReadAllLines("weapons.csv").Skip(1);
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns.Length > 0 && columns[0].Trim().Equals(Name, StringComparison.OrdinalIgnoreCase))
            {
                //finding MaxHP
                if (columns.Length > 2 && !string.IsNullOrWhiteSpace(columns[2]) && !columns[2].Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(columns[2].Trim(), out int parsedHP))
                    {
                        MaxHP = parsedHP;
                    }
                }
                //generating attacks list
                for (int i = 4; i < Math.Min(columns.Length, 9); i++)
                {
                    if (!string.IsNullOrWhiteSpace(columns[i]) && !columns[i].Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        Attacks.Add(columns[i].Trim());
                    }
                }
                break;
            }
        }
        // by default Hp matches Max HP
        HP = MaxHP;
    }
    public void energyAttach()
    {
        energyAttached++;
    }
}