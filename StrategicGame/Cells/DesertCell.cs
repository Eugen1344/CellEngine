using CellEngine;
using CellEngine.Graphics;

namespace StrategicGame.Cells
{
    public class DesertCell : ClickableCell
    {
        public DesertCell() : base(Color.White, "desert.png")
        {
            new Color(0xC2 / 255f, 0xB2 / 255f, 0x80 / 255f);
        }

        protected override void Tick()
        {

        }

        protected override void Remove()
        {

        }

        protected override void MouseClick(int button)
        {
            base.MouseClick(button);
            Engine.CurrentField.SetCell(new InfestationCell(new Color(button == 0 ? 1 : 0, button == 1 ? 1 : 0, button == 2 ? 1 : 0)), y, x);
        }
    }
}