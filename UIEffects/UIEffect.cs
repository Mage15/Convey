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

namespace Convey.Effects
{
    public enum Effects
    {
        Drop,
        Fade,
        Puff,
        Slide
    }

    public class UIEffect
    {
        private int currentIteration;
        private int totalIterations;
        private DrawValues currentValues;

        public DrawValues StartValues { get; set; }
        public DrawValues EndValues { get; set; }
        public bool Done { get; set; }
        public bool IsHiddenOnDone { get; set; }

        public UIEffect(int iterations)
        {
            this.totalIterations = iterations;
        }

        public virtual DrawValues GetValues()
        {
            if (!Done)
            {
                currentIteration++;

                if (currentIteration >= totalIterations) Done = true;
            }

            return currentValues;
        }

        public void Reset()
        {
            currentValues.Depth = StartValues.Depth;
            currentValues.DestinationRectangle = StartValues.DestinationRectangle;
            currentValues.Opacity = StartValues.Opacity;
            currentValues.Origin = StartValues.Origin;
            currentValues.Rotation = StartValues.Rotation;
            currentValues.SourceRectangle = StartValues.SourceRectangle;
            currentValues.SpriteEffect = StartValues.SpriteEffect;

            currentIteration = 0;
        }
    }
}
