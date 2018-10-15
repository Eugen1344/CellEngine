using System;
using System.IO;
using OJE.GLFW;
using TestGame.Cells;
using TestGame.Graphics.GUI;
using TestStrategicGame;
using TestStrategicGame.Graphics;
using TestStrategicGame.Graphics.GUI;

namespace TestGame
{
    public class GameScript : MainScript
    {
        public static int Food = 0;
        public override void Start()
        {
            Console.WriteLine("TestGame started");

            Color infestationColor = Color.RandomColor();
            Cell[,] testCells = new Cell[500, 500];
            for (uint i = 0; i < testCells.GetLength(0); i++)
                for (uint j = 0; j < testCells.GetLength(1); j++)
                    testCells[i, j] = new DesertCell();
            testCells[10, 10] = new CropCell();

            Engine.SwitchField(new Field(testCells));
        }

        public override void Tick()
        {
            Console.WriteLine("Turn");
        }

        public override void Update()
        {
            if (Input.GetKey(Glfw.KEY_ESCAPE) == Input.KeyState.Clicked)
                MainMenu.Enabled = !MainMenu.Enabled;
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