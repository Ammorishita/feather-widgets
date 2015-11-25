﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.Common.UnitTesting;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.jQuery;

namespace Feather.Widgets.TestUI.Framework.Framework.Wrappers.Frontend.Forms
{
    /// <summary>
    /// This is an entry point for FormsWrapper.
    /// </summary>
    public class FormsWrapper : BaseWrapper
    {
        /// <summary>
        /// Verify if text field label is visible
        /// </summary>
        public void VerifyTextFieldLabelIsVisible(string fieldLabel)
        {
            Assert.IsTrue(EM.Forms.FormsFrontend.TextboxField.InnerText.Contains(fieldLabel));
        }

        /// <summary>
        /// Verify if checkboxes field label is visible
        /// </summary>
        public void VerifyCheckboxesFieldLabelIsVisible(string fieldLabel)
        {
            Assert.IsTrue(EM.Forms.FormsFrontend.CheckboxesField.InnerText.Contains(fieldLabel));
        }

        /// <summary>
        /// Verify if checkboxes field label is NOT visible
        /// </summary>
        public void VerifyCheckboxesFieldLabelIsNotVisible(string fieldLabel)
        {
            Assert.IsFalse(EM.Forms.FormsFrontend.CheckboxesField.InnerText.Contains(fieldLabel));
        }

        /// <summary>
        /// Verify if multiple choice field label is visible
        /// </summary>
        public void VerifyMultipleChoiceFieldLabelIsVisible(string fieldLabel)
        {
            Assert.IsTrue(EM.Forms.FormsFrontend.MultipleChoiceField.InnerText.Contains(fieldLabel));
        }

        /// <summary>
        /// Verify if dropdown list field label is visible
        /// </summary>
        public void VerifyDropdownListFieldLabelIsVisible(string fieldLabel)
        {
            Assert.IsTrue(EM.Forms.FormsFrontend.DropdownListField.InnerText.Contains(fieldLabel));
        }

        /// <summary>
        /// Verify if content block content is visible
        /// </summary>
        /// <param name="contentText">The content text.</param>
        /// <param name="isVisible">if set to <c>true</c> [is visible].</param>
        public void VerifyContentBlockFieldTextIsVisible(string contentText, bool isVisible = true)
        {
            HtmlDiv frontendPageMainDiv = BAT.Wrappers().Frontend().Pages().PagesWrapperFrontend().GetPageContent();
            if (!isVisible)
            {
                Assert.IsFalse(frontendPageMainDiv.InnerText.Contains(contentText));
            }
            else
            {
                Assert.IsTrue(frontendPageMainDiv.InnerText.Contains(contentText));
            }
        }

        /// <summary>
        /// Verifies the submit buttons count in front end.
        /// </summary>
        /// <param name="expectedCount">The expected count.</param>
        public void VerifySubmitButtonsCountInFrontEnd(int expectedCount)
        {
            var submitButtons = ActiveBrowser.Find.AllByExpression("tagName=button", "class=sf-SubmitButton btn btn-primary");
            Assert.AreEqual(expectedCount, submitButtons.Count);
        }

        /// <summary>
        /// Verify if Paragraph text field label is visible
        /// </summary>
        public void VerifyParagraphTextFieldLabelIsVisible(string fieldLabel)
        {
            Assert.IsTrue(EM.Forms.FormsFrontend.ParagraphTextField.InnerText.Contains(fieldLabel));
        }

        /// <summary>
        /// Verify if text field is visible
        /// </summary>
        public void VerifyTextFieldlIsVisibleHybrid()
        {
            Assert.IsNotNull(EM.Forms.FormsFrontend.TextboxFieldHybrid, "Text field is not");
            Assert.IsTrue(EM.Forms.FormsFrontend.TextboxFieldHybrid.IsVisible(), "The text input field is not visible");
        }

        /// <summary>
        /// Verify if Submit button is visible
        /// </summary>
        public void VerifySubmitButtonIsVisible()
        {
            Assert.IsTrue(EM.Forms.FormsFrontend.SubmitButton.IsVisible(), "The submit button is not visible");
        }

        /// <summary>
        /// Verify if Submit button is not visible
        /// </summary>
        public void VerifySubmitButtonIsNotVisible()
        {
            Assert.IsFalse(EM.Forms.FormsFrontend.SubmitButton.IsVisible(), "The submit button is visible");
        }
              
        /// <summary>
        /// Verify the delete form in use message is shown on the frontend
        /// </summary>
        public void VerifyMessageIsDisplayedAfterFormIsDeleted()
        {
            var message = EM.Forms.FormsFrontend.DeleteFormInUseMessage;
            Assert.AreEqual(message.InnerText, "The specified form no longer exists or is currently unpublished.");
            Assert.IsTrue(message.IsVisible(), String.Format("Total amount {0} was not found", message.InnerText));
        }

        /// <summary>
        /// Sets the TextBox Content in the Frontend of the form
        /// </summary>
        public void SetTextboxContent(string content)
        {
            HtmlInputText textbox = this.EM.Forms.FormsFrontend.TextField.AssertIsPresent("Text field");

            textbox.ScrollToVisible();
            textbox.Focus();
            textbox.MouseClick();

            Manager.Current.Desktop.KeyBoard.KeyDown(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.A);
            Manager.Current.Desktop.KeyBoard.KeyUp(System.Windows.Forms.Keys.Control);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.Delete);

            Manager.Current.Desktop.KeyBoard.TypeText(content);
            Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.Tab);
        }

        /// <summary>
        /// Sets the Paragraph Text Content in the Frontend of the form
        /// </summary>
        public void SetParagraphTextContent(string content)
        {
            HtmlTextArea textbox = this.EM.Forms.FormsFrontend.ParagraphTextBox.AssertIsPresent("Text field");
            textbox.MouseClick();
            Manager.Current.Desktop.KeyBoard.TypeText(content);
        }

        /// <summary>
        /// Clicks the submit button in the frontend of the form and checks the succsess message
        /// </summary>
        public void SubmitForm()
        {
            HtmlButton submitButton = EM.Forms.FormsFrontend.SubmitButton;
            submitButton.MouseClick();

            this.WaitForSuccessMessage();
        }

        /// <summary>
        /// Clicks the submit button multiple times in the frontend of the form
        /// </summary>
        /// <param name="count">The count.</param>
        public void MultipleSubmitForm(int count)
        {
            HtmlButton submitButton = EM.Forms.FormsFrontend.SubmitButton;
            for (int i = 0; i < count; i++)
            {
                submitButton.MouseClick();
            }

            this.WaitForSuccessMessage();
        }
            
        /// <summary>
        /// Wait for success message after the form is submitted
        /// </summary>
        public void WaitForSuccessMessage()
        {
            var successMsg = ActiveBrowser.Find.AssociatedBrowser.GetControl<HtmlDiv>("tagname=div", "innertext=Success! Thanks for filling out our form!");
        }

        /// <summary>
        /// Clicks the submit button in the frontend of the form
        /// </summary>
        public void ClickSubmit()
        {
            HtmlButton submitButton = EM.Forms.FormsFrontend.SubmitButton;
            submitButton.MouseClick();
        }

        /// <summary>
        /// Selects checkbox from checkboxes field
        /// </summary>
        public void SelectCheckbox(string choice)
        {
            HtmlInputCheckBox checkbox = ActiveBrowser.Find.ByExpression<HtmlInputCheckBox>("tagname=input", "data-sf-role=checkboxes-field-input", "value=" + choice);
            checkbox.Click();
        }

        /// <summary>
        /// Selects radio button from multiplechoice field
        /// </summary>
        public void SelectRadioButton(string choice)
        {
            HtmlInputRadioButton checkbox = ActiveBrowser.Find.ByExpression<HtmlInputRadioButton>("tagname=input", "data-sf-role=multiple-choice-field-input", "value=" + choice);
            checkbox.MouseClick();
            ActiveBrowser.WaitUntilReady();
        }

        /// <summary>
        /// Selects option from dropdown field
        /// </summary>
        public void SelectDropdownOption(string choice)
        {
            HtmlSelect dropdown = ActiveBrowser.Find.ByExpression<HtmlSelect>("tagname=select", "data-sf-role=dropdown-list-field-select");
            dropdown.SelectByText(choice);
            dropdown.AsjQueryControl().InvokejQueryEvent(jQueryControl.jQueryControlEvents.click);
            dropdown.AsjQueryControl().InvokejQueryEvent(jQueryControl.jQueryControlEvents.change);
        }

        /// <summary>
        /// Verify details news page URL
        /// </summary>
        public void VerifyPageUrl(string pageName)
        {
            Assert.IsTrue(ActiveBrowser.Url.EndsWith(pageName.ToLower()));
        }

        /// <summary>
        /// Verify Checkboxes widget is deleted or Null
        /// </summary>
        public void VerifyCheckboxesFieldIsNotVisible()
        {
            Assert.IsNull(EM.Forms.FormsFrontend.CheckboxesField, "Checkboxes field is still visible at the frontend");
        }

        /// <summary>
        /// Clicks the next step button
        /// </summary>
        public void ClickNextButton()
        {
            ActiveBrowser.RefreshDomTree();
            HtmlButton nextButton = EM.Forms.FormsFrontend.NextStepButton;
            nextButton.ScrollToVisible();
            nextButton.Focus();
            nextButton.Click();
        }

        /// <summary>
        /// Clicks the previous step button
        /// </summary>
        public void ClickPreviousButton()
        {
            HtmlAnchor previousButton = EM.Forms.FormsFrontend.PreviousStep;
            previousButton.MouseClick();

            ActiveBrowser.WaitUntilReady();
        }


        /// <summary>
        /// Verify next step text
        /// </summary>
        /// <param name="buttonText">The button text.</param>
        /// <param name="isVisible">if set to <c>true</c> [is visible].</param>
        public void VerifyNextStepText(string buttonText = "Next step", bool isVisible = true)
        {
            HtmlButton nextButton = EM.Forms.FormsFrontend.NextStepButton;

            if (!isVisible)
            {
                nextButton.AssertIsNotVisible("Next step button");
            }
            else
            {
                nextButton.AssertIsVisible("Next step button");
                Assert.IsTrue(nextButton.InnerText.Contains(buttonText), "Button text ");
            }
        }

        /// <summary>
        /// Verify previous step text
        /// </summary>
        /// <param name="buttonText">The button text.</param>
        /// <param name="isVisible">if set to <c>true</c> [is visible].</param>
        public void VerifyPreviousStepText(string buttonText = "Previous step", bool isVisible = true)
        {           
            if (!isVisible)
            {
                 Assert.IsFalse(ActiveBrowser.ContainsText(buttonText));
            }
            else
            {
                HtmlAnchor previousButton = EM.Forms.FormsFrontend.PreviousStep;

                previousButton.AssertIsVisible("Previous step button");
                Assert.IsTrue(previousButton.InnerText.Contains(buttonText), "Button text ");
            }
        }

        /// <summary>
        /// Verifies the navigation pages labels.
        /// </summary>
        /// <param name="labels">The labels.</param>
        /// <param name="navIndex">Index of the nav.</param>
        public void VerifyNavigationPagesLabels(List<string> labels, int navIndex = 0)
        {
            var lists = ActiveBrowser.Find.AllByExpression<HtmlUnorderedList>("class=sf-FormNav");
            lists[navIndex].AssertIsVisible("Navigation list");

            var pageLabels = lists[navIndex].Find.AllByTagName("li");
            for (int i = 0; i < labels.Count; i++)
            {
                Assert.AreEqual((i + 1) + labels[i], pageLabels[i].InnerText);
            }
        }

        /// <summary>
        /// Verifies the active page in navigation.
        /// </summary>
        /// <param name="activePageIndex">Index of the active page.</param>
        public void VerifyActivePageInNavigation(int activePageIndex = 0)
        {
            var activePage = ActiveBrowser.Find.ByExpression<HtmlListItem>("data-sf-navigation-index=" + activePageIndex);
            activePage.AssertIsVisible("Active page in navigation");
            Assert.AreEqual(activePage.CssClass, "active");
        }

        /// <summary>
        /// Verify multipage form on frontend
        /// </summary>
        /// <param name="fieldLabel">Field label</param>
        public void VerifyMultiPageFormFieldOnForntend(string[] fieldLabel)
        {
            List<HtmlDiv> formList = ActiveBrowser.Find.AllByExpression<HtmlDiv>("data-sf-role=form-container").ToList<HtmlDiv>();

            for (int i = 0; i < fieldLabel.Length; i++)
            {
                HtmlDiv activeForm = formList[i].Find.AllByExpression<HtmlDiv>("TagName=div", "data-sf-role=separator").Where(d => d.IsVisible()).FirstOrDefault();
                Assert.IsTrue(activeForm.InnerText.Contains(fieldLabel[i]), "Label of the field is not as expected");
            }
        }

       /// <summary>
        /// Verify if field exist in preview
       /// </summary>
       /// <param name="fieldLabel">Label of the field</param>
       /// <param name="exist">If the field exist set true</param>
        public void VerifyIfFieldExistInPreviewMode(string fieldLabel, bool exist)
        {
             HtmlDiv activeForm = ActiveBrowser.Find.AllByExpression<HtmlDiv>("TagName=div", "data-sf-role=separator").Where(d => d.IsVisible()).FirstOrDefault();
             if (exist)
             {
                 Assert.IsTrue(activeForm.InnerText.Contains(fieldLabel), "Label of the field is not as expected");
             }
             else
             {
                 Assert.IsFalse(activeForm.InnerText.Contains(fieldLabel), "Label of the field is not as expected");
             }            
        }
    }
}
