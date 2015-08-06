﻿using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Web.UI.Fields.Contracts;

namespace Telerik.Sitefinity.Frontend.Forms
{
    /// <summary>
    /// This interface defines API for working with forms field's controllers.
    /// </summary>
    public interface IFormFieldController : IFormFieldControl, IHasFieldDisplayMode
    {
        /// <summary>
        /// Gets the form field model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [Browsable(false)]
        IFormFieldModel FieldModel { get; }

        /// <summary>
        /// Provides view for Read mode of the forms field
        /// </summary>
        /// <param name="value">The value of the forms field.</param>
        /// <returns></returns>
        ActionResult Read(object value);

        /// <summary>
        /// Provides view for Write mode of the forms field
        /// </summary>
        /// <param name="value">The value of the forms field.</param>
        /// <returns></returns>
        ActionResult Write(object value);
    }
}