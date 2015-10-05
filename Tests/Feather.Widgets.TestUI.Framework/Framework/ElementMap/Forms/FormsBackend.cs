﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.TestTemplates;

namespace Feather.Widgets.TestUI.Framework.Framework.ElementMap.Forms
{
    /// <summary>
    /// Elements from Froms backend.
    /// </summary>
    public class FormsBackend : HtmlElementContainer
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="FormsBackend" /> class.
        /// </summary>
        /// <param name="find">The find.</param>
        public FormsBackend(Find find)
            : base(find)
        {
        }

        /// <summary>
        /// Gets the checkbox reponse field in backend
        /// </summary>
        public HtmlDiv GetResponseCheckboxesField
        {
            get
            {
                return this.ResponseDetailsPane.Find.ByExpression<HtmlDiv>("TagName=div", "class=sfChoiceContent", "id=~frmRspnsesCntView_formsBackendListDetail");
            }
        }

        /// <summary>
        /// Gets the dropdown list reponse field in backend
        /// </summary>
        public HtmlDiv GetResponseDropdownListField
        {
            get
            {
                return this.ResponseDetailsPane.Find.ByExpression<HtmlDiv>("TagName=div", "class=sfChoiceContent", "id=~frmRspnsesCntView_formsBackendListDetail");
            }
        }

        /// <summary>
        /// Gets the text field in designer of section header field
        /// </summary>
        public HtmlInputText SectionHeaderText
        {
            get
            {
                return this.Get<HtmlInputText>("TagName=input", "Class=form-control ng-pristine ng-untouched ng-valid");
            }
        }

        /// <summary>
        /// Provides access to the response details pane for the selected response
        /// </summary>
        public HtmlControl ResponseDetailsPane
        {
            get
            {
                return this.Get<HtmlControl>("tagname=div", "class=sfResponceDetailsWrp");
            }
        }

        /// <summary>
        /// Gets the Body Drop Zone in the Forms Edit Screen.
        /// </summary>
        public HtmlDiv BodyDropZone
        {
            get
            {
                return this.Find.AssociatedBrowser.GetControl<HtmlDiv>("id=PublicWrapper");
            }
        }
    }
}
