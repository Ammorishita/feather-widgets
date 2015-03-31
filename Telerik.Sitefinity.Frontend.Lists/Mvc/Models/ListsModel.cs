﻿using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Lists.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Lists;
using Telerik.Sitefinity.Taxonomies.Model;

namespace Telerik.Sitefinity.Frontend.Lists.Mvc.Models
{
    /// <summary>
    /// This class is used as a model for Lists widget.
    /// </summary>
    public class ListsModel : ContentModelBase, IListsModel
    {
        /// <inheritdoc />
        public override Type ContentType
        {
            get
            {
                return typeof(List);
            }
            set
            {

            }
        }

        /// <inheritdoc />
        public override string SerializedSelectedItemsIds
        {
            get
            {
                return base.SerializedSelectedItemsIds;
            }
            set
            {
                base.SerializedSelectedItemsIds = value;

                this.selectedItems = JsonSerializer.DeserializeFromString<IList<string>>(this.SerializedSelectedItemsIds);
            }
        }

        /// <inheritdoc />
        public bool IsEmpty
        {
            get
            {
                return this.selectedItems.Count == 0;
            }
        }

        /// <inheritdoc />
        public override ContentListViewModel CreateListViewModel(ITaxon taxonFilter, int page)
        {
            if (page < 1)
                throw new ArgumentException("'page' argument has to be at least 1.", "page");

            var query = this.GetItemsQuery();
            if (query == null)
                return this.CreateListViewModelInstance();

            var viewModel = this.CreateListViewModelInstance();
            this.PopulateListViewModel(page, query, viewModel);

            foreach (var listModel in viewModel.Items.Cast<ListViewModel>())
            {
                var listItemModel = new ListItemModel(listModel)
                {
                    SortExpression = this.SortExpression,
                    FilterExpression = this.FilterExpression,
                    SerializedAdditionalFilters = this.SerializedAdditionalFilters,
                    // We need only filter list items.
                    SelectionMode = SelectionMode.FilteredItems
                };

                listModel.ListItemViewModel = listItemModel.CreateListViewModel(taxonFilter, page);
            }

            return viewModel;
        }

        /// <inheritdoc />
        protected override ItemViewModel CreateItemViewModelInstance(IDataItem item)
        {
            return new ListViewModel(item);
        }

        /// <inheritdoc />
        protected override string CompileFilterExpression()
        {
            var elements = new List<string>();

            string filterExpression = this.GetSelectedItemsFilterExpression();

            if (string.IsNullOrWhiteSpace(filterExpression))
            {
                return string.Empty;
            }

            elements.Add(filterExpression);

            return string.Join(" AND ", elements.Select(el => "(" + el + ")"));
        }

        /// <inheritdoc />
        protected override IQueryable<IDataItem> UpdateExpression(IQueryable<IDataItem> query, int? skip, int? take, ref int? totalCount)
        {
            var compiledFilterExpression = this.CompileFilterExpression();

            query = this.SetExpression(
                         query,
                         compiledFilterExpression,
                         this.SortExpression,
                         skip,
                         take,
                         ref totalCount);

            return query;
        }

        private string GetSelectedItemsFilterExpression()
        {
            var selectedItemGuids = selectedItems.Select(id => new Guid(id));
            var masterIds = this.GetItemsQuery()
                                .OfType<Content>()
                                .Where(c => selectedItemGuids.Contains(c.Id) || selectedItemGuids.Contains(c.OriginalContentId))
                                .Select(n => n.OriginalContentId != Guid.Empty ? n.OriginalContentId : n.Id)
                                .Distinct();

            var selectedItemConditions = masterIds.Select(id => "Id = {0} OR OriginalContentId = {0}".Arrange(id.ToString("D")));
            var selectedItemsFilterExpression = string.Join(" OR ", selectedItemConditions);

            return selectedItemsFilterExpression;
        }

        /// <inheritdoc />
        protected override IQueryable<IDataItem> GetItemsQuery()
        {
            var manager = (ListsManager)this.GetManager();

            return manager.GetLists();
        }

        private IList<string> selectedItems = new List<string>();
    }
}
