﻿using System;
using System.Collections.Generic;
using System.Linq;
using FeatherWidgets.TestUtilities.CommonOperations;
using MbUnit.Framework;
using News.Mvc.Controllers;
using News.Mvc.Models;
using Telerik.Sitefinity.Modules.News;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Web;

namespace FeatherWidgets.TestIntegration.News
{
    /// <summary>
    /// This is a class with News tests.
    /// </summary>
    [TestFixture]
    [Description("This is a class with News tests for content settings.")]
    public class NewsWidgetContentTests
    {
        /// <summary>
        /// Set up method
        /// </summary>
        [SetUp]
        public void Setup()
        {
            this.pageOperations = new PagesOperations();
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().DeleteAllNews();
        }

        /// <summary>
        /// News widget - test content functionality - All news
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_VerifyAllNewsFunctionality()
        {
            string testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            string pageNamePrefix = testName + "NewsPage";
            string pageTitlePrefix = testName + "News Page";
            string urlNamePrefix = testName + "news-page";
            int index = 1;
            string url = UrlPath.ResolveAbsoluteUrl("~/" + urlNamePrefix + index);

            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            newsController.Model.SelectionMode = NewsSelectionMode.AllItems;
            mvcProxy.Settings = new ControllerSettings(newsController);

            try
            {
                this.pageOperations.CreatePageWithControl(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index);

                int newsCount = 5;

                for (int i = 1; i <= newsCount; i++)
                    Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreateNewsItem(NewsTitle + i);

                string responseContent = PageInvoker.ExecuteWebRequest(url);

                for (int i = 1; i <= newsCount; i++)
                    Assert.IsTrue(responseContent.Contains(NewsTitle + i), "The news with this title was not found!");
            }
            finally
            {
                this.pageOperations.DeletePages();
            }
        }

        /// <summary>
        /// News widget - test content functionality - All news
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_VerifySelectedItemsFunctionality()
        {
            int newsCount = 5;
         
            for (int i = 0; i < newsCount; i++)
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreatePublishedNewsItem(newsTitle: NewsTitle + i, newsContent: NewsTitle + i, author: NewsTitle + i);
            }

            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            newsController.Model.SelectionMode = NewsSelectionMode.SelectedItems;                   

            var newsManager = NewsManager.GetManager();
            var selectedNewsItem = newsManager.GetNewsItems().FirstOrDefault(n => n.Title == "Title2" && n.OriginalContentId != Guid.Empty);
            newsController.Model.SerializedSelectedItemsIds = "[\"" + selectedNewsItem.Id.ToString() + "\"]";
            mvcProxy.Settings = new ControllerSettings(newsController);

            newsController.Index(null);

            Assert.AreEqual(1, newsController.Model.Items.Count, "The count of news is not as expected");

            Assert.IsTrue(newsController.Model.Items[0].Title.Equals("Title2", StringComparison.CurrentCulture), "The news with this title was not found!");                          
        }

        /// <summary>
        /// News widget - test content functionality - All news
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_VerifySelectedItemsFunctionalityWithSortNewsDescending()
        {
            int newsCount = 10;
            string sortExpession = "Title DESC";
            string[] selectedNewsTitles = { "Title2", "Title7", "Title5" };
            var selectedNewsItems = new NewsItem[3];

            for (int i = 0; i < newsCount; i++)
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreatePublishedNewsItem(newsTitle: NewsTitle + i, newsContent: NewsTitle + i, author: NewsTitle + i);
            }

            var newsController = new NewsController();
            newsController.Model.SelectionMode = NewsSelectionMode.SelectedItems;
            newsController.Model.SortExpression = sortExpession;

            var newsManager = NewsManager.GetManager();

            for (int i = 0; i < selectedNewsTitles.Count(); i++)
            { 
                selectedNewsItems[i] = newsManager.GetNewsItems().FirstOrDefault(n => n.Title == selectedNewsTitles[i] && n.OriginalContentId != Guid.Empty);                           
            }

            //// SerializedSelectedItemsIds string should appear in the following format: "[\"ca782d6b-9e3d-6f9e-ae78-ff00006062c4\",\"66782d6b-9e3d-6f9e-ae78-ff00006062c4\"]"
            newsController.Model.SerializedSelectedItemsIds =
                                                             "[\"" + selectedNewsItems[0].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[1].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[2].Id.ToString() + "\"]";

            newsController.Index(null);

            Assert.AreEqual(3, newsController.Model.Items.Count, "The count of news is not as expected");

            for (int i = 0; i < newsController.Model.Items.Count; i++)
            {
                Assert.IsTrue(newsController.Model.Items[i].Title.Value.Equals(selectedNewsTitles[i]), "The news with this title was not found!");
            }

            newsController.Model.SelectionMode = NewsSelectionMode.AllItems;

            newsController.Index(null);

            int lastIndex = 9;
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(newsController.Model.Items[i].Title.Value.Equals(NewsTitle + lastIndex), "The news with this title was not found!");
                lastIndex--;
            }
        }

        /// <summary>
        /// News widget - test content functionality - All news
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_VerifySelectedItemsFunctionalityWithPaging()
        {
            string testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            string pageNamePrefix = testName + "NewsPage";
            string pageTitlePrefix = testName + "News Page";
            string urlNamePrefix = testName + "news-page";
            int index = 1;
            string index2 = "/2";
            string index3 = "/3";
            int itemsPerPage = 3;
            string url = UrlPath.ResolveAbsoluteUrl("~/" + urlNamePrefix + index);
            string url2 = UrlPath.ResolveAbsoluteUrl("~/" + urlNamePrefix + index + index2);
            string url3 = UrlPath.ResolveAbsoluteUrl("~/" + urlNamePrefix + index + index3);

            int newsCount = 20;
            string[] selectedNewsTitles = { "Title7", "Title15", "Title11", "Title3", "Title5", "Title8", "Title2", "Title16", "Title6" };
            var selectedNewsItems = new NewsItem[9];

            for (int i = 0; i < newsCount; i++)
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreatePublishedNewsItem(newsTitle: NewsTitle + i, newsContent: NewsTitle + i, author: NewsTitle + i);
            }

            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            newsController.Model.SelectionMode = NewsSelectionMode.SelectedItems;
            newsController.Model.ItemsPerPage = itemsPerPage;

            var newsManager = NewsManager.GetManager();

            for (int i = 0; i < selectedNewsTitles.Count(); i++)
            {
                selectedNewsItems[i] = newsManager.GetNewsItems().FirstOrDefault(n => n.Title == selectedNewsTitles[i] && n.OriginalContentId != Guid.Empty);
            }

            //// SerializedSelectedItemsIds string should appear in the following format: "[\"ca782d6b-9e3d-6f9e-ae78-ff00006062c4\",\"66782d6b-9e3d-6f9e-ae78-ff00006062c4\"]"
            newsController.Model.SerializedSelectedItemsIds =
                                                             "[\"" + selectedNewsItems[0].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[1].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[2].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[3].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[4].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[5].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[6].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[7].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[8].Id.ToString() + "\"]";

            mvcProxy.Settings = new ControllerSettings(newsController);

            this.VerifyCorrectNewsOnPages(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index, url, url2, url3, selectedNewsTitles);
        }

        /// <summary>
        /// News widget - test content functionality - All news
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_VerifySelectedItemsFunctionalityWithUseLimit()
        {
            string testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            string pageNamePrefix = testName + "NewsPage";
            string pageTitlePrefix = testName + "News Page";
            string urlNamePrefix = testName + "news-page";
            int index = 1;    
            string url = UrlPath.ResolveAbsoluteUrl("~/" + urlNamePrefix + index);

            int newsCount = 20;
            string[] selectedNewsTitles = { "Title7", "Title15", "Title11", "Title3", "Title5", "Title8", "Title2", "Title16", "Title6" };
            var selectedNewsItems = new NewsItem[9];

            for (int i = 0; i < newsCount; i++)
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreatePublishedNewsItem(newsTitle: NewsTitle + i, newsContent: NewsTitle + i, author: NewsTitle + i);
            }

            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            newsController.Model.SelectionMode = NewsSelectionMode.SelectedItems;
            newsController.Model.DisplayMode = ListDisplayMode.Limit;
            newsController.Model.ItemsPerPage = 5;

            var newsManager = NewsManager.GetManager();

            for (int i = 0; i < selectedNewsTitles.Count(); i++)
            {
                selectedNewsItems[i] = newsManager.GetNewsItems().FirstOrDefault(n => n.Title == selectedNewsTitles[i] && n.OriginalContentId != Guid.Empty);
            }

            //// SerializedSelectedItemsIds string should appear in the following format: "[\"ca782d6b-9e3d-6f9e-ae78-ff00006062c4\",\"66782d6b-9e3d-6f9e-ae78-ff00006062c4\"]"
            newsController.Model.SerializedSelectedItemsIds =
                                                             "[\"" + selectedNewsItems[0].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[1].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[2].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[3].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[4].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[5].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[6].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[7].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[8].Id.ToString() + "\"]";

            mvcProxy.Settings = new ControllerSettings(newsController);

            this.VerifyCorrectNewsOnPageWithUseLimitsOption(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index, url, selectedNewsTitles);
        }

        /// <summary>
        /// News widget - test content functionality - All news
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_VerifySelectedItemsFunctionalityWithNoLimit()
        {
            string testName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
            string pageNamePrefix = testName + "NewsPage";
            string pageTitlePrefix = testName + "News Page";
            string urlNamePrefix = testName + "news-page";
            int index = 1;
            string url = UrlPath.ResolveAbsoluteUrl("~/" + urlNamePrefix + index);

            int newsCount = 25;
            string[] selectedNewsTitles = { "Title7", "Title15", "Title11", "Title3", "Title5", "Title8", "Title2", "Title16", "Title6" };
            var selectedNewsItems = new NewsItem[9];
            string[] newsNames = new string[newsCount];

            for (int i = 0; i < newsCount; i++)
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreatePublishedNewsItem(newsTitle: NewsTitle + i, newsContent: NewsTitle + i, author: NewsTitle + i);
                newsNames[i] = NewsTitle + i;
            }

            var mvcProxy = new MvcControllerProxy();
            mvcProxy.ControllerName = typeof(NewsController).FullName;
            var newsController = new NewsController();
            newsController.Model.SelectionMode = NewsSelectionMode.SelectedItems;
            newsController.Model.DisplayMode = ListDisplayMode.All;

            var newsManager = NewsManager.GetManager();

            for (int i = 0; i < selectedNewsTitles.Count(); i++)
            {
                selectedNewsItems[i] = newsManager.GetNewsItems().FirstOrDefault(n => n.Title == selectedNewsTitles[i] && n.OriginalContentId != Guid.Empty);
            }

            //// SerializedSelectedItemsIds string should appear in the following format: "[\"ca782d6b-9e3d-6f9e-ae78-ff00006062c4\",\"66782d6b-9e3d-6f9e-ae78-ff00006062c4\"]"
            newsController.Model.SerializedSelectedItemsIds =
                                                             "[\"" + selectedNewsItems[0].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[1].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[2].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[3].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[4].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[5].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[6].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[7].Id.ToString() + "\"," +
                                                             "\"" + selectedNewsItems[8].Id.ToString() + "\"]";

            mvcProxy.Settings = new ControllerSettings(newsController);

            this.VerifyCorrectNewsOnPageWithNoLimitsOption(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index, url, selectedNewsTitles);

            newsController.Model.SelectionMode = NewsSelectionMode.AllItems;

            this.VerifyCorrectNewsOnPageWithNoLimitsOption(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index, url, newsNames);
        }

        /// <summary>
        /// News widget - test select by tag functionality 
        /// </summary>
        [Test]
        [Category(TestCategories.News)]
        [Author("FeatherTeam")]
        public void NewsWidget_SelectByTagNewsFunctionality()
        {
            int newsCount = 2;
            Guid[] taxonId = new Guid[newsCount];
            Guid[] newsId = new Guid[newsCount];
            string newsTitle = "News ";
            string tagTitle = "Tag ";
            var newsController = new NewsController();
            string[] tagTitles = new string[newsCount];

            try
            {
                for (int i = 0; i < newsCount; i++)
                {
                    taxonId[i] = Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Taxonomies().CreateFlatTaxon(Telerik.Sitefinity.TestUtilities.CommonOperations.TaxonomiesConstants.TagsTaxonomyId, tagTitle + i);
                    tagTitles[i] = tagTitle + i;
                    newsId[i] = Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().CreatePublishedNewsItem(newsTitle + i);
                    Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.News().AssignTaxonToNewsItem(newsId[i], "Tags", taxonId[i]);
                }

                for (int i = 0; i < newsCount; i++)
                {
                    ITaxon taxonomy = TaxonomyManager.GetManager().GetTaxon(taxonId[i]);
                    newsController.ListByTaxon(taxonomy, null);

                    for (int j = 0; j < newsController.Model.Items.Count; j++)
                        Assert.IsTrue(newsController.Model.Items[j].Title.Equals(newsTitle + i, StringComparison.CurrentCulture), "The news with this title was not found!");
                }
            }
            finally
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Taxonomies().DeleteTags(tagTitles);
            }
        }

        private void VerifyCorrectNewsOnPages(MvcControllerProxy mvcProxy, string pageNamePrefix, string pageTitlePrefix, string urlNamePrefix, int index, string url, string url2, string url3, string[] selectedNewsTitles)
        {
            try
            {
                this.pageOperations.CreatePageWithControl(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index);

                string responseContent = PageInvoker.ExecuteWebRequest(url);
                string responseContent2 = PageInvoker.ExecuteWebRequest(url2);
                string responseContent3 = PageInvoker.ExecuteWebRequest(url3);

                for (int i = 0; i < selectedNewsTitles.Count(); i++)
                {
                    if (i <= 2)
                    {
                        Assert.IsTrue(responseContent.Contains(selectedNewsTitles[i]), "The news with this title was not found!");
                        Assert.IsFalse(responseContent2.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                        Assert.IsFalse(responseContent3.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                    }
                    else if (i > 2 && i <= selectedNewsTitles.Count() - 4)
                    {
                        Assert.IsTrue(responseContent2.Contains(selectedNewsTitles[i]), "The news with this title was not found!");
                        Assert.IsFalse(responseContent.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                        Assert.IsFalse(responseContent3.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                    }
                    else
                    {
                        Assert.IsTrue(responseContent3.Contains(selectedNewsTitles[i]), "The news with this title was not found!");
                        Assert.IsFalse(responseContent.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                        Assert.IsFalse(responseContent2.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                    }
                }
            }
            finally
            {
                this.pageOperations.DeletePages();
            }
        }

        private void VerifyCorrectNewsOnPageWithUseLimitsOption(MvcControllerProxy mvcProxy, string pageNamePrefix, string pageTitlePrefix, string urlNamePrefix, int index, string url, string[] selectedNewsTitles)
        {
            try
            {
                this.pageOperations.CreatePageWithControl(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index);

                string responseContent = PageInvoker.ExecuteWebRequest(url);

                for (int i = 0; i < selectedNewsTitles.Count(); i++)
                {
                    if (i <= 4)
                    {
                        Assert.IsTrue(responseContent.Contains(selectedNewsTitles[i]), "The news with this title was not found!");
                    }
                    else
                    {
                        Assert.IsFalse(responseContent.Contains(selectedNewsTitles[i]), "The news with this title was found!");
                    }
                }
            }
            finally
            {
                this.pageOperations.DeletePages();
            }
        }

        private void VerifyCorrectNewsOnPageWithNoLimitsOption(MvcControllerProxy mvcProxy, string pageNamePrefix, string pageTitlePrefix, string urlNamePrefix, int index, string url, string[] selectedNewsTitles)
        {
            try
            {
                this.pageOperations.CreatePageWithControl(mvcProxy, pageNamePrefix, pageTitlePrefix, urlNamePrefix, index);

                string responseContent = PageInvoker.ExecuteWebRequest(url);

                for (int i = 0; i < selectedNewsTitles.Count(); i++)
                { 
                    Assert.IsTrue(responseContent.Contains(selectedNewsTitles[i]), "The news with this title was not found!");                 
                }
            }
            finally
            {
                this.pageOperations.DeletePages();
            }
        }
  
        #region Fields and constants

        private const string NewsTitle = "Title";
        private PagesOperations pageOperations;
        
        #endregion
    }
}