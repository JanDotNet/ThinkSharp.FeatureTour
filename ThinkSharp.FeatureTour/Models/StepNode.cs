// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FeatureTouring.Models
{
    internal class StepNode
    {
        public StepNode(Step step)
        {
            Step = step;
        }
        public StepNode Next { get; set; }
        public StepNode Previous { get; set; }

        public Step Step { get; private set; }
        public int StepNo { get; set; }

        public Step NextStep
        {
            get
            {
                if (Next == null)
                    return null;
                return Next.Step;
            }
        }

        public Step PreviousStep
        {
            get
            {
                if (Previous == null)
                    return null;
                return Previous.Step;
            }
        }
    }
}
