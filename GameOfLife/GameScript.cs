using System;
using System.Diagnostics;
using System.IO;
using GameOfLife.Cells;
using OJE.GLFW;
using CellEngine;
using CellEngine.Graphics;

namespace GameOfLife
{
    public class GameScript : MainScript
    {
        private Stopwatch timer = new Stopwatch();

        public override void Start()
        {
            Console.WriteLine("Game of life started");

            Color infestationColor = Color.RandomColor();
            Cell[,] testCells = new Cell[1000, 1000];

            for (uint i = 0; i < testCells.GetLength(0); i++)
                for (uint j = 0; j < testCells.GetLength(1); j++)
                    testCells[i, j] = new DeadCell();

            Random rand = new Random();

            for (int i = 0; i < 50000; i++)
            {
                testCells[rand.Next(0, 1000), rand.Next(0, 1000)] = new AliveCell();
            }
            

            Engine.SwitchField(new Field(testCells));

            timer.Start();
        }

        public override void Tick()
        {

        }

        public override void Update()
        {
            if (timer.ElapsedMilliseconds >= 100)
            {
                timer.Restart();
                Engine.Tick();
            }

            if (Input.GetKey(Glfw.KEY_SPACE) == Input.KeyState.Clicked)
                Engine.Tick();
            if (Input.GetKey(Glfw.KEY_F5) == Input.KeyState.Clicked)
            {
                FileStream file = new FileStream("quick.save", FileMode.Create, FileAccess.Write);
                SaveData.Write(file);
                file.Close();
            }
            if (Input.GetKey(Glfw.KEY_F9) == Input.KeyState.Clicked)
            {
                try
                {
                    FileStream file = new FileStream("quick.save", FileMode.Open, FileAccess.Read);
                    SaveData save = new SaveData();
                    save.Read(file);
                    file.Close();
                    save.Load();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Unable to find quick save file");
                }
            }
        }
    }
}
