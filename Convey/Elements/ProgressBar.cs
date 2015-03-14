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

namespace Convey
{
    public class UIProgressBar : UIElement
    {
        protected int fontSize;
        protected string _lable;
        protected int _value;

        public SpriteFont Font { get; set; }

        public Texture2D BarTexture {get; set; }
        public Color BarColor { get; set; }

        public string Label
        {
            get { return _lable; }
            set { _lable = value; ShowLabel = true; }
        }        
        public Color ColorLabel { get; set; }
        public bool ShowLabel { get; set; }
        public Edge FillFrom { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Value 
        {
            get { return _value; }
            set
            {
                if (value >= Max)
                {
                    _value = Max;
                    if (OnChange != null) OnChange(this, EventArgs.Empty);
                    if (OnComplete != null) OnComplete(this, EventArgs.Empty);
                }
                else if (value <= Min)
                {
                    _value = Min;
                    if (OnChange != null) OnChange(this, EventArgs.Empty);

                }
                else
                {
                    _value = value;
                    if (OnChange != null) OnChange(this, EventArgs.Empty);
                }
            }
        }

        public EventHandler OnChange;
        public EventHandler OnComplete;
        public EventHandler OnCreate;
        
        public UIProgressBar(Texture2D elementTexture, Texture2D barTexture)
            : base (elementTexture)
        {
            BarTexture = barTexture;

            Max = 100;
            Min = 0;
            Value = 0;

            BarColor = Color.White;
            FillFrom = Edge.Left;
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destination, Nullable<Rectangle> sourceRectangle, float rotation, Vector2 origin, SpriteEffects flip, float depth)
        {
            // Background Element
            base.Draw(spriteBatch, destination, sourceRectangle, rotation, origin, flip, depth);

            // Bar
            if(Max != 0)
            {

            }

            // Label
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Background Element
            base.Draw(spriteBatch);

            // Bar
            if (Max != 0)
            {
                Rectangle barSource;
                float percentFilled = (float)Value / Max;
                int drawnHeight, drawnWidth;

                switch(FillFrom)
                {
                    case Edge.Bottom:
                        drawnHeight = (int)Math.Round(BarTexture.Height * percentFilled);
                        barSource = new Rectangle(0, BarTexture.Height - drawnHeight, BarTexture.Width, drawnHeight);
                        break;
                    case Edge.Right:
                        drawnWidth = (int)Math.Round(BarTexture.Width * percentFilled);
                        barSource = new Rectangle(BarTexture.Width - drawnWidth, 0, drawnWidth, BarTexture.Height);
                        break;
                    case Edge.Top:
                        barSource = new Rectangle(0, 0, BarTexture.Width, (int)Math.Round(BarTexture.Height * percentFilled));
                        break;
                    default: // Default is FillFrom = Edge.Left
                        barSource = new Rectangle(0, 0, (int)Math.Round(BarTexture.Width * percentFilled), BarTexture.Height);
                        break;
                }

                spriteBatch.Draw(BarTexture, destinationRectangle, barSource, BarColor, Rotation, Origin, flip, Depth);
            }

            // Label
        }
    }
}
