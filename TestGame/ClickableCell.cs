using TestGame.Graphics.GUI;
using TestStrategicGame;
using TestStrategicGame.Graphics;
using TestStrategicGame.Graphics.GUI;

namespace TestGame
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