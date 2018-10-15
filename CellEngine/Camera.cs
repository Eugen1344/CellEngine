using OJE.GLFW;

namespace CellEngine
{
    public static class Camera
    {
        public static bool Enabled = true;
        public static float x;
        public static float y;
        public static float Scale = 20f;
        public static int ScaleFactor;

        public const float MaximumScale = 2f;
        public const float MinimalScale = 500f;
        public const float MoveSpeed = 0.06f;
        public const float ZoomSpeed = 2.5f;

        public static void UpdateInput()
        {
            if (!Enabled)
                return;

            float moveFactor = MoveSpeed * Scale;

            if (Input.GetKey(Glfw.KEY_W) == Input.KeyState.Pressed)
                y += (float)Time.DeltaTime * moveFactor;
            if (Input.GetKey(Glfw.KEY_A) == Input.KeyState.Pressed)
                x -= (float)Time.DeltaTime * moveFactor;
            if (Input.GetKey(Glfw.KEY_S) == Input.KeyState.Pressed)
                y -= (float)Time.DeltaTime * moveFactor;
            if (Input.GetKey(Glfw.KEY_D) == Input.KeyState.Pressed)
                x += (float)Time.DeltaTime * moveFactor;

            float zoomFactor = (float)Time.DeltaTime * ZoomSpeed * Scale;
            if (Input.GetKey(Glfw.KEY_EQUAL) == Input.KeyState.Pressed)
            {
                float newScale = Scale - zoomFactor;
                Scale = newScale < MaximumScale ? MaximumScale : newScale;
            }
            if (Input.GetKey(Glfw.KEY_MINUS) == Input.KeyState.Pressed)
            {
                float newScale = Scale + zoomFactor;
                Scale = newScale > MinimalScale ? MinimalScale : newScale;
            }

        }
    }
}