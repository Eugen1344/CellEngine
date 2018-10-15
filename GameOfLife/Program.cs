using CellEngine;

namespace GameOfLife
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Engine.Start(new GameScript(), "Game of life");
        }
    }
}