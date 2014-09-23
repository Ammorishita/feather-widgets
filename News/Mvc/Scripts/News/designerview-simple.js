﻿(function ($) {
    angular.module('designer').requires.push('expander', 'selectors', 'dataProviders');

    angular.module('designer').controller('SimpleCtrl', ['$scope', 'propertyService', function ($scope, propertyService) {
        $scope.feedback.showLoadingIndicator = true;
        $scope.taxonSelector = { selectedTaxonomies: [] , taxonFilters :{}};

        $scope.$watch(
            'taxonSelector.taxonFilters',
            function (newTaxonFilters, oldTaxonFilters) {
                if (newTaxonFilters !== oldTaxonFilters) {
                    $scope.properties.SerializedTaxonomyFilter.PropertyValue = JSON.stringify(newTaxonFilters);
                }
            },
            true
        );

        $scope.$watch(
            'taxonSelector.selectedTaxonomies',
            function (newSelectedTaxonomies, oldSelectedTaxonomies) {
                if (newSelectedTaxonomies !== oldSelectedTaxonomies) {
                    $scope.properties.SerializedSelectedTaxonomies.PropertyValue = JSON.stringify(newSelectedTaxonomies);
                }
            },
            true
        );

        $scope.$watch(
	        'properties.ProviderName.PropertyValue',
	        function (newProviderName, oldProviderName) {
	            if (newProviderName !== oldProviderName) {
	                $scope.properties.SelectionMode.PropertyValue = 'AllItems';
	                $scope.properties.SelectedItemId.PropertyValue = null;
	            }
	        },
	        true
        );

        propertyService.get()
            .then(function (data) {
                if (data) {
                    $scope.properties = propertyService.toAssociativeArray(data.Items);

                    $scope.taxonSelector.selectedTaxonomies = $.parseJSON($scope.properties.SerializedSelectedTaxonomies.PropertyValue);
                    var taxonFilters = $.parseJSON($scope.properties.SerializedTaxonomyFilter.PropertyValue);

                    if (taxonFilters) {
                        $scope.taxonSelector.taxonFilters = taxonFilters;
                    }
                }
            },
            function (data) {
                $scope.feedback.showError = true;
                if (data)
                    $scope.feedback.errorMessage = data.Detail;
            })
            .finally(function () {
                $scope.feedback.showLoadingIndicator = false;
            });
    }]);
})(jQuery);