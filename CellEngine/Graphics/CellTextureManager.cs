using System.Collections.Generic;

namespace TestStrategicGame.Graphics
{
    public static class CellTextureManager
    {
        public static List<CellTexture> Textures = new List<CellTexture>(); //TODO think about texture deletion

        public static int GetTextureLayer(CellTexture texture)
        {
            if (texture == null)
                return -1; //Hack if texture == null

            int index = Textures.IndexOf(texture);
            int textureIndex;
            if (index == -1)
            {
                Textures.Add(texture);
                textureIndex = Textures.Count - 1;
            }
            else
                textureIndex = index;
            return textureIndex;
        }
    }
}
