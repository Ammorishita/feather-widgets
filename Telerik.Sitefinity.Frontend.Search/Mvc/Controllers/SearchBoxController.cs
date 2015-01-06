﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Frontend.Search.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Frontend.Search.Mvc.Models;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using System.ComponentModel;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Services.Search.Configuration;
using Telerik.Sitefinity.Services;
using System.Globalization;

namespace Telerik.Sitefinity.Frontend.Search.Mvc.Controllers
{
    /// <summary>
    /// This class represents the controller of Search box widget.
    /// </summary>
    [ControllerToolboxItem(Name = "SearchBox", Title = "Search box", SectionName = "MvcWidgets")]
    [Localization(typeof(SearchWidgetsResources))]
    public class SearchBoxController : Controller
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the template that will be displayed.
        /// </summary>
        /// <value></value>
        public string TemplateName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the CSS class that will be applied on the wrapper div of the NavigationWidget (if such is presented).
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        public string CssClass
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Search box widget model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ISearchBoxModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = this.InitializeModel();

                return this.model;
            }
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            this.Model.Language = SystemManager.CurrentContext.AppSettings.Multilingual ?
                CultureInfo.CurrentUICulture.Name : null;

            return this.View("Default", this.Model);
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Initializes the model.
        /// </summary>
        /// <returns>
        /// The <see cref="INavigationModel"/>.
        /// </returns>
        private ISearchBoxModel InitializeModel()
        {
            var constructorParams = new Dictionary<string, object>
            {
                {"suggestionsRoute", "/restapi/search/suggestions"},
                {"minSuggestionLength", Config.Get<SearchConfig>().MinSuggestLength},
                {"suggestionFields", "Title,Content"}
            };
            return ControllerModelFactory.GetModel<ISearchBoxModel>(this.GetType(), constructorParams);
        }
        #endregion

        #region Private fields and constants
        private ISearchBoxModel model;
        #endregion
    }
}
