﻿using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.TestTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.TestUI.Core.ElementMap;

namespace Feather.Widgets.TestUI.Framework.Framework.ElementMap.Content
{
    public class ContentBlockWidgetScreen : HtmlElementContainer
    {
        public ContentBlockWidgetScreen(Find find)
            : base(find)
        {

        }

        /// <summary>
        /// Provides access to Content block widget body.
        /// </summary>
        public HtmlDiv ContentBlockWidgetPlaceholder
        {
            get
            {
                return this.Get<HtmlDiv>("tagname=div", "id=viewsPlaceholder");
            }
        }

        /// <summary>
        /// Provides access to Save changes button.
        /// </summary>
        public HtmlButton SaveChangesButton
        {
            get
            {
                return this.Get<HtmlButton>("tagname=button", "InnerText=Save");
            }
        }

        /// <summary>
        /// Provides access to editable area.
        /// </summary>
        public HtmlTableCell EditableArea
        {
            get
            {
                return this.Get<HtmlTableCell>("tagname=td", "class=k-editable-area");
            }
        }

        /// <summary>
        /// Provides access to share content title.
        /// </summary>
        public HtmlInputText ShareContentTitle
        {
            get
            {
                return this.Get<HtmlInputText>("tagname=input", "id=sharedContentTitle");
            }
        }

        /// <summary>
        /// Provides access to share button.
        /// </summary>
        public HtmlButton ShareButton
        {
            get
            {
                return this.Get<HtmlButton>("tagname=button", "InnerText=Share this content");
            }
        }

        /// <summary>
        /// Provides access to unshare button.
        /// </summary>
        public HtmlButton UnshareButton
        {
            get
            {
                return this.Get<HtmlButton>("tagname=button", "InnerText=Yes, Unshare this content");
            }
        }

        /// <summary>
        /// Provides access to Content block wlist with shared content blocks.
        /// </summary>
        public HtmlDiv ContentBlockList
        {
            get
            {
                return this.Get<HtmlDiv>("tagname=div", "class=list-group s-items-list-wrp");
            }
        }

        /// <summary>
        /// Provides access to done selecting button.
        /// </summary>
        public HtmlButton DoneSelectingButton
        {
            get
            {
                return this.Get<HtmlButton>("tagname=button", "InnerText=Done selecting");
            }
        }

        /// <summary>
        /// Provides access to create content.
        /// </summary>
        public HtmlAnchor CreateContent
        {
            get
            {
                return this.Get<HtmlAnchor>("tagname=a", "class=sfAddContentLnk");
            }
        }
    }
}
