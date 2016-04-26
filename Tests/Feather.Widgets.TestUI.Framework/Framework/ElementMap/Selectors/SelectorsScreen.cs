﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.TestTemplates;

namespace Feather.Widgets.TestUI.Framework.Framework.ElementMap.Selectors
{
    /// <summary>
    /// Provides access to selectors screen
    /// </summary>
    public class SelectorsScreen : HtmlElementContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorsScreen" /> class.
        /// </summary>
        /// <param name="find">The find.</param>
        public SelectorsScreen(Find find)
            : base(find)
        {
        }

        /// <summary>
        /// Gets Done selecting button.
        /// </summary>
        public HtmlButton DoneSelectingButton
        {
            get
            {
                return this.Get<HtmlButton>("tagname=button", "InnerText=Done selecting");
            }
        }

        /// <summary>
        /// Gets search div.
        /// </summary>
        public HtmlDiv SearchByTypingDiv
        {
            get
            {
                return this.Get<HtmlDiv>("tagname=div", "class=input-group m-bottom-sm");
            }
        }

        /// <summary>
        /// Gets no items div.
        /// </summary>
        public HtmlDiv NoItemsFoundDiv
        {
            get
            {
                return this.Get<HtmlDiv>("tagname=div", "InnerText=No items found");
            }
        }

        /// <summary>
        /// Gets the selector items.
        /// </summary>
        /// <value>The selector items.</value>
        public ICollection<HtmlAnchor> SelectorItems
        {
            get
            {
                return this.Find.AllByExpression<HtmlAnchor>("ng-repeat=item in items");
            }
        }

        /// <summary>
        /// Gets the search input.
        /// </summary>
        /// <value>The search input.</value>
        public HtmlInputText SearchInput
        {
            get
            {
                return this.Get<HtmlInputText>("ng-model=filter.search");
            }
        }

        /// <summary>
        /// Gets selected items span
        /// </summary>
        public HtmlSpan SelectedItemsSpan
        {
            get
            {
                return this.Get<HtmlSpan>("tagname=span", "class=label label-taxon label-full ng-binding");
            }
        }

        /// <summary>
        /// Gets the active tab.
        /// </summary>
        /// <value>The active tab.</value>
        public HtmlDiv ActiveTab
        {
            get
            {
                return this.Get<HtmlDiv>("class=~k-content k-state-active");
            }
        }

        /// <summary>
        /// Gets the form selector.
        /// </summary>
        /// <value>The active tab.</value>
        public HtmlDiv FormSelector
        {
            get
            {
                return this.Get<HtmlDiv>("class=~list-group list-group-endless ng-isolate-scope");
            }
        }

        /// <summary>
        /// Gets the all tab.
        /// </summary>
        /// <value>The all tab.</value>
        public HtmlSpan AllTab
        {
            get
            {
                return this.Get<HtmlSpan>("class=k-link", "innertext=~All");
            }
        }

        /// <summary>
        /// Gets the selected tab.
        /// </summary>
        /// <value>The selected tab.</value>
        public HtmlSpan SelectedTab
        {
            get
            {
                return this.Get<HtmlSpan>("class=k-link", "innertext=~Selected");
            }
        }

        /// <summary>
        /// Get the page link.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <returns></returns>
        public HtmlAnchor PageLink(string pageName)
        {
            return this.Get<HtmlAnchor>("tagname=a", "ng-click=sfSelectItem({ dataItem: dataItem })", "innertext=~" + pageName);
        }

        /// <summary>
        /// Get the page div.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        /// <returns></returns>
        public HtmlDiv PageDiv(string pageName)
        {
            return this.Get<HtmlDiv>("tagname=div", "ng-click=itemClicked($index, item)", "innertext=~" + pageName);
        }

        /// <summary>
        /// Gets the external urls tab.
        /// </summary>
        /// <value>The external urls tab.</value>
        public HtmlSpan ExternalUrlsTab
        {
            get
            {
                return this.Get<HtmlSpan>("class=k-link", "innertext=~External URLs");
            }
        }

        /// <summary>
        /// Gets all external urls titles.
        /// </summary>
        /// <value>All external urls titles.</value>
        public ICollection<HtmlInputText> AllExternalUrlsTitles
        {
            get
            {
                return this.Find.AllByExpression<HtmlInputText>("tagname=input", "ng-model=item.TitlesPath");
            }
        }

        /// <summary>
        /// Gets all external urls urls.
        /// </summary>
        /// <value>All external urls urls.</value>
        public ICollection<HtmlInputText> AllExternalUrlsUrls
        {
            get
            {
                return this.Find.AllByExpression<HtmlInputText>("tagname=input", "ng-model=item.Url");
            }
        }

        /// <summary>
        /// Gets the add external URL button.
        /// </summary>
        /// <value>The add external URL button.</value>
        public HtmlInputButton AddExternalUrlButton
        {
            get
            {
                return this.Get<HtmlInputButton>("tagname=input", "value=Add external URL");
            }
        }

        /// <summary>
        /// Gets the remove external urls icon.
        /// </summary>
        /// <value>The remove external urls icon.</value>
        public HtmlAnchor RemoveExternalUrlsIcon
        {
            get
            {
                return this.Get<HtmlAnchor>("class=text-danger", "ng-click=removeItem($index, item)");
            }
        }

        /// <summary>
        /// Gets the open external urls in new tab checkbox.
        /// </summary>
        /// <value>The open external urls in new tab checkbox.</value>
        public HtmlInputCheckBox OpenExternalUrlsInNewTabCheckbox
        {
            get
            {
                return this.Get<HtmlInputCheckBox>("tagname=input", "id=openInNewTab");
            }
        }

        /// <summary>
        /// Gets user and role providers drop down.
        /// </summary>
        /// <value>Providers drop down</value>
        public HtmlSelect ProvidersDropdown
        {
            get
            {
                return this.Get<HtmlSelect>("tagname=select", "ng-model=sfProvider");
            }
        }

        /// <summary>
        /// Gets all RadioButton.
        /// </summary>
        /// <value>
        /// All RadioButton.
        /// </value>
        public HtmlInputRadioButton AllRadioButton
        {
            get
            {
                return this.Get<HtmlInputRadioButton>("tagname=input", "value=AllItems");
            }
        }
    }
}
