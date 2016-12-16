﻿using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Frontend.Identity.Mvc.Models.Profile;

namespace FeatherWidgets.TestUnit.DummyClasses.Identity
{
    /// <summary>
    /// This class is used for testing the profile controller.
    /// </summary>
    public class DummyProfileModel : IProfileModel
    {
        public string CssClass { get; set; }

        public SaveAction SaveChangesAction { get; set; }

        public Guid ProfileSavedPageId { get; set; }

        public string UserName { get; set; }

        public string ProfileBindings { get; set; }

        public string ProfileProvider { get; set; }

        public bool SendEmailOnChangePassword { get; set; }

        public string MembershipProvider { get; set; }

        public ProfilePreviewViewModel GetProfilePreviewViewModel()
        {
            return new ProfilePreviewViewModel();
        }

        public ProfileEditViewModel GetProfileEditViewModel()
        {
            return new ProfileEditViewModel();
        }

        public bool CanEdit()
        {
            return true;
        }

        public string GetPageUrl(Guid? pageId)
        {
            if (pageId.HasValue)
            {
                return "http://" + pageId.Value.ToString("D");
            }
            else
            {
                return null;
            }
        }

        public bool EditUserProfile(ProfileEditViewModel model)
        {
            return true;
        }

        public Guid GetUserId()
        {
            return Guid.NewGuid();
        }

        public void InitializeUserRelatedData(ProfileEditViewModel model, bool emailUpdate = true)
        {
            // Do nothing.
        }

        public void ValidateProfileData(ProfileEditViewModel viewModel, System.Web.Mvc.ModelStateDictionary modelState)
        {
            // Do nothing.
        }
    }
}
