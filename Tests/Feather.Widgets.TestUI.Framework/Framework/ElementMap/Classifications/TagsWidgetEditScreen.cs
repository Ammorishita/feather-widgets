﻿using System;
using System.Linq;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.TestTemplates;

namespace Feather.Widgets.TestUI.Framework.Framework.ElementMap.Classifications
{
    /// <summary>
    /// Provides access to Tags widget designer elements.
    /// </summary>
    public class TagsWidgetEditScreen : HtmlElementContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListsWidgetEditScreen" /> class.
        /// </summary>
        /// <param name="find">The find.</param>
        public TagsWidgetEditScreen(Find find)
            : base(find)
        {
        }

        /// <summary>
        /// Gets Sort list item drop down.
        /// </summary>
        /// <value>Sorting drop down</value>
        public HtmlSelect SortDropdown
        {
            get
            {
                return this.Get<HtmlSelect>("id=sortOptions");
            }
        }

        /// <summary>
        /// Gets List items template drop down.
        /// </summary>
        /// <value>List items template drop down</value>
        public HtmlSelect TemplateDropdown
        {
            get
            {
                return this.Get<HtmlSelect>("id=navTemplateName");
            }
        }

        /// <summary>
        /// Gets the used by content type option.
        /// </summary>
        /// <value>The used by content type option.</value>
        public HtmlSelect UsedByContentTypeOption
        {
            get
            {
                return this.Get<HtmlSelect>("ng-show=properties.TaxaToDisplay.PropertyValue === 'UsedByContentType'");
            }
        }
    }
}
