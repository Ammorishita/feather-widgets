﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Sitefinity.ContentLocations;
using Telerik.Sitefinity.Modules;
using Telerik.Sitefinity.Modules.Libraries;
using SfDocument = Telerik.Sitefinity.Libraries.Model.Document;

namespace Telerik.Sitefinity.Frontend.Media.Mvc.Models.Document
{
    public class DocumentModel :IDocumentModel
    {
        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentModel" /> class.
        /// </summary>
        public DocumentModel()
        {
        }
        #endregion

        #region IDocumentModel implementation
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <inheritdoc />
        public string ProviderName { get; set; }

        /// <inheritdoc />
        public string CssClass { get; set; }

        /// <inheritdoc />
        public virtual DocumentViewModel GetViewModel()
        {
            var viewModel = new DocumentViewModel()
            {               
                CssClass = this.CssClass
            };

            if (this.Id != Guid.Empty)
            {
                SfDocument document;
                viewModel.DocumentWasNotFound = !this.TryGetDocument(out document);

                if (viewModel.DocumentWasNotFound)
                    return viewModel;

                viewModel.MediaUrl = this.ResolveMediaUrl(document);
                viewModel.Title = this.GetTitle(document);
                viewModel.FileSize = this.GetFileSize(document);
                viewModel.Extension = this.GetExtension(document);
            }
            else
            {
                viewModel.DocumentWasNotFound = true;
            }

            return viewModel;
        }

        /// <inheritdoc />
        public IEnumerable<IContentLocationInfo> GetLocations()
        {
            var location = new ContentLocationInfo();
            location.ContentType = typeof(SfDocument);
            location.ProviderName = this.ProviderName;

            var imageItem = LibrariesManager.GetManager(this.ProviderName).GetImage(this.Id);
            var filterExpression = string.Format("(Id = {0} OR OriginalContentId = {1})", this.Id.ToString("D"), imageItem.OriginalContentId);
            location.Filters.Add(new BasicContentLocationFilter(filterExpression));

            return new[] { location };
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <returns></returns>
        protected virtual bool TryGetDocument(out SfDocument document)
        {
            LibrariesManager librariesManager = LibrariesManager.GetManager(this.ProviderName);
            document = librariesManager.GetDocuments()
                .Where(i => i.Id == this.Id)
                .Where(PredefinedFilters.PublishedItemsFilter<SfDocument>())
                .FirstOrDefault();

            return document != null;
        }

        /// <summary>
        /// Resolves the media URL of the given document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The absolute media url.</returns>
        protected virtual string ResolveMediaUrl(SfDocument document)
        {
            return document.ResolveMediaUrl();
        }

        /// <summary>
        /// Gets the title of the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        protected virtual string GetTitle(SfDocument document)
        {
            return document.Title;
        }

        /// <summary>
        /// Gets the file extension of the given document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The extension without the dot at the beginning.</returns>
        protected virtual string GetExtension(SfDocument document)
        {
            var ext = document.Extension;
            if (ext.Length > 0)
                ext = ext.Remove(0, 1);

            return ext;
        }

        /// <summary>
        /// Gets the file size of the given document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The size in KB.</returns>
        protected virtual long GetFileSize(SfDocument document)
        {
            return (long)Math.Ceiling(document.TotalSize / 1024d);
        }
        #endregion
    }
}
