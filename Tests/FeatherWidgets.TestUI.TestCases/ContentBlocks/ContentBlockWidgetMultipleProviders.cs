﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using Feather.Widgets.TestUI.Framework;
using FeatherWidgets.TestUI.TestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.WebAii.Controls;

namespace FeatherWidgets.TestUI
{
    /// <summary>
    /// ContentBlockWidgetMultipleProviders test class.
    /// </summary>
    [TestClass]
    public class ContentBlockWidgetMultipleProviders_ : FeatherTestCase
    {
        /// <summary>
        /// UI test ContentBlockWidgetMultipleProviders
        /// </summary>
        [TestMethod,
       Microsoft.VisualStudio.TestTools.UnitTesting.Owner("Feather team"),
       TestCategory(FeatherTestCategories.PagesAndContent)]
        public void ContentBlockWidgetMultipleProviders()
        {
            this.AddProviderToTheSiteInMultitenancy();
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().SelectExtraOptionForWidget(OperationName);
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SelectContentBlockInProvider(SecondProviderName, ContentBlockName);
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
        
            this.NavigatePageOnTheFrontend(PageName);
            BATFeather.Wrappers().Frontend().ContentBlock().ContentBlockWrapper().VerifyContentOfContentBlockOnThePageFrontend(ContentBlockContent);
        }

        private void AddProviderToTheSiteInMultitenancy()
        {
            string elementId = "_configureModulesView_change_Telerik_Sitefinity_Modules_GenericContent_ContentManager";
            BAT.Wrappers().Backend().Multisite().MultisiteWrapper().AddProviderToSite(elementId, SecondProviderName, Manager.Current.Log);
        }

        /// <summary>
        /// Navigate page on the front end
        /// </summary>
        /// <param name="pageName">Page name</param>
        private void NavigatePageOnTheFrontend(string pageName)
        {
            BAT.Macros().NavigateTo().CustomPage("~/" + pageName.ToLower());
            ActiveBrowser.WaitUntilReady();
        }

        /// <summary>
        /// Performs Server Setup and prepare the system with needed data.
        /// </summary>
        protected override void ServerSetup()
        {
            BAT.Macros().User().EnsureAdminLoggedIn();
            BAT.Arrange(this.TestName).ExecuteSetUp();
        }

        /// <summary>
        /// Performs clean up and clears all data created by the test.
        /// </summary>
        protected override void ServerCleanup()
        {
            BAT.Arrange(this.TestName).ExecuteTearDown();
        }

        private const string PageName = "ContentBlock";
        private const string ContentBlockName = "Content Block 2";
        private const string ContentBlockContent = "Content 2";
        private const string WidgetName = "ContentBlock";
        private const string SecondProviderName = "ContentSecondDataProvider";
        private const string OperationName = "Use shared";
    }
}
