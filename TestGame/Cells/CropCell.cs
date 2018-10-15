using System;
using System.IO;
using TestStrategicGame;
using TestStrategicGame.Graphics;

namespace TestGame.Cells
{
    public class CropCell : ClickableCell
    {
        private const int GrowthLimit = 3;

        public int GrowthStage;

        public CropCell() : base(Color.White, "crop.png")
        {

        }

        protected override void Tick()
        {
            if (GrowthStage == GrowthLimit)
                GameScript.Food++;
            else
                GrowthStage++;
            Color = new Color((float)GrowthStage / GrowthLimit, 1, 1);
        }

        protected override void Remove()
        {
            Console.WriteLine("Crop is ruined");
        }

        protected override void Serialize(BinaryWriter stream)
        {
            stream.Write(GrowthStage);
        }

        protected override void Deserialize(BinaryReader stream)
        {
            GrowthStage = stream.ReadInt32();
            Color = new Color((float)GrowthStage / GrowthLimit, 1, 1);
        }
    }
}