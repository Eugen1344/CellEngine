using System;
using System.Runtime.InteropServices;
using OJE.GL;
using TestStrategicGame.Utils;

namespace TestStrategicGame.Graphics.GUI
{
    public unsafe class GUIText : GUIBase
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector2 pos;
            public Color color;
            public Vector2 texturePos;
            public Vector2 textureSize;
            public Vector2 sizeMultiplier; //TODO maybe move to shader?
            public float page;
        }

        public Font Font;

        private string text;
        public string Text
        {
            get => text;

            set
            {
                text = value;
                UpdateText();
            }
        }

        private int vao;
        private int vbo;
        private Vertex[] vertexData;

        public float CharSize => SizeY; //TODO char size

        public GUIText(Window window, Color color, Vector2 pos, float sizeX, float sizeY, int z, Font font, string text) : base(window, color, pos, sizeX, sizeY, z)
        {
            Font = font;
            this.text = text;

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(GL.ARRAY_BUFFER, vbo);
            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, sizeof(Vertex), IntPtr.Zero);
            GL.VertexAttribPointer(1, 4, GL.FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector2));
            GL.VertexAttribPointer(2, 2, GL.FLOAT, false, sizeof(Vertex), (void*)(sizeof(Vector2) + sizeof(Color)));
            GL.VertexAttribPointer(3, 2, GL.FLOAT, false, sizeof(Vertex), (void*)(sizeof(Vector2) * 2 + sizeof(Color)));
            GL.VertexAttribPointer(4, 2, GL.FLOAT, false, sizeof(Vertex), (void*)(sizeof(Vector2) * 3 + sizeof(Color)));
            GL.VertexAttribPointer(5, 1, GL.FLOAT, false, sizeof(Vertex), (void*)(sizeof(Vector2) * 4 + sizeof(Color)));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);
            GL.EnableVertexAttribArray(4);
            GL.EnableVertexAttribArray(5);
            GL.BindVertexArray(0);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);

            UpdateText();
        }

        private void UpdateText()
        {
            if (!string.IsNullOrEmpty(Text))
            {
                vertexData = new Vertex[Text.Length];
                UpdateModel();
            }
        }

        protected override void Draw()
        {
            if (string.IsNullOrEmpty(Text)) return;

            Font.Use(CharSize);
            GL.BindVertexArray(vao);
            GL.DrawArrays(GL.POINTS, 0, vertexData.Length);
            GL.BindVertexArray(0);
            Font.Unbind();
        }

        protected override void UpdateModel()
        {
            if (string.IsNullOrEmpty(Text)) return;

            GL.BindBuffer(GL.ARRAY_BUFFER, vbo);

            Vector2 position = globalPos;
            for (var i = 0; i < vertexData.Length; i++)
            {
                Font.Character character = Font.GetChar(Text[i]);

                Vertex vertex = new Vertex { pos = new Vector2(position.x + character.Offset.x * CharSize, position.y + CharSize * (1 - character.Offset.y - character.SizeMultiplier.y)), color = Color, texturePos = character.TexturePos, textureSize = character.TextureSize, sizeMultiplier = character.SizeMultiplier, page = character.Page };
                vertexData[i] = vertex;
                if (i < vertexData.Length - 1)
                {
                    position.x += character.Xadvance * CharSize;
                    if (character.Kerning.TryGetValue(Text[i + 1], out float kerning))
                        position.x += kerning * CharSize;
                }
            }

            fixed (void* pointer = vertexData)
            {
                GL.BufferData(GL.ARRAY_BUFFER, sizeof(Vertex) * vertexData.Length, pointer, GL.DYNAMIC_DRAW);
            }
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
        }
    }
}