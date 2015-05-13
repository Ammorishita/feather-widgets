﻿using System;
using System.Linq;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace Telerik.Sitefinity.Frontend.Navigation.Mvc.StringResources
{
    /// <summary>
    /// Sitefinity localizable strings for the Site selector widget.
    /// </summary>
    [ObjectInfo("SiteSelectorResources", ResourceClassId = "SiteSelectorResources", Title = "SiteSelectorResourcesTitle", TitlePlural = "SiteSelectorResourcesTitlePlural", Description = "SiteSelectorResourcesDescription")]
    public class SiteSelectorResources : Resource
    {
        #region Construction
        /// <summary>
        /// Initializes new instance of <see cref="SiteSelectorResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public SiteSelectorResources()
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="SiteSelectorResources"/> class with the provided <see cref="ResourceDataProvider"/>.
        /// </summary>
        /// <param name="dataProvider"><see cref="ResourceDataProvider"/></param>
        public SiteSelectorResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Class Description
        /// <summary>
        /// The title of the class.
        /// </summary>
        /// <value>SiteSelectorResources labels</value>
        [ResourceEntry("SiteSelectorResourcesTitle",
            Value = "SiteSelectorResources labels",
            Description = "The title of this class.",
            LastModified = "2015/05/12")]
        public string SiteSelectorResourcesTitle
        {
            get
            {
                return this["SiteSelectorResourcesTitle"];
            }
        }

        /// <summary>
        /// The plural title of this class.
        /// </summary>
        /// <value>SiteSelectorResources labels</value>
        [ResourceEntry("SiteSelectorResourcesTitlePlural",
            Value = "SiteSelectorResources labels",
            Description = "The plural title of this class.",
            LastModified = "2015/05/12")]
        public string SiteSelectorResourcesTitlePlural
        {
            get
            {
                return this["SiteSelectorResourcesTitlePlural"];
            }
        }

        /// <summary>
        /// The description of this class.
        /// </summary>
        /// <value>Contains localizable resources.</value>
        [ResourceEntry("SiteSelectorResourcesDescription",
            Value = "Contains localizable resources.",
            Description = "The description of this class.",
            LastModified = "2015/05/12")]
        public string SiteSelectorResourcesDescription
        {
            get
            {
                return this["SiteSelectorResourcesDescription"];
            }
        }
        #endregion

        #region Resources
        /// <summary>
        /// Phrase: Include the current site in the selector
        /// </summary>
        /// <value>Include the current site in the selector</value>
        [ResourceEntry("IncludeCurrentSite",
            Value = "Include the current site in the selector",
            Description = "Phrase: Include the current site in the selector",
            LastModified = "2015/05/13")]
        public string IncludeCurrentSite
        {
            get
            {
                return this["IncludeCurrentSite"];
            }
        }

        /// <summary>
        /// Phrase: Display each language version as a separate sit
        /// </summary>
        /// <value>Display each language version as a separate site</value>
        [ResourceEntry("LanguageVersionAsSeparateSite",
            Value = "Display each language version as a separate site",
            Description = "Phrase: Display each language version as a separate sit",
            LastModified = "2015/05/13")]
        public string LanguageVersionAsSeparateSite
        {
            get
            {
                return this["LanguageVersionAsSeparateSite"];
            }
        }

        /// <summary>
        /// Phrase: Show site names and languages
        /// </summary>
        /// <value>Show site names and languages</value>
        [ResourceEntry("SiteNamesAndLanguages",
            Value = "Show site names and languages",
            Description = "Phrase: Show site names and languages",
            LastModified = "2015/05/13")]
        public string SiteNamesAndLanguages
        {
            get
            {
                return this["SiteNamesAndLanguages"];
            }
        }

        /// <summary>
        /// Phrase: Show languages only
        /// </summary>
        /// <value>Show languages only</value>
        [ResourceEntry("LanguagesOnly",
            Value = "Show languages only",
            Description = "Phrase: Show languages only",
            LastModified = "2015/05/13")]
        public string LanguagesOnly
        {
            get
            {
                return this["LanguagesOnly"];
            }
        }

        /// <summary>
        /// Phrase: Template
        /// </summary>
        /// <value>Template</value>
        [ResourceEntry("Template",
            Value = "Template",
            Description = "Phrase: Template",
            LastModified = "2015/05/13")]
        public string Template
        {
            get
            {
                return this["Template"];
            }
        }

        /// <summary>
        /// Phrase: More options
        /// </summary>
        /// <value>More options</value>
        [ResourceEntry("MoreOptions",
            Value = "More options",
            Description = "Phrase: More options",
            LastModified = "2015/05/13")]
        public string MoreOptions
        {
            get
            {
                return this["MoreOptions"];
            }
        }

        /// <summary>
        /// Phrase: CSS classes
        /// </summary>
        /// <value>CSS classes</value>
        [ResourceEntry("CssClasses",
            Value = "CSS classes",
            Description = "Phrase: CSS classes",
            LastModified = "2015/05/13")]
        public string CssClasses
        {
            get
            {
                return this["CssClasses"];
            }
        }
        #endregion
    }
}
