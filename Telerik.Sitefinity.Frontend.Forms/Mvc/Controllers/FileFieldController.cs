﻿using Telerik.Sitefinity.Data.Metadata;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers.Base;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.FileField;
using Telerik.Sitefinity.Frontend.Forms.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Mvc;

namespace Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "MvcFileField", Title = "File upload", Toolbox = FormsConstants.FormControlsToolboxName, SectionName = FormsConstants.CommonSectionName, CssClass = FileFieldController.WidgetIconCssClass)]
    [DatabaseMapping(UserFriendlyDataType.FileUpload)]
    [Localization(typeof(FieldResources))]
    public class FileFieldController : FormFieldControllerBase<IFileFieldModel>
    {
        public override IFileFieldModel Model 
        {
            get
            {
                if (this.model == null)
                    this.model = ControllerModelFactory.GetModel<IFileFieldModel>(this.GetType());

                return this.model;
            }
        }

        private IFileFieldModel model;
        private const string WidgetIconCssClass = "sfFileUploadIcn sfMvcIcn";
    }
}
