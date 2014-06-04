﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.DummyClasses.ContentBlock;
using System.Web.Mvc;
using ContentBlock.Mvc.Models;
using Newtonsoft.Json;
using ContentBlock.Mvc.StringResources;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Configuration.Data;
using Telerik.Sitefinity.Localization.Configuration;
using Telerik.Sitefinity.Project.Configuration;
using UnitTests.DummyClasses;
using Telerik.Sitefinity.Localization;
using System.Linq;
using System.Reflection;
using ContentBlock.Mvc.Controllers;


namespace UnitTests.ContentBlock
{
    /// <summary>
    /// Tests methods for the Content Block 
    /// </summary>
    [TestClass]
    public class ContentBlockTests
    {
        [TestMethod]
        [Owner("Bonchev")]
        [Description("Checks whether the ContentBlockController properly creates its model and pass it to the Index action result.")]
        public void CreateContentBlockController_CallTheIndexAction_EnsuresTheModelIsProperlyCreated()
        {
            //Arrange: Create a dummy controller and set fake property values
            using (new ObjectFactoryContainerRegion())
            {
                this.RegisterResourceClasses();

                DummyContentBlockController controller = new DummyContentBlockController();
                controller.Content = "Fake controller";
                controller.ProviderName = "Fake provider";
                controller.SharedContentID = Guid.NewGuid();
                controller.EnableSocialSharing = true;

                //Act: Call the Index action and get the model from the ActionResult 
                var res = controller.Index() as ViewResult;
                var model = res.Model;
                var contentBlockModel = model as IContentBlockModel;

                //Assert: ensures the model is properly created and all its properties are properly populated
                Assert.IsNotNull(contentBlockModel, "The model is null or its not implementing the IContentBlockInterface");
                Assert.AreEqual(controller.Content, contentBlockModel.Content, "The Content property of the model is not properly set");
                Assert.AreEqual(controller.ProviderName, contentBlockModel.ProviderName, "The provider name is not properly set");
                Assert.AreEqual(controller.SharedContentID, contentBlockModel.SharedContentID, "The Id of the shared content item is not properly set");
                Assert.AreEqual(controller.EnableSocialSharing, contentBlockModel.EnableSocialSharing, "The indicator which shows if the content block allows the social share options is not properly set");
            }
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("Checks whether the ContentBlockController properly creates its model and pass it to the UseSharedContentItem action result.")]
        public void CreateContentBlockController_CallTheUseSharedContentItemAction_EnsuresTheModelIsProperlyCreated()
        {
            //Arrange: Create a dummy controller and set fake property values
            DummyContentBlockController controller = new DummyContentBlockController();
            controller.Content = "Fake controller";
            controller.ProviderName = "Fake provider";
            controller.SharedContentID = Guid.NewGuid();
            controller.EnableSocialSharing = true;

            //Act: Call the Index action and get the model from the ActionResult 
            var res = controller.UseSharedContentItem() as ViewResult;
            var model = res.Model;
            var contentBlockModel = model as IContentBlockModel;

            //Assert: ensures the model is properly created and all its properties are properly populated
            Assert.IsNotNull(contentBlockModel, "The model is null or its not implementing the IContentBlockInterface");
            Assert.AreEqual(controller.Content, contentBlockModel.Content, "The Content property of the model is not properly set");
            Assert.AreEqual(controller.ProviderName, contentBlockModel.ProviderName, "The provider name is not properly set");
            Assert.AreEqual(controller.SharedContentID, contentBlockModel.SharedContentID, "The Id of the shared content item is not properly set");
            Assert.AreEqual(controller.EnableSocialSharing, contentBlockModel.EnableSocialSharing, "The indicator which shows if the content block allows the social share options is not properly set");
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the Share action of the ContentBlockController properly gets a blank content item and assign it to the VievBag property of the view")]
        public void CreateContentBlockController_ShareAction_EnsuresAnEmptyContentItemIsAssigned()
        {
            //Arrange: Create a dummy controller and set fake property values
            DummyContentBlockController controller = new DummyContentBlockController();
            controller.Content = "Fake controller";
            controller.ProviderName = "Fake provider";
            controller.SharedContentID = Guid.NewGuid();
            controller.EnableSocialSharing = true;

            //Act: Call the Index action and get the model from the ActionResult 
            var res = controller.Share() as ViewResult;
            var contentItemString = res.ViewBag.BlankDataItem;
            var contentItem = JsonConvert.DeserializeObject<DummyContentItem>(contentItemString);

            //Assert: ensures the contentItem is properly created and all its properties are properly populated
            Assert.IsNotNull(contentItemString, "The ViewBag of the view does not contains the required JSON string ");
            Assert.IsNotNull(contentItem, "The blank item JSON string is cannot be deserialized from the ViewBag ");
            Assert.AreEqual(contentItem.Content, "DummyContent", "The Content item has incorrect content value");
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the Unshare action of the ContentBlockController uses the right view name")]
        public void CreateContentBlockController_UnshareAction_EnsuresTheActionusesTheRightViewName()
        {
            //Arrange: Create a dummy controller and set fake property values
            DummyContentBlockController controller = new DummyContentBlockController();
            controller.Content = "Fake controller";
            controller.ProviderName = "Fake provider";
            controller.SharedContentID = Guid.NewGuid();
            controller.EnableSocialSharing = true;

            //Act: Call the Index action and get the model from the ActionResult 
            var res = controller.Unshare() as ViewResult;
            var viewName = res.ViewName;

            //Assert: the action uses the right view name
            Assert.AreEqual(viewName, "Unshare", "The requested view does not have the right name");
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the UseSharedContentItem action of the ContentBlockController uses the right view name")]
        public void CreateContentBlockController_UseSharedContentItemAction_EnsuresTheActionusesTheRightViewName()
        {
            //Arrange: Create a dummy controller and set fake property values
            ContentBlockController controller = new ContentBlockController();

            //Act: Call the Index action and get the model from the ActionResult 
            var res = controller.UseSharedContentItem() as ViewResult;
            var viewName = res.ViewName;

            //Assert: the action uses the right view name
            Assert.AreEqual(viewName, "SharedContentItemSelector", "The requested view does not have the right name");
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the Index action of the ContentBlockController uses the right view name")]
        public void CreateContentBlockController_IndexAction_EnsuresTheActionusesTheRightViewName()
        {
            //Arrange: Create a dummy controller and set fake property values
            using (new ObjectFactoryContainerRegion())
            {
                this.RegisterResourceClasses();
                DummyContentBlockController controller = new DummyContentBlockController();
                controller.Content = "Fake controller";
                controller.ProviderName = "Fake provider";
                controller.SharedContentID = Guid.NewGuid();
                controller.EnableSocialSharing = true;

                //Act: Call the Index action and get the model from the ActionResult 
                var res = controller.Index() as ViewResult;
                var viewName = res.ViewName;

                //Assert: the action uses the right view name
                Assert.AreEqual(viewName, "Default", "The requested view does not have the right name");
            }
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the Share action of the ContentBlockController uses the right view name")]
        public void CreateContentBlockController_ShareAction_EnsuresTheActionusesTheRightViewName()
        {
            //Arrange: Create a dummy controller and set fake property values
            DummyContentBlockController controller = new DummyContentBlockController();
            controller.Content = "Fake controller";
            controller.ProviderName = "Fake provider";
            controller.SharedContentID = Guid.NewGuid();
            controller.EnableSocialSharing = true;

            //Act: Call the Index action and get the model from the ActionResult 
            var res = controller.Share() as ViewResult;
            var viewName = res.ViewName;

            //Assert: the action uses the right view name
            Assert.AreEqual(viewName, "Share", "The requested view does not have the right name");
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the IsShare method of the ContentBlockModel returns false as expected")]
        public void DummyContentBlockModel_IsShared_ExpectFalse()
        {
            //Arrange: Create a dummy model 
            DummyContentBlockModel model = new DummyContentBlockModel();

            //Act: Call the IIsShared method
            bool isShared = model.PublicIsShared();

            //Assert:Tests if the model is not shared as expected
            Assert.IsFalse(isShared, "The model IsShared method returns true when expecting false.");
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the controller properly creates a commands list with all commands for a content block item which is not yet shared.")]
        public void ContentBlockController_InitializeCommandsForNotSharedContent_EnsureCommandsAreCorrectly()
        {
            //Arrange: Register the ContentBlockResources and a dummy Config provider
            using (new ObjectFactoryContainerRegion())
            {
                this.RegisterResourceClasses();

                //Act:Instantiate a controller and initialize the commands
                DummyContentBlockController controller = new DummyContentBlockController();
                var commands = controller.InitializeCommands();

                //Assert:Ensures the commands list is properly created
                Assert.IsTrue(commands.Count >= 5, "Commands are less then expected.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<Labels>().Delete).Count() == 1, "Commands list does not contain the Delete command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<Labels>().Duplicate).Count() == 1, "Commands list does not contain the Duplicate command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<ContentBlockResources>().Share).Count() == 1, "Commands list does not contain the Share command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<ContentBlockResources>().UseShared).Count() == 1, "Commands list does not contain the Delete command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<Labels>().Permissions).Count() == 1, "Commands list does not contain the Delete command.");

            }
        }

        [TestMethod]
        [Owner("Bonchev")]
        [Description("The test ensures that the controller properly creates a commands list with all commands for a content block item which is already shared.")]
        public void ContentBlockController_InitializeCommandsForSharedContent_EnsureCommandsAreCorrectly()
        {
            //Arrange: Register the ContentBlockResources and a dummy Config provider
            using (new ObjectFactoryContainerRegion())
            {
                this.RegisterResourceClasses();


                //Act:Instantiate a controller and initialize the commands
                DummyContentBlockController controller = new DummyContentBlockController();
                controller.SharedContentID = Guid.NewGuid();
                var commands = controller.InitializeCommands();

                //Assert:Ensures the commands list is properly created
                Assert.IsTrue(commands.Count >= 5, "Commands are less then expected.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<Labels>().Delete).Count() == 1, "Commands list does not contain the Delete command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<Labels>().Duplicate).Count() == 1, "Commands list does not contain the Duplicate command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<ContentBlockResources>().Unshare).Count() == 1, "Commands list does not contain the Unshare command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<ContentBlockResources>().UseShared).Count() == 1, "Commands list does not contain the Delete command.");
                Assert.IsTrue(commands.Where(c => c.Text == Res.Get<Labels>().Permissions).Count() == 1, "Commands list does not contain the Delete command.");

            }
        }

        /// <summary>
        /// Registers the resource classes.
        /// </summary>
        private void RegisterResourceClasses()
        {
            var resourceClassType = typeof(ContentBlockResources);
            var labelsClassType = typeof(Labels);
            ObjectFactory.Container.RegisterType<ConfigManager, ConfigManager>(typeof(XmlConfigProvider).Name.ToUpperInvariant(),
                new InjectionConstructor(typeof(XmlConfigProvider).Name));
            ObjectFactory.Container.RegisterType<XmlConfigProvider, DummyConfigProvider>();
            Config.RegisterSection<ResourcesConfig>();
            Config.RegisterSection<ProjectConfig>();
            Res.RegisterResource(resourceClassType);
            Res.RegisterResource(labelsClassType);
        }

    }
}
