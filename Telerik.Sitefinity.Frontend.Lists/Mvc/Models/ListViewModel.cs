﻿using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Model;

namespace Telerik.Sitefinity.Frontend.Lists.Mvc.Models
{
    /// <summary>
    /// This class represents List view model.
    /// </summary>
    public class ListViewModel : ItemViewModel
    {
        public ListViewModel(IDataItem listItem)
            : base(listItem)
        {
        }
      
        /// <summary>
        /// Gets or sets the list item view model.
        /// </summary>
        /// <value>The list item view model.</value>
        public ContentListViewModel ListItemViewModel { get; set; }
    }
}
