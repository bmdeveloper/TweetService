$(document).ready(function () {
    
    gettweets("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=arsenal&count=4");

    $("#QueryButton").on("click", function () {
        var url = $("#TwitterQuery").val().trim();

        //get tweets
        gettweets(url);


    });
   

});


//run plugin passing through the resource url and add update interval if required 
function gettweets(url)
{
    //update every 5 minutes
    var updateintervalminutes = 5 * 60 * 1000;

    $(".twitterbox").tweetservice(
       {
           resourceurl: url, updateinterval: updateintervalminutes
           

       }

       );
}
