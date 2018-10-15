using System.Diagnostics;
using OJE.GL;
using OJE.GLFW;

namespace CellEngine
{
    public static class Engine
    {
        public static Window MainWindow; //TODO multiple window support
        public static Field CurrentField { get; private set; }
        internal static MainScript MainScript;

        public static void Start(MainScript mainScript, string name)
        {
            MainScript = mainScript;

            //Init OpenGL
            Glfw.Init();
            MainWindow = new Window(800, 800, name);

            Glfw.WindowHint(Glfw.CONTEXT_VERSION_MAJOR, 3);
            Glfw.WindowHint(Glfw.CONTEXT_VERSION_MINOR, 3);
            Glfw.MakeContextCurrent(MainWindow.GlfwWindow);
            GL.Viewport(0, 0, MainWindow.Width, MainWindow.Height);
            GL.Enable(GL.BLEND);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            Field.LoadRender();

            //MainMenu.Enabled = true;
            GL.ClearColor(1, 0, 1, 1);

            Stopwatch clock = new Stopwatch();
            clock.Start();

            MainScript.Start();

            while (Glfw.WindowShouldClose(MainWindow.GlfwWindow) == 0)
            {
                Glfw.PollEvents();

                Input.Update();

                GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

                CurrentField?.RenderLoop();
                MainWindow.RenderGUI();

                Glfw.SwapBuffers(MainWindow.GlfwWindow);
                Time.DeltaTime = clock.Elapsed.TotalSeconds;
                clock.Restart();
            }

            Glfw.WaitEvents();
            Glfw.Terminate();
        }

        public static void Tick()
        {
            MainScript.Tick();
            CurrentField.Tick();
        }

        public static void SwitchField(Field field)
        {
            CurrentField?.Unload();
            CurrentField = field;
            field.Load();
        }
    }
}