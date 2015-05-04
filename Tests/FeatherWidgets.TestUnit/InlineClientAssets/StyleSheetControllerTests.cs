﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.InlineClientAssets.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.InlineClientAssets.Mvc.Models;
using Telerik.Sitefinity.Frontend.InlineClientAssets.Mvc.Models.StyleSheet;
using Telerik.Sitefinity.Frontend.InlineClientAssets.Mvc.StringResources;

namespace FeatherWidgets.TestUnit.InlineClientAssets
{
    /// <summary>
    /// Unit tests for the StyleSheetController.
    /// </summary>
    [TestClass]
    public class StyleSheetControllerTests
    {
        [TestMethod]
        [Owner("Boyko-Karadzhov")]
        [Description("Calls the Index view without description in the model and verifies that the CSS is displayed and added to head.")]
        public void Index_NoDescription_AddsToHeadAndDisplaysMarkup()
        {
            var controller = new StyleSheetTestController();
            var result = controller.Index();

            Assert.AreEqual(StyleSheetTestModel.Markup, controller.HeadContent, "CSS markup was not added to the Head of the page.");
            var content = result as ContentResult;
            Assert.IsNotNull(content, "The result was not of the expected type.");
            Assert.IsTrue(content.Content.Contains(StyleSheetTestModel.Markup), "The Content did not contain the CSS markup.");
        }

        [TestMethod]
        [Owner("Boyko-Karadzhov")]
        [Description("Calls the Index view with description and verifies that the description is displayed and CSS added to head.")]
        public void Index_Description_AddsToHeadAndDisplaysDescription()
        {
            var description = "Test Description";
            var controller = new StyleSheetTestController();
            controller.Model.Description = description;
            var result = controller.Index();

            Assert.AreEqual(StyleSheetTestModel.Markup, controller.HeadContent, "CSS markup was not added to the Head of the page.");
            var content = result as ContentResult;
            Assert.IsNotNull(content, "The result was not of the expected type.");
            Assert.AreEqual(description, content.Content, "The result is not the expected description.");
        }

        private class StyleSheetTestController : StyleSheetController
        {
            public override IStyleSheetModel Model
            {
                get
                {
                    return this.model;
                }
            }

            public string HeadContent { get; set; }

            protected override bool ShouldDisplayContent()
            {
                return true;
            }

            protected override void AddCssInHead(string cssMarkup)
            {
                this.HeadContent = cssMarkup;
            }

            protected override string Resource<TResource>(string key)
            {
                if (typeof(TResource) == typeof(StyleSheetResources) && key == "IncludedInHead")
                {
                    return "Included in the HTML &lt;head&gt; tag";
                }

                return base.Resource<TResource>(key);
            }

            private readonly StyleSheetTestModel model = new StyleSheetTestModel();
        }

        private class StyleSheetTestModel : IStyleSheetModel
        {
            public string InlineStyles { get; set; }

            public string ResourceUrl { get; set; }

            public string Description { get; set; }

            public string MediaType { get; set; }

            public ResourceMode Mode { get; set; }

            public string GetMarkup()
            {
                return StyleSheetTestModel.Markup;
            }

            public string GetShortInlineStylesEncoded()
            {
                return StyleSheetTestModel.Markup;
            }

            public const string Markup = "Test CSS markup";
        }
    }
}
