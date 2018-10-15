using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OJE.GL;
using TestStrategicGame.Shaders;
using TestStrategicGame.Utils;

namespace TestStrategicGame.Graphics
{
    public class Font
    {
        public struct Character
        {
            public int Id;
            public Vector2 TexturePos;
            public Vector2 TextureSize;
            public Vector2 SizeMultiplier;
            public Vector2 Offset;
            public float Xadvance;
            public byte Page;
            public Dictionary<int, float> Kerning;
        }

        private readonly Dictionary<int, Character> chars;
        private readonly int textures;

        public readonly string Name;
        public readonly int Size;
        public readonly int Width;
        public readonly int Height;

        private readonly int charSizeUniform;

        public readonly ShaderProgram Shader; //TODO maybe too many shaders?

        public Font(string path) //TODO KERNING!!!
        {
            string rootPath = Path.GetDirectoryName(path);
            XDocument document = XDocument.Load(path);
            XElement root = document.Root;
            Name = root.Element("info").Attribute("face").Value;
            Size = int.Parse(root.Element("info").Attribute("size").Value);
            Width = int.Parse(root.Element("common").Attribute("scaleW").Value);
            Height = int.Parse(root.Element("common").Attribute("scaleH").Value);
            int pages = int.Parse(root.Element("common").Attribute("pages").Value);

            textures = GL.GenTexture();
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, textures);

            GL.TexParameter(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_WRAP_S, GL.CLAMP_TO_EDGE);
            GL.TexParameter(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_WRAP_T, GL.CLAMP_TO_EDGE);

            GL.TexParameter(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_MIN_FILTER, GL.LINEAR);
            GL.TexParameter(GL.TEXTURE_2D_ARRAY, GL.TEXTURE_MAG_FILTER, GL.LINEAR);

            GL.TexImage3D(GL.TEXTURE_2D_ARRAY, 0, GL.RGBA8, Width, Height, pages, 0, GL.BGRA, GL.UNSIGNED_BYTE, IntPtr.Zero);

            int i = 0;
            foreach (string texturePath in root.Element("pages").Elements("page").Attributes("file").Select(file => file.Value))
            {
                Bitmap bmp = new Bitmap(Path.Combine(rootPath, texturePath));
                BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                GL.TexSubImage3D(GL.TEXTURE_2D_ARRAY, 0, 0, 0, i, Width, Height, 1, GL.BGRA, GL.UNSIGNED_BYTE, bmpData.Scan0);
                GL.GenerateMipmap(GL.TEXTURE_2D_ARRAY);
                bmp.UnlockBits(bmpData);
                i++;
            }
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, 0);

            Vector2 textureSize = new Vector2(Width, Height);

            chars = root.Element("chars").Elements("char").Select(element => new Character
            {
                Id = int.Parse(element.Attribute("id").Value),
                TexturePos = new Vector2(int.Parse(element.Attribute("x").Value) / textureSize.x, int.Parse(element.Attribute("y").Value) / textureSize.y),
                TextureSize = new Vector2(int.Parse(element.Attribute("width").Value) / textureSize.x, int.Parse(element.Attribute("height").Value) / textureSize.y),
                SizeMultiplier = new Vector2(float.Parse(element.Attribute("width").Value) / Size, float.Parse(element.Attribute("height").Value) / Size),
                Offset = new Vector2(float.Parse(element.Attribute("xoffset").Value) / Size, float.Parse(element.Attribute("yoffset").Value) / Size),
                Xadvance = float.Parse(element.Attribute("xadvance").Value) / Size,
                Page = byte.Parse(element.Attribute("page").Value),
                Kerning = new Dictionary<int, float>()
            }).ToDictionary(c => c.Id);

            foreach (XElement element in root.Element("kernings").Elements("kerning")) //TODO practice linq
            {
                chars[int.Parse(element.Attribute("first").Value)].Kerning.Add(int.Parse(element.Attribute("second").Value), float.Parse(element.Attribute("amount").Value) / Size);
            }

            Shader = new ShaderProgram("GUI/fontVertexShader.txt", "GUI/fontFragmentShader.txt", "GUI/fontGeometryShader.txt");
            charSizeUniform = Shader.GetUniformLocation("CharSize");
        }

        public Character GetChar(char c)
        {
            return chars[c];
        }

        public void Use(float charSize)
        {
            Shader.Use();
            GL.Uniform1(charSizeUniform, charSize);
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, textures);
        }

        public static void Unbind()
        {
            ShaderProgram.Unbind();
            GL.BindTexture(GL.TEXTURE_2D_ARRAY, 0);
        }
    }
}