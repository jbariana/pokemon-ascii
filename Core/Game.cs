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
        Dialogue.IntroScreen();

        string? chosenPokemon = Dialogue.ChoosePokemonScreen();
        Character user = new Character("Combo", chosenPokemon, true);

        Character enemy = new Character("DJ EVIL", "Darkrai", false);

        var battle = new Battle(user, enemy);
        battle.StartBattle();
    }
}
