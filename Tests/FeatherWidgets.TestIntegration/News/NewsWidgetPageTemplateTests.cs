﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using FeatherWidgets.TestUtilities.CommonOperations;
using FeatherWidgets.TestUtilities.CommonOperations.Templates;
using MbUnit.Framework;
using News.Mvc.Controllers;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.TestIntegration.Data.Content;
using Telerik.Sitefinity.Web;

namespace FeatherWidgets.TestIntegration.News
{
    /// <summary>
    /// This is a class with News tests.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestFixture]
    public class NewsWidgetPageTemplateTests
    {
        /// <summary>
        /// Set up method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.templateOperation = new TemplateOperations();
            this.locationGenerator = new PageContentGenerator();
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().DeleteAllNews();
        }

        /// <summary>
        /// News widget - add news widget to page template
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "FeatherWidgets.TestUtilities.CommonOperations.Templates.TemplateOperations.AddControlToTemplate(System.Guid,System.Web.UI.Control,System.String,System.String)"), Test]
        [Category(TestCategories.News)]
        public void NewsWidget_OnPageTemplate()
        {
            string templateName = "TemplateWithNewsWidget";
            string placeHolder = "Body";
            string url = UrlPath.ResolveAbsoluteUrl("~/" + UrlNamePrefix);
            
            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            mvcProxy.Settings = new ControllerSettings(newsController);

            Guid templateId = Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Templates().CreatePageTemplateReturnId(templateName);

            try
            {
                this.templateOperation.AddControlToTemplate(templateId, mvcProxy, placeHolder, CaptionNews);
                Guid pageId = this.locationGenerator.CreatePage(PageNamePrefix, PageTitlePrefix, UrlNamePrefix, null, null);
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Templates().SetTemplateToPage(pageId, templateId);
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreateNewsItem(NewsTitle);

                string responseContent = PageInvoker.ExecuteWebRequest(url);

                Assert.IsTrue(responseContent.Contains(NewsTitle), "The news with this title was not found!");
            }
            finally
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Pages().DeleteAllPages();
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Templates().DeletePageTemplate(templateId);
            }
        }

        /// <summary>
        /// News widget - add news widget to page template
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "FeatherWidgets.TestUtilities.CommonOperations.Templates.TemplateOperations.AddControlToTemplate(System.Guid,System.Web.UI.Control,System.String,System.String)"), Test]
        [Category(TestCategories.News)]
        public void NewsWidget_OnBootstrapPageTemplate()
        {
            string templateName = "defaultNew";
            string placeHolder = "Contentplaceholder1";
            string url = UrlPath.ResolveAbsoluteUrl("~/" + UrlNamePrefix);

            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            mvcProxy.Settings = new ControllerSettings(newsController);

            PageManager pageManager = PageManager.GetManager();
            int templatesCount = pageManager.GetTemplates().Count();

            var layoutTemplatePath = Path.Combine(this.templateOperation.SfPath, "ResourcePackages", "Bootstrap", "MVC", "Views", "Layouts", "default.cshtml");
            var newLayoutTemplatePath = Path.Combine(this.templateOperation.SfPath, "ResourcePackages", "Bootstrap", "MVC", "Views", "Layouts", "defaultNew.cshtml");

            File.Copy(layoutTemplatePath, newLayoutTemplatePath);

            this.templateOperation.WaitForTemplatesCountToIncrease(templatesCount, 1);

                Guid templateId = this.templateOperation.GetTemplateIdByTitle(templateName);

                try
                {
                    this.templateOperation.AddControlToTemplate(templateId, mvcProxy, placeHolder, CaptionNews);
                    Guid pageId = this.locationGenerator.CreatePage(PageNamePrefix, PageTitlePrefix, UrlNamePrefix, null, null);
                    Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Templates().SetTemplateToPage(pageId, templateId);
                    Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreateNewsItem(NewsTitle);

                    string responseContent = PageInvoker.ExecuteWebRequest(url);

                    Assert.IsTrue(responseContent.Contains(NewsTitle), "The news with this title was not found!");
                }
                finally
                {
                    this.templateOperation.GetTemplateIdByTitle(templateName);

                    Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Pages().DeleteAllPages();
                    Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Templates().DeletePageTemplate(templateId);

                    File.Delete(newLayoutTemplatePath);
                }
        }

        #region Fields and constants

        private const string NewsTitle = "Title";
        private const string PageNamePrefix = "NewsPage";
        private const string PageTitlePrefix = "News Page";
        private const string UrlNamePrefix = "news-page";
        private const string CaptionNews = "News";
        private TemplateOperations templateOperation;
        private PageContentGenerator locationGenerator;

        #endregion
    }
}
