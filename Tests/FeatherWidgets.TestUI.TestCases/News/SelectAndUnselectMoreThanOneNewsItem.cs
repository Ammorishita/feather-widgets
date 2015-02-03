﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using FeatherWidgets.TestUI.TestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatherWidgets.TestUI
{
    /// <summary>
    /// This is test class for SelectAndUnselectMoreThanOneDynamicWidgetItem.
    /// </summary>
    [TestClass]
    public class SelectAndUnselectMoreThanOneNewsItem_ : FeatherTestCase
    {
        /// <summary>
        /// UI test SelectMoreThanOneNewsItem.
        /// </summary>
        [TestMethod,
        Owner("Sitefinity Team 7"),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.NewsSelectors),
        Ignore]
        public void SelectMoreThanOneNewsItem()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectWhichItemsToDisplay(WhichNewsToDisplay);

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().ClickSelectButton();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(20);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInFlatSelector(this.selectedNewsNames);
            var countOfSelectedItems = this.selectedNewsNames.Count();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SearchItemByTitle("Title15");

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(1);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInFlatSelector(SelectedNewsName15);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems + 1);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().OpenSelectedTab();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(5);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().DoneSelecting();

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().VerifySelectedItemsFromFlatSelector(this.newSelectedNewsNames1);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SaveChanges();

            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            Assert.IsTrue(BATFeather.Wrappers().Frontend().News().NewsWrapper().IsNewsTitlesPresentOnThePageFrontend(this.newSelectedNewsNames1));           
        }

        /// <summary>
        /// UI test UnselectNewsItemInTheSelector.
        /// </summary>
        [TestMethod,
        Microsoft.VisualStudio.TestTools.UnitTesting.Owner("Sitefinity Team 7"),
        TestCategory(FeatherTestCategories.PagesAndContent)]
        public void UnselectNewsItemInTheSelector()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectWhichItemsToDisplay(WhichNewsToDisplay);

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().ClickSelectButton();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(20);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInFlatSelector(this.newSelectedNewsNames1);
            var countOfSelectedItems = this.newSelectedNewsNames1.Count();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().DoneSelecting();

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().VerifySelectedItemsFromFlatSelector(this.newSelectedNewsNames1);

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().ClickSelectButton();

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInFlatSelector("News Item Title5");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems - 1);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().OpenAllTab();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SearchItemByTitle(SelectedNewsName15);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().WaitForItemsToAppear(1);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SelectItemsInFlatSelector(SelectedNewsName15);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().OpenSelectedTab();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().CheckNotificationInSelectedTab(countOfSelectedItems - 2);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().DoneSelecting();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().VerifySelectedItemsFromFlatSelector(this.newSelectedNewsNames2);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerContentScreenWrapper().SaveChanges();
            foreach (var newsTitle in this.newSelectedNewsNames2)
            {
                BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().CheckWidgetContent(WidgetName, newsTitle);
            }

            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            Assert.IsTrue(BATFeather.Wrappers().Frontend().News().NewsWrapper().IsNewsTitlesPresentOnThePageFrontend(this.newSelectedNewsNames2));    
        }

        /// <summary>
        /// Performs Server Setup and prepare the system with needed data.
        /// </summary>
        protected override void ServerSetup()
        {
            BAT.Macros().User().EnsureAdminLoggedIn();
            BAT.Arrange(ArrangementClassName).ExecuteSetUp();
        }

        /// <summary>
        /// Performs clean up and clears all data created by the test.
        /// </summary>
        protected override void ServerCleanup()
        {
            BAT.Arrange(ArrangementClassName).ExecuteTearDown();
        }

        private const string ArrangementClassName = "SelectAndUnselectMoreThanOneNewsItem";
        private const string PageName = "News";
        private const string WhichNewsToDisplay = "Selected news";
        private const string WidgetName = "News";
        private const string SelectedNewsName15 = "News Item Title15";

        private readonly string[] selectedNewsNames = { "News Item Title1", "News Item Title5", "News Item Title6", "News Item Title12" };
        private readonly string[] newSelectedNewsNames1 = { "News Item Title1", "News Item Title5", "News Item Title6", "News Item Title12", "News Item Title15" };
        private readonly string[] newSelectedNewsNames2 = { "News Item Title1", "News Item Title6", "News Item Title12" };
    }
}