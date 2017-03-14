﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatherWidgets.TestUI.TestCases.ContentBlocks.VideoSelector
{
    /// <summary>
    /// This is a test class for content block > video selector tests
    /// </summary>
    [TestClass]
    public class InsertVideoFromAlreadyUploaded_ : FeatherTestCase
    {
        /// <summary>
        /// UI test InsertVideoFromAlreadyUploaded
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam7),
        //TestCategory(FeatherTestCategories.MediaSelector),
        TestCategory(FeatherTestCategories.ContentBlock)]
        public void InsertVideoFromAlreadyUploaded()
        {
            BAT.Macros().NavigateTo().Pages(this.Culture);
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().OpenVideoSelector();           
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().PressCancelButton();

            // Uploading video after epmty screen is verified.
            string videoId = BAT.Arrange(this.TestName).ExecuteArrangement("UploadVideo").Result.Values["videoId"];

            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().OpenVideoSelector();

            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().VerifySelectedFilter(SelectedFilterName);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().VerifyMediaTooltip(VideoName, LibraryName, VideoType);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().SelectMediaFile(VideoName);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().ConfirmMediaFileSelection();
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifySmallVideoProperites(this.GetVideoSource());
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifySelectedOptionAspectRatioSelector("Auto");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().PlayVideo();
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifyBigVideoProperites(this.GetVideoSource());
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().SelectOptionAspectRatioSelector("4x3");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifyWidthAndHeightValues("600", "450");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().ConfirmMediaProperties();

            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper()
                      .VerifyContentBlockVideoDesignMode(this.GetVideoSource(), this.GetSfRef(videoId), "600", "450");
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorMediaWrapper().VerifyVideo(this.GetVideoSource(), this.Culture, Width, Height);
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), true, this.Culture);
            BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().VerifyVideo(this.GetVideoSource(), Width, Height);
        }

        /// <summary>
        /// Performs Server Setup and prepare the system with needed data.
        /// </summary>
        protected override void ServerSetup()
        {
            BAT.Macros().User().EnsureAdminLoggedIn();
            BAT.Arrange(this.TestName).ExecuteSetUp();
            currentProviderUrlName = BAT.Arrange(this.TestName).ExecuteArrangement("GetCurrentProviderUrlName").Result.Values["CurrentProviderUrlName"];
        }

        /// <summary>
        /// Performs clean up and clears all data created by the test.
        /// </summary>
        protected override void ServerCleanup()
        {
            BAT.Arrange(this.TestName).ExecuteTearDown();
        }

        private string GetSfRef(string videoId)
        {
            string provider = currentProviderUrlName;
            if (this.Culture == null)
            {
                provider = "OpenAccessDataProvider";
            }

            return "[videos|" + provider + "]" + videoId;
        }

        private string GetVideoSource()
        { 
            string libraryUrl = LibraryName.ToLower();
            string imageUrl = VideoName.ToLower() + VideoType.ToLower();
            string url;

            if (this.Culture == null)
            {
                url = this.BaseUrl;
            }
            else
            {
                url = ActiveBrowser.Url.Substring(0, 20);
            }

            string scr = BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().GetMediaSource(libraryUrl, imageUrl, "videos", currentProviderUrlName);
            return scr;
        }

        private const string PageName = "PageWithVideo";
        private const string WidgetName = "ContentBlock";
        private const string VideoName = "big_buck_bunny";
        private const string LibraryName = "TestVideoLibrary";
        private const string VideoType = ".MP4";
        private const string SelectedFilterName = "Recent Videos";
        private const int Width = 600;
        private const int Height = 450;
        private string currentProviderUrlName;
        private const string SecondProviderName = "SecondSite Libraries";
    }
}
