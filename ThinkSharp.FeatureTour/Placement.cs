// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FeatureTouring
{
    /// <summary>
    /// Defines the placement of the popup.
    /// </summary>
    public enum Placement
    {
        /// <summary>
        /// The popup will be placed in center of the target control.
        /// </summary>
        Center,

        /// <summary>
        /// The popup will be placed above the target control left-aligned.
        /// </summary>
        TopLeft,
        /// <summary>
        /// The popup will be placed above the target control centered.
        /// </summary>
        TopCenter,
        /// <summary>
        /// The popup will be placed above the target control right-aligned.
        /// </summary>
        TopRight,

        /// <summary>
        /// The popup will be placed to the left of the target control (at the top).
        /// </summary>
        LeftTop,
        /// <summary>
        /// The popup will be placed to the left of the target control (at the center).
        /// </summary>
        LeftCenter,
        /// <summary>
        /// The popup will be placed to the left of the target control (at the bottom).
        /// </summary>
        LeftBottom,

        /// <summary>
        /// The popup will be placed below the target control left-aligned.
        /// </summary>
        BottomLeft,
        /// <summary>
        /// The popup will be placed below the target control centered.
        /// </summary>
        BottomCenter,
        /// <summary>
        /// The popup will be placed below the target control right-aligned.
        /// </summary>
        BottomRight,

        /// <summary>
        /// The popup will be placed to the right of the target control (at the top).
        /// </summary>
        RightTop,
        /// <summary>
        /// The popup will be placed to the right of the target control (at the center).
        /// </summary>
        RightCenter,
        /// <summary>
        /// The popup will be placed to the right of the target control (at the bottom).
        /// </summary>
        RightBottom
    }
}
