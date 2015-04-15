﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.DynamicModules.Web.UI.Frontend;
using Telerik.Sitefinity.Frontend.ContentBlock.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.DynamicContent.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Identity.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Identity.Mvc.Models.Profile;
using Telerik.Sitefinity.Frontend.Media.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.News.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.SocialShare.Mvc.Controllers;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.TestIntegration.Data.Content;
using Telerik.Sitefinity.TestUtilities.CommonOperations;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web.UI.ContentUI;

namespace FeatherWidgets.TestUtilities.CommonOperations
{
    /// <summary>
    /// This class provides access to page operations
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class PagesOperations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#")]
        public Guid CreatePageWithControl(Control control, string pageNamePrefix, string pageTitlePrefix, string urlNamePrefix, int index)
        {
            var controls = new List<System.Web.UI.Control>();
            controls.Add(control);

            this.locationGenerator = new PageContentGenerator();
            var pageId = this.locationGenerator.CreatePage(
                                    string.Format(CultureInfo.InvariantCulture, "{0}{1}", pageNamePrefix, index.ToString(CultureInfo.InvariantCulture)),
                                    string.Format(CultureInfo.InvariantCulture, "{0}{1}", pageTitlePrefix, index.ToString(CultureInfo.InvariantCulture)),
                                    string.Format(CultureInfo.InvariantCulture, "{0}{1}", urlNamePrefix, index.ToString(CultureInfo.InvariantCulture)));

            PageContentGenerator.AddControlsToPage(pageId, controls);

            return pageId;
        }

        /// <summary>
        /// Adds content block widget to existing page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        /// <param name="html">Html value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddContentBlockWidgetToPage(Guid pageId, string html = "", string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(ContentBlockController).FullName;

                mvcWidget.Settings = new Telerik.Sitefinity.Mvc.Proxy.ControllerSettings(new ContentBlockController()
                {
                    Content = html
                });

                this.CreateControl(pageManager, page, mvcWidget, "ContentBlock", placeholder);
            }
        }

        /// <summary>
        /// Add shared content block widget to the page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        /// <param name="contentBlockTitle">Content block title</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddSharedContentBlockWidgetToPage(Guid pageId, string contentBlockTitle, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            var content = App.WorkWith().ContentItems()
            .Published()
            .Where(c => c.Title == contentBlockTitle)
            .Get().Single();

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(ContentBlockController).FullName;
                mvcWidget.Settings = new Telerik.Sitefinity.Mvc.Proxy.ControllerSettings(new ContentBlockController()
                {
                    Content = content.Content.Value,
                    SharedContentID = content.Id
                });

                this.CreateControl(pageManager, page, mvcWidget, "ContentBlock", placeholder);
            }
        }

        /// <summary>
        /// Adds news widget to existing page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddNewsWidgetToPage(Guid pageId, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(NewsController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "News", placeholder);
            }
        }

        /// <summary>
        /// Adds social share widget to existing page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddSocialShareWidgetToPage(Guid pageId, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(SocialShareController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "Social share", placeholder);
            }
        }

        /// <summary>
        /// Adds dynamic widget to existing page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddDynamicWidgetToPage(Guid pageId, string type, string widgetName, string widgetCaption, string placeholder = "Body")
        {
            using (var mvcProxy = new MvcWidgetProxy())
            {
                PageManager pageManager = PageManager.GetManager();
                pageManager.Provider.SuppressSecurityChecks = true;
                var pageDataId = pageManager.GetPageNode(pageId).PageId;
                var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

                mvcProxy.ControllerName = typeof(DynamicContentController).FullName;
                var dynamicController = new DynamicContentController();
                dynamicController.Model.ContentType = TypeResolutionService.ResolveType(type);

                mvcProxy.Settings = new ControllerSettings(dynamicController);
                mvcProxy.WidgetName = widgetName;

                this.CreateControl(pageManager, page, mvcProxy, widgetCaption, placeholder);
            }
        }

        /// <summary>
        /// Adds registration widget to existing page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddRegistrationWidgetToPage(Guid pageId, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(RegistrationController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "Registration", placeholder);
            }
        }

        /// <summary>
        /// Adds profile widget to existing page
        /// </summary>
        /// <param name="pageId">Page id value</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddProfileWidgetToPage(Guid pageId, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(ProfileController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "Profile", placeholder);
            }
        }

        /// <summary>
        /// Add login form widget to existing page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="placeholder">the placeholder.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
        public void AddLoginFormWidgetToPage(Guid pageId, string placeholder)
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(LoginFormController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "Login form", placeholder);
            }
        }

        /// <summary>
        /// Adds the image gallery widget to page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="placeholder">The placeholder.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddImageGalleryWidgetToPage(Guid pageId, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(ImageGalleryController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "Image gallery", placeholder);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public void AddDocumentsListWidgetToPage(Guid pageId, string placeholder = "Body")
        {
            PageManager pageManager = PageManager.GetManager();
            pageManager.Provider.SuppressSecurityChecks = true;
            var pageDataId = pageManager.GetPageNode(pageId).PageId;
            var page = pageManager.EditPage(pageDataId, CultureInfo.CurrentUICulture);

            using (var mvcWidget = new Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy())
            {
                mvcWidget.ControllerName = typeof(DocumentsListController).FullName;

                this.CreateControl(pageManager, page, mvcWidget, "Documents list", placeholder);
            }
        }

        /// <summary>
        /// Deletes the pages.
        /// </summary>
        public void DeletePages()
        {
            this.locationGenerator.Dispose();
        }

        /// <summary>
        /// Denies a specific page permissions for selected role.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <param name="roleProvider">The role provider name.</param>
        /// <param name="pageTitle">The page title.</param>
        public void DenyPermissionsForRole(string roleName, string roleProvider, string pageTitle)
        {
            RoleManager roleManager = RoleManager.GetManager(roleProvider);
            var role = roleManager.GetRole(roleName);

            PageManager pagemanager = PageManager.GetManager();
            PageNode mypage = pagemanager.GetPageNodes().FirstOrDefault(pn => pn.Title == pageTitle);

            if (mypage != null)
            {
                pagemanager.BreakPermiossionsInheritance(mypage);
                pagemanager.SaveChanges();

                if (role != null)
                {
                    var perm = pagemanager.GetPermissions()
                                .Where(p =>
                                       p.SetName == SecurityConstants.Sets.Pages.SetName &&
                                       p.PrincipalId == role.Id &&
                                       p.ObjectId == mypage.Id)
                                .SingleOrDefault();

                    if (perm == null)
                    {
                        perm = pagemanager.CreatePermission(
                        SecurityConstants.Sets.Pages.SetName,
                        mypage.Id,
                        role.Id);

                        mypage.Permissions.Add(perm);
                    }

                    perm = pagemanager.GetPermission(
                        SecurityConstants.Sets.Pages.SetName,
                        mypage.Id,
                        role.Id);

                    perm.DenyActions(
                        false,
                        SecurityConstants.Sets.Pages.View,
                        SecurityConstants.Sets.Pages.Create,
                        SecurityConstants.Sets.Pages.Modify,
                        SecurityConstants.Sets.Pages.CreateChildControls,
                        SecurityConstants.Sets.Pages.EditContent,
                        SecurityConstants.Sets.Pages.ChangeOwner,
                        SecurityConstants.Sets.Pages.Delete);

                    pagemanager.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Republish a page.
        /// </summary>
        /// <param name="pageTitle">The page title.</param>
        public void RepublishPage(string pageTitle)
        {
            PageManager pm = PageManager.GetManager();

            PageNode page = pm.GetPageNodes().FirstOrDefault(pn => pn.Title == pageTitle);

            var pageData = page.GetPageData();
            var master = pm.PagesLifecycle.GetMaster(pageData);
            pm.PagesLifecycle.Publish(master);
            pm.SaveChanges();
        }

        /// <summary>
        /// Asserts if widget is present in the toolbox config.
        /// </summary>
        /// <param name="section">The toolbox section.</param>
        /// <param name="widgetTitle">The widget name.</param>
        /// <returns></returns>
        public bool IsWidgetPresentInToolbox(ToolboxSection section, string widgetTitle)
        {
            var item = this.GetToolboxItem(section, widgetTitle);

            if (item == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a toolbox item from the config section.
        /// </summary>
        /// <param name="section">The toolbox section.</param>
        /// <param name="widgetTitle">The name of the widget.</param>
        /// <returns></returns>
        public ToolboxItem GetToolboxItem(ToolboxSection section, string widgetTitle)
        {          
            if (section == null)
            {
                throw new ArgumentException("Section was not found");
            }

            var item = section.Tools.FirstOrDefault<ToolboxItem>(e => e.Title == widgetTitle);

            return item;
        }

        /// <summary>
        /// Gets the dynamic widget toolbox section.
        /// </summary>
        /// <param name="widgetSectionTitle">The section title.</param>
        /// <returns></returns>
        public ToolboxSection GetDynamicWidgetToolboxSection(string widgetSectionTitle)
        {
            ConfigManager configurationManager = ConfigManager.GetManager();
            var toolboxesConfig = configurationManager.GetSection<ToolboxesConfig>();
            var pageControls = toolboxesConfig.Toolboxes["PageControls"];

            var section = pageControls.Sections.FirstOrDefault<ToolboxSection>(s => s.Title == widgetSectionTitle);

            return section;
        }

        /// <summary>
        /// Gets Content toolbox section.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ToolboxSection GetContentToolboxSection()
        {
            ConfigManager configurationManager = ConfigManager.GetManager();
            var toolboxesConfig = configurationManager.GetSection<ToolboxesConfig>();
            var pageControls = toolboxesConfig.Toolboxes["PageControls"];

            var section = pageControls.Sections.FirstOrDefault<ToolboxSection>(s => s.Name == "ContentToolboxSection");

            return section;
        }

        /// <summary>
        /// Gets Test Utilities Assembly.
        /// </summary>
        /// <returns>The Test Utilities Assembly.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public Assembly GetTestUtilitiesAssembly()
        {
            var testUtilitiesAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.Equals("FeatherWidgets.TestUtilities")).FirstOrDefault();
            if (testUtilitiesAssembly == null)
            {
                throw new DllNotFoundException("Assembly wasn't found");
            }

            return testUtilitiesAssembly;
        }

        /// <summary>
        /// Creates the mvcWidget control.
        /// </summary>
        /// <param name="pageManager">The page manager.</param>
        /// <param name="page">The page.</param>
        /// <param name="mvcWidget">The MVC widget.</param>
        private void CreateControl(PageManager pageManager, PageDraft page, Telerik.Sitefinity.Mvc.Proxy.MvcControllerProxy mvcWidget, string widgetCaption, string placeholder = "Body")
        {
            var draftControlDefault = pageManager.CreateControl<PageDraftControl>(mvcWidget, placeholder);
            draftControlDefault.Caption = widgetCaption;
            pageManager.SetControlDefaultPermissions(draftControlDefault);
            page.Controls.Add(draftControlDefault);

            pageManager.PublishPageDraft(page, CultureInfo.CurrentUICulture);
            pageManager.SaveChanges();
        }

        private PageContentGenerator locationGenerator;
    }
}
