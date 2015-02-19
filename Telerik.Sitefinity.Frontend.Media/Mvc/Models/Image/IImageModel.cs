﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Model;

namespace Telerik.Sitefinity.Frontend.Media.Mvc.Models.Image
{
    /// <summary>
    /// This interface is used as a model for the ImageController.
    /// </summary>
    public interface IImageModel
    {
        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the markup.
        /// </summary>
        /// <value>
        /// The markup.
        /// </value>
        string Markup { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the alternative text.
        /// </summary>
        /// <value>
        /// The alternative text.
        /// </value>
        string AlternativeText { get; set; }

        /// <summary>
        /// Gets or sets the css class.
        /// </summary>
        /// <value>
        /// The css class.
        /// </value>
        string CssClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use image as link.
        /// </summary>
        /// <value>
        ///   <c>true</c> if should use image as link; otherwise, <c>false</c>.
        /// </value>
        bool UseAsLink { get; set; }

        /// <summary>
        /// Gets or sets the page identifier to use as link.
        /// </summary>
        /// <value>
        /// The page identifier to use as link.
        /// </value>
        Guid PageIdToUseAsLink { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns></returns>
        ImageViewModel GetViewModel();
    }
}