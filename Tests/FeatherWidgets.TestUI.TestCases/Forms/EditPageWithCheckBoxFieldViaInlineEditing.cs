﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.WebAii.Controls.Html;

namespace FeatherWidgets.TestUI.TestCases.Forms
{
    /// <summary>
    /// EditPageWithCheckBoxFieldViaInlineEditing test class.
    /// </summary>
    [TestClass]
    public class EditPageWithCheckBoxFieldViaInlineEditing_ : FeatherTestCase
    {
        /// <summary>
        /// UI test EditPageWithCheckBoxFieldViaInlineEditing
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam6),
        TestCategory(FeatherTestCategories.Bootstrap),
        TestCategory(FeatherTestCategories.Forms)]
        public void EditPageWithCheckBoxFieldViaInlineEditing()
        {
            BAT.Macros().NavigateTo().Modules().Forms(this.Culture);
            BAT.Wrappers().Backend().Forms().FormsDashboard().OpenFormFromTheGrid(FormName);
            BAT.Wrappers().Backend().Forms().FormsContentScreen().PublishForm();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), true, this.Culture);
            BAT.Wrappers().Frontend().InlineEditing().InlineEditingWrapper().OpenPageForEdit();
            BAT.Wrappers().Frontend().InlineEditing().InlineEditingWrapper().VerifyEditIsOn(PageName);
            BATFeather.Wrappers().Frontend().ContentBlock().ContentBlockWrapper().EditContentBlock();
            BAT.Wrappers().Frontend().InlineEditing().InlineEditingWrapper().PublishPage();
            BAT.Wrappers().Frontend().InlineEditing().InlineEditingWrapper().VerifyEditIsOff();
            BATFeather.Wrappers().Frontend().ContentBlock().ContentBlockWrapper().VerifyContentOfContentBlockOnThePageFrontend("edited content block");
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

        private const string FormName = "NewForm";
        private const string PageName = "FormPage";
    }
}
