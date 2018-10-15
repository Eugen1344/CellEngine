using CellEngine;

namespace CubesCrush
{
    public class GameScript : MainScript
    {
        public override void Start()
        {
            Camera.Enabled = false;

            Cell[,] cubeCells = new Cell[30, 10];
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