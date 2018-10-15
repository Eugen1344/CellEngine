using CellEngine;

namespace CubesCrush
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Start(new GameScript(), "Cubes crush");
        }
    }
}