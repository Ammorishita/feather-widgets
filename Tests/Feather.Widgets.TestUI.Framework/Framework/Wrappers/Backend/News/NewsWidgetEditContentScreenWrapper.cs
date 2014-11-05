﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.jQuery;
using ArtOfTest.Common.UnitTesting;
using System.Globalization;

namespace Feather.Widgets.TestUI.Framework.Framework.Wrappers.Backend
{
    /// <summary>
    /// This is the entry point class for news widget edit wrapper.
    /// </summary>
    public class NewsWidgetEditContentScreenWrapper : BaseWrapper
    {
        /// <summary>
        /// Selects which news to display in the widget designer
        /// </summary>
        /// <param name="mode">Which news to display</param>
        public void SelectWhichNewsToDisplay(string mode)
        {
            int position;
            HtmlDiv optionsDiv = EM.News
                                   .NewsWidgetContentScreen
                                   .WhichNewsToDisplayList
                                   .AssertIsPresent("Which news to display options list");

            List<HtmlDiv> newsDivs = optionsDiv.Find.AllByExpression<HtmlDiv>("tagname=div", "class=radio").ToList<HtmlDiv>();

            if (mode.Contains("Selected"))
            {
                position = 1;
            }
            else if (mode.Contains("Narrow"))
            {
                position = 2;
            }
            else
            {
                position = 0;
            }

            HtmlInputRadioButton optionButton = newsDivs[position].Find.ByExpression<HtmlInputRadioButton>("tagname=input")
                                                                  .AssertIsPresent("Which news to display option radio button");
            optionButton.Click();
        }

        /// <summary>
        /// Selects the taxonomy.
        /// </summary>
        /// <param name="taxonomy">The taxonomy.</param>
        public void SelectCheckBox(string taxonomy)
        {
            ActiveBrowser.WaitForAsyncOperations();

            HtmlInputCheckBox optionButton = ActiveBrowser.Find
                                                          .ByExpression<HtmlInputCheckBox>("id=" + taxonomy)
                                                          .AssertIsPresent("Taxonomy option");

            optionButton.Click();
            ActiveBrowser.WaitForAsyncOperations();
        }

        /// <summary>
        /// Provide access to done button
        /// </summary>
        public void DoneSelecting()
        {
            HtmlButton shareButton = EM.News
                                       .NewsWidgetContentScreen
                                       .DoneSelectingButton
                                       .AssertIsPresent("Done selecting button");
            shareButton.Click();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Select news item
        /// </summary>
        /// <param name="newsTitle">The title of the news item</param>
        public void SelectItem(string newsTitle)
        {
            HtmlDiv newsList = EM.News
                                 .NewsWidgetContentScreen
                                 .NewsList
                                 .AssertIsPresent("News list");

            var itemDiv = newsList.Find
                                  .ByExpression<HtmlDiv>("class=ng-binding", "InnerText=" + newsTitle)
                                  .AssertIsPresent("News with this title was not found");

            itemDiv.Wait.ForVisible();
            itemDiv.ScrollToVisible();
            itemDiv.MouseClick();
            ActiveBrowser.WaitForAsyncRequests();
        }

        /// <summary>
        /// Selects the item.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        public void SelectItemInMultipleSelector(params string[] itemNames)
        {
            foreach (var itemName in itemNames)
            {
                var divs = this.EM.News.NewsWidgetContentScreen.Find.AllByCustom<HtmlDiv>(a => a.InnerText.Equals(itemName));
                foreach (var div in divs)
                {
                    if (div.IsVisible())
                    {
                        div.Click();
                        ActiveBrowser.RefreshDomTree();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            HtmlButton saveButton = EM.News
                                      .NewsWidgetContentScreen
                                      .SaveChangesButton
                                      .AssertIsPresent("Save button");
            saveButton.Click();

            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
            ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Select tag by title
        /// </summary>
        public void ClickSelectButton()
        {
            var selectButtons = EM.News.NewsWidgetContentScreen.SelectButtons;
            foreach (var button in selectButtons)
            {
                if (button.IsVisible())
                {
                    button.Click();
                    break;
                }
            }

            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncRequests();
            ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Search tag by title
        /// </summary>
        /// <param name="title">The title of the tag</param>
        public void SearchItemByTitle(string title)
        {
            HtmlDiv inputDiv = EM.News
                                 .NewsWidgetContentScreen
                                 .SearchByTypingDiv
                                 .AssertIsPresent("Search field div");

            HtmlInputText input = inputDiv.Find
                                          .ByExpression<HtmlInputText>("placeholder=Narrow by typing")
                                          .AssertIsPresent("Search field");

            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, input.GetRectangle());
            Manager.Current.Desktop.KeyBoard.TypeText(title);
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Sets a text to search in t he search input.
        /// </summary>
        /// <param name="text">The text to be searched for.</param>
        public void ChangeSearchText(string text)
        {
            var inputList = this.EM.News.NewsWidgetContentScreen.Find.AllByExpression<HtmlInputText>("ng-model=filter.searchString");

            foreach (var inputElement in inputList)
            {
                if (inputElement.IsVisible())
                {
                    inputElement.Focus();
                    inputElement.MouseClick();
                    if (text != "")
                    {
                        inputElement.Text = string.Empty;
                        Manager.Current.Desktop.KeyBoard.TypeText(text);
                    }
                    else
                    {
                        //// select all and delete current text typing
                        Manager.Current.Desktop.KeyBoard.KeyDown(System.Windows.Forms.Keys.Control);
                        Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.A);
                        Manager.Current.Desktop.KeyBoard.KeyUp(System.Windows.Forms.Keys.Control);
                        Manager.Current.Desktop.KeyBoard.KeyPress(System.Windows.Forms.Keys.Back);
                    }
                    break;
                }
            }

            ActiveBrowser.WaitForAsyncRequests();
            ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Waits for items count to appear.
        /// </summary>
        /// <param name="expectedCount">The expected items count.</param>
        public void WaitForItemsToAppear(int expectedCount)
        {
            Manager.Current.Wait.For(() => this.ScrollToLatestItemAndCountItems(expectedCount), 50000);
        }

        /// <summary>
        /// Verifies if the items count is as expected.
        /// </summary>
        /// <param name="expected">The expected items count.</param>
        /// <returns>True or false depending on the items count.</returns>
        public bool ScrollToLatestItemAndCountItems(int expected)
        {
            ActiveBrowser.RefreshDomTree();
            var activeDialog = this.EM.News.NewsWidgetContentScreen.ActiveTab.AssertIsPresent("Content container");

            var items = activeDialog.Find.AllByExpression<HtmlDiv>("ng-bind=~bindIdentifierField(item");

            //// if items count is more than 12 elements, then you need to scroll
            if (items.Count() > 12)
            {
                HtmlDiv newsList = EM.News
                     .NewsWidgetContentScreen
                     .NewsList
                     .AssertIsPresent("News list");

                List<HtmlDiv> itemDiv = newsList.Find
                                      .AllByExpression<HtmlDiv>("class=ng-scope list-group-item list-group-item-multiselect").ToList<HtmlDiv>();

                int divsCount = itemDiv.Count;

                itemDiv[divsCount - 1].Wait.ForVisible();
                itemDiv[divsCount - 1].ScrollToVisible();
            }

            bool isCountCorrect = (expected == items.Count);
            return isCountCorrect;
        }

        /// <summary>
        /// No news items found
        /// </summary>
        public void NoItemsFound()
        {
            HtmlDiv noItemsFound = EM.News
                                     .NewsWidgetContentScreen
                                     .NoItemsFoundDiv
                                     .AssertIsPresent("No items found div");

            var isContained = noItemsFound.InnerText.Contains("No items found");
            Assert.IsTrue(isContained, "Message not found");
        }

        /// <summary>
        /// Verifies the selected item.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        public void VerifySelectedItemInMultipleSelectors(string[] itemNames)
        {
            var divList = this.EM.News.NewsWidgetContentScreen.Find.AllByExpression<HtmlDiv>("ng-repeat=item in selectedItems | limitTo:5");
            int divListCount = divList.Count;

            for (int i = 0; i < divListCount; i++)
            {
                Assert.AreEqual(divList[i].InnerText, itemNames[i]);
            }
        }

        /// <summary>
        /// Checks the notification in selected tab.
        /// </summary>
        /// <param name="itemNames">The item names.</param>
        public void CheckNotificationInSelectedTab(int expectedCout)
        {
            var span = this.EM.News.NewsWidgetContentScreen.Find.ByExpression<HtmlSpan>("class=badge ng-binding", string.Format("InnerText=~{0}", expectedCout));
            span.AssertIsPresent("item name not present");
        }

        /// <summary>
        /// Opens the selected tab.
        /// </summary>
        public void OpenSelectedTab()
        {
            HtmlAnchor selectedTab = this.EM.News.NewsWidgetContentScreen.SelectedTab
                                         .AssertIsPresent("selected tab");
            selectedTab.Click();
            ActiveBrowser.WaitForAsyncRequests();
            ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Opens the all tab.
        /// </summary>
        public void OpenAllTab()
        {
            HtmlAnchor allTab = this.EM.News.NewsWidgetContentScreen.AllTab
                                    .AssertIsPresent("all tab");

            allTab.Click();
            ActiveBrowser.WaitForAsyncRequests();
            ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Selects display items published in
        /// </summary>
        /// <param name="option">Selects display items published in</param>
        public void SelectDisplayItemsPublishedIn(string option, string divClass = "radio")
        {
            int position;
            HtmlForm optionsForm = EM.News
                                     .NewsWidgetContentScreen
                                     .DisplayItemsPublishedIn
                                     .AssertIsPresent("Selects display items published in");

            List<HtmlDiv> newsDivs = optionsForm.Find.AllByExpression<HtmlDiv>("tagname=div", "class=" + divClass).ToList<HtmlDiv>();

            if (option.Contains("Custom"))
            {
                position = 1;
            }
            else
            {
                position = 0;
            }

            HtmlInputRadioButton optionButton = newsDivs[position].Find.ByExpression<HtmlInputRadioButton>("tagname=input")
                                                                  .AssertIsPresent("Which news to display option radio button");

            optionButton.Click();
        }

        /// <summary>
        /// Set From date by typing to custom date selector
        /// </summary>
        /// <param name="dayAgo">Day ago</param>
        public void SetFromDateByTyping(int dayAgo)
        {
            DateTime publicationDateStart = DateTime.UtcNow.AddDays(dayAgo);
            String publicationDateStartFormat = publicationDateStart.ToString("dd-MMMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));

            List<HtmlInputText> inputDate = ActiveBrowser.Find.AllByExpression<HtmlInputText>("tagname=input", "id=fromInput").ToList<HtmlInputText>();

            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, inputDate[0].GetRectangle());
            Manager.Current.Desktop.KeyBoard.TypeText(publicationDateStartFormat);
            Manager.Current.ActiveBrowser.WaitUntilReady();
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Set To date by date picker to custom date selector
        /// </summary>
        /// <param name="dayForward">Day forward</param>
        public void SetToDateByDatePicker(int dayForward)
        {
            DateTime publicationDateEnd = DateTime.UtcNow.AddDays(dayForward);
            String publicationDateEndFormat = publicationDateEnd.ToString("dd", CultureInfo.CreateSpecificCulture("en-US"));

            List<HtmlSpan> buttonDate = ActiveBrowser.Find.AllByExpression<HtmlSpan>("tagname=span", "class=input-group-btn").ToList<HtmlSpan>();
            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, buttonDate[1].GetRectangle());

            List<HtmlTable> dateTable = ActiveBrowser.Find.AllByExpression<HtmlTable>("tagname=table", "role=grid").ToList<HtmlTable>();
            List<HtmlTableCell> toDay = dateTable[1].Find.AllByExpression<HtmlTableCell>("tagname=td", "InnerText=" + publicationDateEndFormat).ToList<HtmlTableCell>();
            HtmlButton buttonToDay;

            if (toDay.Count == 2)
            {
                buttonToDay = toDay[1].Find.ByExpression<HtmlButton>("tagname=button");
            }
            else
            {
                buttonToDay = toDay[0].Find.ByExpression<HtmlButton>("tagname=button");
            }

            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, buttonToDay.GetRectangle());
            Manager.Current.ActiveBrowser.WaitUntilReady();
        }

        /// <summary>
        ///  Add hour to custom date selector
        /// </summary>
        /// <param name="hour">Hour value to select</param>
        /// <param name="isFrom">Is from or to hour</param>
        public void AddHour(string hour, bool isFrom)
        {
            List<HtmlAnchor> hourAnchor = ActiveBrowser.Find.AllByExpression<HtmlAnchor>("tagname=a", "InnerText=Add hour").ToList<HtmlAnchor>();
            int fromOrTo = 0;

            if (isFrom.Equals(false))
            {
                fromOrTo = 1;
            }

            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, hourAnchor[fromOrTo].GetRectangle());
            Manager.Current.ActiveBrowser.WaitUntilReady();
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();

            List<HtmlSelect> hoursSelector = ActiveBrowser.Find.AllByExpression<HtmlSelect>("tagname=select", "ng-change=updateHours(hstep)").ToList<HtmlSelect>();
            hoursSelector[fromOrTo].SelectByValue(hour);
            hoursSelector[fromOrTo].AsjQueryControl().InvokejQueryEvent(jQueryControl.jQueryControlEvents.click);
            hoursSelector[fromOrTo].AsjQueryControl().InvokejQueryEvent(jQueryControl.jQueryControlEvents.change);
            Manager.Current.ActiveBrowser.WaitUntilReady();
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        ///  Add minute to custom date selector
        /// </summary>
        /// <param name="minute">Minute value to select</param>
        /// <param name="isFrom">Is from or to minute</param>
        public void AddMinute(string minute, bool isFrom)
        {
            List<HtmlAnchor> minutesAnchor = ActiveBrowser.Find.AllByExpression<HtmlAnchor>("tagname=a", "InnerText=Add minutes").ToList<HtmlAnchor>();
            int fromOrTo = 0;

            if (isFrom.Equals(false))
            {
                fromOrTo = 1;
            }

            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, minutesAnchor[fromOrTo].GetRectangle());
            Manager.Current.ActiveBrowser.WaitUntilReady();
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();

            List<HtmlSelect> minutesSelector = ActiveBrowser.Find.AllByExpression<HtmlSelect>("tagname=select", "ng-change=updateMinutes(mstep)").ToList<HtmlSelect>();
            minutesSelector[fromOrTo].Click();
            minutesSelector[fromOrTo].SelectByValue(minute);
            minutesSelector[fromOrTo].AsjQueryControl().InvokejQueryEvent(jQueryControl.jQueryControlEvents.click);
            minutesSelector[fromOrTo].AsjQueryControl().InvokejQueryEvent(jQueryControl.jQueryControlEvents.change);
            Manager.Current.ActiveBrowser.WaitUntilReady();
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();
        }

        /// <summary>
        /// Verify mesage in news widget - when News module is deactivated
        /// </summary>
        public void CheckInactiveNewsWidget()
        {
            HtmlDiv optionsDiv = EM.News
                                  .NewsWidgetContentScreen
                                  .InactiveWidget
                                  .AssertIsPresent("Inactive widget");
            var isContained = optionsDiv.InnerText.Contains("This widget doesn't work, becauseNewsmodule has been deactivated.");
            Assert.IsTrue(isContained, "Message not found");
        }
    }
}