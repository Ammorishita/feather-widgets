﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Feather.Widgets.TestUI.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUtilities;

namespace FeatherWidgets.TestUI.TestCases.Forms
{
    /// <summary>
    /// ViewFormWithParagraphTextFieldOnPageAndVerifyResponseInBackend test class.
    /// </summary>
    [TestClass]
    public class ViewFormWithParagraphTextFieldOnPageAndVerifyResponseInBackend_ : FeatherTestCase
    {
        /// <summary>
        /// UI test ViewFormWithParagraphTextFieldOnPageAndVerifyResponseInBackend
        /// </summary>
        [TestMethod,
        Owner(FeatherTeams.FeatherTeam),
        TestCategory(FeatherTestCategories.PagesAndContent),
        TestCategory(FeatherTestCategories.Forms)]
        public void ViewFormWithParagraphTextFieldOnPageAndVerifyResponseInBackend()
        {
            BAT.Macros().NavigateTo().Modules().Forms(this.Culture);
            BAT.Wrappers().Backend().Forms().FormsDashboard().ClickCreateAFormButton();
            BAT.Wrappers().Backend().Forms().FormsCreateScreen().SetFormName(FormName);
            BAT.Wrappers().Backend().Forms().FormsCreateScreen().ClickCreateAndAddContent();
            BATFeather.Wrappers().Backend().Forms().FormsContentScreenWrapper().AddField(FieldName);
            BAT.Wrappers().Backend().Forms().FormsContentScreen().PublishForm();

            BAT.Macros().NavigateTo().Pages(this.Culture);
            BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().AddMvcWidgetHybridModePage(WidgetName);
            BATFeather.Wrappers().Backend().Pages().PageZoneEditorWrapper().EditWidget(WidgetName);
            BATFeather.Wrappers().Backend().Widgets().SelectorsWrapper().SelectItemsInFormWidgetSelector(FormName);
            BATFeather.Wrappers().Backend().Widgets().WidgetDesignerWrapper().SaveChanges();
            BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

            BAT.Macros().NavigateTo().CustomPage("~/" + PageName.ToLower(), true, this.Culture);
            BATFeather.Wrappers().Frontend().Forms().FormsWrapper().VerifyParagraphTextFieldLabelIsVisible(LabelName);
            BATFeather.Wrappers().Frontend().Forms().FormsWrapper().SetParagraphTextContent(TextBoxContent);
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

        private const string FormName = "MvcForm";
        private const string PageName = "FormPage";
        private const string FieldName = "Paragraph Text";
        private const string WidgetName = "Form";
        private const string LabelName = "Untitled";
        private const string TextBoxContent = "Paragraph Field Text";
        private const int ExpectedResponsesCount = 1;
        private const int ResponseNumber = 1;
        private const string ExpectedAuthorName = "admin";
    }
}