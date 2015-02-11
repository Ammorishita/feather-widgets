﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.Common.UnitTesting;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;

namespace Feather.Widgets.TestUI.Framework.Framework.Wrappers.Backend
{
    /// <summary>
    /// This is the entry point class for content block edit wrapper.
    /// </summary>
    public class ContentBlockWidgetEditWrapper : BaseWrapper
    {
        /// <summary>
        /// Fill content to the content block widget
        /// </summary>
        /// <param name="content">The content value</param>
        public void FillContentToContentBlockWidget(string content)
        {
            HtmlTableCell editable = EM.GenericContent.ContentBlockWidget.EditableArea
                .AssertIsPresent("Editable area");

            editable.ScrollToVisible();
            editable.Focus();
            editable.MouseClick();

            Manager.Current.Desktop.KeyBoard.TypeText(content);
        }

        /// <summary>
        /// Selects the content in editable area.
        /// </summary>
        public void SelectContentInEditableArea()
        {
            HtmlTableCell editable = EM.GenericContent.ContentBlockWidget.EditableArea
                .AssertIsPresent("Editable area");
            editable.ScrollToVisible();
            editable.Focus();
            editable.MouseClick();

            Manager.Current.Desktop.KeyBoard.KeyDown(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.A);
            Manager.Current.Desktop.KeyBoard.KeyUp(System.Windows.Forms.Keys.Control);
        }

        /// <summary>
        /// Verifies the content in HTML editable area.
        /// </summary>
        /// <param name="content">The content.</param>
        public void VerifyContentInHtmlEditableArea(string content)
        {
            HtmlTextArea editable = EM.GenericContent.ContentBlockWidget.EditableHtmlArea
                .AssertIsPresent("Html editable area");
            Assert.AreEqual(content, editable.TextContent);
        }

        /// <summary>
        /// Save content block widget
        /// </summary>
        public void SaveChanges()
        {
            HtmlButton saveButton = EM.GenericContent.ContentBlockWidget.SaveChangesButton
            .AssertIsPresent("Save button");
            saveButton.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Provide access to "Create content" link
        /// </summary>
        public void CreateContentLink()
        {
            HtmlAnchor createContent = EM.GenericContent.ContentBlockWidget.CreateContent
            .AssertIsPresent("Create content");
            createContent.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Selects the content block in provider.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="contentBlockName">Name of the content block.</param>
        public void SelectContentBlockInProvider(string providerName, string contentBlockName)
        {
            HtmlAnchor selectProvider = EM.GenericContent.ContentBlockWidget.SelectProviderDropdown
            .AssertIsPresent("Provider dropdown");
            selectProvider.Click();

            var provider = ActiveBrowser.Find.ByExpression<HtmlAnchor>("class=ng-binding", "InnerText=" + providerName).AssertIsPresent("Provider"); 
            provider.Click();

            HtmlDiv sharedContentBlockList = EM.GenericContent.ContentBlockWidget.ContentBlockList
           .AssertIsPresent("Shared content list");

            var itemSpan = sharedContentBlockList.Find.ByExpression<HtmlSpan>("class=ng-binding", "InnerText=" + contentBlockName).AssertIsPresent("Content Block");
            itemSpan.Click();
            this.DoneSelectingButton();
        }

        /// <summary>
        /// Provide access to done button
        /// </summary>
        public void DoneSelectingButton()
        {
            HtmlButton shareButton = EM.GenericContent.ContentBlockWidget.DoneSelectingButton
            .AssertIsPresent("Done selecting button");
            shareButton.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Provide access to advance button
        /// </summary>
        public void AdvanceButtonSelecting()
        {
            HtmlDiv contentBlockFooter = EM.GenericContent.ContentBlockWidget.ContentBlockWidgetFooter
                .AssertIsPresent("Footer");

            HtmlAnchor advanceButton = contentBlockFooter.Find.ByExpression<HtmlAnchor>("class=btn btn-default btn-xs m-top-xs ng-scope", "InnerText=Advanced")
            .AssertIsPresent("Advance selecting button");

            advanceButton.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
            ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Enable social share buttons
        /// </summary>
        /// <param name="isEnabled">Is social share buttons enabled</param>
        public void EnableSocialShareButtons(string isEnabled)
        {
            HtmlInputText input = EM.GenericContent.ContentBlockWidget.EnableSocialSharing
                .AssertIsPresent("Social share field");

            input.Wait.ForExists();
            input.ScrollToVisible();
            input.Focus();
            input.MouseClick();

            Manager.Current.Desktop.KeyBoard.KeyDown(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.A);
            Manager.Current.Desktop.KeyBoard.KeyUp(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.Delete);

            ActiveBrowser.WaitForAsyncOperations();

            Manager.Current.Desktop.KeyBoard.TypeText(isEnabled);
        }

        /// <summary>
        /// Opens the link selector.
        /// </summary>
        public void OpenLinkSelector()
        {
            HtmlAnchor createContent = EM.GenericContent.ContentBlockWidget.LinkSelector
            .AssertIsPresent("link selector");
            createContent.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Switches to HTML view.
        /// </summary>
        public void SwitchToHtmlView()
        {
            HtmlButton htmlButton = EM.GenericContent.ContentBlockWidget.HtmlButton
            .AssertIsPresent("html view");
            htmlButton.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Switches to design view.
        /// </summary>
        public void SwitchToDesignView()
        {
            HtmlButton designButton = EM.GenericContent.ContentBlockWidget.DesignButton
            .AssertIsPresent("design view");
            designButton.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Presses the specific button.
        /// </summary>
        /// <param name="title">The title.</param>
        public void PressSpecificButton(string title)
        {
            HtmlAnchor createContent = ActiveBrowser.Find.ByExpression<HtmlAnchor>("title=" + title)
            .AssertIsPresent(title);
            createContent.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Verifies the content block text in design mode.
        /// </summary>
        /// <param name="content">The content.</param>
        public void VerifyContentBlockTextDesignMode(string content)
        {
            FrameInfo frameInfo = new FrameInfo(string.Empty, string.Empty, "javascript:\"\"", 1);
            Browser frame = ActiveBrowser.Frames[frameInfo];

            var contentArea = frame.Find.AllByTagName("body").FirstOrDefault();
            Assert.AreEqual(content, contentArea.InnerText, "contents are not equal");
        }
    }
}
