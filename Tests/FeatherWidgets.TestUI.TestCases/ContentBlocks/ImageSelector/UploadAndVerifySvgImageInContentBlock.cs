﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatherWidgets.TestUI.TestCases.ContentBlocks.ImageSelector
{
    /// <summary>
    /// Upload and verify svg in MVC content block and in frontend 
    /// </summary>
    [TestClass]
    public class UploadAndVerifySvgImageInContentBlock_ : FeatherTestCase
    {
         /// <summary>
        /// UI test 
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam8),
            //TestCategory(FeatherTestCategories.MediaSelector),
        TestCategory(FeatherTestCategories.ContentBlock)]
        public void UploadAndVerifySvgImageInContentBlock()
        {
            BAT.Macros().NavigateTo().Pages(this.Culture);
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);

            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().OpenImageSelector();
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().SwitchToUploadMode();
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().WaitForContentToBeLoaded(true);
            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().SelectMediaFileFromYourComputer();
            string fullImagesPath = DeploymentDirectory + @"\";
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().PerformSingleFileUpload(FileToUpload, fullImagesPath);

            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().WaitForContentToBeLoaded();
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().CancelUpload();

            BATFeather.Wrappers().Backend().Media().MediaSelectorWrapper().SelectMediaFileFromYourComputer();
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().PerformSingleFileUpload(FileToUpload, fullImagesPath);

            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().WaitForContentToBeLoaded();
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().VerifyMediaToUploadSection(FileToUpload, "3 KB");
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().ClickSelectLibraryButton();
            BATFeather.Wrappers().Backend().Widgets().SelectorsWrapper().SelectItemsInHierarchicalSelector(ChildImageLibrary);
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().DoneSelecting();
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().VerifySelectedLibrary(LibraryName + " > " + ChildImageLibrary);
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().IsMediaFileTitlePopulated(ImageName);
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().EnterTitle(NewImageName);
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().EnterImageAltText(NewImageAltText);
            BATFeather.Wrappers().Backend().Media().MediaUploadPropertiesWrapper().UploadMediaFile();

            BATFeather.Wrappers().Backend().Media().ImagePropertiesWrapper().VerifyImageThumbnailOptionsForSVG(2);
            BATFeather.Wrappers().Backend().Media().ImagePropertiesWrapper().ConfirmMediaProperties();
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), false, this.Culture);
            var scr = this.GetImageSource(ImageName, ImageType);
            BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().VerifyImage(NewImageName, NewImageAltText, scr);
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

        /// <summary>
        /// Get image source
        /// </summary>
        /// <param name="imageName">Image name</param>
        /// <param name="imageType">Image type</param>
        /// <returns>Returns image src</returns>
        private string GetImageSource(string imageName, string imageType)
        {
            currentProviderUrlName = BAT.Arrange(this.TestName).ExecuteArrangement("GetCurrentProviderUrlName").Result.Values["CurrentProviderUrlName"];
            string libraryUrl = LibraryName.ToLower() + "/" + ChildImageLibrary.ToLower();
            string imageUrl = imageName.ToLower() + imageType.ToLower();
            string url;

            if (this.Culture == null)
            {
                url = this.BaseUrl;
            }
            else
            {
                url = ActiveBrowser.Url.Substring(0, 20);
            }

            string scr = BATFeather.Wrappers().Frontend().MediaWidgets().MediaWidgetsWrapper().GetMediaSource(libraryUrl, imageUrl, "images", currentProviderUrlName);
            return scr;
        }

        private const string PageName = "PageWithSvgImage";
        private const string WidgetName = "ContentBlock";
        private const string LibraryName = "TestImageLibrary";
        private const string ChildImageLibrary = "ChildImageLibrary";
        private const string FileToUpload = "kiwi.svg";
        private const string ImageName = "kiwi";
        private const string ImageType = ".svg";
        private const string NewImageName = "ImageSvgTitleEdited";
        private const string NewImageAltText = "ImageSvgAltTextEdited";
        private string currentProviderUrlName;
    }
}
