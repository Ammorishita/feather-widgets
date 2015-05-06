﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telerik.Sitefinity.Frontend.Navigation.Mvc.Models.LanguageSelector
{
    /// <summary>
    /// This class represents the Language selector model that will rendered inside the Language selector widget.
    /// </summary>
    public class LanguageSelectorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSelectorViewModel" /> class.
        /// </summary>
        public LanguageSelectorViewModel()
        {
            this.Languages = new List<LanguageSelectorItem>();
        }

        /// <summary>
        /// Gets the languages that should be displayed.
        /// </summary>
        /// <value>The languages.</value>
        public IList<LanguageSelectorItem> Languages { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the current language in the list of languages.
        /// </summary>
        /// <value><c>true</c> in order to include the current language in the list of languages; otherwise, <c>false</c>.</value>
        public bool IncludeCurrentLanguage { get; set; }
    }
}
