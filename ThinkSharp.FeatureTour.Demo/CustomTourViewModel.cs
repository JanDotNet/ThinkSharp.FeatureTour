// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThinkSharp.FeatureTouring.ViewModels;

namespace ThinkSharp.FeatureTouring
{
    public class CustomTourViewModel : TourViewModel
    {
        public CustomTourViewModel(ITourRun run)
            : base(run) { }

        public string CustomProperty => "S T E P S";
    }
}
