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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Convey
{
    public class UIDocument
    {
        private KeyboardState currentKeyboardSate, previousKeyboardState;
        private MouseState currentMouseState, previousMouseState;
        private UIElement focusedElement;
        private UICursor cursor;

        public Dictionary<string, UIPage> ChildPages { get; private set; }
        public Dictionary<string, Texture2D> SharedTextures { get; set; }
        public Dictionary<string, SpriteFont> SharedFonts { get; set; }
        public UICursor Cursor 
        {
            get { return cursor; }
            set
            {
                cursor = value;

                // If using callback for texture
                if (!String.IsNullOrEmpty(cursor.SharedTextureId))
                {
                    cursor.ElementTexture = this.SharedTextures[cursor.SharedTextureId];
                }
            } 
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color BackgroundColor { get; set; }
        
        public UIDocument(int screenWidth, int screenHeight)
        {
            this.ChildPages = new Dictionary<string, UIPage>();
            this.SharedTextures = new Dictionary<string, Texture2D>();
            this.SharedFonts = new Dictionary<string, SpriteFont>();

            focusedElement = new UIElement("");
        }
        
        public void AddPage(string id, UIPage page)
        {
            page.ParentDocument = this;
            this.ChildPages.Add(id, page);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(UIPage page in ChildPages.Values)
            {
                page.Draw(spriteBatch);
            }

            Cursor.Draw(spriteBatch);
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState)
        {
            currentKeyboardSate = keyboardState; 
            currentMouseState = mouseState;

            cursor.Update(previousKeyboardState, currentKeyboardSate, previousMouseState, currentMouseState);
                        
            //Parallel.ForEach(ChildPages, item => item.Value.Update(previousKeyboardState, currentKeyboardSate, previousMouseState, currentMouseState));
            foreach (UIPage page in ChildPages.Values) page.Update(previousKeyboardState, currentKeyboardSate, previousMouseState, currentMouseState);

            previousKeyboardState = currentKeyboardSate;
            previousMouseState = currentMouseState;
        }

        public void Element_ChangeFocus(object sender, EventArgs e)
        {
            focusedElement.HasFocus = false;
            (sender as UIElement).HasFocus = true;
            focusedElement = sender as UIElement;
        }
    }
}
