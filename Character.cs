using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace turn_based_game;

public class Character
{
    public string Name { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public string Weapon { get; set; }
    public List<string> Attacks { get; set; } = new List<string>();
    public Character(string name, int hP, int maxHP, string weapon)
    {
        Name = name;
        HP = hP;
        MaxHP = maxHP;
        Weapon = weapon;
        var attacks = new List<string>();

        //find attack list
        var lines = System.IO.File.ReadAllLines("weapons.csv");
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns.Length > 1 && columns[0].Trim().Equals(weapon, StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 1; i < columns.Length; i += 2)
                {
                    attacks.Add(columns[i].Trim());
                }
                break;
            }
        }
        Attacks = attacks;
    }
}
