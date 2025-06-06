namespace turn_based_game.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Pokemon
{
    // again, removed some of the setters from properties that aren't changed after construction
    public string Name { get; }
    public int HP { get; private set; }
    public int MaxHP { get; }
    public int EnergyAttached { get; private set; }
    public List<Attack> Attacks { get; } = []; // i felt like this was a solid change, making it a list of attacks rather than strings

    public Pokemon(string name)
    {
        //name is passed in
        Name = name;
        EnergyAttached = 0;
        bool found = false;
        MaxHP = 0;

        // !!!!! IMPORTANT NOTE FOR FUTURE PROGRAMS !!!!!
        // HACK: use relative path project roots, otherwise any file access will be broken when someone clones your repo
        var path = Path.Combine(AppContext.BaseDirectory, "weapons.csv");
        var lines = File.ReadAllLines(path).Skip(1);

        // tried to clean up this foreach a lil, lmk what you think
        foreach (var line in lines)
        {
            var columns = line.Split(',');

            if (columns.Length == 0)
            {
                continue;
            }

            if (columns[0].Trim().Equals(Name, StringComparison.OrdinalIgnoreCase))
            {
                found = true;

                // grab max hp from column 2 if it exists
                if (columns.Length > 2)
                {
                    if (int.TryParse(columns[2].Trim(), out int parsedHP))
                    {
                        MaxHP = parsedHP;
                    }
                }

                //generating attacks list
                for (int i = 4; i + 2 < columns.Length; i += 3)
                {
                    string attackName = columns[i].Trim();
                    string dmgStr = columns[i + 1].Trim();
                    string costStr = columns[i + 2].Trim();

                    // skip if any part of the attack is empty or says null
                    if (string.IsNullOrWhiteSpace(attackName))
                    {
                        continue;
                    }

                    if (dmgStr.Equals("null", StringComparison.OrdinalIgnoreCase) ||
                        costStr.Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // only adding it if damage and cost are valid ints
                    if (int.TryParse(dmgStr, out int damage) && int.TryParse(costStr, out int cost))
                    {
                        Attacks.Add(new Attack(attackName, damage, cost));
                    }
                }

                break;
            }
        }

        // if nothing matched the name, throw an error instead of failing silently. we can make a better fix later
        if (!found)
        {
            throw new Exception($"pokemon '{name}' not found in weapons.csv");
        }

        // set hp equal to max hp by default
        HP = MaxHP;
    }

    public void AttachEnergy()
    {
        EnergyAttached++;
    }

    // new method so that we arent changing the hp value outside of this class
    public void TakeDamage(int amount)
    {
        // subtract hp but clamp to 0
        HP -= amount;

        if (HP < 0)
        {
            HP = 0;
        }
    }
}