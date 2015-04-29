﻿using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace Telerik.Sitefinity.Frontend.Comments.Mvc.StringResources
{
    /// <summary>
    /// Localizable strings for the Comments widget
    /// </summary>
    [ObjectInfo(typeof(CommentsResources), ResourceClassId = "CommentsResources", Title = "CommentsResourcesTitle", Description = "CommentsResourcesDescription")]
    public class CommentsResources : Resource
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsResources"/> class. 
        /// Initializes new instance of <see cref="CommentsResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public CommentsResources()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsResources"/> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        public CommentsResources(ResourceDataProvider dataProvider)
            : base(dataProvider)
        {
        }

        #endregion

        #region Meta resources

        /// <summary>
        /// Gets Comments widget resources title.
        /// </summary>
        [ResourceEntry("CommentsResourcesTitle",
            Value = "Comments widget resources",
            Description = "Title for the comments widget resources class.",
            LastModified = "2015/04/29")]
        public string CommentsResourcesTitle
        {
            get
            {
                return this["CommentsResourcesTitle"];
            }
        }

        /// <summary>
        /// Gets Comments widget resources description.
        /// </summary>
        [ResourceEntry("CommentsResourcesDescription",
            Value = "Localizable strings for the Comments widget.",
            Description = "Description for the comments widget resources class.",
            LastModified = "2015/04/29")]
        public string CommentsResourcesDescription
        {
            get
            {
                return this["CommentsResourcesDescription"];
            }
        }

        #endregion

        #region Frontend resources

        #endregion

        #region Designer resources

        #endregion
    }
}
