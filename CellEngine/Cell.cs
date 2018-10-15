using CellEngine.Graphics;
using System.IO;

namespace CellEngine
{
    public abstract class Cell
    {
        public uint x { get; protected internal set; }
        public uint y { get; protected internal set; }

        public Color Color { get; protected set; }

        private CellTexture texture;
        public CellTexture Texture
        {
            get { return texture; }
            protected set
            {
                TextureLayer = CellTextureManager.GetTextureLayer(value);
                texture = value;
            }
        }

        public int TextureLayer { get; private set; }

        protected Cell(Color color, string texturePath)
        {
            Color = color;
            if (string.IsNullOrEmpty(texturePath))
                TextureLayer = -1;
            else
            {
                Texture = CellTexture.GetTexture(@"Resources\Textures\Cells\" + texturePath);
                TextureLayer = CellTextureManager.GetTextureLayer(Texture);
            }
        }

        /*private static Texture GetCellTexture(string texturePath)
        {
            if (string.IsNullOrWhiteSpace(texturePath)) return null;

            return Texture.GetTexture(texturePath) ?? new Texture(texturePath, Texture.Type.Nearest);
        }*/

        protected internal virtual void MouseDown(int button) { }
        protected internal virtual void MouseUp(int button) { }
        protected internal virtual void MouseClick(int button) { }
        protected internal virtual void MouseDrag(int button) { }
        protected internal virtual void Serialize(BinaryWriter stream) { }
        protected internal virtual void Deserialize(BinaryReader stream) { }
        protected internal abstract void Tick();
        protected internal abstract void Remove();
    }
}