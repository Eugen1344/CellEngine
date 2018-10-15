using TestStrategicGame;
using TestStrategicGame.Graphics;
using TestStrategicGame.Graphics.GUI;
using TestStrategicGame.Utils;

namespace TestGame.Graphics.GUI
{
    public static class InfoPanel
    {
        private static GUIBox background = new GUIBox(Engine.MainWindow, new Color(0.7f, 0, 0.2f), new Vector2(1, -1), 1f, 0.4f, 1) { Pivot = Pivot.BottomRight };
        private static GUIText typeText = new GUIText(Engine.MainWindow, Color.Black, new Vector2(0.05f, -0.05f), 0.1f, 0.1f, 2, new Font(@"Resources\Fonts\Tnr\tnr.fnt"), null) { Pivot = Pivot.TopLeft, ParentPivot = Pivot.TopLeft, Parent = background };

        public static bool Enabled { get; private set; }

        public static void SetEnabled(Cell cell, bool enabled)
        {
            typeText.Text = cell.GetType().Name;
            background.Enabled = enabled;
            typeText.Enabled = enabled;
            Enabled = enabled;
        }
    }
}
