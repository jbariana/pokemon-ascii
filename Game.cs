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
        string weapon = "Sword";
        Character user = new Character("Combo", 45, 60, weapon);

        //intro
        Dialogue.introScreen();

        //game loop
        Character enemy = new Character("DJ EVIL", 33, 33, weapon);
        Battle battle = new Battle(user, enemy);
        battle.StartBattle();
    }

}
