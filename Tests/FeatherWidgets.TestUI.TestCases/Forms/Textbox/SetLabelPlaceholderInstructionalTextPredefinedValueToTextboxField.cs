﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeatherWidgets.TestUI.TestCases.Forms.Textbox
{
    /// <summary>
    /// SetLabelPlaceholderInstructionalTextPredefinedValueToTextboxField test class.
    /// </summary>
    [TestClass]
    public class SetLabelPlaceholderInstructionalTextPredefinedValueToTextboxField_ : FeatherTestCase
    {
        /// <summary>
        /// UI test SetLabelPlaceholderInstructionalTextPredefinedValueToTextboxField
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.SitefinityTeam6),
        TestCategory(FeatherTestCategories.Bootstrap),
        TestCategory(FeatherTestCategories.Forms)]
        public void SetLabelPlaceholderInstructionalTextPredefinedValueToTextboxField()
        {
            BAT.Macros().NavigateTo().Modules().Forms(this.Culture);
            BAT.Wrappers().Backend().Forms().FormsDashboard().OpenFormFromTheGrid(FormName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(TextField);
            BATFeather.Wrappers().Backend().Forms().FormsContentScreenWrapper().ChangeTexboxLabel(LabelTextField);
            BATFeather.Wrappers().Backend().Forms().FormsContentScreenWrapper().ChangePlaceholder(PlaceholderTextField);
            BATFeather.Wrappers().Backend().Forms().FormsContentScreenWrapper().ChangePredefinedValue(PredefinedValueTextField);
            BATFeather.Wrappers().Backend().Forms().FormsContentScreenWrapper().ClickInstructionalTextLink();
            BATFeather.Wrappers().Backend().Forms().FormsContentScreenWrapper().ChangeInstructionalText(InstructionalTextField);
            BATFeather.Wrappers().Backend().ContentBlocks().ContentBlocksWrapper().SaveChanges();
            BAT.Wrappers().Backend().Forms().FormsContentScreen().PublishForm();
            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), true, this.Culture);
            Assert.IsTrue(ActiveBrowser.ContainsText(LabelTextField), "Text was not found on the page");
            Assert.IsTrue(ActiveBrowser.ContainsText(PredefinedValueTextField), "Text was not found on the page");
            Assert.IsTrue(ActiveBrowser.ContainsText(InstructionalTextField), "Text was not found on the page");                  
            BATFeather.Wrappers().Frontend().Forms().FormsWrapper().RemoveTextboxContent();
            Assert.IsTrue(ActiveBrowser.ContainsText(PlaceholderTextField), "Text was not found on the page");
            BATFeather.Wrappers().Frontend().Forms().FormsWrapper().SetTextboxContent(TextBoxContent);
            BATFeather.Wrappers().Frontend().Forms().FormsWrapper().SubmitForm();

            BAT.Macros().NavigateTo().Modules().Forms(this.Culture);
            BAT.Wrappers().Backend().Forms().FormsDashboard().ViewFormResponses(FormName);

            BATFeather.Wrappers().Backend().Forms().FormsWrapper().VerifyNumberOfResponses(ExpectedResponsesCount);
            BAT.Wrappers().Backend().Forms().FormsResponseScreen().SelectResponse(ResponseNumber);
            BATFeather.Wrappers().Backend().Forms().FormsWrapper().VerifyResponseAuthorUsername(ExpectedAuthorName);
            BATFeather.Wrappers().Backend().Forms().FormsWrapper().VerifyResponseSubmitDate();
            BATFeather.Wrappers().Backend().Forms().FormsWrapper().VerifyResponseTextboxAnswer(TextBoxContent);
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
        private const string TextField = "TextFieldController";
        private const string LabelTextField = "Label of text field";
        private const string PlaceholderTextField = "Placeholder of text field";
        private const string PredefinedValueTextField = "Predefined value of text field";
        private const string InstructionalTextField = "Instructional text";
        private const string PageName = "FormPage";
        private const string TextBoxContent = "Textbox Field Text";
        private const int ExpectedResponsesCount = 1;
        private const int ResponseNumber = 1;
        private const string ExpectedAuthorName = "admin@test.test";
    }
}
