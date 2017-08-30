// Copyright (c) Jan-Niklas Schäfer. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThinkSharp.FeatureTouring.Navigation;

namespace ThinkSharp.FeatureTouring.Models
{
    public class Step
    {
        public Step(string elementID, object header, object content)
            : this(elementID, header, content, elementID)
        {}

        public Step(string elementID, object header, object content, string id)
        {
            Header = header;
            ElementID = elementID;
            ID = id;
            Content = content;
        }

        /// <summary>
        /// Gets the header object to show in the header area.
        /// </summary>
        public object Header { get; private set; }

        /// <summary>
        /// Gets the content to show in main area of the popup.
        /// </summary>
        public object Content { get; private set; }

        /// <summary>
        /// Gets the element ID that indicates the visual element where the step is shown. 
        /// </summary>
        /// <remarks>
        /// Use attached property <see cref="TourHelper.ElementIDProperty"/> to define a element id
        /// for an <see cref="UIElement"/>.
        /// </remarks>
        public string ElementID { get; private set; }

        /// <summary>
        /// Gets the ID of the step.
        /// </summary>
        /// <remarks>
        /// If ID is not set; it will be equal to the <see cref="ElementID"/>
        /// The ID can be used in <see cref="FeatureTour.ForStep"/> or <see cref="FeatureTour.IfCurrentStepEquals"/>
        /// to reference the step.
        /// </remarks>
        public string ID { get; private set; }

        /// <summary>
        /// Gets or sets a value that determines if next button is shown.
        /// If <see cref="ShowNextButton"/> is null (default), the value from <see cref="Tour.ShowNextButtonDefault"/> is used.
        /// </summary>
        public bool? ShowNextButton { get; set; }

        /// <summary>
        /// Gets or sets the key of the content data template.
        /// </summary>
        /// <remarks>
        /// A data template with that key must be accessible by <see cref="FrameworkElement.TryFindResource"/> 
        /// from the <see cref="UIElement"/> with the element id <see cref="ElementID"/>.
        /// If <see cref="ContentDataTemplateKey"/> is not set (null) or the template is not available, 
        /// the content will be displayed in a text block.
        /// </remarks>
        public string ContentDataTemplateKey { get; set; }

        /// <summary>
        /// Gets or sets the key of the header data template.
        /// </summary>
        /// <remarks>
        /// A data template with that key must be accessible by <see cref="FrameworkElement.TryFindResource"/> 
        /// from the <see cref="UIElement"/> with the element id <see cref="ElementID"/>.
        /// If <see cref="HeaderDataTemplateKey"/> is not set (null) or the template is not available, 
        /// the content will be displayed in a text block.
        /// </remarks>
        public string HeaderDataTemplateKey { get; set; }

        /// <summary>
        /// Gets or sets a custom object that can be attached to the step.
        /// </summary>
        public object Tag { get; set; }
    }
}
