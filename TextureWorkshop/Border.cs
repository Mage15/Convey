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
using Microsoft.Xna.Framework.Graphics;

namespace Convey.TextureWorkshop
{
    public partial class UITextureWorkshop
    {
        /// <summary>
        /// Takes a texture to draw a border around as well as the desired border width, style, and color.
        /// This method calls NewBorderTexture(Texture2D texture, int borderWidth, BorderStyle style, Color color, int inset)
        /// with an inset of 0.
        /// </summary>
        public void PaintBorder(ref Texture2D texture, int borderWidth, BorderStyle style, Color color)
        {
            PaintBorder(ref texture, borderWidth, style, color, 0);
        }

        /// <summary>
        /// Takes a texture to draw a border around as well as the desired border width, style, color, and inset.
        /// </summary>
        public void PaintBorder(ref Texture2D texture, int borderWidth, BorderStyle style, Color color, int inset)
        {
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);

            //Texture2D borderTexture = new Texture2D(graphics, texture.Width, texture.Height);
            //Color[] borderData = new Color[borderTexture.Width * borderTexture.Height];
            //for (int i = 0; i < borderData.Length; i++) borderData[i] = new Color(0, 0, 0) * 0f;

            Point position = new Point(inset, inset);
            int furthestPixel = inset + borderWidth;

            for (; position.X < texture.Width; position.X++)
            {
                for (; position.Y < texture.Height; position.Y++)
                {
                    //Maintain shape
                    if (textureData[position.Y * texture.Width + position.X].A == 0) continue;

                    // If too close to the boundry move on
                    if (
                            position.X - inset < 0 ||
                            position.Y - inset < 0 ||
                            position.X + inset > texture.Width - 1 ||
                            position.Y + inset > texture.Height - 1
                       )
                        continue;

                    // If no legit edges in range move on
                    if (
                            (
                                position.X - furthestPixel < 0 &&
                                position.Y - furthestPixel < 0 &&
                                position.X + furthestPixel > texture.Width - 1 &&
                                position.Y + furthestPixel > texture.Height - 1
                            ) &&
                            (
                                textureData[(position.Y - furthestPixel) * texture.Width + (position.X - furthestPixel)].A != 0 &&
                                textureData[(position.Y + furthestPixel) * texture.Width + (position.X - furthestPixel)].A != 0 &&
                                textureData[(position.Y - furthestPixel) * texture.Width + (position.X + furthestPixel)].A != 0 &&
                                textureData[(position.Y + furthestPixel) * texture.Width + (position.X + furthestPixel)].A != 0
                            )

                       )
                        continue;

                    for (int lookAround = 0; lookAround <= furthestPixel; lookAround++)
                    {
                        if (position.X - lookAround < 0 ||
                             position.Y - lookAround < 0 ||
                             position.X + lookAround > texture.Width - 1 ||
                             position.Y + lookAround > texture.Height - 1
                            ) break;

                        if (lookAround >= inset)
                        {
                            // Set color
                            if (position.X + lookAround == texture.Width - 1)
                            {
                                textureData[position.Y * texture.Width + position.X] = color;
                            }
                            else if (position.Y + lookAround == texture.Height - 1)
                            {
                                textureData[position.Y * texture.Width + position.X] = color;
                            }
                            else if (position.X - lookAround == 0 || position.Y - lookAround == 0)
                            {
                                textureData[position.Y * texture.Width + position.X] = color;
                            }
                            else if (
                                textureData[(position.Y - furthestPixel) * texture.Width + (position.X - furthestPixel)].A == 0 ||
                                textureData[(position.Y + furthestPixel) * texture.Width + (position.X - furthestPixel)].A == 0 ||
                                textureData[(position.Y - furthestPixel) * texture.Width + (position.X + furthestPixel)].A == 0 ||
                                textureData[(position.Y + furthestPixel) * texture.Width + (position.X + furthestPixel)].A == 0
                            )
                            {
                                textureData[position.Y * texture.Width + position.X] = color;
                            }

                        }
                        else // Make sure we are not too close to an edge or boundry
                        {
                            if (textureData[(position.Y - furthestPixel) * texture.Width + (position.X - furthestPixel)].A == 0 ||
                                textureData[(position.Y + furthestPixel) * texture.Width + (position.X - furthestPixel)].A == 0 ||
                                textureData[(position.Y - furthestPixel) * texture.Width + (position.X + furthestPixel)].A == 0 ||
                                textureData[(position.Y + furthestPixel) * texture.Width + (position.X + furthestPixel)].A == 0
                                )
                            {
                                // Too close to an edge
                                break;
                            }
                        }
                    }
                }

                // Reset Y value
                position.Y = inset;
            }

            texture.SetData(textureData);
        }
    }
}
