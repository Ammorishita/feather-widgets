﻿using Feather.Widgets.TestUI.Framework;
using FeatherWidgets.TestUI.TestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatherWidgets.TestUI
{
    /// <summary>
    /// SearchAndSelectDynamicItemsByCategorySelectedTab test class.
    /// </summary>
    [TestClass]
    public class SearchAndSelectDynamicItemsByCategorySelectedTab_ : FeatherTestCase
    {
        /// <summary>
        /// UI test SearchAndSelectDynamicItemsByCategorySelectedTab
        /// </summary>
        [TestMethod,
        Owner("Sitefinity Team 7"),
        TestCategory(FeatherTestCategories.DynamicWidgets)]
        public void SearchAndSelectDynamicItemsByCategorySelectedTab()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectWhichItemsToDisplay(WhichNewsToDisplay);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectCheckBox(TaxonomyName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().ClickSelectButton();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(2);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInHierarchicalSelector(selectedTaxonTitles);
            var countOfSelectedItems = selectedTaxonTitles.Count();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().OpenSelectedTab();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(4);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SearchItemByTitle("An");

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(2);
   
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInFlatSelector(FullName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems - 1);

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().DoneSelecting();

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().VerifySelectedItemsFromHierarchicalSelector(new[] { "Category0 > Category1", "Category0 > Category1 > Category2 > Category3 > Category4 > Category5", "AnotherCategory0 > AnotherCategory1 > AnotherCategory2" });

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SaveChanges();
            foreach (var itemsTitle in itemsTitles)
            {
                BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().CheckWidgetContent(WidgetName, itemsTitle);
            }
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            this.VerifyNewsOnTheFrontend();
        }

        /// <summary>
        /// Verify news widget on the frontend
        /// </summary>
        public void VerifyNewsOnTheFrontend()
        {
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            ActiveBrowser.WaitUntilReady();
            BATFeather.Wrappers().Frontend().News().NewsWrapper().VerifyNewsTitlesOnThePageFrontend(this.itemsTitles);
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
        private readonly string[] selectedTaxonTitles = { "Category1", "Category5", "AnotherCategory2", "AnotherCategory5" };
        private readonly string[] itemsTitles = new string[] { "Title8", "Title5", "Title1" };
        private const string WhichNewsToDisplay = "Narrow selection by...";
        private const string TaxonomyName = "Category";
        private const string FullName = "AnotherCategory0 > ... > AnotherCategory4 > AnotherCategory5";
    }
}
