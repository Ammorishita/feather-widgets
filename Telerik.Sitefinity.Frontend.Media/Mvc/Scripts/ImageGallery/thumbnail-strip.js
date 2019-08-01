﻿; (function ($) {
    $(document).ready(function () {
        var populateDefaultItem = function () {
            var defaultElementIndex = 0;
            var firstImageElement = $('.js-Gallery-thumbs').find('a')[defaultElementIndex];
            if (firstImageElement) {
                populateSelecteditem(firstImageElement);
            }
        };

        var populateSelecteditem = function (element, updateUrl) {
            $(element).addClass('is-selected');
            var item = $.parseJSON($(element).attr('data-item') || null);
            var selectedElementIndex = $(element).index();

            $('.js-Gallery-image').find('img').attr('src', item.MediaUrl);
            $('.js-Gallery-image').find('img').attr('title', item.Title);
            $('.js-Gallery-image').find('img').attr('alt', item.AlternativeText);

            if (item.Width) {
                $('.js-Gallery-image').find('img').attr("width", item.Width);
            }
            else {
                $('.js-Gallery-image').find('img').removeAttr("width");
            }

            if (item.Height) {
                $('.js-Gallery-image').find('img').attr("height", item.Height);
            }
            else {
                $('.js-Gallery-image').find('img').removeAttr("height");
            }

            $('.js-Gallery-title').html(item.Title);
            $('.js-Gallery-description').html(item.Description);
            $('.js-Gallery-index').html(selectedElementIndex + 1);

            if (updateUrl) {
                var detailUrl = $(element).attr('data-detail-url');
                if (detailUrl) {
                    history.pushState(detailUrl, item.Title, detailUrl);
                }
            }
        };

        var selectPrev = function () {
            var currentlySelected = $('.js-Gallery-thumbs').find('a.is-selected');
            if (currentlySelected && currentlySelected.length > 0) {
                var prevElement = currentlySelected.prev('a');
                if (prevElement && prevElement.length > 0) {
                    removeCurrentlySelected();
                    populateSelecteditem(prevElement, true);
                }
            }
        };

        var selectNext = function () {
            var currentlySelected = $('.js-Gallery-thumbs').find('a.is-selected');
            if (currentlySelected && currentlySelected.length > 0) {
                var nextElement = currentlySelected.next('a');
                if (nextElement && nextElement.length > 0) {
                    removeCurrentlySelected();
                    populateSelecteditem(nextElement, true);
                }
            }
        };

        var removeCurrentlySelected = function () {
            var currentlySelected = $('.js-Gallery-thumbs').find('a.is-selected');
            currentlySelected.removeClass('is-selected');
        };

        $('.js-Gallery-thumbs').find('a').bind('click', function (e) {
            removeCurrentlySelected();
            populateSelecteditem(this, true);
        });

        $('.js-Gallery-thumbs').find('a').bind('keypress', function (e) {
            var key = e.which;
            if (key == 13)  // the enter key code
            {
                removeCurrentlySelected();
                populateSelecteditem(this, true);
            }
        });

        $('.js-Gallery-prev').bind('click', function (e) {
            selectPrev();
        });

        $('.js-Gallery-next').bind('click', function (e) {
            selectNext();
        });

        $('.js-Gallery-prev').bind('keypress', function (e) {
            var key = e.which;
            if (key == 13)  // the enter key code
            {
                selectPrev();
            }
        });

        $('.js-Gallery-next').bind('keypress', function (e) {
            var key = e.which;
            if (key == 13)  // the enter key code
            {
                selectNext();
            }
        });

        populateDefaultItem();

        window.addEventListener('popstate', function (e) {
            if (e.state) {
                var img = $('[data-detail-url="' + e.state + '"]');
                if (img.length > 0) {
                    populateSelecteditem(img[0]);
                }
            }
            else {
                populateDefaultItem();
            }
        });
    });
})(jQuery);