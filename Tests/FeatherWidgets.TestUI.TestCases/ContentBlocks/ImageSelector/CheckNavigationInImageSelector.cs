﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUtilities;

namespace FeatherWidgets.TestUI.TestCases.ContentBlocks.ImageSelector
{
    /// <summary>
    /// This is a test class for content block > image selector tests
    /// </summary>
    [TestClass]
    public class CheckNavigationInImageSelector_ : FeatherTestCase
    {     
        /// <summary>
        /// UI test CheckNavigationInImageSelector
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.Team7),
        TestCategory(FeatherTestCategories.ImageSelector),
        TestCategory(FeatherTestCategories.ContentBlock),
        TestCategory(FeatherTestCategories.PagesAndContent)]
        public void CheckNavigationInImageSelector()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
          
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().OpenImageSelector();
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifySelectedFilter(SelectedFilterName);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(3);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(0);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName1, ImageName2, ImageName3);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFilter(MyImages);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(2);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(0);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName1, ImageName2);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFilter(AllLibraries);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(0);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(2);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectFolders(DefaultLibrary, LibraryName);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFolder(LibraryName);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectFolders(ChildImageLibrary);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFolder(ChildImageLibrary);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName2);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectFolders(NextChildImageLibrary);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFolder(NextChildImageLibrary);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(0);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName3);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFolderFromBreadCrumb(ChildImageLibrary);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName2);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectFolders(NextChildImageLibrary);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectFolderFromSideBar(NextChildImageLibrary);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfImages(1);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectCountOfFolders(0);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().VerifyCorrectImages(ImageName3);

            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().SelectImage(ImageName3);
            BATFeather.Wrappers().Backend().Media().ImageSelectorWrapper().ConfirmImageSelection();
            Assert.IsTrue(BATFeather.Wrappers().Backend().Media().ImagePropertiesWrapper().IsImageTitlePopulated(ImageName3), "Image title is not populated correctly");
            Assert.IsTrue(BATFeather.Wrappers().Backend().Media().ImagePropertiesWrapper().IsImageAltTextPopulated(ImageAltText), "Image alt text is not populated correctly");
            BATFeather.Wrappers().Backend().Media().ImagePropertiesWrapper().ConfirmImageProperties();
        
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            BATFeather.Wrappers().Frontend().CommonWrapper().VerifyImage(ImageName3, ImageAltText, this.GetImageSource(false));
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

        private string GetImageSource(bool isBaseUrlIncluded)
        {
            string libraryUrl = LibraryName.ToLower() + "/" + ChildImageLibrary.ToLower() + "/" + NextChildImageLibrary.ToLower();
            string contentType = "images";
            string providerUrl = "default-source";
            string imageUrl = ImageName3.ToLower() + ImageType.ToLower();

            if (isBaseUrlIncluded)
            {
                string baseUrl = this.BaseUrl;

                return baseUrl + contentType + "/" + providerUrl + "/" + libraryUrl + "/" + imageUrl;
            }
            else
            {
                return "/" + contentType + "/" + providerUrl + "/" + libraryUrl + "/" + imageUrl;
            }
        }

        private const string PageName = "PageWithImage";
        private const string WidgetName = "ContentBlock";      
        private const string LibraryName = "TestImageLibrary";         
        private const string SelectedFilterName = "Recent Images";
        private const string ImageName1 = "Image1";
        private const string ImageName2 = "Image2";
        private const string ImageName3 = "Image3";
        private const string ImageAltText = "AltText Image3";
        private const string ImageType = ".JPG";
        private const string ChildImageLibrary = "ChildImageLibrary";
        private const string NextChildImageLibrary = "NextChildImageLibrary";
        private const string DefaultLibrary = "Default Library";
        private const string MyImages = "My Images";
        private const string AllLibraries = "All Libraries";
    }
}