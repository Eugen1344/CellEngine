using System;
using System.Diagnostics;
using System.IO;
using GameOfLife.Cells;
using OJE.GLFW;
using CellEngine;
using CellEngine.Utils;

namespace GameOfLife
{
    public class GameScript : MainScript
    {
        public bool Paused = false;
        public const int sizeX = 160;
        public const int sizeY = 160;
        private Stopwatch timer = new Stopwatch();

        public override void Start()
        {
            Console.WriteLine("Game of life started");

            Cell[,] testCells = new Cell[sizeY, sizeX];

            for (uint i = 0; i < testCells.GetLength(0); i++)
                for (uint j = 0; j < testCells.GetLength(1); j++)
                    testCells[i, j] = new DeadCell();

            Random rand = new Random();

            for (int i = 0; i < 5000; i++)
            {
                testCells[rand.Next(0, sizeY), rand.Next(0, sizeX)] = new AliveCell();
            }


            Engine.SwitchField(new Field(testCells));

            timer.Start();
        }

        public override void Tick()
        {

        }

        public override void Update()
        {
            if (!Paused)
                Engine.Tick();
            /*if (timer.ElapsedMilliseconds >= 100)
            {
                timer.Restart();
                Engine.Tick();
            }*/

            if (Input.GetKey(Glfw.KEY_BACKSPACE) == Input.KeyState.Clicked)
            {
                for (uint i = 0; i < Engine.CurrentField.SizeY; i++)
                {
                    for (uint j = 0; j < Engine.CurrentField.SizeX; j++)
                    {
                        Engine.CurrentField.SetCell(new DeadCell(), i, j);
                    }
                }
            }

            if (Input.GetKey(Glfw.KEY_PERIOD) == Input.KeyState.Clicked)
                Engine.Tick();

            if (Input.GetKey(Glfw.KEY_SPACE) == Input.KeyState.Clicked)
                Paused = !Paused;
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
