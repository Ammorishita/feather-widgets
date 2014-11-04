﻿using System;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Model;

namespace DynamicContent.Mvc.Model
{
    /// <summary>
    /// This class represents model used for DynamicContent widget.
    /// </summary>
    public class DynamicContentModel : ContentModelBase, IDynamicContentModel
    {
        /// <inheritdoc />
        protected override IQueryable<IDataItem> GetItemsQuery()
        {
            if (this.ContentType == null)
                throw new InvalidOperationException("ContentType cannot be inferred from the WidgetName. A required module might be deactivated.");

            var manager = DynamicModuleManager.GetManager(this.ProviderName);
            return manager.GetDataItems(this.ContentType);
        }
    }
}
