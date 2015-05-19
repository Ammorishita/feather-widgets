﻿; (function ($) {
    'use strict';

    /*
        Widget
    */
    var CommentsCountWidget = function (rootUrl, resources, useReviews) {
        if (rootUrl === null || rootUrl.length === 0)
            rootUrl = '/';
        else if (rootUrl.charAt(rootUrl.length - 1) !== '/')
            rootUrl = rootUrl + '/';

        this.rootUrl = rootUrl;
        this.resources = resources;
        this.useReviews = useReviews;
    };

    CommentsCountWidget.prototype = {
        getCommentsCounts: function () {
            var threadKeys = this.collectThreadIds();
            var commentsCountSubpath = this.useReviews ? 'reviews_statistics' : 'count';
            var getCommentsCountsUrl = this.rootUrl + 'comments/' + commentsCountSubpath + '?ThreadKey=' + encodeURIComponent(threadKeys);

            return $.ajax({
                type: 'GET',
                url: getCommentsCountsUrl,
                contentType: 'application/json',
                cache: false,
                accepts: {
                    text: 'application/json'
                },
                processData: false
            });
        },

        collectThreadIds: function () {
            var commmentsCounterControls = $('[data-sf-role="comments-count-wrapper"]');
            var uniqueKeys = {};
            for (var i = 0; i < commmentsCounterControls.length; i++) {
                uniqueKeys[$(commmentsCounterControls[i]).attr('data-sf-thread-key')] = true;
            }

            var threadKeys = [];
            $.each(uniqueKeys, function (key) {
                threadKeys.push(key);
            });

            return threadKeys;
        },

        setCommentsCounts: function () {
            var self = this;

            self.getCommentsCounts().then(function (response) {
                if (response) {
                    var threadCountList = response.Items || response;

                    for (var i = 0; i < threadCountList.length; i++) {
                        if (threadCountList[i].Count >= 0) {
                            $('div[data-sf-thread-key="' + threadCountList[i].Key + '"]').each(self.populateCommentsCountTextCallBack(threadCountList[i].Count, threadCountList[i].AverageRating));
                        }
                    }
                }
            });
        },

        populateCommentsCountTextCallBack: function (currentCount, currentRating) {
            var self = this;
            return function (index, element) {
                self.populateCommentsCountText($(element), currentCount, currentRating);
            };
        },

        populateCommentsCountText: function (element, currentCount, currentRating) {
            var currentCountFormatted = '';
            if (!currentCount) {
                currentCountFormatted = this.resources.leaveComment;
            }
            else {
                currentCountFormatted = currentCount;

                if (currentCount == 1)
                    currentCountFormatted += ' ' + this.resources.comment.toLowerCase();
                else
                    currentCountFormatted += ' ' + this.resources.commentsPlural.toLowerCase();
            }

            // set the comments count text in the counter control
            element.find('[data-sf-role="comments-count-anchor-text"]').text(currentCountFormatted);

            // render average rating
            if (currentCount && this.useReviews) {
                // remove if any old ratings
                element.find('[data-sf-role="rating-average"]').remove();

                var averageRatingEl = $('<span data-sf-role="rating-average" />');
                averageRatingEl.mvcRating({ readOnly: true, value: currentRating });
                averageRatingEl.prepend($('<span />').text(this.resources.averageRating));
                averageRatingEl.append($('<span />').text('(' + currentRating + ')'));

                element.prepend(averageRatingEl);
            }
        },
        
        initialize: function () {
            var self = this;

            self.setCommentsCounts();

            $(document).on('sf-comments-count-received', function (event, args) {
                if (self.useReviews) {
                    self.setCommentsCounts();
                }
                else {
                    $('div[data-sf-thread-key="' + args.key + '"]').each(self.populateCommentsCountTextCallBack(args.count));
                }
            });
        },
    };

    /*
        Widgets creation
    */
    $(function () {
        var serviceUrl = $('[data-sf-role="comments-count-wrapper"]').find('[data-sf-role="service-url"]').val();
        var useReviews = JSON.parse($('[data-sf-role="comments-count-wrapper"]').find('[data-sf-role="comments-use-reviews"]').val());
        var resources = JSON.parse($('[data-sf-role="comments-count-wrapper"]').find('[data-sf-role="comments-count-resources"]').val());
        (new CommentsCountWidget(serviceUrl, resources, useReviews)).initialize();
    });
}(jQuery));
