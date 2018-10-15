using System;
using System.Drawing;
using System.Drawing.Imaging;
using OJE.GL;

namespace CellEngine.Graphics
{
    public class CellTexture
    {
        public string Path { get; }
        public int TextureId { get; }
        public int Width { get; }
        public int Height { get; }
        public IntPtr DataPointer { get; }

        public static CellTexture GetTexture(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            return CellTextureManager.Textures.Find(texture => texture.Path == path) ?? new CellTexture(path);
        }

        private CellTexture(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            BitmapData bitData = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            Path = path;
            DataPointer = bitData.Scan0;
            TextureId = GL.GenTexture();
            Width = bitmap.Width;
            Height = bitmap.Height;

            GL.BindTexture(GL.TEXTURE_2D, TextureId);

            GL.TexParameter(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
            GL.TexParameter(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);

            GL.TexImage2D(GL.TEXTURE_2D, 0, GL.RGBA8, Width, Height, 0, GL.BGRA, GL.UNSIGNED_BYTE, DataPointer);

            GL.BindTexture(GL.TEXTURE_2D, 0);
        }
    }
}
