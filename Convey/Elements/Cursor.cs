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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Convey
{
    public class UICursor : UIElement
    {
        public UICursor(string sharedTextureName)
            : base (sharedTextureName)
        { }

        public UICursor(Texture2D elementTexture)
            : base (elementTexture)
        { }

        public override void Update(KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, MouseState previousMouseState, MouseState currentMouseState)
        {
            destinationRectangle.X = currentMouseState.Position.X;
            destinationRectangle.Y = currentMouseState.Position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsHidden)
            {
                spriteBatch.Draw(ElementTexture, destinationRectangle, ElementColor);
            }
        }
    }
}
