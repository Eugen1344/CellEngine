using System;
using System.IO;
using System.Linq;
using TestStrategicGame;
using TestStrategicGame.Graphics;
using TestStrategicGame.Utils;

namespace TestGame.Cells
{
    public class InfestationCell : ClickableCell
    {
        private static readonly Random rand = new Random();

        public InfestationCell(Color color)
            : base(color, "infestation.png")
        {

        }

        //TODO remove default empty constructors, think something clever
        public InfestationCell() //For serialization
            : this(Color.Red)
        { }

        protected override void Tick()
        {
            //int same = World.CurrentField.NearbyCells(y, x, 1).Count(cell => cell.Color.Equals(Color) && cell.GetType() == typeof(InfestationCell)) + 1;
            //for (int i = 0; i < same; i++)
            {
                Cell replace =
                    Engine.CurrentField.NearbyCells(y, x, 1)
                        .Where(cell => !cell.Color.Equals(Color) || cell.GetType() != typeof(InfestationCell))
                        .RandomElement(rand);
                if (replace != null)
                {
                    Engine.CurrentField.SetCell(new InfestationCell(Color), replace.y, replace.x);
                }
            }
        }

        protected override void Remove()
        {
            //foreach (InfestationCell cell in World.CurrentField.NearbyCells<InfestationCell>(x, y, 1))
            //   cell.blocked = false;
        }

        protected override void Serialize(BinaryWriter stream)
        {
            stream.Write(Color.r);
            stream.Write(Color.g);
            stream.Write(Color.b);
            stream.Write(Color.a);
        }

        protected override void Deserialize(BinaryReader stream)
        {
            Color = new Color(stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle(), stream.ReadSingle());
        }
    }
}