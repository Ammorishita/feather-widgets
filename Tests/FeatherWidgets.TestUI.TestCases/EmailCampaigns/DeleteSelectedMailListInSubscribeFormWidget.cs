﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUtilities;

namespace FeatherWidgets.TestUI.TestCases.EmailCampaigns
{
    /// <summary>
    /// DeleteSelectedMailListInSubscribeFormWidget test class.
    /// </summary>
    [TestClass]
    public class DeleteSelectedMailListInSubscribeFormWidget_ : FeatherTestCase
    {
        /// <summary>
        /// UI test DeleteSelectedMailListInSubscribeFormWidget
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.FeatherTeam),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.EmailCampaigns),
        TestCategory(FeatherTestCategories.Bootstrap)]
        public void DeleteSelectedMailListInSubscribeFormWidget()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().ClickSelectButton();
            BATFeather.Wrappers().Backend().EmailCampaigns().SubscribeFormWrapper().SelectItemsInFlatSelector(MailingList);
            BATFeather.Wrappers().Backend().Widgets().SelectorsWrapper().DoneSelecting();
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false);
            BATFeather.Wrappers().Frontend().EmailCampaigns().SubscibeFormWrapper().VerifySubscribeMessageOnTheFrontend();
            BAT.Arrange(this.TestName).ExecuteArrangement("DeleteMailingList");

            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().EmailCampaigns().SubscribeFormWrapper().VerifyErrorMessageForDeletedMailList();
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().ClickSelectButton();
            BATFeather.Wrappers().Backend().EmailCampaigns().SubscribeFormWrapper().SelectItemsInFlatSelector(SecondMailingList);
            BATFeather.Wrappers().Backend().Widgets().SelectorsWrapper().DoneSelecting();
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false); 
            BATFeather.Wrappers().Frontend().EmailCampaigns().SubscibeFormWrapper().VerifySubscribeMessageOnTheFrontend();
            BATFeather.Wrappers().Frontend().EmailCampaigns().SubscibeFormWrapper().FillEmail(SubscriberEmail);
            BATFeather.Wrappers().Frontend().EmailCampaigns().SubscibeFormWrapper().ClickSubscribeButton();
            BATFeather.Wrappers().Frontend().EmailCampaigns().SubscibeFormWrapper().VerifySuccessfullySubscribeMessageOnTheFrontend(SubscriberEmail);
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

        private const string PageName = "SubscribeFormPage";
        private const string PageTemplateName = "Bootstrap.default";
        private const string MailingList = "MailList";
        private const string SecondMailingList = "SecondMailList";
        private const string SubscriberEmail = "testNew@email.com";
        private const string WidgetName = "Subscribe form";
    }
}
