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

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Convey.TextureWorkshop
{
    public partial class UITextureWorkshop
    {
        /************************************
         * This could use some optimization *
         ************************************/
        public void PaintShadow(ref Texture2D texture, Vector2 locationOffset, int blur, int spread, Color color)
        {
            int reach = blur + spread;
            Point newSize = new Point(0, 0);
            newSize.X = (int)(texture.Width + (reach * 2) + locationOffset.X);
            newSize.Y = (int)(texture.Height + (reach * 2) + locationOffset.Y);

            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);

            Texture2D shadowTexture = new Texture2D(graphics,  newSize.X,  newSize.Y);
            Color[] shadowData = new Color[shadowTexture.Width * shadowTexture.Height];
            for (int i = 0; i < shadowData.Length; i++) shadowData[i] = new Color(0, 0, 0) * 0f;

            Color[] alphaSpectrum = new Color[reach];
            float alphaStep = (float)1 / (blur + 1);

            for (int i = alphaSpectrum.Length; i > 0; i--)
            {
                alphaSpectrum[alphaSpectrum.Length - i] = (alphaStep * i > color.A / 255f) ? color * (color.A / 255f) : color * (alphaStep * i);
            }

            // Draw shadow on new texture
            Point texturePosition = new Point(0, 0);
            Point shadowPosition = new Point(0, 0);

            for (; texturePosition.X < texture.Width; texturePosition.X++)
            {
                for (; texturePosition.Y < texture.Height; texturePosition.Y++)
                {
                    if (textureData[texturePosition.Y * texture.Width + texturePosition.X].A != 0)
                    {
                        shadowPosition.X = texturePosition.X + (int)(reach + locationOffset.X);
                        shadowPosition.Y = texturePosition.Y + (int)(reach + locationOffset.Y);

                        shadowData[shadowPosition.Y * shadowTexture.Width + shadowPosition.X] = color;

                        /* Algorithm adapted from http://www.mathopenref.com/coordcirclealgorithm.html */
                        for (int radius = 1; radius <= alphaSpectrum.Length; radius++)
                        {
                            for (double theta = 0.0; theta < 2 * Math.PI; theta += 0.1)
                            {
                                int x = shadowPosition.X + (int)(radius * Math.Cos(theta));
                                int y = shadowPosition.Y - (int)(radius * Math.Sin(theta));    //note 2.

                                if (x > 0 && x < shadowTexture.Width && y > 0 && y < shadowTexture.Height)
                                {
                                    if (shadowData[(int)(Math.Round(y * (double)shadowTexture.Width + x))].A < alphaSpectrum[radius - 1].A)
                                        shadowData[(int)(Math.Round(y * (double)shadowTexture.Width + x))] = alphaSpectrum[radius - 1];
                                }
                            }
                        }
                    }
                }

                // Reset Y value
                texturePosition.Y = 0;
            }

            // Draw original texture over shadow
            texturePosition = new Point(0, 0);
            shadowPosition = new Point(0, 0);

            for (; texturePosition.X < texture.Width; texturePosition.X++)
            {
                for (; texturePosition.Y < texture.Height; texturePosition.Y++)
                {
                    if (textureData[texturePosition.Y * texture.Width + texturePosition.X].A != 0)
                    {
                        shadowPosition.X = texturePosition.X + reach;
                        shadowPosition.Y = texturePosition.Y + reach;

                        shadowData[shadowPosition.Y * shadowTexture.Width + shadowPosition.X] = textureData[texturePosition.Y * texture.Width + texturePosition.X];
                    }
                }

                // Reset Y value
                texturePosition.Y = 0;
            }

            shadowTexture.SetData(shadowData);
            texture = shadowTexture;
        }
    }
}
