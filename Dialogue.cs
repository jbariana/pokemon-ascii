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
        Console.WriteLine("Welcome to the pokemon-ascii!");
        Console.WriteLine("Prepare for battle!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }


    public static string BattleMenu(Character user, Character enemy, int round)
    {
        display(user, enemy, round);
        Console.WriteLine("1. Attack");
        Console.WriteLine("2. Attach Energy");
        Console.WriteLine("3. Inventory (WIP)");
        Console.WriteLine("4. End Turn");

        Console.WriteLine("Enter your choice (1-4): ");
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
                Console.WriteLine($"{user.Name} has ended their turn.");
                Thread.Sleep(1500);
                return "end turn";
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return BattleMenu(user, enemy, round); // re-prompt for valid input
        }
    }

    //change when can use multiple pokemon
    public static void AttachEnergy(Character user)
    {

        Console.WriteLine($"{user.Name} has attached energy to {user.Pokemon.Name}!");
        Thread.Sleep(1500);
    }
    public static string DisplayAttackChoices(Character user, Character enemy, int round)
    {
        display(user, enemy, round);
        var attacks = user.Pokemon.Attacks;
        int numAttacks = attacks.Count / 3;
        for (int i = 0; i < numAttacks; i++)
        {
            Console.WriteLine($"{i + 1}. {attacks[i * 3]}");
        }
        Console.WriteLine($"{numAttacks + 1}. Back");

        int userChoiceIndex = 1;

        Console.WriteLine("Please choose your attack: ");
        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out userChoiceIndex) &&
                userChoiceIndex >= 1 && userChoiceIndex <= numAttacks + 1)
            {
                break;
            }
            Console.WriteLine($"Invalid input. Please enter a number between 1 and {numAttacks + 1}:");
        }

        if (userChoiceIndex == numAttacks + 1)
        {
            return BattleMenu(user, enemy, round);
        }

        return attacks[(userChoiceIndex - 1) * 3];
    }

    public static void display(Character user, Character enemy, int round)
    {
        Console.Clear();
        string border = new string('=', 48);
        string innerBorder = new string('-', 46);

        Console.WriteLine(border);
        Console.WriteLine($"|{"Round",-8}: {round,-36}|");
        Console.WriteLine(innerBorder);

        // User info
        Console.WriteLine($"| Player: {user.Name,-12}  Pokemon: {user.Pokemon.Name,-14}|");
        Console.WriteLine($"|   HP: {user.Pokemon.HP,4} / {user.Pokemon.MaxHP,-4}   Energy: {user.Pokemon.energyAttached,2}{"",15}|");
        Console.WriteLine(innerBorder);

        // Enemy info
        Console.WriteLine($"| Enemy:  {enemy.Name,-12}  Pokemon: {enemy.Pokemon.Name,-14}|");
        Console.WriteLine($"|   HP: {enemy.Pokemon.HP,4} / {enemy.Pokemon.MaxHP,-4}   Energy: {enemy.Pokemon.energyAttached,2}{"",15}|");
        Console.WriteLine(border);
    }

    public static void Battle_Dialogue(Character attacker, Character defender, string attackName, int damage, int round)
    {
        if (attacker.AttackingFirst)
        {
            display(attacker, defender, round);
        }
        else
        {
            display(defender, attacker, round);
        }
        Console.WriteLine($"{attacker.Name}: {attacker.Pokemon.Name}, use {attackName}!");
        Thread.Sleep(1500);

        Console.WriteLine($"{attacker.Pokemon.Name} uses {attackName}! It deals {damage} damage!");
        Thread.Sleep(2000);
    }

    public static void EndTurn(Character user)
    {
        Console.WriteLine($"{user.Name} has ended their turn.");
        Thread.Sleep(1500);
    }

    public static void CannotAttachEnergy()
    {
        Console.WriteLine("You cannot attach any more energy this turn!");
        Thread.Sleep(1500);
    }
}