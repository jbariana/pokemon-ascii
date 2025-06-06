/* my refactor ideas:
 * 1. abstracting the creation of characters as they are seperate functions
 * 2. using private methods since they are only meant to be accessed within this class, avoids possible overflow
 * 3. note: these COULD be static, since they dont access any instance data, but if we expand their functionality later we would likely remove it, 
 * so ill leave them as instance classes
 * 
 * TODO: idea: currently we rely on boolean in battle for our attacksFirst logic. We could make it cleaner by having an enum for turn order thats more explicit
 */

using turn_based_game.Models;
using turn_based_game.UI;

namespace turn_based_game.Core;

public class Game
{
    public void GameLoop()
    {
        Character user = CreatePlayerCharacter();
        Character enemy = CreateEnemyCharacter();

        Dialogue.IntroScreen();

        var battle = new Battle(user, enemy);
        battle.StartBattle(); // this is actually the game loop we lowkey should rename this method
    }

    private Character CreatePlayerCharacter()
    {
        // TODO: ill let you deal with how you wanna get player names
        string playerName = "Combo";
        string pokemonName = "Giratina";
        bool attacksFirst = true;

        return new Character(playerName, pokemonName, attacksFirst);
    }

    private Character CreateEnemyCharacter()
    {
        string enemyName = "DJ EVIL";
        string pokemonName = "Darkrai";
        bool attacksFirst = false;

        return new Character(enemyName, pokemonName, attacksFirst);
    }
}
