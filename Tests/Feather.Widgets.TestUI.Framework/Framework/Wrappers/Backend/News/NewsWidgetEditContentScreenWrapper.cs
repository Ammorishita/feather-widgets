﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.jQuery;
using ArtOfTest.Common.UnitTesting;

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
            HtmlDiv optionsDiv = EM.News.NewsWidgetContentScreen.WhichNewsToDisplayList
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
        public void SelectTaxonomy(string taxonomy)
        {
            ActiveBrowser.WaitForAsyncOperations();

            HtmlInputCheckBox optionButton = ActiveBrowser.Find.ByExpression<HtmlInputCheckBox>("id=" + taxonomy)
                .AssertIsPresent("Taxonomy option");

            optionButton.Click();

            ActiveBrowser.WaitForAsyncOperations();
        }

        /// <summary>
        /// Provide access to done button
        /// </summary>
        public void DoneSelectingButton()
        {
            HtmlButton shareButton = EM.News.NewsWidgetContentScreen.DoneSelectingButton
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
            HtmlDiv newsList = EM.News.NewsWidgetContentScreen.NewsList
            .AssertIsPresent("News list");

            var itemDiv = newsList.Find.ByExpression<HtmlDiv>("class=ng-binding", "InnerText=" + newsTitle)
                .AssertIsPresent("News with this title was not found");

            itemDiv.Wait.ForVisible();
            itemDiv.ScrollToVisible();
            itemDiv.MouseClick();
            this.DoneSelectingButton();
        }

        /// <summary>
        /// Save news widget
        /// </summary>
        public void SaveChanges()
        {
            HtmlButton saveButton = EM.News.NewsWidgetContentScreen.SaveChangesButton
            .AssertIsPresent("Save button");
            saveButton.Click();
        }

        /// <summary>
        /// Select tag by title
        /// </summary>
        public void SelectTags()
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
        public void SearchTagByTitle(string title)
        {
            this.SelectTags();

            HtmlDiv inputDiv = EM.News.NewsWidgetContentScreen.SearchByTypingDiv
                .AssertIsPresent("Search field div");

            HtmlInputText input = inputDiv.Find.ByExpression<HtmlInputText>("placeholder=Narrow by typing")
            .AssertIsPresent("Search field");

            Manager.Current.Desktop.Mouse.Click(MouseClickType.LeftClick, input.GetRectangle());
            Manager.Current.Desktop.KeyBoard.TypeText(title);
            Manager.Current.ActiveBrowser.WaitUntilReady();
            Manager.Current.ActiveBrowser.WaitForAsyncJQueryRequests();
            Manager.Current.ActiveBrowser.RefreshDomTree();
        }
        /// <summary>
        /// Waits for items count to appear.
        /// </summary>
        /// <param name="expectedCount">The expected items count.</param>
        public void WaitForItemsToAppear(int expectedCount)
        {
            Manager.Current.Wait.For(() => this.CountItems(expectedCount), 50000);
        }
      
        /// <summary>
        /// Counts the items.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <returns></returns>
        public bool CountItems(int expected)
        {
            ActiveBrowser.RefreshDomTree();
            HtmlDiv scroller = ActiveBrowser.Find.ByExpression<HtmlDiv>("class=list-group s-items-list-wrp endlessScroll")
                .AssertIsPresent("Scroller");
            scroller.MouseClick(MouseClickType.LeftDoubleClick);
            Manager.Current.Desktop.Mouse.TurnWheel(1000, MouseWheelTurnDirection.Backward);
            var items = EM.News.NewsWidgetContentScreen.SelectorItems;
            return expected == items.Count;
        }

        /// <summary>
        /// No news items found
        /// </summary>
        public void NoItemsFound()
        {
            HtmlDiv noItemsFound = EM.News.NewsWidgetContentScreen.NoItemsFoundDiv
            .AssertIsPresent("No items found div");

            var isContained = noItemsFound.InnerText.Contains("No items found");
            Assert.IsTrue(isContained, "Message not found");
        }
    }
}
