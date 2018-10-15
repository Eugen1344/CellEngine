using CellEngine;
using CellEngine.Graphics;

namespace CubesCrush
{
    public class CubeCell : Cell
    {
        public CubeCell(Color color, string texturePath = null) : base(color, texturePath)
        {
        }

        protected override void Tick()
        {

        }

        protected override void Remove()
        {

        }

        protected override void MouseDrag(int button)
        {
            base.MouseDrag(button);
        }
    }
}