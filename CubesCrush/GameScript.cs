using CellEngine;
using CellEngine.Graphics;

namespace CubesCrush
{
    public class GameScript : MainScript
    {
        public override void Start()
        {
            //Camera.Enabled = false;

            Cell[,] cubeCells = new Cell[30, 10];

            for (uint i = 0; i < cubeCells.GetLength(0); i++)
                for (uint j = 0; j < cubeCells.GetLength(1); j++)
                    cubeCells[i, j] = new CubeCell(new Color((i + j) % 2, 1, 0));

            Field mainField = new Field(cubeCells);

            Engine.SwitchField(mainField);
        }

        public override void Tick()
        {

        }

        public override void Update()
        {

        }
    }
}