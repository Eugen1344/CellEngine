using System.Linq;
using CellEngine;
using CellEngine.Graphics;

namespace GameOfLife.Cells
{
    public class DeadCell : Cell
    {
        public DeadCell() : base(Color.Black, null)
        {

        }

        protected override void Tick()
        {
            if (Engine.CurrentField.NearbyCells(y, x, 1).Count(cell => cell.GetType() == typeof(AliveCell)) == 3)
                Engine.CurrentField.SetCell(new AliveCell(), y, x);
        }

        protected override void MouseClick(int button)
        {
            Engine.CurrentField.SetCell(new AliveCell(), y, x);
            Engine.CurrentField.UpdateCells();
        }

        protected override void Remove()
        {

        }
    }
}