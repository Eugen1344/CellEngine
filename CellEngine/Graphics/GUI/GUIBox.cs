using System;
using System.Runtime.InteropServices;
using CellEngine.Shaders;
using CellEngine.Utils;
using OJE.GL;

namespace CellEngine.Graphics.GUI
{
    public unsafe class GUIBox : GUIBase
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public Vector2 pos;
            public Color color;
        }

        public readonly ShaderProgram Shader;

        private int vao;
        private int vbo;
        private Vertex[] vertexData = new Vertex[4];

        public GUIBox(Window window, Color color, Vector2 pos, float sizeX, float sizeY, int z = 1, ShaderProgram shader = null) : base(window, color, pos, sizeX, sizeY, z)
        {
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(GL.ARRAY_BUFFER, vbo);
            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, sizeof(Vertex), IntPtr.Zero);
            GL.VertexAttribPointer(1, 4, GL.FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector2));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);

            Shader = shader ?? new ShaderProgram("GUI\\vertexShader.txt", "GUI\\fragmentShader.txt"); //TODO maybe make shader database for duplicate shaders?
        }

        protected override void Draw()
        {
            Shader.Use();
            GL.BindVertexArray(vao);
            GL.DrawArrays(GL.TRIANGLE_FAN, 0, 4);
            GL.BindVertexArray(0);
            ShaderProgram.Unbind();
        }

        protected override void UpdateModel()
        {
            GL.BindBuffer(GL.ARRAY_BUFFER, vbo);
            vertexData[0] = new Vertex { pos = globalPos, color = Color };
            vertexData[1] = new Vertex { pos = new Vector2(globalPos.x, globalPos.y + SizeY), color = Color };
            vertexData[2] = new Vertex { pos = new Vector2(globalPos.x + SizeX, globalPos.y + SizeY), color = Color };
            vertexData[3] = new Vertex { pos = new Vector2(globalPos.x + SizeX, globalPos.y), color = Color };

            fixed (void* pointer = vertexData)
            {
                GL.BufferData(GL.ARRAY_BUFFER, sizeof(Vertex) * vertexData.Length, pointer, GL.DYNAMIC_DRAW);
            }
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
        }
    }
}
