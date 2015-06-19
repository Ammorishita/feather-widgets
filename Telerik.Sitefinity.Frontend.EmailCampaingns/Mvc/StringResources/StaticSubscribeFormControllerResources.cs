﻿using System;
using System.Linq;
using Telerik.Sitefinity.Localization;

namespace Telerik.Sitefinity.Frontend.EmailCampaigns.Mvc.StringResources
{
    /// <summary>
    /// This class is used as a resource class for localization of strings rendered by the MVC framework. It should not be used directly.
    /// </summary>
    public static class StaticSubscribeFormResources
    {
        public static string EmailAddressErrorMessageResourceName
        {
            get
            {
                return Res.Get<SubscribeFormResources>().EmailAddressErrorMessageResourceName;
            }
        }
    }
}
