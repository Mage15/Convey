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
        public void PaintLinearGradient(ref Texture2D texture, Color colorFrom, Color colorTo)
        {
            PaintLinearGradient(ref texture, Edge.Bottom, colorFrom, colorTo);
        }

        public void PaintLinearGradient(ref Texture2D texture, Edge edgeTo, Color colorFrom, Color colorTo)
        {
            // Instead of inverting the algorithm just invert the colors
            if (edgeTo == Edge.Top || edgeTo == Edge.Left)
            {
                Color temp = colorFrom;
                colorFrom = colorTo;
                colorTo = temp;
            }

            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            // Calculate color deltas
            int deltaRed = colorFrom.R - colorTo.R;
            int deltaGreen = colorFrom.G - colorTo.G;
            int deltaBlue = colorFrom.B - colorTo.B;
            int deltaAlpha = colorFrom.A - colorTo.A;

            int red, green, blue, alpha;
            float step;
            Point position = new Point(0, 0);

            switch (edgeTo)
            {
                case Edge.Top:
                case Edge.Bottom:
                    for (; position.Y < texture.Height; position.Y++)
                    {
                        step = (float)position.Y / texture.Height;

                        for (; position.X < texture.Width; position.X++)
                        {
                            red = (int)(colorFrom.R - deltaRed * step);
                            green = (int)(colorFrom.G - deltaGreen * step);
                            blue = (int)(colorFrom.B - deltaBlue * step);
                            alpha = (int)(colorFrom.A - deltaAlpha * step);

                            // Maintain the shap of the drawn object
                            if (data[position.Y * texture.Width + position.X].A == 0) continue;

                            if (red > 255) red = 255;
                            if (green > 255) green = 255;
                            if (blue > 255) blue = 255;
                            if (alpha > 255) alpha = 255;

                            data[position.Y * texture.Width + position.X] = new Color(red, green, blue) * ((float)alpha / 255);
                        }

                        // Reset X
                        position.X = 0;
                    }
                    break;

                case Edge.Left:
                case Edge.Right:
                    for (; position.X < texture.Width; position.X++)
                    {
                        step = (float)position.X / texture.Width;

                        for (; position.Y < texture.Height; position.Y++)
                        {
                            red = (int)(colorFrom.R - deltaRed * step);
                            green = (int)(colorFrom.G - deltaGreen * step);
                            blue = (int)(colorFrom.B - deltaBlue * step);
                            alpha = (int)(colorFrom.A - deltaAlpha * step);

                            // Maintain the shap of the drawn object
                            if (data[position.Y * texture.Width + position.X].A == 0) continue;

                            if (red > 255) red = 255;
                            if (green > 255) green = 255;
                            if (blue > 255) blue = 255;
                            if (alpha > 255) alpha = 255;

                            data[position.Y * texture.Width + position.X] = new Color(red, green, blue) * ((float)alpha / 255);
                        }

                        // Reset Y
                        position.Y = 0;
                    }
                    break;
            }

            texture.SetData(data);
        }


        //public void PaintLinearGradient(ref Texture2D texture, Color top, Color bottom, float percentStart, float percentEnd)
        //{
        //    if (percent > 1f) percent = 1f;
        //    if (percent < 0f) percent = 0f;                    
        //}
        
        //public void PaintLinearGradient(ref Texture2D texture, Edge edgeTo, Color colorFrom, Color[] colorsTo)
        //{

        //}

        public void PaintLinearGradient(ref Texture2D texture, Corner cornerTo, Color colorFrom, Color colorTo)
        {

        }

        //public void PaintLinearGradient(ref Texture2D texture, Corner cornerTo, Color colorFrom, Color[] colorsTo)
        //{

        //}

        public void PaintRadialGradient(ref Texture2D texture, Color colorFrom, Color colorTo)
        {

        }
    }
}
