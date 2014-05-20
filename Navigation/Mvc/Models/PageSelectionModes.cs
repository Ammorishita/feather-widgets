﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Mvc.Models
{
    /// <summary>
    /// The rendering options for the Navigation widget. 
    /// </summary>
    /// <remarks>
    /// Each option describes different selection of sitemap nodes that will be included while rendering the Navigation widget.
    /// </remarks>
    public enum PageSelectionModes
    {
        /// <summary>
        /// Refers to top-level pages and all their child pages
        /// </summary>
        TopLevelPages,
        /// <summary>
        /// Refers to all child pages under particular page.
        /// </summary>
        SelectedPageChildren,
        /// <summary>
        /// Refers to custom selection of pages.
        /// </summary>
        SelectedPages,
        /// <summary>
        /// Refers to child pages under currently opened page.
        /// </summary>
        CurrentPageChildren,
        /// <summary>
        /// Refers to page siblings of the currently opened page.
        /// </summary>
        CurrentPageSiblings
    }
}
