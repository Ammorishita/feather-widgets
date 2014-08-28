﻿using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.News;
using Telerik.Sitefinity.News.Model;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using System.Globalization;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Modules;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Taxonomies;
using ServiceStack.Text;
using System.Text;
using System.ComponentModel;

namespace News.Mvc.Models
{
    /// <summary>
    /// This class represents model used for News widget.
    /// </summary>
    public class NewsModel : INewsModel
    {
        #region Properties

        /// <inheritdoc />
        public IList<NewsItem> News
        {
            get
            {
                return this.news;
            }
            private set
            {
                this.news = value;
            }
        }

        /// <inheritdoc />
        public string ListCssClass
        {
            get;
            set;
        }

        /// <inheritdoc />
        public string DetailCssClass
        {
            get;
            set;
        }

        /// <inheritdoc />
        public Guid SelectedNewsId
        {
            get;
            set;
        }

        /// <inheritdoc />
        public NewsItem DetailNews
        {
            get;
            set;
        }

        /// <inheritdoc />
        public bool EnableSocialSharing 
        { 
            get; 
            set; 
        }

        /// <inheritdoc />
        public string ProviderName 
        { 
            get;
            set; 
        }

        /// <inheritdoc />
        public NewsSelectionMode SelectionMode
        {
            get;
            set;
        }

        /// <inheritdoc />
        public ListDisplayMode DisplayMode
        {
            get;
            set;
        }

        /// <inheritdoc />
        public int? TotalPagesCount
        {
            get;
            set;
        }

        /// <inheritdoc />
        public int CurrentPage
        {
            get;
            set;
        }

        /// <inheritdoc />
        public int? ItemsPerPage
        {
            get
            {
                return this.itemsPerPage;
            }
            set
            {
                this.itemsPerPage = value;
            }
        }

        /// <inheritdoc />
        public string SortExpression
        {
            get
            {
                return this.sortExpression;
            }
            set
            {
                this.sortExpression = value;
            }
        }

        /// <inheritdoc />
        public string FilterExpression
        {
            get;
            set;
        }

        [Browsable(false)]
        public Dictionary<string, IList<Guid>> TaxonomyFilter
        {
            get;
            set;
        }

        public string SerializedTaxonomyFilter
        {
            get
            {
                return this.serializedTaxonomyFilter;
            }
            set
            {
                if (this.serializedTaxonomyFilter != value)
                {
                    this.serializedTaxonomyFilter = value;
                    if (!this.serializedTaxonomyFilter.IsNullOrEmpty())
                    {
                        this.TaxonomyFilter = JsonSerializer.DeserializeFromString<Dictionary<string, IList<Guid>>>(this.serializedTaxonomyFilter);
                    }
                }
            }
        }

        #endregion 

        #region Public methods

        /// <inheritdoc />
        public virtual void PopulateNews(ITaxon taxonFilter, string taxonField, int? page)
        {
            this.InitializeManager();

            IQueryable<NewsItem> newsItems = this.GetNewsItems();

            if (taxonFilter != null && !taxonField.IsNullOrEmpty())
                newsItems = newsItems.Where(n => n.GetValue<IList<Guid>>(taxonField).Contains(taxonFilter.Id));

            this.AdaptMultilingualFilterExpression();

            this.ApplyListSettings(page, ref newsItems);

            this.News = newsItems.ToArray();
        }

        /// <inheritdoc />
        public virtual string CompileFilterExpression()
        {
            var elements = new List<string>();

            if (this.SelectionMode == NewsSelectionMode.FilteredNews)
            {
                var taxonomyFilterExpression = string.Join(
                    " AND ",
                    this.TaxonomyFilter
                        .Where(tf => tf.Value.Count > 0)
                        .Select(tf => "(" + string.Join(" OR ", tf.Value.Select(id => "{0}.Contains(({1}))".Arrange(tf.Key, id))) + ")")
                );
                if (!taxonomyFilterExpression.IsNullOrEmpty())
                {
                    elements.Add("(" + taxonomyFilterExpression + ")");
                }
            }

            if (!this.FilterExpression.IsNullOrEmpty())
            {
                elements.Add("(" + this.FilterExpression + ")");
            }

            return string.Join(" AND ", elements);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Gets the news items depending on the Content section of the property editor.
        /// </summary>
        /// <returns></returns>
        private IQueryable<NewsItem> GetNewsItems()
        {
            IQueryable<NewsItem> newsItems;

            if (this.SelectionMode == NewsSelectionMode.SelectedNews)
            {
                var selectedItems = new List<NewsItem>() { this.manager.GetNewsItem(this.SelectedNewsId) };
                newsItems = selectedItems.Where(n => n.Status == ContentLifecycleStatus.Live && n.Visible == true).AsQueryable();
            }
            else 
            {
                newsItems = this.manager.GetNewsItems()
                    .Where(n => n.Status == ContentLifecycleStatus.Live && n.Visible == true);
            }

            return newsItems;
        }

        /// <summary>
        /// Applies the list settings.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="newsItems">The news items.</param>
        private void ApplyListSettings(int? page, ref IQueryable<NewsItem> newsItems)
        {
            if (page == null || page < 1)
                page = 1;

            int? itemsToSkip = ((page.Value - 1) * this.ItemsPerPage);
            itemsToSkip = this.DisplayMode==ListDisplayMode.Paging? ((page.Value - 1) * this.ItemsPerPage) : null ;
            int? totalCount = 0;
            int? itemsPerPage = this.DisplayMode == ListDisplayMode.All ?  null: this.ItemsPerPage;

            var compiledFilterExpression = this.CompileFilterExpression();
            newsItems = DataProviderBase.SetExpressions(
                newsItems,
                compiledFilterExpression,
                this.SortExpression,
                itemsToSkip,
                itemsPerPage,
                ref totalCount);

            this.TotalPagesCount = (int)Math.Ceiling((double)(totalCount.Value / (double)this.ItemsPerPage.Value));
            this.TotalPagesCount = this.DisplayMode == ListDisplayMode.Paging ? this.TotalPagesCount : null;
            this.CurrentPage = page.Value;
        }

        /// <summary>
        /// Adapts the filter expression in multilingual.
        /// </summary>
        private void AdaptMultilingualFilterExpression()
        {
            CultureInfo uiCulture = null;

            if (SystemManager.CurrentContext.AppSettings.Multilingual)
            {
                uiCulture = System.Globalization.CultureInfo.CurrentUICulture;
            }

            //the filter is adapted to the implementation of ILifecycleDataItemGeneric, so the culture is taken in advance when filtering published items.
            this.FilterExpression = ContentHelper.AdaptMultilingualFilterExpressionRaw(this.FilterExpression, uiCulture);
        }

        /// <summary>
        /// Initializes the manager.
        /// </summary>
        private void InitializeManager()
        {
            NewsManager manager;

            // try to resolve manager with control definition provider
            manager = this.ResolveManagerWithProvider(this.ProviderName);
            if (manager == null)
            {
                manager = this.ResolveManagerWithProvider(null);
            }

            this.manager = manager;
        }

        /// <summary>
        /// Resolves the manager with provider.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns></returns>
        private NewsManager ResolveManagerWithProvider(string providerName)
        {
            try
            {
                return NewsManager.GetManager(providerName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion 

        #region Privte properties and constants

        private IList<NewsItem> news = new List<NewsItem>();
        private int? itemsPerPage = 2;
        private string sortExpression = "PublicationDate DESC";

        private NewsManager manager;
        private string serializedTaxonomyFilter;

        #endregion

    }
}