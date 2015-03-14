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
using Microsoft.Xna.Framework.Input;

namespace Convey
{
    public class UIButton : UIElement
    {

        protected string lable;
        protected float depressionScale;

        public bool DepressionEffect { get; set; }
        public string Label
        {
            get { return lable; }
            set { lable = value; ShowLabel = true; }
        }
        public SpriteFont Font { get; set; }
        public string SharedFontId { get; set; }
        public bool ShowLabel { get; set; }
        public Color ColorLabel { get; set; }

        public UIButton(string sharedTextureName)
            : base(sharedTextureName)
        {
            DepressionEffect = true;
            depressionScale = 1f;
        }

        public UIButton(Texture2D texture)
            : base(texture)
        {
            DepressionEffect = true;
            depressionScale = 1f;
        }
        
        public override void Update(KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, MouseState previousMouseState, MouseState currentMouseState)
        {
            base.Update(previousKeyboardState, currentKeyboardState, previousMouseState, currentMouseState);

            if (DepressionEffect)
            {
                depressionScale = (currentMouseOver && currentMouseState.LeftButton == ButtonState.Pressed) ? .95f : 1f;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Button
            Vector2 drawnPosition = new Vector2(
                        GetPosition().X + (float)Math.Ceiling(((1 - depressionScale) * Width) / 2.0),
                        GetPosition().Y + (float)Math.Ceiling(((1 - depressionScale) * Height) / 2.0)
                        );
            spriteBatch.Draw(ElementTexture, drawnPosition, null, ElementColor, Rotation, Origin, depressionScale, flip, Depth);

            // Lable
            if (ShowLabel && Label != null)
            {
                Vector2 textSize = Font.MeasureString(Label);
                Vector2 textLocation = new Vector2(
                    (float)Math.Ceiling(drawnPosition.X + ((ElementTexture.Width - textSize.X) / 2.0)),
                    (float)Math.Ceiling(drawnPosition.Y + ((ElementTexture.Height - textSize.Y) / 2.0))
                    );
                spriteBatch.DrawString(Font, Label, textLocation, ColorLabel, Rotation, Origin, 1f, flip, Depth);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Rectangle destination, Nullable<Rectangle> sourceRectangle, float rotation, Vector2 origin, SpriteEffects flip, float layer)
        {
            // Button
            spriteBatch.Draw(ElementTexture, destination, sourceRectangle, ElementColor, rotation, origin, flip, layer);
            
            // Lable
            if (ShowLabel && Label != null)
            {
                Vector2 textSize = Font.MeasureString(Label);
                Vector2 textLocation = new Vector2(
                    (float)Math.Ceiling(destination.X + ((ElementTexture.Width - textSize.X) / 2.0)),
                    (float)Math.Ceiling(destination.Y + ((ElementTexture.Height - textSize.Y) / 2.0))
                    );
                spriteBatch.DrawString(Font, Label, textLocation, ColorLabel, Rotation, Origin, 1f, flip, Depth);
            }
        }
    }
}
