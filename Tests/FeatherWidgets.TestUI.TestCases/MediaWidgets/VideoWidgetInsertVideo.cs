﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatherWidgets.TestUI.TestCases.MediaWidgets
{
    /// <summary>
    /// This is a test class for VideoWidgetInsertVideo tests
    /// </summary>
    [TestClass]
    public class VideoWidgetInsertVideo_ : FeatherTestCase
    {
        /// <summary>
        /// UI test VideoWidgetInsertVideo
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam2),
        TestCategory(FeatherTestCategories.MediaSelector),
        TestCategory(FeatherTestCategories.PagesAndContent)]
        public void VideoWidgetInsertVideo()
        {
            BAT.Macros().NavigateTo().Pages(this.Culture);
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().AddMvcWidgetHybridModePage(WidgetName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName, 0, true);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().WaitForContentToBeLoaded(false);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().VerifySelectedFilter(SelectedFilterName);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().VerifyMediaTooltip(VideoName, LibraryName, VideoType);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().SelectMediaFile(VideoName);            
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().PressCancelButton();
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName, 0, true);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().WaitForContentToBeLoaded(false);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().VerifySelectedFilter(SelectedFilterName);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().VerifyMediaTooltip(VideoName, LibraryName, VideoType);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().SelectMediaFile(VideoName);       
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().ConfirmMediaFileSelectionInWidget();

            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifySmallVideoProperites(this.GetVideoSource());
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifySelectedOptionAspectRatioSelector("Auto");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().PlayVideo();
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifyBigVideoProperites(this.GetVideoSource());
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().SelectOptionAspectRatioSelector("4x3");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifyWidthAndHeightValues("600", "450");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().VerifyTemplateDropdownValueInWidget("Default");
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().ApplyCssClasses(CssClassesToApply);
            BATFeather.Wrappers().Backend().Media().VideoPropertiesWrapper().ConfirmMediaPropertiesInWidget();

            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), true, this.Culture);
            BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().VerifyVideo(this.GetVideoSource(), Width, Height);
            BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().VerifyVideoCssClass(CssClassesToApply, this.GetVideoSource());
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

        private string GetVideoSource()
        {
            string libraryUrl = LibraryName.ToLower();
            string videoUrl = VideoName.ToLower() + VideoType.ToLower();

            string url;

            if (this.Culture == null)
            {
                url = this.BaseUrl;
            }
            else
            {
                url = ActiveBrowser.Url.Substring(0, 20);
            }

            string scr = BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().GetMediaSource(libraryUrl, videoUrl, "videos", currentProviderUrlName);
            return scr;
        }

        private const string PageName = "PageWithVideo";
        private const string WidgetName = "Video";
        private const string LibraryName = "TestVideoLibrary";
        private const string VideoName = "big_buck_bunny2";
        private const string VideoType = ".MP4";
        private const string SelectedFilterName = "Recent Videos";
        private const int Width = 600;
        private const int Height = 450;
        private const string CssClassesToApply = "testCssClass";
        private string currentProviderUrlName;
    }
}
