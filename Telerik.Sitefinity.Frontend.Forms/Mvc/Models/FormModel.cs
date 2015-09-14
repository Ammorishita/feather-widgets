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
                    return Res.Get<FormResources>().SuccessfullySubmittedMessage;
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
        public string CssClass { get; set; }

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
                FormId = this.FormId,
                ViewMode = this.ViewMode,
                CssClass = this.CssClass
            };

            if (!FormsManager.GetManager().GetForms().Any(f => f.Id == this.FormId && f.Status == ContentLifecycleStatus.Live && f.Visible))
            {
                viewModel.Error = Res.Get<FormsResources>().TheSpecifiedFormNoLongerExists;
            }

            return viewModel;
        }

        /// <inheritDoc/>
        public virtual string GetViewPath()
        {
            var currentPackage = new PackageManager().GetCurrentPackage();
            if (string.IsNullOrEmpty(currentPackage))
            {
                currentPackage = "default";
            }

            var viewPath = FormsVirtualRazorResolver.Path + currentPackage + "/" + this.FormId.ToString("D") + ".cshtml";

            return viewPath;
        }

        /// <inheritDoc/>
        public virtual bool TrySubmitForm(FormCollection collection, HttpFileCollectionBase files, string userHostAddress)
        {
            var success = false;

            var manager = FormsManager.GetManager();
            var form = manager.GetForm(this.FormId);

            if (this.IsValidForm(form, collection, files, manager))
            {
                var formFields = new HashSet<string>(form.Controls.Select(this.FormFieldName).Where((f) => !string.IsNullOrEmpty(f)));

                var formData = new List<KeyValuePair<string, object>>(collection.Count);
                for (int i = 0; i < collection.Count; i++)
                {
                    if (formFields.Contains(collection.Keys[i]))
                    {
                        formData.Add(new KeyValuePair<string, object>(collection.Keys[i], collection[collection.Keys[i]]));
                    }
                }

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

                var formLanguage = SystemManager.CurrentContext.AppSettings.Multilingual ? CultureInfo.CurrentUICulture.Name : null;
                FormsHelper.SaveFormsEntry(form, formData, postedFiles, userHostAddress, ClaimsManager.GetCurrentUserId(), formLanguage);

                success = true;
            }

            return success;
        }

        /// <inheritDoc/>
        public virtual string GetRedirectPageUrl()
        {
            if (this.CustomConfirmationPageId == Guid.Empty)
            {
                var currentNode = SiteMapBase.GetActualCurrentNode();
                if (currentNode == null)
                    return null;

                this.CustomConfirmationPageId = currentNode.Id;
            }

            return HyperLinkHelpers.GetFullPageUrl(this.CustomConfirmationPageId);
        }

        /// <inheritDoc/>
        public virtual string GetSubmitMessage(bool submitedSuccessfully)
        {
            if (submitedSuccessfully)
            {
                return this.CustomConfirmationMessage;
            }

            return Res.Get<FormResources>().UnsuccessfullySubmittedMessage;
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

                if (controlBehaviorObject is IFormFieldController<IFormFieldModel>)
                {
                    var formField = (IFormFieldController<IFormFieldModel>)controlBehaviorObject;
                    object fieldValue = (object)collection[formField.MetaField.FieldName] ?? (object)files.GetMultiple(formField.MetaField.FieldName);

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

        private string customConfirmationMessage;

        #endregion
    }
}