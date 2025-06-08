using System.ComponentModel.DataAnnotations;
using turn_based_game.Models;
using turn_based_game.UI;

namespace turn_based_game.Core;

public class Battle
{
    private readonly Character _user;
    private readonly Character _enemy;
    private bool _userTurn; // adding this so its created in the constructor 

    public Battle(Character user, Character enemy)
    {
        // the this keyword isnt needed since the names are different
        _user = user;
        _enemy = enemy;
        _userTurn = user.AttackingFirst;
    }

    public void StartBattle()
    {
        int round = 0;

        while (_user.Pokemon.HP > 0 && _enemy.Pokemon.HP > 0) // relying on hp values rather than booleans
        {
            round++;
            Dialogue.Display(_user, _enemy, round);

            // cleaned up this while loop a lil
            if (_userTurn)
            {
                PlayerTurn(round);
            }
            else
            {
                Dialogue.Display(_user, _enemy, round);
                EnemyTurn(round);
            }

            _userTurn = !_userTurn;
        }

        EndBattle(); // wrap it up!
    }

    private void PlayerTurn(int round)
    {
        bool energyCanBeAttached = true;
        bool turnComplete = false;

        while (!turnComplete)
        {
            string userChoice = Dialogue.BattleMenu(_user, _enemy, round); // idk if i told you but i fw the dialog class

            if (userChoice == "attach energy")
            {
                if (energyCanBeAttached)
                {
                    energyCanBeAttached = false;
                    _user.Pokemon.AttachEnergy(); // do we wanna be attaching energy for them if they forget?
                    Dialogue.AttachEnergy(_user);
                }
                else
                {
                    Dialogue.CannotAttachEnergy();
                }

                Dialogue.Display(_user, _enemy, round);
                continue;
            }

            // added this so that if the user presses 3 it doesnt end their turn
            if (userChoice == "inventory not currently implemented")
            {
                Console.WriteLine("Inventory is coming soon!! be patient, please.. (press any key to continue)");
                Console.ReadKey();
                Dialogue.Display(_user, _enemy, round);
                continue;
            }

            if (userChoice == "end turn")
            {
                turnComplete = true;
                Dialogue.EndTurn(_user);
                return;
            }

            // updated this to actually look for an Attack object by name with some linq magic
            var selectedAttack = _user.Pokemon.Attacks
                .FirstOrDefault(a => a.Name.Equals(userChoice.Trim(), StringComparison.OrdinalIgnoreCase));

            if (selectedAttack != null)
            {
                if (_user.Pokemon.EnergyAttached < selectedAttack.Cost)
                {
                    Dialogue.Display(_user, _enemy, round);
                    Dialogue.NotEnoughEnergy(_user, selectedAttack.Cost);
                    continue;
                }
                BattleSequence(_user, _enemy, selectedAttack, round);
            }
            else
            {
                // fallback in case something weird gets selected
                Console.WriteLine("Invalid attack choice. Please try again.");
                Thread.Sleep(1500);
                Dialogue.Display(_user, _enemy, round);
            }

            turnComplete = true;
        }
    }

    private void EnemyTurn(int round)
    {
        Dialogue.Display(_user, _enemy, round);

        Attack? chosenAttack = EnemyAttackChooser();
        if (chosenAttack != null)
        {
            Console.WriteLine(chosenAttack.Name); // Only print if not null
            BattleSequence(_enemy, _user, chosenAttack, round);
        }
        else
        {
            // if the enemy couldn't attack, it just attaches energy, checks if it can attack w/ the new energy, ends its turn
            _enemy.Pokemon.AttachEnergy();
            Dialogue.AttachEnergy(_enemy);

            // Try again after attaching energy
            chosenAttack = EnemyAttackChooser();
            if (chosenAttack != null)
            {
                BattleSequence(_enemy, _user, chosenAttack, round);
            }
            else
            {
                Dialogue.EndTurn(_enemy);
            }
        }
    }

    private Attack? EnemyAttackChooser()
    {
        var attacks = _enemy.Pokemon.Attacks;

        if (attacks.Count == 0)
        {
            throw new Exception($"{_enemy.Name}'s pokemon has no attacks loaded.");
        }

        // finds all useable attacks based on energy cost
        var usableAttacks = attacks
            .Where(a => a.Cost <= _enemy.Pokemon.EnergyAttached)
            .ToList();

        // finds a random attack that can be used
        if (usableAttacks.Count > 0)
        {
            // Pick a random usable attack
            var rng = new Random();
            return usableAttacks[rng.Next(usableAttacks.Count)];
        }
        //If no attacks can be used, this will return null
        else
        {
            return null;
        }
    }

    private void BattleSequence(Character attacker, Character defender, Attack attack, int round)
    {
        Dialogue.BattleDialogue(attacker, defender, attack.Name, attack.Damage, round);
        defender.Pokemon.TakeDamage(attack.Damage);

    }

    // this could basically take the place of game over and enemy defeated for now atleast
    private void EndBattle()
    {
        if (_user.Pokemon.HP <= 0)
        {
            // wanna add a Dialogue.GameOver(_user) here?
            return;
        }

        if (_enemy.Pokemon.HP <= 0)
        {
            // and a Dialogue.Victory(_user) here?
        }
    }
}
