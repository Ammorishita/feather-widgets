﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.Common.UnitTesting;

namespace Feather.Widgets.TestUI.Framework.Framework.Wrappers.Frontend.CommentsAndReviews
{
    /// <summary>
    /// This is the entry point class for comments on the frontend.
    /// </summary>
    public class CommentsWrapper : BaseWrapper
    {
        /// <summary>
        /// Asserts that Leave a comment message is present.
        /// </summary>
        public void AssertLeaveACommentMessage()
        {
            this.EM.CommentsAndReviews.CommentsFrontend.CommentsHeader.AssertIsPresent("Leave a comment message");
        }

        /// <summary>
        /// Asserts comments count
        /// </summary>
        public void AssertCommentsCount(string commentCount)
        {
            HtmlAnchor commentLink = this.EM.CommentsAndReviews.CommentsFrontend.LeaveAComment.AssertIsPresent("Comments count link");
            bool isPresent = commentLink.InnerText.Contains(commentCount);
            Assert.IsTrue(isPresent);
        }

        /// <summary>
        /// Click comment link
        /// </summary>
        public void ClickCommentLink()
        {
            HtmlAnchor commentLink = this.EM.CommentsAndReviews.CommentsFrontend.LeaveAComment
                .AssertIsPresent("Comments count link");

            commentLink.Wait.ForVisible();
            commentLink.ScrollToVisible();
            commentLink.MouseClick();
            ActiveBrowser.WaitUntilReady();
        }

        /// <summary>
        /// Type a comment.
        /// </summary>
        /// <param name="commentBody">Comment body.</param>
        public void TypeAComment(string commentBody)
        {
            HtmlDiv editable = this.EM.CommentsAndReviews.CommentsFrontend.
                                       LeaveACommentArea
                                       .AssertIsPresent("Leave acomment area");

            editable.ScrollToVisible();
            editable.Focus();
            editable.MouseClick();

            Manager.Current.Desktop.KeyBoard.KeyDown(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.A);
            Manager.Current.Desktop.KeyBoard.KeyUp(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.Delete);

            Manager.Current.Desktop.KeyBoard.TypeText(commentBody);
        }

        /// <summary>
        /// Click submit button
        /// </summary>
        public void ClickSubmitButton()
        {
            HtmlButton submitButton = this.EM.CommentsAndReviews.CommentsFrontend.SubmitButton
            .AssertIsPresent("Submit button");
            submitButton.Click();
            ActiveBrowser.WaitUntilReady();
        }

        /// <summary>
        /// Verify comments author and content
        /// </summary>
        /// <param name="commentsAuthor">Comments author</param>
        /// <param name="commentsContent">Comments content</param>
        public void VerifyCommentsAuthorAndContent(string[] commentsAuthor, string[] commentsContent)
        {
            IList<HtmlDiv> allCommentsDivs = this.EM.CommentsAndReviews.CommentsFrontend.ResultsCommentsList;

            Assert.IsNotNull(allCommentsDivs, "Comments list is null");
            Assert.AreNotEqual(0, allCommentsDivs.Count, "Comments list has no elements");

            Assert.AreEqual(commentsAuthor.Count(), allCommentsDivs.Count, "Expected and actual count of comments are not equal");

            for (int i = 0; i < allCommentsDivs.Count(); i++)
            {
                Assert.AreEqual(commentsAuthor[i], allCommentsDivs[i].ChildNodes[0].InnerText);
                Assert.AreEqual(commentsContent[i], allCommentsDivs[i].ChildNodes[2].InnerText);
            }
        }
    }
}
