using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace turn_based_game;

public class Game
{
    public void Game_Loop()
    {
        //character creator
        string pokemon = "Darkrai";
        Character user = new Character("Combo", 45, 60, pokemon);

        //intro
        Dialogue.introScreen();

        //game loop
        string pokemon2 = "Giratina";
        Character enemy = new Character("DJ EVIL", 33, 33, pokemon2);
        Battle battle = new Battle(user, enemy);
        battle.StartBattle();
    }

}
