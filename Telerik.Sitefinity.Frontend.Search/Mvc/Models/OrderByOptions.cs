﻿using System;
using System.Linq;

namespace Telerik.Sitefinity.Frontend.Search.Mvc.Models
{
    /// <summary>
    /// The sorting options that are available
    /// </summary>
    public enum OrderByOptions
    {
        /// <summary>
        /// The most relevant results first.
        /// </summary>
        Relevance,

        /// <summary>
        /// The newest results firs.
        /// </summary>
        Newest,

        /// <summary>
        /// The oldest results firs.
        /// </summary>
        Oldest,

        /// <summary>
        /// Sort by Title (A-Z)
        /// </summary>
        TitleAsc,

        /// <summary>
        /// Sort by Title (Z-A)
        /// </summary>
        TitleDesc,

        /// <summary>
        /// The newly modified results first.
        /// </summary>
        NewModified
    }
}
