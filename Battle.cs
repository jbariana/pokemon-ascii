using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace turn_based_game;

public class Battle
{
    public Character user;
    public Character enemy;

    public Battle(Character user, Character enemy)

    {
        this.user = user;
        this.enemy = enemy;
    }
    public void StartBattle()
    {
        bool gameIsOver = false;
        bool enemyIsAlive = true;
        int round = 0;

        while (!gameIsOver && enemyIsAlive)
        {
            round++;

            // GAME OVER STATES
            if (gameIsOver)
            {
                gameOver();
                return;
            }
            else if (!enemyIsAlive)
            {
                enemyDefeated();
                return;
            }

            // GAME CONTINUE STATES
            else if (round % 2 == 1)
            {
                // Display
                Dialogue.display(user, enemy, round);

                // choose attack
                string userChoice = Dialogue.DisplayAttackChoices(user);

                // start battle sequence
                Battle_Sequence(user, enemy, userChoice);

                if (enemy.HP <= 0)
                {
                    enemyIsAlive = false; // enemy defeated
                }

                else if (user.HP <= 0)
                {
                    gameIsOver = true; // user defeated
                }
            }
            else
            {
                // display
                Dialogue.display(user, enemy, round);

                // start battle and choose attack for ai
                Battle_Sequence(enemy, user, EnemyAttackChooser());

                if (enemy.HP <= 0)
                {
                    enemyIsAlive = false; // enemy defeated
                }

                else if (user.HP <= 0)
                {
                    gameIsOver = true; // user defeated
                }
            }
        }

    }

    //make better
    private string EnemyAttackChooser()
    {
        var random = new Random();
        return enemy.Attacks[random.Next(enemy.Attacks.Count)];
    }

    //fix later
    private void enemyDefeated()
    {
        return;
    }        //get 

    //fix this later
    private void gameOver()
    {
        return;
    }

    public static void Battle_Sequence(Character attacker, Character defender, string attackChoice)
    {
        int damage = CalculateDamage(attacker.Pokemon.Trim(), attackChoice.Trim());

        defender.HP -= damage;

        Dialogue.Battle_Dialogue(attacker, defender, attackChoice, damage);

        if (defender.HP < 0)
        {
            defender.HP = 0;
        }
    }

    public static int CalculateDamage(string weapon, string attackChoice)
    {
        var lines = File.ReadAllLines("weapons.csv");
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts[0].Trim().Equals(weapon, StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 1; i < parts.Length - 1; i += 2)
                {
                    if (parts[i].Trim().Equals(attackChoice, StringComparison.OrdinalIgnoreCase))
                    {
                        if (int.TryParse(parts[i + 1].Trim(), out int dmg))
                            return dmg;
                    }
                }
            }
        }
        return 0;
    }
}
