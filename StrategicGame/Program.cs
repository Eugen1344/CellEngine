using TestGame.Cells;
using TestStrategicGame;
using TestStrategicGame.Graphics;

namespace TestGame
{
    static class Program
    {
        static void Main(string[] args)
        {
            Engine.Start(new GameScript(), "Civilization VII");
        }
    }
}