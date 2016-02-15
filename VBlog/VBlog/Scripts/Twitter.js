//$(document).ready(function() {
//    twitter.update('twitter', 3, '#tweets');
//});

//var twitter = {
//    update:function(name, count, element) {
//        $.ajax({
//            url: 'http://api.twitter.com/1/statuses/user_timeline.json?screen_name=' + name + '&count=' + count,
//            dataType: 'jsonp',
//            error: function() {
//                twitter.displayError(element);
//            },
//            success: function(json) {
//                twitter.showTweeets(json, element);
//            }
//        });
//    },
//    displayError:function(element) {
//        $(element).html("Error");
//    },

//    showTweets:function(tweets, element) 
//    {
//        $(element).empty();       
//            var list = $('<ul class="tweetList"></ul>').appendTo(element);
//            $(tweets).each(function (index, tweet)
//            {
//                var time = Date.parse(tweet.created_at.replace('+', 'UTC+'));
//                time = twitter.getRelativeTime(time);
//                var message = tweet.text;
//                list.append('<li class="tweet"><span class="tweetTime"' + time + '</span>' + message + '</li>');
//            });
//    },
//    getRelativeTime:function(time) {
        
//    }
//    }

