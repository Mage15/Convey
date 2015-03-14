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
        public void CornerClip(ref Texture2D texture, Corner corner, float percent)
        {
            if (percent > 1f) percent = 1f;
            if (percent < 0f) percent = 0f;

            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            int clipWidth;
            int clipHeight;
            //int index;

            if (texture.Width > texture.Height)
            {
                clipWidth = (int)(texture.Height / 2f * percent);
            }
            else
            {
                clipWidth = (int)(texture.Width / 2f * percent);
            }

            clipHeight = clipWidth;
            Point position;

            switch (corner)
            {
                case Corner.TopLeft:
                    position = new Point(0, 0);

                    for (; position.Y < clipHeight; position.Y++)
                    {
                        for (; position.Y + position.X < clipWidth; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = 0;
                    }
                    break;
                case Corner.TopRight:
                    position = new Point(texture.Width - clipWidth - 1, 0);

                    for (; position.Y <= clipHeight; position.Y++)
                    {
                        for (; position.X < texture.Width; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = texture.Width - clipWidth + position.Y;
                    }
                    break;
                case Corner.BottomLeft:
                    position = new Point(0, texture.Height - clipHeight);

                    for (; position.Y < texture.Height; position.Y++)
                    {
                        for (; position.X < position.Y - (texture.Height - clipHeight); position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = 0;
                    }
                    break;
                case Corner.BottomRight:
                    position = new Point(texture.Width - (0 - (texture.Height - clipHeight)), texture.Height - clipHeight);

                    for (; position.Y < texture.Height; position.Y++)
                    {
                        for (; position.X < texture.Width; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = texture.Width - (position.Y - (texture.Height - clipHeight));
                    }
                    break;
            }

            texture.SetData(data);
        }

        public void CornerRadius(ref Texture2D texture, Corner corner, float percent)
        {
            if (percent > 1f) percent = 1f;
            if (percent < 0f) percent = 0f;

            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            int clipWidth;
            int clipHeight;

            if (texture.Width > texture.Height)
            {
                clipWidth = (int)(texture.Height / 2f * percent);
            }
            else
            {
                clipWidth = (int)(texture.Width / 2f * percent);
            }

            clipHeight = clipWidth;


            switch (corner)
            {
                case Corner.TopLeft:

                    break;
                case Corner.TopRight:

                    break;
                case Corner.BottomLeft:

                    break;
                case Corner.BottomRight:

                    break;
            }

            texture.SetData(data);
        }

        public void CornerInvert(ref Texture2D texture, Corner corner, float percent)
        {
            if (percent > 1f) percent = 1f;
            if (percent < 0f) percent = 0f;

            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            int clipWidth;
            int clipHeight;

            if (texture.Width > texture.Height)
            {
                clipWidth = (int)(texture.Height / 2f * percent);
            }
            else
            {
                clipWidth = (int)(texture.Width / 2f * percent);
            }

            clipHeight = clipWidth;
            Point position;

            switch (corner)
            {
                case Corner.TopLeft:
                    position = new Point(0, 0);

                    for (; position.Y < clipHeight; position.Y++)
                    {
                        for (; position.X < clipWidth; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = 0;
                    }
                    break;
                case Corner.TopRight:
                    position = new Point(texture.Width - clipWidth, 0);

                    for (; position.Y < clipHeight; position.Y++)
                    {
                        for (; position.X < texture.Width; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = texture.Width - clipWidth;
                    }
                    break;
                case Corner.BottomLeft:
                    position = new Point(0, texture.Height - clipHeight);

                    for (; position.Y < texture.Height; position.Y++)
                    {
                        for (; position.X < clipWidth; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset x
                        position.X = 0;
                    }
                    break;
                case Corner.BottomRight:
                    position = new Point(texture.Width - clipWidth, texture.Height - clipHeight);
                    for (; position.Y < texture.Height; position.Y++)
                    {
                        for (; position.X < texture.Width; position.X++)
                        {
                            data[position.Y * texture.Width + position.X] *= 0f;
                        }

                        // Reset X
                        position.X = texture.Width - clipWidth;
                    }
                    break;
            }

            texture.SetData(data);
        }
    }
}
