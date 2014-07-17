$(document).ready(function () {

    $("#QueryButton").on("click", function () {
        var url = $("#TwitterQuery").val().trim();

        //get tweets
        gettweets(url)
    });
   

});


//run plugin passing through the resource url
function gettweets(url)
{

    $(".twitterbox").tweetservice(
       {
           resourceurl: url

       }

       );
}
