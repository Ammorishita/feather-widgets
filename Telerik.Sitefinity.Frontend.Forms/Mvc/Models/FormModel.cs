﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Forms.Model;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields;
using Telerik.Sitefinity.Frontend.Forms.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Frontend.Resources;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Modules.Forms.Web;
using Telerik.Sitefinity.Modules.Forms.Web.UI.Fields;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.UI;

namespace Telerik.Sitefinity.Frontend.Forms.Mvc.Models
{
    /// <summary>
    /// This class represents the model used for Form widget.
    /// </summary>
    public class FormModel : ContentModelBase, IFormModel
    {
        #region Properties

        /// <inheritDoc/>
        public Guid FormId { get; set; }

        /// <inheritDoc/>
        public FormViewMode ViewMode { get; set; }

        /// <inheritDoc/>
        public bool UseCustomConfirmation { get; set; }

        /// <inheritDoc/>
        public CustomConfirmationMode CustomConfirmationMode { get; set; }

        /// <inheritDoc/>
        public string CustomConfirmationMessage
        {
            get
            {
                if (string.IsNullOrEmpty(this.customConfirmationMessage))
                {
                    this.customConfirmationMessage = this.FormData == null ? (Lstring)Res.Get<FormResources>().SuccessfullySubmittedMessage : this.FormData.SuccessMessage;
                }

                return this.customConfirmationMessage;
            }
            set
            {
                this.customConfirmationMessage = value;
            }
        }

        /// <inheritDoc/>
        public Guid CustomConfirmationPageId { get; set; }

        /// <inheritDoc/>
        public string CssClass
        { 
            get 
            {
                if (this.cssClass == null && this.FormData != null)
                    this.cssClass = this.FormData.CssClass;

                return this.cssClass;
            }

            set 
            {
                this.cssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control to use Ajax submit when the form submit button is clicked
        /// </summary>
        public bool UseAjaxSubmit { get; set; }

        /// <summary>
        /// Gets or sets the submit URL when using AJAX for submitting.
        /// </summary>
        /// <value>
        /// The ajax submit URL.
        /// </value>
        public string AjaxSubmitUrl { get; set; }

        /// <summary>
        /// Represents the current form
        /// </summary>        
        public FormDescription FormData
        {
            get
            {
                FormDescription descr = null;
                if (this.FormId != Guid.Empty)
                {
                    var manager = FormsManager.GetManager();
                    if (this.FormId != Guid.Empty)
                    {
                        descr = manager.GetForm(this.FormId);
                    }
                }

                return descr;
            }
        }

        /// <inheritDoc/>
        public virtual bool NeedsRedirect
        {
            get
            {
                if (this.UseCustomConfirmation)
                    return this.CustomConfirmationMode == CustomConfirmationMode.RedirectToAPage;
                else
                    return this.FormData.SubmitAction == SubmitAction.PageRedirect;
            }
        }

        #endregion

        #region Public methods

        /// <inheritDoc/>
        public virtual FormViewModel GetViewModel()
        {
            if (this.FormId == Guid.Empty)
            {
                return null;
            }

            var viewModel = new FormViewModel()
            {
                ViewMode = this.ViewMode,
                CssClass = this.CssClass,
                UseAjaxSubmit = this.UseAjaxSubmit
            };

            var form = FormsManager.GetManager().GetForms().FirstOrDefault(f => f.Id == this.FormId && f.Status == ContentLifecycleStatus.Live && f.Visible);
            if (form != null)
            {
                if (viewModel.UseAjaxSubmit)
                {
                    string baseUrl;
                    if (this.AjaxSubmitUrl.IsNullOrEmpty())
                    {
                        var currentNode = SiteMapBase.GetCurrentNode();
                        baseUrl = currentNode != null ? currentNode.Url + "/AjaxSubmit" : "";
                    }
                    else
                    {
                        baseUrl = this.AjaxSubmitUrl;
                    }

                    viewModel.AjaxSubmitUrl = baseUrl.StartsWith("~/") ? RouteHelper.ResolveUrl(baseUrl, UrlResolveOptions.Rooted) : baseUrl;
                    viewModel.SuccessMessage = this.GetSubmitMessage(SubmitStatus.Success);

                    if (this.NeedsRedirect)
                    {
                        viewModel.RedirectUrl = this.GetRedirectPageUrl();
                        if (viewModel.RedirectUrl.StartsWith("~/"))
                            viewModel.RedirectUrl = RouteHelper.ResolveUrl(viewModel.RedirectUrl, UrlResolveOptions.Rooted);
                    }
                }
            }
            else
            {
                viewModel.Error = Res.Get<FormsResources>().TheSpecifiedFormNoLongerExists;
            }

            return viewModel;
        }

        /// <inheritDoc/>
        public virtual SubmitStatus TrySubmitForm(FormCollection collection, HttpFileCollectionBase files, string userHostAddress)
        {
            var manager = FormsManager.GetManager();
            var form = manager.GetForm(this.FormId);

            var formEntry = new FormEntryDTO(form);
            var formSubmition = new FormsSubmitionHelper();

            if (!this.ValidateFormSubmissionRestrictions(formSubmition, formEntry))
                return SubmitStatus.RestrictionViolation;

            if (!this.IsValidForm(form, collection, files, manager))
                return SubmitStatus.InvalidEntry;

            var formFields = new HashSet<string>(form.Controls.Select(this.FormFieldName).Where((f) => !string.IsNullOrEmpty(f)));

            var postedFiles = new Dictionary<string, List<FormHttpPostedFile>>();
            for (int i = 0; i < files.AllKeys.Length; i++)
            {
                if (formFields.Contains(files.AllKeys[i]))
                {
                    postedFiles[files.AllKeys[i]] = files.GetMultiple(files.AllKeys[i]).Where(f => !f.FileName.IsNullOrEmpty()).Select(f =>
                        new FormHttpPostedFile()
                        {
                            FileName = f.FileName,
                            ContentLength = f.ContentLength,
                            ContentType = f.ContentType,
                            InputStream = f.InputStream
                        }).ToList();
                }
            }

            var formData = new List<KeyValuePair<string, object>>(collection.Count);
            for (int i = 0; i < collection.Count; i++)
            {
                if (formFields.Contains(collection.Keys[i]))
                {
                    formData.Add(new KeyValuePair<string, object>(collection.Keys[i], collection[collection.Keys[i]]));
                }
            }

            formEntry.PostedData = formData;
            formEntry.Files = postedFiles;
            formSubmition.Save(formEntry);
            
            return SubmitStatus.Success;
        }

        /// <inheritDoc/>
        public virtual string GetRedirectPageUrl()
        {
            if (!this.UseCustomConfirmation && !string.IsNullOrEmpty(this.FormData.RedirectPageUrl))
            {
                return this.FormData.RedirectPageUrl;
            }
            else if (this.CustomConfirmationPageId == Guid.Empty)
            {
                var currentNode = SiteMapBase.GetActualCurrentNode();
                if (currentNode == null)
                    return null;

                this.CustomConfirmationPageId = currentNode.Id;
            }

            return HyperLinkHelpers.GetFullPageUrl(this.CustomConfirmationPageId);
        }

        /// <inheritDoc/>
        public virtual string GetSubmitMessage(SubmitStatus submitedSuccessfully)
        {
            switch (submitedSuccessfully)
            {
                case SubmitStatus.Success:
                    return this.CustomConfirmationMessage;
                case SubmitStatus.InvalidEntry:
                    return Res.Get<FormResources>().UnsuccessfullySubmittedMessage;
                case SubmitStatus.RestrictionViolation:
                    return Res.Get<FormsResources>().YouHaveAlreadySubmittedThisForm;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Validates the form against the preset submit restrictions.
        /// </summary>
        protected virtual bool ValidateFormSubmissionRestrictions(FormsSubmitionHelper formsSubmition, FormEntryDTO formEntry)
        {
            string errorMessage;
            var isValid = formsSubmition.ValidateRestrictions(formEntry, out errorMessage);

            return isValid;
        }

        /// <summary>
        /// Determines whether a form is valid or not.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="manager">The manager.</param>
        /// <returns>true if form is valid, false otherwise.</returns>
        protected virtual bool IsValidForm(FormDescription form, FormCollection collection, HttpFileCollectionBase files, FormsManager manager)
        {
            this.SanitizeFormCollection(collection);
            var behaviorResolver = ObjectFactory.Resolve<IControlBehaviorResolver>();
            foreach (var control in form.Controls)
            {
                if (control.IsLayoutControl)
                    continue;

                Type controlType;
                if (control.ObjectType.StartsWith("~/"))
                    controlType = FormsManager.GetControlType(control);
                else
                    controlType = TypeResolutionService.ResolveType(behaviorResolver.GetBehaviorObjectType(control), true);
        
                if (!controlType.ImplementsGenericInterface(typeof(IFormElementController<>)))
                    continue;
                
                var controlInstance = manager.LoadControl(control);
                var controlBehaviorObject = behaviorResolver.GetBehaviorObject(controlInstance);
                var formField = controlBehaviorObject as IFormFieldController<IFormFieldModel>;

                if (formField != null)
                {
                    var multipleFiles = files.GetMultiple(formField.MetaField.FieldName);
                    object fieldValue;

                    if (multipleFiles != null && multipleFiles.Count() > 0)
                    {
                        fieldValue = (object)multipleFiles;
                    }
                    else if (collection.Keys.Contains(formField.MetaField.FieldName))
                    {
                        collection[formField.MetaField.FieldName] = collection[formField.MetaField.FieldName] ?? string.Empty;
                        fieldValue = (object)collection[formField.MetaField.FieldName];
                    }
                    else
                    {
                        fieldValue = null;
                    }

                    if (!formField.Model.IsValid(fieldValue))
                        return false;
                }
                else
                {
                    var formElement = (IFormElementController<IFormElementModel>)controlBehaviorObject;
                    if (!formElement.IsValid())
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Sanitizes the form collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        protected virtual void SanitizeFormCollection(FormCollection collection)
        {
            const char ReplacementSymbol = '_';
            var forbidenSymbols = new char[] { '-' };

            if (collection != null)
            {
                var forbidenKeys = collection.AllKeys.Where(k => k.IndexOfAny(forbidenSymbols) >= 0);
                foreach (var key in forbidenKeys)
                {
                    var newKey = key.ToCharArray();
                    for (int i = 0; i < newKey.Length; i++)
                    {
                        if (forbidenSymbols.Contains(newKey[i]))
                        {
                            newKey[i] = ReplacementSymbol;
                        }
                    }

                    collection.Add(new string(newKey), collection[key]);
                    collection.Remove(key);
                }
            }
        }

        #endregion

        #region ContentModelBase

        /// <inheritDoc/>
        public override Type ContentType
        {
            get { return typeof(FormDescription); }
            set { }
        }

        /// <inheritDoc/>
        protected override IQueryable<IDataItem> GetItemsQuery()
        {
            return ((FormsManager)this.GetManager()).GetForms().Where(f => f.Framework == FormFramework.Mvc);
        }

        #endregion

        #region Private methods

        private string FormFieldName(FormControl control)
        {
            if (control.IsLayoutControl)
                return null;

            var behaviorResolver = ObjectFactory.Resolve<IControlBehaviorResolver>();
            var controlInstance = FormsManager.GetManager().LoadControl(control);
            var controller = behaviorResolver.GetBehaviorObject(controlInstance);
            var fieldController = controller as IFormFieldControl;

            if (fieldController != null && fieldController.MetaField != null)
                return fieldController.MetaField.FieldName;
            else
                return null;
        }

        #endregion

        #region Private fields

        private string cssClass;
        private string customConfirmationMessage;

        #endregion
    }
}