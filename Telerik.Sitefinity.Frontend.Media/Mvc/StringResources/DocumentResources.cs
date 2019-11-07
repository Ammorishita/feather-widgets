﻿using System;
using System.Linq;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace Telerik.Sitefinity.Frontend.Media.Mvc.StringResources
{
    /// <summary>
    /// Sitefinity localizable strings
    /// </summary>
    [ObjectInfo("DocumentResources", ResourceClassId = "DocumentResources", Title = "DocumentResourcesTitle", TitlePlural = "DocumentResourcesTitlePlural", Description = "DocumentResourcesDescription")]
    public class DocumentResources : Resource
    {
        #region Construction
        /// <summary>
        /// Initializes new instance of <see cref="DocumentResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public DocumentResources()
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="DocumentResources"/> class with the provided <see cref="ResourceDataProvider"/>.
        /// </summary>
        /// <param name="dataProvider"><see cref="ResourceDataProvider"/></param>
        public DocumentResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }
        #endregion

        #region Class Description
        /// <summary>
        /// The title of the class.
        /// </summary>
        /// <value>DocumentResources labels</value>
        [ResourceEntry("DocumentResourcesTitle",
            Value = "DocumentResources labels",
            Description = "The title of this class.",
            LastModified = "2015/03/23")]
        public string DocumentResourcesTitle
        {
            get
            {
                return this["DocumentResourcesTitle"];
            }
        }

        /// <summary>
        /// The plural title of this class.
        /// </summary>
        /// <value>DocumentResources labels</value>
        [ResourceEntry("DocumentResourcesTitlePlural",
            Value = "DocumentResources labels",
            Description = "The plural title of this class.",
            LastModified = "2015/03/23")]
        public string DocumentResourcesTitlePlural
        {
            get
            {
                return this["DocumentResourcesTitlePlural"];
            }
        }

        /// <summary>
        /// The description of this class.
        /// </summary>
        /// <value>Contains localizable resources.</value>
        [ResourceEntry("DocumentResourcesDescription",
            Value = "Contains localizable resources.",
            Description = "The description of this class.",
            LastModified = "2015/03/23")]
        public string DocumentResourcesDescription
        {
            get
            {
                return this["DocumentResourcesDescription"];
            }
        }
        #endregion

        #region Resources
        /// <summary>
        /// Phrase: Select a document or other file
        /// </summary>
        /// <value>Select a document or other file</value>
        [ResourceEntry("SelectDocument",
            Value = "Select a document or other file",
            Description = "Phrase: Select a document or other file",
            LastModified = "2015/03/23")]
        public string SelectDocument
        {
            get
            {
                return this["SelectDocument"];
            }
        }

        /// <summary>
        /// Phrase: was not selected or has been deleted. Please select another one.
        /// </summary>
        /// <value>A document was not selected or has been deleted. Please select another one.</value>
        [ResourceEntry("DocumentWasNotSelectedOrHasBeenDeleted",
            Value = "A document was not selected or has been deleted. Please select another one.",
            Description = "Phrase: was not selected or has been deleted. Please select another one.",
            LastModified = "2015/03/23")]
        public string DocumentWasNotSelectedOrHasBeenDeleted
        {
            get
            {
                return this["DocumentWasNotSelectedOrHasBeenDeleted"];
            }
        }

        /// <summary>
        /// Gets phrase : More options
        /// </summary>
        [ResourceEntry("MoreOptions",
            Value = "More options",
            Description = "phrase : More options",
            LastModified = "2015/03/23")]
        public string MoreOptions
        {
            get
            {
                return this["MoreOptions"];
            }
        }

        /// <summary>
        /// Gets phrase : CSS classes
        /// </summary>
        [ResourceEntry("CssClasses",
            Value = "CSS classes",
            Description = "phrase : CSS classes",
            LastModified = "2015/03/23")]
        public string CssClasses
        {
            get
            {
                return this["CssClasses"];
            }
        }

        /// <summary>
        /// Gets phrase: Template
        /// </summary>
        [ResourceEntry("Template",
            Value = "Template",
            Description = "phrase : Template",
            LastModified = "2015/03/23")]
        public string Template
        {
            get
            {
                return this["Template"];
            }
        }

        /// <summary>
        /// phrase: File extension
        /// </summary>
        /// <value>File extension</value>
        [ResourceEntry("FileExtension",
            Value = "File extension",
            Description = "phrase: File extension",
            LastModified = "2018/09/13")]
        public string FileExtension
        {
            get
            {
                return this["FileExtension"];
            }
        }

        /// <summary>
        /// phrase: File size
        /// </summary>
        /// <value>File size</value>
        [ResourceEntry("FileSize",
            Value = "File size",
            Description = "phrase: File size",
            LastModified = "2018/09/13")]
        public string FileSize
        {
            get
            {
                return this["FileSize"];
            }
        }

        /// <summary>
        /// Control name: Document link
        /// </summary>
        [ResourceEntry("DocumentLinkControlTitle",
            Value = "Document link",
            Description = "Control title: Document link",
            LastModified = "2019/06/03")]
        public string DocumentLinkControlTitle
        {
            get
            {
                return this["DocumentLinkControlTitle"];
            }
        }

        /// <summary>
        /// Control description: A control for displaying a link to download a document.
        /// </summary>
        [ResourceEntry("DocumentLinkControlDescription",
            Value = "Link for downloading a selected document of other file",
            Description = "Control description: A control for displaying a link to download a document.",
            LastModified = "2019/06/03")]
        public string DocumentLinkControlDescription
        {
            get
            {
                return this["DocumentLinkControlDescription"];
            }
        }

        #endregion
    }
}
