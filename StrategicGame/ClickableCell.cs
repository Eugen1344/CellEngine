using CellEngine;
using CellEngine.Graphics;
using StrategicGame.Graphics.GUI;

namespace StrategicGame
{
    public abstract class ClickableCell : Cell
    {
        protected ClickableCell(Color color, string texturePath) : base(color, texturePath) { }

        protected override void MouseClick(int button)
        {
            if (button == 0)
            {
                InfoPanel.SetEnabled(this, true);
            }
        }
    }
}