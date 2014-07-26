(function ($) {

    var TweetObject = new Object();

    //This is the resource url. Currently added via the textbox on the page
    TweetObject.resourceurl = "";

    //example query for using the user timeline api
    //https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=arsenal&count=4&include_rts=false
    //https://api.twitter.com/1.1/search/tweets.json?q=#WorldCup2014&count=3

    $.fn.tweetservice = function (options) {
        // Establish our default settings
        var settings = $.extend({
            //set default to twitter api
            resourceurl: "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=twitterapi",
            updateinterval: null
        }, options);

        
        return this.each(function () {
            var element = $(this);

            MakeTwitterRequest(settings.resourceurl, element);

        });

        function MakeTwitterRequest(resourceurl, element) {
            TweetObject.resourceurl = resourceurl;
            element.empty();
            TwitterAjaxCall(element);
            if (settings.updateinterval != null)
             {
                setInterval(function () {
                element.empty();
                TwitterAjaxCall(element);
            }, settings.updateinterval);
            }
        }


        function TwitterAjaxCall(element) {
            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                url: "TwitterService.asmx/GetTwitterData",
                data: JSON.stringify(TweetObject),
                success: function (data) {
                    LoadTweets(data.d, element);
                },
                error: function (xhr, status, error) {
                    if (console) {
                        console.log("There is an error: " + error);
                    }
                }
            });


        }
        function LoadTweets(jsondata, element) {

            var data = JSON.parse(jsondata);

            console.log(data);

            var datatoloopthrough;

            //check whether a search query or timeline query
            if (data.statuses != undefined) {

                //its search query so loop through statuses
                datatoloopthrough = data.statuses;
            }

            else {
                //its timeline query so statuses does not exist
                datatoloopthrough = data;
            }


            //loop through each tweet and display values
            //index is the number of the iteration. Value is the Json of the particular tweet that you are currently looping through
            $.each(datatoloopthrough, function (index, value) {
                //get icon
                var icon = "<a class='iconimg'><img src='" + value.user.profile_image_url_https + "'></img></a>";

                //add links to tweet, remove any image urls and add usermentions
                TreatTweetText(value);

                //set the tweet to a variable
                var tweet = "<p class=\"tweettext\">" + value.text + "</p>";

                //set images to variable
                var images = GetImagesForTweet(value);


                element.append("<div class=\"tweet\">" + icon + tweet + images + "</div>")

            });

            //write method that processes links and hashtags
        }

        function TreatTweetText(tweet) {
            tweet.text = removeimageurls(tweet);

            tweet.text = AddLinksToTweet(tweet);

            tweet.text = AddUserMentions(tweet);

            tweet.text = AddHashTags(tweet);

        }

        function AddLinksToTweet(tweet) {
            var treatedtweetstext = tweet.text;

            //check whether there are urls in the tweet
            if (tweet.entities.urls[0] != undefined) {
                //loop through urls in the tweet
                $.each(tweet.entities.urls, function (index, value) {
                    var urltoreplace = value.url;
                    var displayurl = value.display_url;
                    var expandedurl = value.expanded_url;
                    treatedtweetstext = treatedtweetstext.replace(urltoreplace, "<a class=\"tweeturl\" target=\"_blank\" href=\"" + urltoreplace + "\" title=\"" + expandedurl + "\">" + displayurl + "</a>");
                }

                );
            }

            return treatedtweetstext;
        }

        function AddUserMentions(tweet) {
            var treatedtweetstext = tweet.text;

            //check whether there are usermentions in the tweet
            if (tweet.entities.user_mentions[0] != undefined) {
                //loop through usermentions in the tweet
                $.each(tweet.entities.user_mentions, function (index, value) {
                    var usermentiontoreplace = value.screen_name;

                    //ignore case
                    var usermentiontoreplaceignorecase = new RegExp("@" + value.screen_name, "i");
                    var linktouser = "https:\/\/twitter.com\/" + usermentiontoreplace;

                    treatedtweetstext = treatedtweetstext.replace(usermentiontoreplaceignorecase, "<a href=\"" + linktouser + "\" class=\"usermention\" target=\"_blank\" >" + "@" + usermentiontoreplace + "</a>");
                }

                );
            }

            return treatedtweetstext;
        }

        function AddHashTags(tweet) {
            var treatedtweetstext = tweet.text;

            //check whether there are hashtags in the tweet
            if (tweet.entities.hashtags[0] != undefined) {
                //loop through urls in the tweet
                $.each(tweet.entities.hashtags, function (index, value) {
                    var hashtag = "#" + value.text;
                    //ignore case
                    var hashtagtoreplaceignorecase = new RegExp("#" + value.text, "i");
                    var hashtaglink = "https:\/\/twitter.com\/hashtag\/" + value.text + "?src=hash";
                    treatedtweetstext = treatedtweetstext.replace(hashtagtoreplaceignorecase, "<span class=\"hashtag\"><a target=\"_blank\" href=\"" + hashtaglink + "\">" + hashtag + "</a></span>");
                }

                );
            }

            return treatedtweetstext;
        }

        function GetImagesForTweet(tweet) {
            var imagescode = "";

            if (tweet.entities.media != undefined) {
                //loop through images in the tweet
                $.each(tweet.entities.media, function (index, value) {
                    var tweettext = tweet.text;
                    var urltoreplace = value.url;
                    var displayurl = value.display_url;
                    var expandedurl = value.expanded_url;
                    var embeddedimageurl = value.media_url_https;
                    imagescode += "<a class=\"tweetimage\" target=\"_blank\" href=\"" + urltoreplace + "\"><img alt=\"" + displayurl + "\"" + "src=\"" + embeddedimageurl + "\" />" + "</a>";
                }


            );
            }
            return imagescode;
        }

        //remove any image urls from the text as we are displaying the images
        function removeimageurls(tweet) {
            var tweetstext = tweet.text;

            if (tweet.entities.media != undefined) {
                //loop through images in the tweet
                $.each(tweet.entities.media, function (index, value) {

                    var urltoreplace = value.url;

                    tweetstext = tweetstext.replace(urltoreplace, "");
                }


            );
            }

            return tweetstext;
        }



    }

    
  


}(jQuery));