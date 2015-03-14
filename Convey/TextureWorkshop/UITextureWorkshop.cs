/*
 Copyright (C) 2015  Matthew Gefaller

 Convey is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 Convey is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Convey.TextureWorkshop
{
    public enum Slash
    {
        Forward,
        Back,
        Center
    }

    public enum BorderStyle
    {
        Solid
    }

    public partial class UITextureWorkshop
    {
        private GraphicsDevice graphics;
        private ContentManager content;

        public UITextureWorkshop(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            this.graphics = graphicsDevice;
            this.content = contentManager;
        }

        /// <summary>
        /// Creates a new Texture2D with a heigth and width of 48 and the color white
        /// </summary>
        public Texture2D NewTexture()
        { 
            return NewTexture(new Rectangle(0, 0, 48, 48), Color.White);
        }

        /// <summary>
        /// Creates a new Texture2D with the specified width and height and the color white
        /// </summary>
        public Texture2D NewTexture(Rectangle attributes)
        {
            return NewTexture(attributes, Color.White);
        }

        /// <summary>
        /// Creates a new Texture2D with the specified width and height and the specified color
        /// </summary>
        public Texture2D NewTexture(Rectangle attributes, Color color)
        {
            Texture2D objectTexture = new Texture2D(graphics, attributes.Width, attributes.Height);
            Color[] objectData = new Color[objectTexture.Width * objectTexture.Height];
            for (int i = 0; i < objectData.Length; i++) objectData[i] = color;
            objectTexture.SetData(objectData);

            return objectTexture;
        }

        /// <summary>
        /// Creates a deep copy of Texture2D provided
        /// </summary>
        //public Texture2D NewTexture(Texture2D texture)
        //{
        //    /*************************
        //     * Doesn't seem to work. *
        //     *************************/
        //    Color[] textureData = new Color[texture.Width * texture.Height];

        //    Texture2D objectTexture = new Texture2D(graphics, texture.Width, texture.Height);
        //    Color[] objectData = new Color[objectTexture.Width * objectTexture.Height];

        //    for (int i = 0; i < objectData.Length; i++)
        //    {
        //        if (textureData[i].A == 0) continue;

        //        objectData[i] = new Color(textureData[i].R, textureData[i].G, textureData[i].B) * (textureData[i].A / 255f);
        //    }
            
        //    objectTexture.SetData(objectData);

        //    return objectTexture;
        //}

        /// <summary>
        /// Paints the Texture2D a specified color maintaining all alpha values
        /// </summary>
        public void PaintColor(ref Texture2D texture, Color color)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int index = 0; index < data.Length; index++)
            {
                if (data[index].A == 0) continue;
                data[index] = new Color((int)color.R, (int)color.G, (int)color.B) * (color.A / 255f);
            }

            texture.SetData(data);
        }
    }
}
