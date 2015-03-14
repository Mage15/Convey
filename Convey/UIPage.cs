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
using Convey.Effects;
using System.Threading.Tasks;

namespace Convey
{
    public class UIPage
    {
        protected Vector2 currentPosition;
        protected SpriteEffects flip;
        protected bool isHidden;
        protected bool isDisabled;
        protected UIEffect currentEffect;
        protected Rectangle destinationRectangle;
        protected Rectangle sourceRectangle;
        protected int setHeight;
        protected int setWidth;
        
        public UIDocument ParentDocument { get; set; }
        public UIPage ParentPage { get; set; }
        public Dictionary<string, UIPage> ChildPages { get; set; }
        public List<UIElement> ChildElements { get; set; }

        public Texture2D BackgroundTexture { get; set; }
        public Color BackgroundColor { get; set; }
        public int Height 
        {
            get { return (setHeight > -1) ? setHeight : destinationRectangle.Height; }
            set { setHeight = value; }
        }
        public int Width 
        {
            get { return (setWidth > -1) ? setWidth : destinationRectangle.Width; }
            set { setWidth = value; }
        }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public float Opacity { get; set; }
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
        
        /// <summary>
        /// Aligns elements relative to the page's position. The default is to align from the top
        /// of the page.
        /// </summary>
        public Edge AlignElements { get; set; }

        /// <summary>
        /// Aligns page's element relative to the page's position. The default is to align from the top
        /// of the page.
        /// </summary>
        public Edge AlignPages { get; set; }
        
        /// <summary>
        /// Sets the Page and all Child Elements to the value specified.
        /// <para>&#160;</para><br />
        /// <para>Note: Disabled means that it is still drawn but no events are raised.</para>
        /// <para>Tip: You could use a grayscale texture for a visual queue to the user</para>
        /// </summary>
        public bool IsDisabled 
        {
            get { return isDisabled; }
            set
            {
                Parallel.ForEach(ChildPages.Values, page => page.IsDisabled = value);
                Parallel.ForEach(ChildElements, element => element.IsDisabled = value);
                isDisabled = value;
            } 
        }
        public bool IsHidden 
        {
            get { return isHidden; }
            set
            {
                Parallel.ForEach(ChildPages.Values, page => page.IsDisabled = value);
                Parallel.ForEach(ChildElements, element => element.IsHidden = value);
                isHidden = value;
            }
        }

        public UIPage()
        {
            ChildElements = new List<UIElement>();
            ChildPages = new Dictionary<string, UIPage>();
            currentPosition = Vector2.Zero;
            setHeight = -1;
            setWidth = -1;
        }

        public void AddElement(UIElement element)
        {
            element.OnLeftClick += ParentDocument.Element_ChangeFocus;
            element.ParentPage = this;

            if (!String.IsNullOrEmpty(element.SharedTextureId)) element.ElementTexture = this.ParentDocument.SharedTextures[element.SharedTextureId];

            ChildElements.Add(element);
        }

       public void AddPage(UIPage page)
        {

        }

        private void AlignChildElements()
        {
            Vector2 elementPostion = currentPosition;

            if (AlignElements == Edge.Top)
            {
                for (int i = 0; i < ChildElements.Count; i++)
                {
                    // Don't account for hidden elements
                    if (ChildElements[i].IsHidden) continue;

                    elementPostion.Y += ChildElements[i].MarginTop;

                    ChildElements[i].SetPosition(elementPostion);

                    elementPostion.Y += ChildElements[i].Height;
                    elementPostion.Y += ChildElements[i].MarginBottom;

                    // Also set the X value
                    elementPostion.X = currentPosition.X + ChildElements[i].MarginLeft;
                }
            }
            else if (AlignElements == Edge.Left)
            {
                for (int i = 0; i < ChildElements.Count; i++)
                {
                    // Don't account for hidden elements
                    if (ChildElements[i].IsHidden) continue;

                    elementPostion.X += ChildElements[i].MarginLeft;

                    ChildElements[i].SetPosition(elementPostion);

                    elementPostion.X += ChildElements[i].Width;
                    elementPostion.X += ChildElements[i].MarginRight;

                    // Also set the Y value
                    elementPostion.Y = currentPosition.Y + ChildElements[i].MarginTop;
                }
            }
            else if (AlignElements == Edge.Bottom)
            {
                int totalHeight = 0;
                foreach(UIElement element in ChildElements)
                {
                    // Don't account for hidden elements
                    if(element.IsHidden == false) totalHeight += element.MarginTop + element.Height + element.MarginBottom;
                }

                if(destinationRectangle.Height > totalHeight)
                {
                    elementPostion.Y = destinationRectangle.Height - totalHeight;
                }

                for(int i = ChildElements.Count -1; i <= 0; i--)
                {
                    // Don't account for hidden elements
                    if (ChildElements[i].IsHidden) continue;

                    elementPostion.Y += ChildElements[i].MarginTop;

                    ChildElements[i].SetPosition(elementPostion);

                    elementPostion.Y += ChildElements[i].Height;
                    elementPostion.Y += ChildElements[i].MarginBottom;

                    // Also set the X value
                    elementPostion.X = currentPosition.X + ChildElements[i].MarginLeft;
                }
            }
            else if (AlignElements == Edge.Right)
            {
                int totalWidth = 0;
                foreach (UIElement element in ChildElements)
                {
                    // Don't account for hidden elements
                    if (element.IsHidden == false) totalWidth += element.MarginLeft + element.Width + element.MarginRight;
                }

                if (destinationRectangle.Width > totalWidth)
                {
                    elementPostion.X = destinationRectangle.Width - totalWidth;
                }

                for (int i = ChildElements.Count - 1; i <= 0; i--)
                {
                    // Don't account for hidden elements
                    if (ChildElements[i].IsHidden) continue;

                    elementPostion.X += ChildElements[i].MarginLeft;

                    ChildElements[i].SetPosition(elementPostion);

                    elementPostion.X += ChildElements[i].Width;
                    elementPostion.X += ChildElements[i].MarginRight;

                    // Also set the Y value
                    elementPostion.Y = currentPosition.Y + ChildElements[i].MarginTop;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isHidden)
            {
                // If we have an effect to play then use then use the effects values
                if (currentEffect != null)
                {
                    DrawValues values = currentEffect.GetValues();

                    foreach (UIPage page in ChildPages.Values)
                    {
                        // Still needs implementation
                        //page.Draw(spriteBatch);
                    }
                    foreach (UIElement element in ChildElements)
                    {
                        //Still needs implementation
                        //Vector2 newPosition = new Vector2(0, 0);  // New position based on page's currentPostion
                        //Rectangle newSourceRectangle = new Rectangle(0,0,0,0); // Calculate how much of the element is showing based on page's new height and width
                        //element.Draw(spriteBatch, newPosition, newSourceRectangle, values.Rotation, values.Origin, this.Scale, values.SpriteEffect, values.Depth);
                    }

                    if (currentEffect.Done)
                    {
                        this.IsHidden = currentEffect.IsHiddenOnDone;
                        
                        // Were done with the effect so throw it away
                        currentEffect = null;
                    }
                }
                else
                {
                    if(BackgroundTexture != null)
                    {
                        Rectangle drawnRectangle = new Rectangle(
                            (int)currentPosition.X,
                            (int)currentPosition.Y,
                            Width,
                            Height
                            );
                        spriteBatch.Draw(BackgroundTexture, drawnRectangle, sourceRectangle, BackgroundColor, Rotation, Origin, flip, Depth);
                    }

                    foreach (UIPage page in ChildPages.Values)
                    {
                        page.Draw(spriteBatch);
                    }
                    foreach (UIElement element in ChildElements)
                    {
                        element.Draw(spriteBatch);
                    }
                }
            }
        }

        public virtual void Update(KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, MouseState previousMouseState, MouseState currentMouseState)
        {
            if (!isHidden )
            {
                if (!isDisabled)
                {
                    foreach (UIPage page in ChildPages.Values) page.Update(previousKeyboardState, currentKeyboardState, previousMouseState, currentMouseState);
                    foreach (UIElement element in ChildElements) element.Update(previousKeyboardState, currentKeyboardState, previousMouseState, currentMouseState);
                }

                AlignChildElements();
            }
        }

        public void Hide(UIEffect effect)
        {
            if(effect != null)
            {
                effect.StartValues.Depth = this.Depth;
                effect.StartValues.DestinationRectangle = this.destinationRectangle;
                effect.StartValues.Opacity = this.Opacity;
                effect.StartValues.Origin = this.Origin;
                effect.StartValues.Rotation = this.Rotation;
                effect.StartValues.SourceRectangle = this.sourceRectangle;
                effect.StartValues.SpriteEffect = this.flip;
                effect.IsHiddenOnDone = true;
            }
            else
            {
                this.IsHidden = true;
            }

            this.currentEffect = effect;
        }

        /// <summary>
        /// Sets the absolute position of the Page on the screen
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            currentPosition = position;
        }

        /// <summary>
        /// Returns the absolute position of the Page on the screen
        /// </summary>
        public Vector2 GetPosition()
        {
            return currentPosition;
        }

        /// <summary>
        /// Sets the position relative to the ParentDocument
        /// </summary>
        public void Position(Quadrent my, Quadrent at)
        {

        }

        /// <summary>
        /// Sets the position relative to the specified Page
        /// </summary>
        public void Position(Quadrent my, Quadrent at, ref UIPage of)
        {

        }

        /// <summary>
        /// Sets the position relative to the specified Element
        /// </summary>
        public void Position(Quadrent my, Quadrent at, ref UIElement of)
        {

        }

        public void Show(UIEffect effect)
        {
            if(effect != null)
            {
                effect.EndValues.Depth = this.Depth;
                effect.EndValues.DestinationRectangle = this.destinationRectangle;
                effect.EndValues.Opacity = this.Opacity;
                effect.EndValues.Origin = this.Origin;
                effect.EndValues.Rotation = this.Rotation;
                effect.EndValues.SourceRectangle = this.sourceRectangle;
                effect.EndValues.SpriteEffect = this.flip;
                effect.IsHiddenOnDone = false;
            }
            else
            {
                this.IsHidden = false;
            }

            this.currentEffect = effect;
        }
    }
}
