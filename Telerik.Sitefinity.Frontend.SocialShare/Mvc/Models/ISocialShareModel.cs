namespace Telerik.Sitefinity.Frontend.SocialShare.Mvc.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Classes that implement this interface could be used as model for the Social share widget.
    /// </summary>
    public interface ISocialShareModel
    {
        /// <summary>
        /// Gets the social buttons
        /// </summary>
        ICollection<SocialButtonModel> SocialButtons { get; }

        /// <summary>
        /// Gets or sets the CSS class that will be applied on the wrapper div of the widget.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        string CssClass { get; set; }

        /// <summary>
        /// Gets the basic settings social share depending on the Sitefinity configurations.
        /// </summary>
        void InitializeSocialShareButtons(IList<SocialShareGroupMap> socialShareMaps);
    }
}