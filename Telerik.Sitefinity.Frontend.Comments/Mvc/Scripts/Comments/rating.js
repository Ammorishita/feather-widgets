﻿; (function ($) {
    'use strict';

    var Rating = function (element, options) {
        this.ratingContainer = element;
        self = this;
        this.settings = {
            minValue: 0,   // min number of stars
            maxValue: 5,   // max number of stars
            value: 0,    // number of selected stars
            step: 1,  //setting for selecting stars. Could be floating number.
            reviewCount: null, // number of reviews
            reviewCountNavigateUrl: null, // the href if the review count should be a link
            readOnly: false,
            templateId: '', //element id. Inner Html wil be rendered as many times as {{maxValue}}
            selectedClass: 'on', //Class applied to the element when it is selected
            hoverClass: 'hover' //Class applied to the element when it is hovered
        };

        if (options) {
            jQuery.extend(this.settings, options);
        }

        return this;
    };

    Rating.prototype = {
        getValue: function () {
            return this.settings.value;
        },

        setValue: function (newValue) {
            this.settings.value = newValue;
            this.elementsRenderer.reset();
        },

        getSingleElementMarkup: function () {
            var markup = $("#" + this.settings.templateId).html();

            return markup;
        },
        getSelectedElementMarkup: function () {
            var markup = $("#" + this.settings.templateId).html();
            $(markup).attr('class', this.settings.selectedClass);

            return markup;
        },

        //re-renderer which shows which stars are active, hovered and so on.
        elementsRenderer: {
            fill: function (e) { // fill to the current mouse position.
                var elements = self.ratingContainer.children();
                var index = elements.index(e.srcElement) + 1;
                elements.slice(0, index).addClass(self.settings.selectedClass);
            },
            hover: function (e) { // fill to the current mouse position with hover class.
                var elements = self.ratingContainer.children();
                elements.removeClass(self.settings.hoverClass);

                var index = elements.index(e.srcElement) + 1;
                elements.slice(0, index).addClass(self.settings.hoverClass);
            },
            drainHover: function () { // drain hovering of the elements.
                self.ratingContainer.children().removeClass(self.settings.hoverClass);
            },
            reset: function () { // resets the element to the selected value.
                var elements = self.ratingContainer.children();
                elements.removeClass(self.settings.selectedClass).removeClass(self.settings.hoverClass);
                elements.slice(0, self.settings.value).addClass(self.settings.selectedClass);
            }
        },

        selectElement: function (e) {
            var elements = self.ratingContainer.children();
            self.settings.value = elements.index(e.srcElement) + 1;
            self.elementsRenderer.reset();
        },

        attachEvents: function () {
            var elements = self.ratingContainer.children();
            elements.bind('click', self.selectElement);
            elements.bind('mouseover', self.elementsRenderer.hover);
            elements.bind('mouseout', self.elementsRenderer.drainHover);
            elements.bind('focus', self.elementsRenderer.hover);
            elements.bind('blur', self.elementsRenderer.reset);
        },

        render: function () {
            this.ratingContainer.empty();

            var singleElementTemplate = this.getSingleElementMarkup();
            var selectedElementTemplate = this.getSelectedElementMarkup();

            for (var i = 0; i < this.settings.value; i++) {
                this.ratingContainer.append(selectedElementTemplate);
            }
            for (var j = this.settings.value; j < this.settings.maxValue; j++) {
                this.ratingContainer.append(singleElementTemplate);
            }

            if (!this.settings.readOnly) {
                this.attachEvents();
            }
        }

    };

    $.fn.extend({
        featherRating: function (settings) {
            var fRating = new Rating($(this), settings);
            fRating.render();

            return fRating;
        },
    });

}(jQuery));
