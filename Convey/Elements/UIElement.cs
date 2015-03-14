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
    public partial class UIElement
    {
        //internal Rectangle BoundingBox; // Used for expansion into relative positioning
        protected bool previousMouseOver;
        protected bool currentMouseOver;
        protected SpriteEffects flip;
        protected bool hasFocus;
        protected Rectangle destinationRectangle;
        protected Texture2D elementTexture;

        public UIPage ParentPage { get; set; }
        public Texture2D ElementTexture
        {
            get { return elementTexture; }
            set
            {
                elementTexture = value;
                this.Height = value.Height;
                this.Width = value.Width;
            }
        }
        public Color ElementColor { get; set; }
        public string SharedTextureId { get; set; }

        /// <summary>
        /// Scales element to height
        /// </summary>
        public int Height 
        {
            get { return destinationRectangle.Height; }
            set { destinationRectangle.Height = value; }
        }
        /// <summary>
        /// Scales element to width
        /// </summary>
        public int Width 
        {
            get { return destinationRectangle.Width; }
            set { destinationRectangle.Width = value; }
        }

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Flip FlipElement
        {
            get
            {
                switch (flip)
                {
                    case SpriteEffects.FlipHorizontally ^ SpriteEffects.FlipVertically:
                        return Flip.Both;
                    case SpriteEffects.FlipHorizontally:
                        return Flip.Horizontal;
                    case SpriteEffects.FlipVertically:
                        return Flip.Vertical;
                    default:
                        return Flip.None;
                }
            }
            set
            {
                switch (value)
                {
                    case Flip.Both:
                        flip = SpriteEffects.FlipHorizontally ^ SpriteEffects.FlipVertically;
                        break;
                    case Flip.Horizontal:
                        flip = SpriteEffects.FlipHorizontally;
                        break;
                    case Flip.Vertical:
                        flip = SpriteEffects.FlipVertically;
                        break;
                    default:
                        flip = SpriteEffects.None;
                        break;
                }
            }
        }
        public float Depth { get; set; }

        public int MarginTop { get; set; }
        public int MarginRight { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }

        public bool IsDisabled { get; set; }
        public bool IsHidden { get; set; }

        public bool HasFocus
        {
            get { return hasFocus; }
            set
            {
                if (value)
                {
                    hasFocus = value;
                    if (OnFocus != null) OnFocus(this, EventArgs.Empty);
                }
                else
                {
                    hasFocus = value;
                    if (OnBlur != null) OnBlur(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler OnLeftClick;
        public event EventHandler OnDoubleClick;
        public event EventHandler OnMiddleClick;
        public event EventHandler OnRightClick;
        public event EventHandler OnScrollWheel;
        public event EventHandler OnMouseLeftDown;
        public event EventHandler OnMouseRightDown;
        public event EventHandler OnMouseLeftUp;
        public event EventHandler OnMouseRightUp;
        public event EventHandler OnMouseEnter;
        public event EventHandler OnMouseLeave;
        public event EventHandler OnMouseMove;
        public event EventHandler OnMouseOver;
        public event EventHandler OnMouseOut;

        public event EventHandler OnFocus;
        public event EventHandler OnBlur;

        public UIElement(string sharedTextureName)
        {
            this.SharedTextureId = sharedTextureName;
            this.ElementColor = Color.White;
        }

        public UIElement(Texture2D elementTexture)
        {
            this.ElementTexture = elementTexture;
            this.SharedTextureId = String.Empty;
            this.ElementColor = Color.White;
        }

        public virtual void Update(KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, MouseState previousMouseState, MouseState currentMouseState)
        {
            if (!IsDisabled)
            {
                Color[] data = new Color[ElementTexture.Width * ElementTexture.Height];
                ElementTexture.GetData(data);

                currentMouseOver = currentMouseState.Position.X > destinationRectangle.X &&
                                    currentMouseState.Position.X < destinationRectangle.X + Width &&
                                    currentMouseState.Position.Y > destinationRectangle.Y &&
                                    currentMouseState.Position.Y < destinationRectangle.Y + Height &&
                                    data[
                                            (/*row*/currentMouseState.Position.Y - destinationRectangle.Y)
                                            * ElementTexture.Width
                                            + (/*col*/currentMouseState.X - destinationRectangle.X)
                                        ].A == 255;

                if (previousMouseOver && currentMouseOver)
                {
                    if (OnLeftClick != null)
                    {
                        if (previousMouseState.LeftButton == ButtonState.Pressed &&
                            currentMouseState.LeftButton == ButtonState.Released)
                        {
                            OnLeftClick(this, EventArgs.Empty);
                        }
                    }

                    if (OnRightClick != null)
                    {
                        if (previousMouseState.RightButton == ButtonState.Pressed &&
                            currentMouseState.RightButton == ButtonState.Released)
                        {
                            OnRightClick(this, EventArgs.Empty);
                        }
                    }

                    if (OnMouseOver != null)
                    {
                        if (
                            currentMouseState.LeftButton != ButtonState.Pressed &&
                            currentMouseState.RightButton != ButtonState.Pressed &&
                            currentMouseState.MiddleButton != ButtonState.Pressed
                            )
                        {
                            OnMouseOver(this, EventArgs.Empty);
                        }
                    }

                    if (OnMouseMove != null)
                    {
                        if (previousMouseState.Position != currentMouseState.Position)
                        {
                            OnMouseMove(this, EventArgs.Empty);
                        }
                    }
                }
                
                if (OnMouseEnter != null)
                {
                    if (!previousMouseOver && currentMouseOver)
                    {
                        OnMouseEnter(this, EventArgs.Empty);
                    }
                }
                
                if (OnMouseLeave != null)
                {
                    if (previousMouseOver && !currentMouseOver)
                    {
                        OnMouseLeave(this, EventArgs.Empty);
                    }
                }

                if (OnMouseLeftDown != null)
                {
                    if (currentMouseOver && currentMouseState.LeftButton == ButtonState.Pressed)
                    {
                        OnMouseLeftDown(this, EventArgs.Empty);
                    }
                }

                if (OnMouseRightDown != null)
                {
                    if (currentMouseOver && currentMouseState.RightButton == ButtonState.Pressed)
                    {
                        OnMouseRightDown(this, EventArgs.Empty);
                    }
                }

                previousMouseOver = currentMouseOver;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination, Nullable<Rectangle> sourceRectangle, float rotation, Vector2 origin, SpriteEffects flip, float depth)
        {
            spriteBatch.Draw(ElementTexture, destination, sourceRectangle, ElementColor, rotation, origin, flip, depth); 
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ElementTexture, destinationRectangle, null, ElementColor, Rotation, Origin, flip, Depth);
        }

        /// <summary>
        /// Sets the absolute position of the Element on the screen
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            destinationRectangle.X = (int)position.X;
            destinationRectangle.Y = (int)position.Y;
        }

        /// <summary>
        /// Returns the absolute position of the Element on the screen
        /// </summary>
        public Vector2 GetPosition()
        {
            return new Vector2(destinationRectangle.X, destinationRectangle.Y);
        }

        /// <summary>
        /// The position relative to the containing Page
        /// </summary>
        public void Position(Quadrent my, Quadrent at)
        {

        }

        /// <summary>
        /// The position relative to the specified Element within the same Page
        /// </summary>
        public void Position(Quadrent my, Quadrent at, ref UIElement of)
        {

        }
    }
}
