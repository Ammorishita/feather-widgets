﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using FeatherWidgets.TestUI.TestCases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Feather.Widgets.TestUI.Framework.Framework.Wrappers.Backend.ImageGallery;

namespace FeatherWidgets.TestUI.TestCases.ImageGallery
{
    /// <summary>
    /// SelectAllPublishedImagesWithLimitOption test class.
    /// </summary>
    [TestClass]
    public class SelectAllPublishedImagesWithLimitOption_ : FeatherTestCase
    {
        /// <summary>
        /// UI test SelectAllPublishedImagesWithLimitOption
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.Team7),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.ImageGallery)]
        public void SelectAllPublishedImagesWithLimitOption()
        {
            BAT.Macros().NavigateTo().Pages();
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().AddMvcWidgetHybridModePage(WidgetName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().VerifyCheckedRadioButtonOption(ImageGalleryRadioButtonIds.allPublished);
            BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().ExpandNarrowSelectionByArrow();
            BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().VerifyCheckedRadioButtonOption(ImageGalleryRadioButtonIds.allItems);

            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SwitchToListSettingsTab();
            BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().VerifyCheckedRadioButtonOption(ImageGalleryRadioButtonIds.usePaging);
            BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().SelectRadioButtonOption(ImageGalleryRadioButtonIds.useLimit);

            //// Can't find way to persist values and because of that the following line is commented
            //// BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().ChangePagingOrLimitValue("2", "Limit");
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SwitchToSingleItemSettingsTab();
            BATFeather.Wrappers().Backend().ImageGallery().ImageGalleryWrapper().VerifyCheckedRadioButtonOption(ImageGalleryRadioButtonIds.samePage);           
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SaveChanges();
            foreach (var image in this.imageTitles)
            {
                string src = this.GetImageSource(false, image, ImageType);
                BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().VerifyImageThumbnail(image, src);
            }

            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower());
            int i = 3;
            foreach (var image in this.imageTitles)
            {
                var src = this.GetImageSource(false, image, ImageType);
                BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().VerifyImage(ImageAltText + i, src);
                i--;
            }
            BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().ClickImage(ImageAltText + 2);
            Assert.IsTrue(BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().IsImageTitlePresentOnDetailMasterPage(this.imageTitles[1]));

            var scr = this.GetImageSource(false, this.imageTitles[1], ImageTypeFrontend);
            BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().VerifyImage(ImageAltText + 2, scr);

            var hrefPrevious = this.GetImageHref(true, this.imageTitles[0]);
            BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().VerifyPreviousImage(hrefPrevious);

            var hrefNext = this.GetImageHref(true, this.imageTitles[2]);
            BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().VerifyNextImage(hrefNext);

            var hrefBack = "/" + PageName.ToLower() + "/" + "Index/";
            BATFeather.Wrappers().Frontend().ImageGallery().ImageGalleryWrapper().VerifyBackToAllImages(hrefBack);
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

        private string GetImageSource(bool isBaseUrlIncluded, string imageName, string imageType)
        {
            string libraryUrl = LibraryName.ToLower();
            string imageUrl = imageName.ToLower() + imageType.ToLower();
            string scr = BATFeather.Wrappers().Frontend().CommonWrapper().GetImageSource(isBaseUrlIncluded, libraryUrl, imageUrl, this.BaseUrl);
            return scr;
        }

        private string GetImageHref(bool isBaseUrlIncluded, string imageName)
        {
            string libraryUrl = LibraryName.ToLower();
            string imageUrl = imageName.ToLower();
            string href = BATFeather.Wrappers().Frontend().CommonWrapper().GetImageSource(isBaseUrlIncluded, libraryUrl, imageUrl, this.BaseUrl, PageName.ToLower() + "/images");
            return href;
        }

        private const string PageName = "PageWithImage";
        private readonly string[] imageTitles = new string[] { "Image3", "Image2", "Image1" };
        private const string WidgetName = "Image gallery";
        private const string LibraryName = "TestImageLibrary";
        private const string ImageAltText = "AltText_Image";
        private const string ImageType = ".TMB";
        private const string ImageTypeFrontend = ".JPG";
    }
}