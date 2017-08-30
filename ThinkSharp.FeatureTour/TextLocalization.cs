﻿// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace ThinkSharp.FeatureTouring
{
    public static class TextLocalization
    {
        static TextLocalization()
        {
            Reset();
        }

        public static string Next { get; set; }
        public static string Close { get; set; }
        public static string DoIt { get; set; }

        public static void Reset()
        {
            Next = "Next >>";
            Close = "Close";
            DoIt = "Do it!";
        }
    }
}