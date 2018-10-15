using CellEngine;
using CellEngine.Graphics;
using CellEngine.Graphics.GUI;
using CellEngine.Utils;
using OJE.GLFW;

namespace StrategicGame.Graphics.GUI
{
    public static class MainMenu
    {
        private static GUIBox background;
        private static GUIBox continueButton;
        private static GUIText continueButtonText;
        private static GUIBox exitButton;
        private static GUIText exitButtonText;

        private static bool enabled;
        public static bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value; //WARNING test
                background.Enabled = value;
                continueButton.Enabled = value;
                continueButtonText.Enabled = value;
                exitButton.Enabled = value;
                exitButtonText.Enabled = value;
            }
        }

        static MainMenu()
        {
            background = new GUIBox(Engine.MainWindow, new Color(0.03f, 0.78f, 0.77f), Vector2.zero, 0.7f, 1f, 1) { Enabled = false, Pivot = Pivot.Center };
            continueButton = new GUIBox(Engine.MainWindow, new Color(0, 0.38f, 0.38f), new Vector2(0, 0.1f), 0.6f, 0.15f, 2) { Enabled = false, Pivot = Pivot.Center, Parent = background };
            continueButton.Clicked += i => Enabled = false;
            continueButtonText = new GUIText(Engine.MainWindow, Color.Red, Vector2.zero, 0.55f, 0.1f, 3, new Font(@"Resources\Fonts\Impact\impact.fnt"), "Continue") { Enabled = false, Clickable = false, Pivot = Pivot.Center, Parent = continueButton };
            exitButton = new GUIBox(Engine.MainWindow, new Color(0.5f, 0.38f, 0.38f), new Vector2(0, -0.1f), 0.6f, 0.15f, 2) { Enabled = false, Pivot = Pivot.Center, Parent = background };
            exitButton.Clicked += i => Glfw.SetWindowShouldClose(Engine.MainWindow.GlfwWindow, 1);
            exitButtonText = new GUIText(Engine.MainWindow, Color.Black, Vector2.zero, 0.55f, 0.1f, 3, new Font(@"Resources\Fonts\Tnr\tnr.fnt"), "Exit") { Enabled = false, Clickable = false, Pivot = Pivot.Center, Parent = exitButton };
        }
    }
}