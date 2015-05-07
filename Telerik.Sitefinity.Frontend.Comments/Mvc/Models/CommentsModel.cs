﻿using System;
using System.ComponentModel;
using Telerik.Sitefinity.Modules.Comments;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Services.Comments;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.UI;

namespace Telerik.Sitefinity.Frontend.Comments.Mvc.Models
{
    /// <summary>
    /// Provides API for working with <see cref="Telerik.Sitefinity.Services.Comments.IComment"/> items.
    /// </summary>
    public class CommentsModel : ICommentsModel
    {
        /// <inheritDoc/>
        public string CssClass { get; set; }

        /// <inheritDoc/>
        public string ThreadKey
        {
            get
            {
                if (this.threadKey.IsNullOrEmpty() && SiteMapBase.GetActualCurrentNode() != null)
                {
                    this.threadKey = ControlUtilities.GetLocalizedKey(SiteMapBase.GetActualCurrentNode().Id, null, CommentsBehaviorUtilities.GetLocalizedKeySuffix(this.ThreadType));
                }

                return this.threadKey;
            }
            set
            {
                this.threadKey = value;
            }
        }

        /// <inheritDoc/>
        public string ThreadType
        {
            get
            {
                if (this.threadType.IsNullOrEmpty())
                {
                    this.threadType = typeof(PageNode).ToString();
                }

                return this.threadType;
            }
            set 
            {
                this.threadType = value;
            }
        }

        /// <inheritDoc/>
        public string GroupKey
        {
            get
            {
                if (this.groupKey.IsNullOrEmpty())
                {
                    this.groupKey = SystemManager.CurrentContext.CurrentSite.Id.ToString();
                }

                return this.groupKey;
            }
            set
            {
                this.groupKey = value;
            }
        }

        /// <inheritDoc/>
        public string ThreadTitle
        {
            get
            {
                if (this.threadTitle.IsNullOrEmpty() && SiteMapBase.GetActualCurrentNode() != null)
                {
                    this.threadTitle = SiteMapBase.GetActualCurrentNode().Title;
                }

                return this.threadTitle;
            }
            set
            {
                this.threadTitle = value;
            }
        }

        /// <inheritDoc/>
        public string DataSource { get; set; }

        /// <inheritDoc/>
        public bool? AllowComments { get; set; }

        /// <inheritDoc/>
        public bool ThreadIsClosed 
        { 
            get
            {
                var thread = this.GetThread();
                return this.threadIsClosed || (thread != null && thread.IsClosed);
            }
            set
            {
                this.threadIsClosed = value;
            }
        }

        /// <inheritDoc/>
        public int CommentTextMaxLength
        {
            get 
            {
                return this.commentTextMaxLength;
            }

            set 
            {
                this.commentTextMaxLength = value;
            }
        }

        /// <inheritDoc/>
        [Browsable(false)]
        public bool ShowComments
        {
            get
            {
                return (this.AllowComments.HasValue ? this.AllowComments.Value : this.ThreadsConfig.AllowComments);
            }
        }

        /// <summary>
        /// Gets the configuration for the thread
        /// </summary>
        [Browsable(false)]
        public ThreadsConfigModel ThreadsConfig
        {
            get
            {
                if (this.threadsConfig == null)
                {
                    this.threadsConfig = new ThreadsConfigModel(this.ThreadType);
                }
                return this.threadsConfig;
            }
        }

        /// <summary>
        /// Gets the configuration for the comments module
        /// </summary>
        [Browsable(false)]
        public CommentsConfigModel CommentsConfig
        {
            get
            {
                if (this.commentsConfig == null)
                {
                    this.commentsConfig = new CommentsConfigModel();
                }
                return this.commentsConfig;
            }
        }

        private IThread GetThread()
        {
            var cs = SystemManager.GetCommentsService();
            IThread thread;
            if (!this.ThreadKey.IsNullOrEmpty())
            {
                thread = cs.GetThread(this.ThreadKey);
            }
            else
            {
                thread = null;
            }

            return thread;
        }

        private string threadType;
        private string threadTitle;
        private string groupKey;
        private string threadKey;
        private ThreadsConfigModel threadsConfig;
        private CommentsConfigModel commentsConfig;
        private int commentTextMaxLength = 100;
        private bool threadIsClosed;
    }
}
