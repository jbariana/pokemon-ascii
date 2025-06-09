/*
 * fixed some things to work with the new attack class
 */

namespace turn_based_game.UI;

using System;
using System.IO;
using System.Linq;
using turn_based_game.Models;

public static class Dialogue
{
    public static void IntroScreen()
    {
        Console.Clear();
        Console.WriteLine("                                  ,'\\"); // Changed here
        Console.WriteLine("    _.----.        ____         ,'  _\\   ___    ___     ____");
        Console.WriteLine("_,-'       `.     |    |  /`.   \\,-'    |   \\  /   |   |    \\  |`.");
        Console.WriteLine("\\      __    \\    '-.  | /   `.  ___    |    \\/    |   '-.   \\ |  |");
        Console.WriteLine(" \\.    \\ \\   |  __  |  |/    ,','_  `.  |          | __  |    \\|  |");
        Console.WriteLine("   \\    \\/   /,' _`.|      ,' / / / /   |          ,' _`.|     |  |");
        Console.WriteLine("    \\     ,-'/  /   \\    ,'   | \\/ / ,`.|         /  /   \\  |     |");
        Console.WriteLine("     \\    \\ |   \\_/  |   `-.  \\    `'  /|  |    ||   \\_/  | |\\    |");
        Console.WriteLine("      \\    \\ \\      /       `-.`.___,-' |  |\\  /| \\      /  | |   |");
        Console.WriteLine("       \\    \\ `.__,'|  |`-._    `|      |__| \\/ |  `.__,'|  | |   |");
        Console.WriteLine("        \\_.-'       |__|    `-._ |              '-.|     '-.| |   |");
        Console.WriteLine("                                `'                            '-._|");
        Console.WriteLine();
        Console.WriteLine("                                          .__.__ ");
        Console.WriteLine("                     _____    ______ ____ |__|__|");
        Console.WriteLine("                     \\__  \\  /  ___// ___\\|  |  |");
        Console.WriteLine("                      / __ \\_\\___ \\\\  \\___|  |  |");
        Console.WriteLine("                     (____  /____  >\\___  >__|__|");
        Console.WriteLine("                          \\/     \\/     \\/ ");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    public static string BattleMenu(Character user, Character enemy, int round)
    {
        Display(user, enemy, round);
        Console.WriteLine("1. Attack");
        Console.WriteLine("2. Attach Energy");
        Console.WriteLine("3. Deck/ (WIP)");
        Console.WriteLine("4. End Turn");

        Console.Write("Enter your choice (1-4): ");
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
        {
            Console.WriteLine("Invalid input. Please enter a number: 1 - 4");
        }

        return BattleMenuHandler(user, enemy, round, choice);
    }

    public static string BattleMenuHandler(Character user, Character enemy, int round, int choice)
    {
        switch (choice)
        {
            case 1:
                return DisplayAttackChoices(user, enemy, round);
            case 2:
                return "attach energy";
            case 3:
                return "inventory not currently implemented";
            case 4:
                return "end turn";
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return BattleMenu(user, enemy, round); // re-prompt for valid input
        }
    }

    public static void AttachEnergy(Character user)
    {
        Console.WriteLine($"{user.Name} has attached energy to {user.Pokemon.Name}!");
        Thread.Sleep(1500);
    }

    public static string DisplayAttackChoices(Character user, Character enemy, int round)
    {
        Display(user, enemy, round);
        var attacks = user.Pokemon.Attacks;

        for (int i = 0; i < attacks.Count; i++)
        {
            var atk = attacks[i];
            Console.WriteLine($"{i + 1}. {atk.Name} (Dmg: {atk.Damage}, Cost: {atk.Cost})");
        }

        Console.WriteLine($"{attacks.Count + 1}. Back");

        Console.WriteLine("Please choose your attack: ");
        while (true)
        {
            var input = Console.ReadLine();

            if (int.TryParse(input, out int userChoiceIndex) &&
                userChoiceIndex >= 1 && userChoiceIndex <= attacks.Count + 1)
            {
                if (userChoiceIndex == attacks.Count + 1)
                {
                    return BattleMenu(user, enemy, round);
                }

                return attacks[userChoiceIndex - 1].Name;
            }

            Console.WriteLine($"Invalid input. Please enter a number between 1 and {attacks.Count + 1}:");
        }
    }

    public static void Display(Character user, Character enemy, int round)
    {
        Console.Clear();
        string border = new('=', 48);
        string innerBorder = new('-', 46);

        Console.WriteLine(border);
        Console.WriteLine($"|{"Round",-8}: {round,-36}|");
        Console.WriteLine(innerBorder);

        // User info
        Console.WriteLine($"| Player: {user.Name,-12}  Pokemon: {user.Pokemon.Name,-14}|");
        Console.WriteLine($"|   HP: {user.Pokemon.HP,4} / {user.Pokemon.MaxHP,-4}   Energy: {user.Pokemon.EnergyAttached,2}{"",15}|");
        Console.WriteLine(innerBorder);

        // Enemy info
        Console.WriteLine($"| Enemy:  {enemy.Name,-12}  Pokemon: {enemy.Pokemon.Name,-14}|");
        Console.WriteLine($"|   HP: {enemy.Pokemon.HP,4} / {enemy.Pokemon.MaxHP,-4}   Energy: {enemy.Pokemon.EnergyAttached,2}{"",15}|");
        Console.WriteLine(border);
    }

    public static void BattleDialogue(Character attacker, Character defender, string attackName, int damage, int round)
    {
        if (attacker.AttackingFirst)
        {
            Display(attacker, defender, round);
        }
        else
        {
            Display(defender, attacker, round);
        }

        Console.WriteLine($"{attacker.Name}: {attacker.Pokemon.Name}, use {attackName}!");
        Thread.Sleep(1500);

        Console.WriteLine($"{attacker.Pokemon.Name} uses {attackName}! It deals {damage} damage!");
        Thread.Sleep(2000);
    }

    public static void EndTurn(Character user)
    {
        Console.WriteLine($"{user.Name} has ended their turn...");
        Thread.Sleep(1500); // zZz
    }

    public static void CannotAttachEnergy()
    {
        Console.WriteLine("You cannot attach any more energy this turn!");
        Thread.Sleep(1500);
    }

    internal static void NotEnoughEnergy(Character attacker, int cost)
    {
        Console.WriteLine($"{attacker.Name}'s {attacker.Pokemon.Name} does not have enough energy to perform this attack! (Cost: {cost})");
        Thread.Sleep(1000);
    }

    private static List<string> LoadAllPokemonNames()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "pokemon-data.csv");
        var lines = File.ReadAllLines(path).Skip(1);
        var names = new List<string>();
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns.Length > 0 && !string.IsNullOrWhiteSpace(columns[0]))
            {
                names.Add(columns[0].Trim());
            }
        }
        return names;
    }

    public static string ChoosePokemonScreen()
    {
        var pokemons = LoadAllPokemonNames();
        const int pageSize = 20;
        int page = 0;
        int totalPages = (pokemons.Count + pageSize - 1) / pageSize;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose your Pokémon:");
            int start = page * pageSize;
            int end = Math.Min(start + pageSize, pokemons.Count);

            for (int i = start; i < end; i++)
            {
                Console.WriteLine($"{i + 1}. {pokemons[i]}");
            }

            if (totalPages > 1)
            {
                Console.WriteLine();
                if (page > 0) Console.WriteLine("P. Previous Page");
                if (page < totalPages - 1) Console.WriteLine("N. Next Page");
            }
            Console.WriteLine("Enter the number of your choice:");

            string input = Console.ReadLine();
            input = input == null ? "" : input.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Press any key to try again...");
                Console.ReadKey();
                continue;
            }
            if (input == "p" && page > 0)
            {
                page--;
                continue;
            }
            if (input == "n" && page < totalPages - 1)
            {
                page++;
                continue;
            }
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= pokemons.Count)
            {
                return pokemons[choice - 1];
            }

            Console.WriteLine("Invalid input. Press any key to try again...");
            Console.ReadKey();
        }
    }
}
