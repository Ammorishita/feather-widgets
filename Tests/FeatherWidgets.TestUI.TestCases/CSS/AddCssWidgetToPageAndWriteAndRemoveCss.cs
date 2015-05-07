﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUtilities;

namespace FeatherWidgets.TestUI.TestCases.CSS
{
    /// <summary>
    /// AddCssWidgetToPageAndWriteAndRemoveCss test class.
    /// </summary>
    [TestClass]
    public class AddCssWidgetToPageAndWriteAndRemoveCss_ : FeatherTestCase
    {
        /// <summary>
        /// UI test AddCssWidgetToPageAndWriteAndRemoveCss
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.Team2),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.Css)]
        public void AddCssWidgetToPageAndWriteAndRemoveCss()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().AddWidgetToPlaceHolderPureMvcMode(WidgetName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Css().CssWidgetEditWrapper().FillCssToCssWidget(CssValue);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            this.VerifyPageOnTheFrontend(CssValue);

            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Css().CssWidgetEditWrapper().FillCssToCssWidget(string.Empty);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            this.VerifyPageOnTheFrontend(string.Empty);
        }

        /// <summary>
        /// Verify css on the frontend
        /// </summary>
        public void VerifyPageOnTheFrontend(string css)
        {
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            bool isContained = BATFeather.Wrappers().Frontend().Css().CssWrapper().IsStylePresentOnFrontend(css);
            Assert.IsTrue(isContained, string.Concat("Expected ", css, " but the style is not found"));
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

        private const string PageName = "PageWithCssWidget";
        private const string ContentBlockContent = "Test content";
        private const string WidgetName = "CSS";
        private const string CssValue = "div { color: #FF0000; font-size: 20px;} ";
    }
}
