﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatherWidgets.TestUtilities.CommonOperations;
using MbUnit.Framework;
using Telerik.Sitefinity.Frontend.Blogs.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Blogs.Mvc.Models.BlogPost;
using Telerik.Sitefinity.Frontend.DynamicContent.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.TestIntegration.Data.Content;
using Telerik.Sitefinity.TestUtilities.CommonOperations;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web;

namespace FeatherWidgets.TestIntegration.BlogPosts
{
    /// <summary>
    /// This is a class with Blog posts widget tests.
    /// </summary>
    [TestFixture]
    [Description("This is a class with Blog posts widget tests.")]
    public class BlogPostsWidgetTests
    {       
        [Test]
        [Category(TestCategories.Blogs)]
        [Author(FeatherTeams.Team2)]
        [Description("Adds Blog posts widget to a page and display posts from selected blogs only.")]
        public void BlogPostsWidget_DisplayPostsFromSelectedBlogsOnly()
        {
            string blog1Title = "Blog1";
            string blog2Title = "Blog2";
            string blog1PostTitle = "Blog1Post1";
            string blog2PostTitle = "Blog2Post1";
            string pageTitle = "PageWithBlogPostsWidget";

            Guid blog1Id = ServerOperations.Blogs().CreateBlog(blog1Title);
            ServerOperations.Blogs().CreatePublishedBlogPost(blog1PostTitle, blog1Id);

            Guid blog2Id = ServerOperations.Blogs().CreateBlog(blog2Title);
            ServerOperations.Blogs().CreatePublishedBlogPost(blog2PostTitle, blog2Id);

            Guid pageId = ServerOperations.Pages().CreatePage(pageTitle);

            try
            {
                var blogPostsWidget = this.CreateBlogPostsMvcWidget(blog1Id);

                var controls = new List<System.Web.UI.Control>();
                controls.Add(blogPostsWidget);

                PageContentGenerator.AddControlsToPage(pageId, controls);

                string url = UrlPath.ResolveAbsoluteUrl("~/" + pageTitle);
                string responseContent = PageInvoker.ExecuteWebRequest(url);

                Assert.IsTrue(responseContent.Contains(blog1PostTitle), "The item with this title was NOT found " + blog1PostTitle);
                Assert.IsFalse(responseContent.Contains(blog2PostTitle), "The item with this title WAS found " + blog2PostTitle);
            }
            finally
            {
                ServerOperations.Pages().DeleteAllPages();
                ServerOperations.Blogs().DeleteAllBlogs();
            }
        }

        private MvcWidgetProxy CreateBlogPostsMvcWidget(Guid selectedParentId)
        {
            var mvcProxy = new MvcWidgetProxy();
            mvcProxy.ControllerName = typeof(BlogPostController).FullName;
            var controller = new BlogPostController();

            controller.Model.ParentFilterMode = ParentFilterMode.Selected;
            controller.Model.SelectionMode = SelectionMode.AllItems;
            controller.Model.SerializedSelectedParentsIds = "[" + selectedParentId.ToString() + "]";

            mvcProxy.Settings = new ControllerSettings(controller);

            return mvcProxy;
        }
    }
}
