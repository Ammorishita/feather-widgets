﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.Models.UnsubscribeForm;
using Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Licensing;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Newsletters;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Services;

namespace Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.Controllers
{
    /// <summary>
    /// This class represents the controller of the Unsubscribe widget.
    /// </summary>
    [ControllerToolboxItem(Name = "UnsubscribeForm_MVC",
        Title = "Unsubscribe",
        SectionName = ToolboxesConfig.NewslettersToolboxSectionName,
        CssClass = UnsubscribeFormController.WidgetIconCssClass)]
    [Localization(typeof(UnsubscribeFormResources))]
    public class UnsubscribeFormController : Controller, ILicensedControl
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the template that widget will be displayed when Link unsubscribe mode is selected.
        /// </summary>
        /// <value></value>
        public string LinkTemplateName
        {
            get
            {
                return this.linkTemplateName;
            }

            set
            {
                this.linkTemplateName = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the template that widget will be displayed when EmailAddress unsubscribe mode is selected.
        /// </summary>
        /// <value></value>
        public string EmailAddressTemplateName
        {
            get
            {
                return this.emailAddressTemplateName;
            }

            set
            {
                this.emailAddressTemplateName = value;
            }
        }

        /// <summary>
        /// Gets the Unsubscribe widget model.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual IUnsubscribeFormModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = ControllerModelFactory.GetModel<IUnsubscribeFormModel>(this.GetType());

                return this.model;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance of the control is licensed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is licensed; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public virtual bool IsLicensed
        {
            get { return LicenseState.CheckIsModuleLicensedInCurrentDomain(NewslettersModule.ModuleId); }
        }

        /// <summary>
        /// Gets the custom licensing message. If null the system will use a default message
        /// </summary>
        /// <value>The licensing message.</value>
        [Browsable(false)]
        public virtual string LicensingMessage
        {
            get { return Res.Get<UnsubscribeFormResources>().ModuleNotLicensed; }
        }

        /// <summary>
        /// Gets the current HTTP context from the SystemManager.
        /// </summary>
        /// <value>The current HTTP context.</value>
        protected virtual HttpContextBase CurrentHttpContext
        {
            get
            {
                return SystemManager.CurrentHttpContext;
            }
        }
        #endregion

        #region Actions
        /// <summary>
        /// Default Action
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Index()
        {
            if (!this.IsLicensed)
            {
                return this.Content(this.LicensingMessage);
            }

            var context = this.CurrentHttpContext;
            var page = context.CurrentHandler as Page;

            if (!SystemManager.IsDesignMode && this.Model.UnsubscribeMode == UnsubscribeMode.Link)
            {
                var subscriberId = page.Request.QueryString["subscriberId"];
                var issueId = page.Request.QueryString["issueId"];
                var subscribe = page.Request.QueryString["subscribe"];
                
                this.Model.ExecuteAction(subscriberId, issueId, subscribe != null);
            }

            var viewModel = this.Model.CreateViewModel();

            var fullTemplateName = this.model.UnsubscribeMode == UnsubscribeMode.Link ?
                                            this.linkTemplateNamePrefix + this.LinkTemplateName :
                                            this.emailAddressTemplateNamePrefix + this.EmailAddressTemplateName;

            return this.View(fullTemplateName, viewModel);
        }

        /// <summary>
        /// Indexes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(UnsubscribeFormViewModel model)
        {
            if (this.ModelState.IsValid && this.Model.ListId != Guid.Empty)
            {
                string error;
                bool isSucceded = this.Model.Unsubscribe(model.Email, out error);

                this.ViewBag.Error = error;
                this.ViewBag.IsSucceded = isSucceded;
            }

            var viewModel = this.Model.CreateViewModel();

            var fullTemplateName = this.emailAddressTemplateNamePrefix + this.EmailAddressTemplateName;

            return this.View(fullTemplateName, viewModel);
        }

        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            this.Index().ExecuteResult(this.ControllerContext);
        }

        #endregion

        #region Private fields and constants
        internal const string WidgetIconCssClass = "sfFormsIcn sfMvcIcn";
        private IUnsubscribeFormModel model;
        private string linkTemplateName = "UnsubscribeMessage";
        private string emailAddressTemplateName = "UnsubscribeForm";
        private readonly string linkTemplateNamePrefix = "UnsubscribeFormByLink.";
        private readonly string emailAddressTemplateNamePrefix = "UnsubscribeFormByEmailAddress.";
        #endregion
    }
}
