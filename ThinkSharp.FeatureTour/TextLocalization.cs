// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace ThinkSharp.FeatureTouring
{
    /// <summary>
    /// Static class that can be used to localize strings used in feature tour pop-up.
    /// </summary>
    public static class TextLocalization
    {
        static TextLocalization()
        {
            Reset();
        }

        /// <summary>
        /// Gets or sets the text for the next button.
        /// </summary>
        public static string Next { get; set; }

        /// <summary>
        /// Gets or sets the text for the close button.
        /// </summary>
        public static string Close { get; set; }

        /// <summary>
        /// Gets or sets a value for the DoIt button.
        /// </summary>
        public static string DoIt { get; set; }

        /// <summary>
        /// Resets the properties back to it's default values.
        /// </summary>
        public static void Reset()
        {
            Next = "Next >>";
            Close = "Close";
            DoIt = "Do it!";
        }
    }
}
