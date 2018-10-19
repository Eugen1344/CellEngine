using CellEngine;
using CellEngine.Graphics;
using System;

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

        protected override void MouseClick(int button)
        {
            Console.WriteLine($"CLICK {x} {y}");
        }

        protected override void MouseDown(int button)
        {
            Texture = CellTexture.GetTexture("Resources\\clicked.png");
            Console.WriteLine($"DOWN {x} {y}");
        }

        protected override void MouseUp(int button)
        {
            Texture = null;
            Console.WriteLine($"UP {x} {y}");
        }
    }
}