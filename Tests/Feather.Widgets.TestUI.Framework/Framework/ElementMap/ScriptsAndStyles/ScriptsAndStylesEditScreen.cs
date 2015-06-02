﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.TestTemplates;

namespace Feather.Widgets.TestUI.Framework.Framework.ElementMap.ScriptsAndStyles
{
    /// <summary>
    /// Provides access to css widget screen
    /// </summary>
    public class ScriptsAndStylesEditScreen : HtmlElementContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CssWidgetEditScreen" /> class.
        /// </summary>
        /// <param name="find">The find.</param>
        public ScriptsAndStylesEditScreen(Find find)
            : base(find)
        {
        }

        /// <summary>
        /// Gets code mirror lines.
        /// </summary>
        public HtmlDiv CodeMirrorLines
        {
            get
            {
                return this.Get<HtmlDiv>("tagname=div", "class=CodeMirror-lines");
            }
        }

        /// <summary>
        /// Gets Link to file button.
        /// </summary>
        public HtmlInputRadioButton LinkToFile
        {
            get
            {
                return this.Get<HtmlInputRadioButton>("tagname=input", "value=Reference");
            }
        }

        /// <summary>
        /// Gets Select buttons.
        /// </summary>
        public HtmlButton SelectButton
        {
            get
            {
                return this.Find.ByExpression<HtmlButton>("class=~btn btn-xs btn-default");
            }
        }

        /// <summary>
        /// Gets the unordered list of folder tree.
        /// </summary>
        public HtmlUnorderedList FolderTree
        {
            get
            {
                return this.Get<HtmlUnorderedList>("class=sf-Tree");
            }
        }

        /// <summary>
        /// Gets the unordered list of file tree.
        /// </summary>
        public HtmlUnorderedList FileTree
        {
            get
            {
                return this.Get<HtmlUnorderedList>("class=sf-Tree ng-scope");
            }
        }

        /// <summary>
        /// Gets More options.
        /// </summary>
        public HtmlAnchor MoreOptions
        {
            get
            {
                return this.Get<HtmlAnchor>("tagname=a", "InnerText=More options");
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public HtmlInputText Description
        {
            get
            {
                return this.Get<HtmlInputText>("tagname=input", "ng-model=properties.Description.PropertyValue");
            }
        }
    }
}
