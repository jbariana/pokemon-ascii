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
        bool energyCanBeAttached = true;
        bool gameIsOver = false;
        bool enemyIsAlive = true;
        int round = 0;

        while (!gameIsOver && enemyIsAlive)
        {
            energyCanBeAttached = true; // reset for each round
            round++;
            Dialogue.display(user, enemy, round);

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
            if (round % 2 == 1)
            {
                bool turnInProgress = true;
                while (turnInProgress)
                {
                    string userChoice = Dialogue.BattleMenu(user, enemy, round);

                    if (userChoice == "attach energy")
                    {
                        if (energyCanBeAttached)
                        {
                            energyCanBeAttached = false;
                            user.Pokemon.energyAttach();
                            Dialogue.AttachEnergy(user);
                            Dialogue.display(user, enemy, round);
                            continue;
                        }
                        else
                        {

                            Dialogue.CannotAttachEnergy();
                            Dialogue.display(user, enemy, round);
                            continue;
                        }
                    }

                    if (userChoice == "end turn")
                    {
                        Dialogue.EndTurn(user);
                        turnInProgress = false;
                        continue;
                    }

                    // If the user chose an attack, do the battle sequence and end their turn
                    Battle_Sequence(user, enemy, userChoice, round);
                    Dialogue.display(user, enemy, round);
                    turnInProgress = false;
                }

                if (enemy.Pokemon.HP <= 0)
                {
                    enemyIsAlive = false; // enemy defeated
                }
                else if (user.Pokemon.HP <= 0)
                {
                    gameIsOver = true; // user defeated
                }
            }
            else
            {
                // display
                Dialogue.display(user, enemy, round);

                // start battle and choose attack for ai
                Battle_Sequence(enemy, user, EnemyAttackChooser(), round);

                if (enemy.Pokemon.HP <= 0)
                {
                    enemyIsAlive = false; // enemy defeated
                }

                else if (user.Pokemon.HP <= 0)
                {
                    gameIsOver = true; // user defeated
                }
            }
        }

    }

    //make better
    private string EnemyAttackChooser()
    {
        var attacks = enemy.Pokemon.Attacks;
        int count = attacks.Count;
        int chosenIndex = 0;

        if (count > 2)
        {
            chosenIndex = (new Random().Next(2)) * 2; // 0 or 2
        }

        return attacks[chosenIndex];
    }

    //fix later
    private void enemyDefeated()
    {
        return;
    }

    //fix this later
    private void gameOver()
    {
        return;
    }

    public void Battle_Sequence(Character attacker, Character defender, string attackChoice, int round)
    {
        int damage = CalculateDamage(attacker.Pokemon, attackChoice.Trim());

        Dialogue.Battle_Dialogue(attacker, defender, attackChoice, damage, round);

        defender.Pokemon.HP -= damage;

        if (defender.Pokemon.HP < 0)
        {
            defender.Pokemon.HP = 0;
        }
    }

    public int CalculateDamage(Pokemon pokemon, string attackChoice)
    {
        var attacks = pokemon.Attacks;
        for (int i = 0; i < attacks.Count; i += 3)
        {
            string attackName = attacks[i].Trim();
            if (!string.IsNullOrEmpty(attackName) &&
                attackName.Equals(attackChoice, StringComparison.OrdinalIgnoreCase))
            {
                // Damage is the next item in the list
                if (i + 1 < attacks.Count && int.TryParse(attacks[i + 1].Trim(), out int dmg))
                    return dmg;
            }
        }
        return 0;
    }
}