﻿using ArtOfTest.Common.UnitTesting;
using ArtOfTest.WebAii.Controls.HtmlControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feather.Widgets.TestUI.Framework.Framework.Wrappers.Backend
{
    /// <summary>
    /// Wrapper for checking social share widget options in page editor
    /// </summary>
    public class SocialSharePageEditorWrapper : BaseWrapper
    {
        /// <summary>
        /// Verifies the social share options in backend.
        /// </summary>
        /// <param name="expectedNumberOfOptions">The expected number of options.</param>
        /// <param name="optionNames">The option names.</param>
        public void VerifySocialShareOptionsPresentInBackend(int expectedNumberOfOptions, params string[] optionNames)
        {
            var list = ActiveBrowser.Find.ByExpression<HtmlUnorderedList>("class=list-inline sf-social-share");
            var count = 0;
            foreach (var optionName in optionNames)
            {
                var option = list.Find.ByExpression<HtmlAnchor>("onclick=~" + optionName);
                if (option == null)
                {
                    option = list.Find.ByExpression<HtmlAnchor>("href=~" + optionName);
                    if (option == null)
                    {
                        var div = list.Find.ByExpression<HtmlDiv>("id=~" + optionName);
                        Assert.IsNotNull(div, "No such option " + optionName + " found");                       
                    }                
                }
                count++;
            }
            Assert.AreEqual(expectedNumberOfOptions, count, "Count is not correct!");
        }

        /// <summary>
        /// Verifies the social share options not existing in backend.
        /// </summary>
        /// <param name="optionNames">The option names.</param>
        public void VerifySocialShareOptionsNotPresentInBackend(params string[] optionNames)
        {
            var list = ActiveBrowser.Find.ByExpression<HtmlUnorderedList>("class=list-inline sf-social-share");
            foreach (var optionName in optionNames)
            {
                var option = list.Find.ByExpression<HtmlAnchor>("onclick=~" + optionName);
                if (option == null)
                {
                    option = list.Find.ByExpression<HtmlAnchor>("title=~" + optionName);
                    if (option == null)
                    {
                        var div = list.Find.ByExpression<HtmlDiv>("id=~" + optionName);
                        Assert.IsNull(div, optionName + " is found");                       
                    }                   
                }
                Assert.IsNull(option, optionName + " is found");
            }
        }
    }
}
