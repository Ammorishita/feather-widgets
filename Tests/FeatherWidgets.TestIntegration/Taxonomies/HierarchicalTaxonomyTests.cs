﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using MbUnit.Framework;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Frontend.Taxonomies.Mvc.Models.HierarchicalTaxonomy;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;
using Telerik.Sitefinity.TestUtilities;
using Telerik.Sitefinity.TestUtilities.CommonOperations;

namespace FeatherWidgets.TestIntegration.Taxonomies
{
    [TestFixture]
    [AssemblyFixture]
    [Description("Integration tests for the Hierarchical taxonomy model.")]
    [Author(TestAuthor.Team7)]
    public class HierarchicalTaxonomyTests
    {
        [FixtureSetUp]
        public void FixtureSetUp()
        {
            using (new ElevatedModeRegion(TaxonomyManager.GetManager()))
            {
                var taxonomyOperations = new TaxonomiesOperations();

                foreach (var taxon in this.taxaNames)
                {
                    string parentName;
                    string taxonName;

                    this.GetTaxonNameAndParentName(taxon, out parentName, out taxonName);

                    taxonomyOperations.CreateHierarchicalTaxon(taxonName, parentName, "Categories");
                }

                var newsOperations = new NewsOperations();
                var newsItemsId = Guid.NewGuid();
                newsOperations.CreateNewsItem("n1", newsItemsId);
            }
        }

        [FixtureTearDown]
        public void FixtureTearDown()
        {
            var newsOperations = new NewsOperations();
            newsOperations.DeleteAllNews();

            this.DeleteAllCategories();
        }

        [Test]
        [Author(TestAuthor.Team7)]
        [Description("Verifies that if the option to show all taxa is set, all taxa will be retrieved.")]
        public void Categories_VerifyAllTaxaIsRetrieved()
        {
            var model = new HierarchicalTaxonomyModel();
            model.TaxaToDisplay = HierarchicalTaxaToDisplay.All;
            model.ShowEmptyTaxa = true;

            var viewModel = model.CreateViewModel();

            Assert.AreEqual(this.taxaNames.Count, viewModel.Taxa.Count);

            for (int i = 0; i < viewModel.Taxa.Count; i++)
            {
                string parentName;
                string taxonName;
                this.GetTaxonNameAndParentName(this.taxaNames[i], out parentName, out taxonName);

                var actual = viewModel.Taxa[i];

                Assert.AreEqual(taxonName, actual.Title);
            }
        }

        [Test]
        [Author(TestAuthor.Team7)]
        [Description("Verifies that if the option to show specific taxa is set, the exactly same taxa will be retrieved.")]
        public void Categories_VerifySpecificTaxaIsRetrieved()
        {
            var model = new HierarchicalTaxonomyModel();
            model.TaxaToDisplay = HierarchicalTaxaToDisplay.Selected;
            model.ShowEmptyTaxa = true;

            var selectedTaxaIds = TaxonomyManager.GetManager()
                .GetTaxa<HierarchicalTaxon>()
                .Where(t => this.taxaNames[1] == t.Title.ToString() || this.taxaNames[2] == t.Title.ToString())
                .Select(t => t.Id);

            var serializer = new JavaScriptSerializer();
            model.SerializedSelectedTaxaIds = serializer.Serialize(selectedTaxaIds);

            var viewModel = model.CreateViewModel();

            Assert.AreEqual(2, viewModel.Taxa.Count);

            for (int i = 0; i < viewModel.Taxa.Count; i++)
            {
                var expected = this.taxaNames[i + 1];
                var actual = viewModel.Taxa[i];

                Assert.AreEqual(expected, actual.Title);
            }
        }

        [Test]
        [Author(TestAuthor.Team7)]
        [Description("Verifies that only top level taxa is retrieved if that option is selected.")]
        public void Categories_VerifyTopLevelTaxaIsRetrieved()
        {
            var model = new HierarchicalTaxonomyModel();
            model.TaxaToDisplay = HierarchicalTaxaToDisplay.TopLevel;
            model.ShowEmptyTaxa = true;

            var viewModel = model.CreateViewModel();

            var topLevelNames = this.taxaNames.Where(tn => !tn.Contains('|')).ToList();

            Assert.AreEqual(topLevelNames.Count, viewModel.Taxa.Count);

            for (int i = 0; i < viewModel.Taxa.Count; i++)
            {
                var expected = topLevelNames[i];
                var actual = viewModel.Taxa[i];

                Assert.AreEqual(expected, actual.Title);
            }
        }

        [Test]
        [Author(TestAuthor.Team7)]
        [Description("Verifies that only taxa under particular parent is retrieved if that option is selected.")]
        public void Categories_VerifyTaxaUnderParticularParentIsRetrieved()
        {
            var model = new HierarchicalTaxonomyModel();
            model.TaxaToDisplay = HierarchicalTaxaToDisplay.UnderParticularTaxon;
            model.ShowEmptyTaxa = true;

            var rootTaxon = TaxonomyManager.GetManager().GetTaxa<HierarchicalTaxon>().FirstOrDefault(t => t.Title == "c3");
            model.RootTaxonId = rootTaxon.Id;

            var viewModel = model.CreateViewModel();

            var innerLevelNames = this.taxaNames
                .Where(tn => tn.Contains("c3|"))
                .Select(tn => tn.Split('|')[1])
                .ToList();

            Assert.AreEqual(innerLevelNames.Count, viewModel.Taxa.Count);

            for (int i = 0; i < viewModel.Taxa.Count; i++)
            {
                var expected = innerLevelNames[i];
                var actual = viewModel.Taxa[i];

                Assert.AreEqual(expected, actual.Title);
            }
        }

        private void DeleteAllCategories()
        {
            var manager = TaxonomyManager.GetManager();
            using (new ElevatedModeRegion(manager))
            {
                var taxa = manager.GetTaxa<HierarchicalTaxon>();
                foreach (var taxon in taxa)
                {
                    manager.DeleteItem(taxon);
                }

                manager.SaveChanges();
            }
        }

        private void GetTaxonNameAndParentName(string taxon, out string parentName, out string taxonName)
        {
            var parts = taxon.Split('|');
            parentName = parts.Length == 2 ? parts[0] : string.Empty;
            taxonName = parts.Length == 2 ? parts[1] : parts[0];
        }

        private List<string> taxaNames = new List<string>() { "c1", "c2", "c3", "c3|c3c1", "c3|c3c2", "c3c2|c3c2c1" };
    }
}
