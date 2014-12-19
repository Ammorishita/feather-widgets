﻿using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatherWidgets.TestUI.TestCases.DynamicWidgets
{
    [TestClass]
    public class InvalidateCacheWhenAddNewItem_ : FeatherTestCase
    {
        /// <summary>
        /// UI test InvalidateCacheWhenAddNewItem
        /// </summary>
        [TestMethod,
       Microsoft.VisualStudio.TestTools.UnitTesting.Owner("Feather team"),
       TestCategory(FeatherTestCategories.DynamicWidgets)]
        public void InvalidateCacheWhenAddNewItem()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().AddWidgetToDropZone(WidgetName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().ModuleBuilder().DynamicWidgetAdvancedSettingsWrapper().ClickAdvancedSettingsButton();
            BATFeather.Wrappers().Backend().ModuleBuilder().DynamicWidgetAdvancedSettingsWrapper().ClickModelButton();
            BATFeather.Wrappers().Backend().ModuleBuilder().DynamicWidgetAdvancedSettingsWrapper().SetItemsPerPage(ItemsPerPage);
            BATFeather.Wrappers().Backend().ModuleBuilder().DynamicWidgetAdvancedSettingsWrapper().SetSortExpression(SortExpression);
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            BAT.Macros().User().LogOut();
            this.NavigatePageOnTheFrontend(this.dynamicTitles, this.dynamicTitlesSecondPage);   
            BAT.Macros().User().EnsureAdminLoggedIn();
            BAT.Arrange(this.TestName).ExecuteArrangement("AddNewItem");
            BAT.Macros().User().LogOut();
            this.NavigatePageOnTheFrontend(this.dynamicTitles2, this.dynamicTitles2Second);
            BAT.Macros().User().EnsureAdminLoggedIn();
        }

        /// <summary>
        /// Navigate page on the frontend
        /// </summary>
        public void NavigatePageOnTheFrontend(string[] dynamicTitle, string[] secondPageTitles)
        {
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            ActiveBrowser.WaitUntilReady();
            BATFeather.Wrappers().Frontend().ModuleBuilder().ModuleBuilderWrapper().VerifyDynamicTitlePresentOnTheFrontend(dynamicTitle);
            BATFeather.Wrappers().Frontend().ModuleBuilder().ModuleBuilderWrapper().NavigateToPage(Page);
            BATFeather.Wrappers().Frontend().ModuleBuilder().ModuleBuilderWrapper().VerifyDynamicTitlePresentOnTheFrontend(secondPageTitles);
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

        private const string PageName = "TestPage";
        private const string WidgetName = "Press Articles MVC";
        private const string ItemsPerPage = "3";
        private const string Page = "2";
        private const string SortExpression = "Title ASC";
        private string[] dynamicTitles = { "Boat", "Cat", "Dog" };
        private string[] dynamicTitlesSecondPage = { "Elephant" };
        private string[] dynamicTitles2 = { "Angel", "Boat", "Cat"};
        private string[] dynamicTitles2Second = { "Dog", "Elephant" };
    }
}
