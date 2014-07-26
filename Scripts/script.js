$(document).ready(function () {
    


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
