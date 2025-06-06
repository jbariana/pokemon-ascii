using turn_based_game.Core;

namespace turn_based_game;

internal class Program
{
    static void Main()
    {
        Game game = new();
        game.GameLoop();
    }
}
