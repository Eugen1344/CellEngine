using System.Collections.Generic;
using CellEngine.Graphics.GUI;
using OJE.GL;
using OJE.GLFW;

namespace CellEngine
{
    public class Window
    {
        public Glfw.Window GlfwWindow;
        public int Width;
        public int Height;

        private readonly List<GUIBase> GUI = new List<GUIBase>();
        private Glfw.SetWindowSizeCallbackDelegate rescaleDelegate;

        public Window(int width, int height, string name)
        {
            Width = width;
            Height = height;

            rescaleDelegate = Rescale;
            GlfwWindow = Glfw.CreateWindow(Width, Height, name);
            Glfw.Vidmode vidmode = Glfw.GetVideoMode(Glfw.GetPrimaryMonitor());
            Glfw.SetWindowPos(GlfwWindow, (vidmode.Width - Width) / 2, (vidmode.Height - Height) / 2);
            Glfw.SetWindowSizeCallback(GlfwWindow, rescaleDelegate);
        }

        public void RenderGUI()
        {
            foreach (GUIBase element in GUI)
                element.updated = false;
            foreach (GUIBase element in GUI)
            {
                if (element != null && element.Enabled && element.Window == this)
                    element.Render();
            }
        }

        private void Rescale(Glfw.Window window, int width, int height)
        {
            Width = width;
            Height = height;
            GL.Viewport(0, 0, width, height);
        }

        public void AddGUIElement(GUIBase element)
        {
            GUI.Add(element);
            SortGUI();
        }

        public void SortGUI()
        {
            GUI.Sort((el1, el2) => el1.z - el2.z); //Sort gui elements by depth for rendering
        }
    }
}