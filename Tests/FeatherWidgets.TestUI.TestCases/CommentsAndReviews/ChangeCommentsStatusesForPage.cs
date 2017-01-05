﻿using System;
using System.Linq;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatherWidgets.TestUI.TestCases.CommentsAndReviews
{
    /// <summary>
    /// ChangeCommentsStatusesForPage test class.
    /// </summary>
    [TestClass]
    public class ChangeCommentsStatusesForPage_ : FeatherTestCase
    {
        /// <summary>
        /// UI test ChangeCommentsStatusesForPage
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam2),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.CommentsAndReviews),
        TestCategory(FeatherTestCategories.Bootstrap)]
        public void ChangeCommentsStatusesForPage()
        {
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false, this.Culture);
            BATFeather.Wrappers().Frontend().CommentsAndReviews().CommentsWrapper().AssertMessageAndCountOnPage(CommentsMessage);
            this.PublishCommentAndVerifyFrontEnd();
            
            BAT.Arrange(this.TestName).ExecuteArrangement("MarkAsSpamComment");
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false, this.Culture);
            BATFeather.Wrappers().Frontend().CommentsAndReviews().CommentsWrapper().AssertMessageAndCountOnPage(CommentsMessage);

            this.PublishCommentAndVerifyFrontEnd();

            BAT.Arrange(this.TestName).ExecuteArrangement("HideComment");
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false, this.Culture);
            BATFeather.Wrappers().Frontend().CommentsAndReviews().CommentsWrapper().AssertMessageAndCountOnPage(CommentsMessage);

            this.PublishCommentAndVerifyFrontEnd();
        }

        public void PublishCommentAndVerifyFrontEnd()
        {
            BAT.Arrange(this.TestName).ExecuteArrangement("PublishComment");
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false, this.Culture);
            BATFeather.Wrappers().Frontend().CommentsAndReviews().CommentsWrapper().AssertMessageAndCountOnPage(CommentsCount);
            BATFeather.Wrappers().Frontend().CommentsAndReviews().CommentsWrapper().VerifyCommentsAuthorAndContent(this.commentAuthor, this.commentToPage);
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

        private const string PageName = "CommentsPage";
        private const string LeaveAComment = "Leave a comment";
        private string[] commentToPage = { "Comment to page waiting for approval comment" };
        private string[] commentAuthor = { FeatherTestCase.AdminNickname };
        private const string CommentsCount = "1comment";
        private const string CommentsMessage = "Leave a comment";
    }
}
