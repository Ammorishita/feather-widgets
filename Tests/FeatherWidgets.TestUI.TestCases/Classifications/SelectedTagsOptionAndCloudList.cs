﻿using System;
using Feather.Widgets.TestUI.Framework;
using Feather.Widgets.TestUI.Framework.Framework.Wrappers.Backend.Classifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUtilities;

namespace FeatherWidgets.TestUI.TestCases.Classifications
{
    /// <summary>
    /// SelectedTagsOptionAndCloudList test class.
    /// </summary>
    [TestClass]
    public class SelectedTagsOptionAndCloudList_ : FeatherTestCase
    {
        /// <summary>
        /// UI test SelectedTagsOptionAndCloudList
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.Team7),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.Classifications)]
        public void SelectedTagsOptionAndCloudList()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Classifications().TagsWrapper().SelectRadioButtonOption(TagsRadioButtonIds.selectedTags);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().ClickSelectButton();
            BATFeather.Wrappers().Backend().Widgets().SelectorsWrapper().SelectItemsInFlatSelector(TagTitle + 1, TagTitle + 2);
            BATFeather.Wrappers().Backend().Widgets().SelectorsWrapper().DoneSelecting();
            BATFeather.Wrappers().Backend().Classifications().TagsWrapper().SelectListTemplate("CloudList");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SaveChanges();
 
            for (int i = 1; i < 4; i++)
            {
                if (i > 0 && i <= 2)
                {
                    BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().CheckWidgetContent(WidgetName, TagTitle + i);
                }
                else
                {
                    BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().CheckWidgetContentForNotExistingContent(WidgetName, TagTitle + i);
                }
            }

            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());

            Assert.IsTrue(BATFeather.Wrappers().Frontend().Classifications().ClassificationsWrapper().IsTagsTitlesPresentOnThePageFrontend(new string[] { TagTitle + 2, TagTitle + 1 }));
            Assert.IsFalse(BATFeather.Wrappers().Frontend().Classifications().ClassificationsWrapper().IsTagsTitlesPresentOnThePageFrontend(new string[] { TagTitle + 3 }));
            BATFeather.Wrappers().Frontend().Classifications().ClassificationsWrapper().VerifCloudListStyle(2);
            BATFeather.Wrappers().Frontend().Classifications().ClassificationsWrapper().VerifCloudListStyle(4);
            BATFeather.Wrappers().Frontend().Classifications().ClassificationsWrapper().ClickTagsTitle(TagTitle + 2);
            Assert.IsTrue(BATFeather.Wrappers().Frontend().News().NewsWrapper().IsNewsTitlesPresentOnThePageFrontend(new string[] { NewsTitle + 2 }));
            Assert.IsTrue(BATFeather.Wrappers().Frontend().News().NewsWrapper().IsNewsTitlesPresentOnThePageFrontend(new string[] { NewsTitle + 3 }));
            Assert.IsFalse(BATFeather.Wrappers().Frontend().News().NewsWrapper().IsNewsTitlesPresentOnThePageFrontend(new string[] { NewsTitle + 1 }));
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

        private const string PageName = "TagsPage";
        private const string WidgetName = "Tags";
        private const string TagTitle = "Tag";
        private const string NewsTitle = "NewsTitle";
    }
}