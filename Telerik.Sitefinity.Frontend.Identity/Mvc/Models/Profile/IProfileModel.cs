﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telerik.Sitefinity.Frontend.Identity.Mvc.Models.Profile
{
    /// <summary>
    /// This interface is used as a model for the ProfileController.
    /// </summary>
    public interface IProfileModel
    {
        /// <summary>
        /// Gets or sets the css class that will be applied on the wrapping element of the widget.
        /// </summary>
        /// <value>
        /// The css class value as a string.
        /// </value>
        string CssClass { get; set; }

        /// <summary>
        /// Gets or sets the save changes action.
        /// </summary>
        /// <value>
        /// The save changes action.
        /// </value>
        SaveAction SaveChangesAction { get; set; }

        /// <summary>
        /// Gets or sets the page identifier where the widget will redirect when profile is saved .
        /// </summary>
        /// <value>
        /// The profile saved page identifier.
        /// </value>
        Guid ProfileSavedPageId { get; set; }

        /// <summary>
        /// Gets or sets the profile saved message.
        /// </summary>
        /// <value>
        /// Message to show when profile is saved.
        /// </value>
        string ProfileSaveMsg { get; set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <returns>
        /// A instance of <see cref="ProfileViewModel"/> as view model.
        /// </returns>
        ProfileViewModel GetViewModel();
    }
}
