namespace turn_based_game;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Dialogue
{
    public static void introScreen()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the turn-based game!");
        Console.WriteLine("Prepare for battle!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public static string DisplayAttackChoices(Character user)
    {

        for (int i = 0; i < user.Attacks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {user.Attacks[i]}");
        }
        Console.WriteLine("Please choose your attack: ");
        int userChoiceIndex;
        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out userChoiceIndex) &&
                userChoiceIndex >= 1 && userChoiceIndex <= user.Attacks.Count)
            {
                break;
            }
            Console.WriteLine("Invalid input. Please enter a valid attack number:");
        }
        return user.Attacks[userChoiceIndex - 1];
    }

    public static void display(Character user, Character enemy, int round)
    {
        Console.Clear();
        Console.WriteLine("===================================");
        string border = new string('-', 34);
        Console.WriteLine(border);
        Console.WriteLine($"| {"Round:",-8} {round,-22}|");
        Console.WriteLine(border);
        Console.WriteLine($"| {user.Name,-10} HP: {user.HP,4} / {user.MaxHP,-4}      |");
        Console.WriteLine($"| {enemy.Name,-10} HP: {enemy.HP,4} / {enemy.MaxHP,-4}      |");
        Console.WriteLine(border);
        Console.WriteLine("===================================");
    }

    public static void Battle_Dialogue(Character attacker, Character defender, string attackName, int damage)
    {
        Console.WriteLine($"{attacker.Name} is preparing to use {attackName}!");
        Thread.Sleep(1500);

        Console.WriteLine($"{attacker.Name} uses {attackName}! It deals {damage} damage!");
        Thread.Sleep(2000);
    }
}