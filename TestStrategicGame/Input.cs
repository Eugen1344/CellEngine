using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OJE.GLFW;
using TestStrategicGame.Graphics.GUI;
using TestStrategicGame.Utils;

namespace TestStrategicGame
{
    //TODO COMPLETELY REWRITE!!!
    public static class Input //TODO multiple windows
    {
        public static bool MouseLeftButtonClick;
        public static bool MouseMiddleButtonClick;
        public static bool MouseRightButtonClick;

        private static readonly KeyState[] Keys = new KeyState[350];
        private static readonly KeyState[] MouseButtons = new KeyState[6];
        private static bool mouseLeftButton;
        private static bool mouseRightButton;
        private static bool mouseMiddleButton;

        //List for click callbacks. Sorted by depth, descending
        private static readonly SortedList<int, Func<Vector2, int, bool>> mouseClick = new SortedList<int, Func<Vector2, int, bool>>(Comparer<int>.Create((el1, el2) => el2 - el1 == 0 ? 1 : el2 - el1)); //TODO maybe sorted dictionary?

        public enum KeyState
        {
            Released,
            Pressed,
            Clicked
        }

        public enum KeyCodes //TODO: write space and other shit
        {

        }

        internal static void Update() //TODO rewrite
        {
            bool[] nowButtons = GetMouseButtons();// [0]-left [1]-right [2]-middle [3]-4th mouse button [4]-5th mouse button.....

            if (mouseLeftButton)
            {
                MouseLeftButtonClick = false;
            }
            else if (nowButtons[0])
            {
                MouseLeftButtonClick = true;
            }

            if (mouseRightButton)
            {
                MouseRightButtonClick = false;
            }
            else if (nowButtons[1])
            {
                MouseRightButtonClick = true;
            }

            if (mouseMiddleButton)
            {
                MouseMiddleButtonClick = false;
            }
            else if (nowButtons[2])
            {
                MouseMiddleButtonClick = true;
            }

            mouseLeftButton = nowButtons[0];
            mouseRightButton = nowButtons[1];
            mouseMiddleButton = nowButtons[2];

            for (short i = 0; i < MouseButtons.Length; ++i)
            {
                int mouse = Glfw.GetMouseButton(Engine.MainWindow.GlfwWindow, i);
                if (mouse == Glfw.PRESS)
                    MouseButtons[i] = KeyState.Pressed;
                else if (mouse == Glfw.RELEASE)
                {
                    if (MouseButtons[i] == KeyState.Pressed)
                    {
                        foreach (KeyValuePair<int, Func<Vector2, int, bool>> pair in mouseClick)
                        {
                            if (pair.Value != null && pair.Value.Invoke(GetMousePosition(), i))
                                break;
                        }
                        MouseButtons[i] = KeyState.Clicked;
                    }
                    else if (MouseButtons[i] == KeyState.Clicked)
                        MouseButtons[i] = KeyState.Released;
                }
            }

            for (short i = 0; i < Keys.Length; ++i)
            {
                int key = Glfw.GetKey(Engine.MainWindow.GlfwWindow, i);
                if (key == Glfw.PRESS)
                    Keys[i] = KeyState.Pressed;
                else if (key == Glfw.RELEASE)
                {
                    if (Keys[i] == KeyState.Pressed)
                        Keys[i] = KeyState.Clicked;
                    else if (Keys[i] == KeyState.Clicked)
                        Keys[i] = KeyState.Released;
                }
            }

            UpdateActions();
        }

        public static void UpdateActions()
        {
            Camera.UpdateInput();
            Engine.MainScript.Update();
        }

        public static Vector2 GetMousePosition()
        {
            //Glfw.GetCursorPos(Program.MainWindow.GlfwWindow, out double x, out double y);
            double x, y;
            Glfw.GetCursorPos(Engine.MainWindow.GlfwWindow, out x, out y);
            return new Vector2(2 * (float)x / Engine.MainWindow.Width - 1, 2 * (Engine.MainWindow.Height - (float)y) / Engine.MainWindow.Height - 1);
        }

        public static void SubscribeMouseClick(Func<Vector2, int, bool> method, int priority)
        {
            mouseClick.Add(priority, method);
        }

        public static void UnsubscribeMouseClick(Func<Vector2, int, bool> method)
        {
            mouseClick.RemoveAt(mouseClick.IndexOfValue(method));
        }

        public static KeyState GetKey(char button)
        {
            return Keys[button.ToString().ToUpper()[0]];
        }

        public static KeyState GetKey(int key)
        {
            return Keys[key];
        }

        public static bool[] GetMouseButtons()
        {
            bool[] result = new bool[5];
            for (int i = 0; i < 5; ++i)
                result[i] = Glfw.GetMouseButton(Engine.MainWindow.GlfwWindow, i) == Glfw.PRESS;
            return result;
        }
    }
}
