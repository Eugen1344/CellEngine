using CellEngine;

namespace StrategicGame
{
    static class Program
    {
        static void Main(string[] args)
        {
            Engine.Start(new GameScript(), "Civilization VII");
        }
    }
}