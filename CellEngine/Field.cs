using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OJE.GL;
using TestStrategicGame.Graphics;
using TestStrategicGame.Shaders;
using TestStrategicGame.Utils;

namespace TestStrategicGame
{
    public unsafe class Field
    {
        private readonly Cell[,] cells;
        private readonly Cell[,] modifiedCells;

        public int SizeX { get; }
        public int SizeY { get; }

        private static int uniformCameraPosition; //TODO temp static, only one shader is supported
        private static int uniformCameraScale;
        private static ShaderProgram mainProgram;

        private static int vao;
        private static int vbo;
        private static int textureId;

        [StructLayout(LayoutKind.Sequential)]
        struct CellPoint
        {
            public float x;
            public float y;

            public Color color;
            public int textureLayer;
        }

        private CellPoint[] field;

        public Field(int sizeY, int sizeX)
        {
            SizeY = sizeY;
            SizeX = sizeX;
            cells = new Cell[sizeY, sizeX];
            modifiedCells = new Cell[sizeY, sizeX];
            field = new CellPoint[sizeY * sizeX];
        }

        public Field(Cell[,] cells)
        {
            SizeY = cells.GetLength(0);
            SizeX = cells.GetLength(1);
            this.cells = new Cell[SizeY, SizeX];
            Array.Copy(cells, this.cells, SizeX * SizeY);
            modifiedCells = new Cell[SizeY, SizeX];
            field = new CellPoint[SizeY * SizeX];
        }

        public void SetCell(Cell cell, uint y, uint x)
        {
            modifiedCells[y, x] = cell;
            cell.x = x;
            cell.y = y;
        }

        public IEnumerable<Cell> Cells()
        {
            foreach (Cell cell in cells)
                yield return cell;
        }

        public IEnumerable<T> NearbyCells<T>(uint y, uint x, uint radius) where T : Cell
        {
            uint ySize = (uint)cells.GetLength(0);
            uint xSize = (uint)cells.GetLength(1);

            for (uint i = y < radius ? 0 : y - radius; i <= (y + radius > ySize - 1 ? ySize - 1 : y + radius); i++)
            {
                for (uint j = x < radius ? 0 : x - radius; j <= (x + radius > xSize - 1 ? xSize - 1 : x + radius); j++)
                {
                    Cell found = cells[i, j];
                    if ((i != y || j != x) && found.GetType() == typeof(T))
                        yield return (T)found;
                }
            }
        }

        public IEnumerable<Cell> NearbyCells(uint y, uint x, uint radius)
        {
            uint ySize = (uint)cells.GetLength(0);
            uint xSize = (uint)cells.GetLength(1);

            for (uint i = y < radius ? 0 : y - radius; i <= (y + radius > ySize - 1 ? ySize - 1 : y + radius); i++)
            {
                for (uint j = x < radius ? 0 : x - radius; j <= (x + radius > xSize - 1 ? xSize - 1 : x + radius); j++)
                {
                    Cell found = cells[i, j];
                    if (i != y || j != x)
                        yield return found;
                }
            }
        }

        public T NearbyCell<T>(uint y, uint x, uint radius) where T : Cell
        {
            uint ySize = (uint)cells.GetLength(0);
            uint xSize = (uint)cells.GetLength(1);

            for (uint i = y < radius ? 0 : y - radius; i <= (y + radius > ySize - 1 ? ySize - 1 : y + radius); i++)
            {
                for (uint j = x < radius ? 0 : x - radius; j <= (x + radius > xSize - 1 ? xSize - 1 : x + radius); j++)
                {
                    Cell found = cells[i, j];
                    if (i != y || j != x)
                        return (T)found;
                }
            }
            return null;
        }

        internal void Load()
        {
            LoadBuffer();
            Input.SubscribeMouseClick(CellClicked, 0);
        }

        internal void Unload()
        {
            Input.UnsubscribeMouseClick(CellClicked);
        }

        public void Tick()
        {
            foreach (Cell cell in cells)
                cell?.Tick();
            for (int i = 0; i < modifiedCells.GetLength(0); i++)
            {
                for (int j = 0; j < modifiedCells.GetLength(1); j++)
                {
                    Cell modifiedCell = modifiedCells[i, j];
                    if (modifiedCell != null)
                    {
                        Cell prevCell = cells[i, j];
                        prevCell?.Remove();
                        cells[i, j] = modifiedCell;
                        modifiedCells[i, j] = null;
                    }
                }
            }
            LoadBuffer();
        }

        private bool CellClicked(Vector2 pos, int mouseButton)
        {
            int j = (int)(pos.x * Camera.Scale / 2 + (Camera.x + 0.5) * 20); //TODO constants (magic numbers)
            int i = (int)(pos.y * Camera.Scale / 2 + (Camera.y + 0.5) * 20);

            if (j < 0 || j > SizeX || i < 0 || i > SizeY)
                return false;
            Cell clickedCell = cells[i, j];
            clickedCell.MouseClick(mouseButton);
            return true;
        }

        public static void LoadRender()
        {
            mainProgram = new ShaderProgram("Field\\vertexShader.txt", "Field\\fragmentShader.txt", "Field\\geometryShader.txt");

            uniformCameraPosition = mainProgram.GetUniformLocation("cameraPosition");
            uniformCameraScale = mainProgram.GetUniformLocation("cameraScale");

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();
            GL.BindVertexArray(vao);
            GL.BindBuffer(GL.ARRAY_BUFFER, vbo);
            textureId = GL.GenTexture();
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, textureId);
            GL.TexParameter(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_MIN_FILTER, GL.NEAREST_MIPMAP_NEAREST);
            GL.TexParameter(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_MAG_FILTER, (int)Texture.Type.Nearest);
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, 0);

            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, sizeof(CellPoint), IntPtr.Zero);
            GL.VertexAttribPointer(1, 4, GL.FLOAT, false, sizeof(CellPoint), (void*)sizeof(Vector2));
            GL.VertexAttribPointer(2, 1, GL.INT, false, sizeof(CellPoint), (void*)(sizeof(Vector2) + sizeof(Color)));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            GL.BindVertexArray(0);
        }

        internal void RenderLoop()
        {
            mainProgram.Use();
            GL.BindVertexArray(vao);
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, textureId);

            GL.Uniform2(uniformCameraPosition, Camera.x, Camera.y);
            GL.Uniform1(uniformCameraScale, Camera.Scale);
            //GL.Uniform1(uniformTime, timer.ElapsedMilliseconds);
            GL.DrawArrays(GL.POINTS, 0, field.Length);
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, 0);
            GL.BindVertexArray(0);
            ShaderProgram.Unbind();
        }

        private void LoadBuffer()
        {
            for (uint i = 0; i < SizeY; i++)
            {
                for (uint j = 0; j < SizeX; j++)
                {
                    Cell cell = cells[i, j];
                    cell.y = i;
                    cell.x = j;
                    field[i * SizeY + j] = new CellPoint { x = j, y = i, color = cell.Color, textureLayer = cell.TextureLayer };
                }
            }

            fixed (void* pointer = field)
            {
                GL.BindBuffer(GL.ARRAY_BUFFER, vbo);
                GL.BindTexture(GL.TEXTURE_2D_ARRAY, textureId);
                GL.TexImage3D(GL.TEXTURE_2D_ARRAY, 0, GL.RGBA8, 16, 16, CellTextureManager.Textures.Count, 0, GL.BGRA, GL.UNSIGNED_BYTE, IntPtr.Zero);
                for (int i = 0; i < CellTextureManager.Textures.Count; i++)
                {
                    CellTexture texture = CellTextureManager.Textures[i];
                    GL.TexSubImage3D(GL.TEXTURE_2D_ARRAY, 0, 0, 0, i, texture.Width, texture.Height, 1, GL.BGRA, GL.UNSIGNED_BYTE, texture.DataPointer);
                }
                GL.GenerateMipmap(GL.TEXTURE_2D_ARRAY);
                GL.BindTexture(GL.TEXTURE_2D_ARRAY, 0);
                GL.BufferData(GL.ARRAY_BUFFER, sizeof(CellPoint) * field.Length, pointer, GL.DYNAMIC_DRAW);
                GL.BindBuffer(GL.ARRAY_BUFFER, 0);
            }
        }
    }
}