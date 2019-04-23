using System.Linq;
using CellEngine;
using CellEngine.Graphics;

namespace GameOfLife.Cells
{
    public class AliveCell : Cell
    {
        public AliveCell() : base(Color.White, null)
        {

        }

        protected override void Tick()
        {
            int count = Engine.CurrentField.NearbyCells(y, x, 1).Count(cell => cell.GetType() == typeof(AliveCell));

            if (count != 2 && count != 3)
                Engine.CurrentField.SetCell(new DeadCell(), y, x);
        }

        protected override void MouseClick(int button)
        {
            Engine.CurrentField.SetCell(new DeadCell(), y, x);
            Engine.CurrentField.UpdateCells();
        }

        protected override void Remove()
        {

        }
    }
}