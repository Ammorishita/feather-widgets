﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtOfTest.Common.UnitTesting;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.jQuery;

namespace Feather.Widgets.TestUI.Framework.Framework.Wrappers.Frontend.EmailCampaigns
{
    /// <summary>
    /// This is the entry point class for unsubscribe on the frontend.
    /// </summary>
    public class UnsubscribeWrapper : BaseWrapper
    {
        /// <summary>
        /// Verify unsubscribe message
        /// </summary>
        public void VerifyUnsubscribeMessageOnTheFrontend()
        {
            HtmlForm unSubscribeForm = this.EM.EmailCampaigns.UnsubscribeFrontend.UnsubscribeForm.AssertIsPresent("Unsubscribe form");
            bool isPresentSubscribe = unSubscribeForm.InnerText.Contains("Unsubscribe");
            Assert.IsTrue(isPresentSubscribe);

            bool isPresentMessage = unSubscribeForm.InnerText.Contains("Unsubscribe from our email newsletter");
            Assert.IsTrue(isPresentMessage);
        }

        /// <summary>
        /// Press unsubscribe button
        /// </summary>
        public void ClickUnsubscribeButton()
        {
            HtmlButton unsubscribeButton = EM.EmailCampaigns.UnsubscribeFrontend.UnsubscribeButton
            .AssertIsPresent("Unsubscribe button");
            unsubscribeButton.MouseClick();
            ActiveBrowser.WaitUntilReady();
            ActiveBrowser.WaitForAsyncJQueryRequests();
        }

        /// <summary>
        ///  Verify successfully unsubscribed message
        /// </summary>
        /// <param name="email">Email</param>
        public void VerifySuccessfullyUnsubscribeMessageOnTheFrontend(string email)
        {
            HtmlForm unsubscribeForm = this.EM.EmailCampaigns.UnsubscribeFrontend.UnsubscribeForm.AssertIsPresent("Unsubscribe form");
            bool isPresentSubscribe = unsubscribeForm.InnerText.Contains("You have been successfully unsubscribed from our newsletter (" + email + ")");
            Assert.IsTrue(isPresentSubscribe);
        }
    }
}
