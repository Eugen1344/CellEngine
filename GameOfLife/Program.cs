using CellEngine;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Start(new GameScript(), "Game of life");
        }
    }
}