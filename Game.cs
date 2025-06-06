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
        string pokemon = "Giratina";
        //name, name of mon, attacking first?
        Character user = new Character("Combo", pokemon, true);

        //intro
        Dialogue.introScreen();

        //game loop
        string pokemon2 = "Darkrai";
        Character enemy = new Character("DJ EVIL", pokemon2, false);
        Battle battle = new Battle(user, enemy);
        battle.StartBattle();
    }

}
