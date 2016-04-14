﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatherWidgets.TestUtilities.CommonOperations;
using FeatherWidgets.TestUtilities.CommonOperations.Forms;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Telerik.Sitefinity.TestArrangementService.Attributes;
using Telerik.Sitefinity.TestUtilities.CommonOperations;

namespace FeatherWidgets.TestUI.Arrangements
{
    /// <summary>
    /// ChangeSectionHeaderTemplate arrangement class.
    /// </summary>
    public class ChangeSectionHeaderTemplate : TestArrangementBase
    {
        /// <summary>
        /// Server side set up.
        /// </summary>
        [ServerSetUp]
        public void SetUp()
        {
            string filePath = FileInjectHelper.GetDestinationFilePath(this.viewPath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            Stream destination = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            var assembly = ServerOperationsFeather.Pages().GetTestUtilitiesAssembly();
            Stream source = assembly.GetManifestResourceStream(FileResource);
            FileInjectHelper.CopyStream(source, destination);
            source.Close();
            destination.Close();

            var fields = new FormFieldType[] 
            { 
                FormFieldType.SectionHeader,
                FormFieldType.SubmitButton
            };

            var formId = ServerOperationsFeather.Forms().CreateFormWithWidgets(fields, FormName);

            Guid pageId = ServerOperations.Pages().CreatePage(PageName);
            ServerOperationsFeather.Forms().AddFormControlToPage(pageId, formId, FeatherGlobals.FormName, "Body");
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [ServerTearDown]
        public void TearDown()
        {
            ServerOperations.Forms().DeleteAllForms();
            ServerOperations.Pages().DeleteAllPages();
            ServerOperations.Widgets().DeleteWidgetTemplate(TemplateName);
        }

        private const string PageName = "FormPage";
        private const string FormName = "NewForm";
        private const string TemplateName = "Read.DefaultSectionHeaderNew";
        private const string FileResource = "FeatherWidgets.TestUtilities.Data.Forms.Read.DefaultSectionHeaderFile.cshtml";
        private string viewPath = Path.Combine("Mvc", "Views", "SectionHeader", ViewName);
        private const string ViewName = "Read.DefaultSectionHeaderFile.cshtml";       
    }
}
