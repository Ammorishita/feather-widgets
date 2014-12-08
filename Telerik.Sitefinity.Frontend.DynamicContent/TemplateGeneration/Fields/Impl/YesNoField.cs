﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Builder.Model;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;

namespace Telerik.Sitefinity.Frontend.DynamicContent.TemplateGeneration.Fields.Impl
{
    /// <summary>
    /// This class represents field generation strategy for yes/no dynamic fields.
    /// </summary>
    public class YesNoField : Field
    {
        /// <inheritdoc/>
        public override bool GetCondition(DynamicModuleField field)
        {
            var condition = base.GetCondition(field)
                && field.FieldType == FieldType.YesNo;

            return condition;
        }

        /// <inheritdoc/>
        protected override string GetTemplatePath(DynamicModuleField field)
        {
            return YesNoField.TemplatePath;
        }

        private const string TemplatePath = "~/Frontend-Assembly/Telerik.Sitefinity.Frontend.DynamicContent/TemplateGeneration/Fields/Templates/YesNoField.cshtml";
    }
}
