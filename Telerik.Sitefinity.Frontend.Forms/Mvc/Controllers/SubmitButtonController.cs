﻿using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.SubmitButton;
using Telerik.Sitefinity.Frontend.Forms.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Metadata.Model;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Web.UI.Fields.Enums;

namespace Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers
{
    /// <summary>
    /// This class represents the controller of the MVC forms submit button.
    /// </summary>
    [ControllerToolboxItem(Name = "MvcSubmitButton", Title = "Submit Button", Toolbox = FormsConstants.FormControlsToolboxName, SectionName = FormsConstants.CommonSectionName)]
    [Localization(typeof(FieldResources))]
    public class SubmitButtonController : Controller, IFormFieldController<ISubmitButtonModel>
    {
        #region Constructors

        public SubmitButtonController()
        {
            this.DisplayMode = FieldDisplayMode.Write;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Form widget model.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ISubmitButtonModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = ControllerModelFactory.GetModel<ISubmitButtonModel>(this.GetType());

                return this.model;
            }
        }

        /// <inheritDocs />
        [Browsable(false)]
        public virtual IMetaField MetaField
        {
            get
            {
                if (this.Model.MetaField == null)
                {
                    this.Model.MetaField = this.Model.GetMetaField(this);
                }

                return this.Model.MetaField;
            }
            set
            {
                this.Model.MetaField = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the template that will be displayed
        /// </summary>
        public string TemplateName
        {
            get
            {
                return this.templateName;
            }

            set
            {
                this.templateName = value;
            }
        }

        /// <inheritDocs />
        public virtual FieldDisplayMode DisplayMode
        {
            get;
            set;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Provides the default view of this field
        /// </summary>
        public virtual ActionResult Index(object value = null)
        {
            if (this.DisplayMode == FieldDisplayMode.Write)
            {
                var viewPath = SubmitButtonController.TemplateNamePrefix + this.TemplateName;
                var viewModel = this.Model.GetViewModel();

                return this.View(viewPath, viewModel);
            }
            else
            {
                return new EmptyResult();
            }
        }

        #endregion

        #region IValidatable

        /// <inheritDocs />
        public bool IsValid()
        {
            return true;
        }

        #endregion

        #region Private fields and constants

        private ISubmitButtonModel model;

        private const string TemplateNamePrefix = "Index.";
        private string templateName = "Default";

        #endregion
    }
}