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
        public void EdgeSlash(ref Texture2D texture, Edge edge, Slash slash)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            int slashWidth;
            int slashHeight;

            if (texture.Width < texture.Height)
            {
                slashWidth = texture.Width;
            }
            else
            {
                slashWidth = texture.Height;
            }

            slashHeight = slashWidth;
            Point position;

            if (edge == Edge.Left && slash == Slash.Back)
            {
                position = new Point(0, 0);

                for (; position.Y < slashHeight; position.Y++)
                {
                    for (; position.X < slashWidth; position.X++)
                    {
                        if (position.X <= position.Y)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }
                    }

                    // Reset X
                    position.X = 0;
                }
            }
            else if (edge == Edge.Left && slash == Slash.Forward)
            {
                position = new Point(0, 0);

                for (; position.Y < slashHeight; position.Y++)
                {
                    for (; position.X < slashWidth - position.Y; position.X++)
                    {
                        data[position.Y * texture.Width + position.X] = Color.Transparent;
                    }

                    // Reset X
                    position.X = 0;
                }
            }
            if (edge == Edge.Right && slash == Slash.Back)
            {
                position = new Point(texture.Width - slashWidth, 0);

                for (; position.Y < slashHeight; position.Y++)
                {
                    for (; position.X < texture.Width; position.X++)
                    {
                        data[position.Y * texture.Width + position.X] *= 0f;
                    }

                    // Reset X
                    position.X = texture.Width - slashWidth + position.Y;
                }
            }
            else if (edge == Edge.Right && slash == Slash.Forward)
            {
                position = new Point(texture.Width - 1, 0);

                for (; position.Y < slashHeight; position.Y++)
                {
                    for (; position.X < texture.Width; position.X++)
                    {
                        data[position.Y * texture.Width + position.X] *= 0f;
                    }

                    // Reset X
                    position.X = texture.Width - 1 - position.Y;
                }
            }

            texture.SetData(data);
        }

        public void EdgeClip(Edge edge)
        {

        }
    }
}
