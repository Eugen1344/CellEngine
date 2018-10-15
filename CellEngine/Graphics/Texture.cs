using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OJE.GL;

namespace CellEngine.Graphics
{
    public class Texture // : IEquatable<Texture>
    {
        public string Path { get; }
        public Type TextureType { get; }
        public int TextureId { get; }
        public int Width { get; }
        public int Height { get; }

        private static List<Texture> textures = new List<Texture>();

        public enum Type
        {
            Nearest = GL.NEAREST,
            Linear = GL.LINEAR
        }

        private Texture(string path, Type textureType)
        {
            Bitmap bitmap = new Bitmap(path);
            BitmapData bitData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            Path = path;
            TextureType = textureType;
            TextureId = GL.GenTexture();
            Width = bitmap.Width;
            Height = bitmap.Height;

            GL.BindTexture(GL.TEXTURE_2D, TextureId);

            GL.TexParameter(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, (int)TextureType);
            GL.TexParameter(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, (int)TextureType);

            GL.TexImage2D(GL.TEXTURE_2D, 0, GL.RGBA8, Width, Height, 0, GL.BGRA, GL.UNSIGNED_BYTE, bitData.Scan0);

            GL.BindTexture(GL.TEXTURE_2D, 0);

            bitmap.UnlockBits(bitData);

            textures.Add(this);
        }

        ~Texture()
        {
            //UnloadTexture();
        }

        public static Texture GetTexture(string path, Type textureType)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            return textures.Find(texture => texture.Path == path && texture.TextureType == textureType) ?? new Texture(path, textureType);
        }

        public void UnloadTexture()
        {
            GL.DeleteTextures(1, new[] { TextureId });
        }

        /*public bool Equals(Texture other)
        {
            return TextureType == other.TextureType && TextureId == other.TextureId;
        }

        public override bool Equals(object obj)
        {
            if (obj?.GetType() != GetType()) return false;
            return Equals((Texture)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)TextureType * 397) ^ TextureId;
            }
        }*/
    }
}