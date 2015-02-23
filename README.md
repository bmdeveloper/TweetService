TweetService
============

Uses ASP.net we service to retrieve a set of tweets from a user timeline or search query using the Twitter REST API 1.1. 

This simple app was created so other developers can review the code to get a better understanding of how OAUTH 1.1 works and how it is implemented to get twitter data.

Any contributers or ideas for improvement are welcome!

This application uses an ASP.net web service to get twitter JSON data using the Twitter REST API.  

The service is called using the JQuery AJAX request functionality. The raw JSON data is subsequently processed and displayed using a JQuery plugin called jquery.tweetservice.js

To get started generate keys via your twitter account and add them to TwitterSettings.config.

For generating keys more information can be found on https://dev.twitter.com/docs. In the REST API section.

After this set up and load the ASP.net website on your local environment. Add the search or timeline query in the textbox and click submit to see the application working.

I decided to make the only argument the resource url (where all of the configuration options are in the url query string see https://dev.twitter.com/docs/api/1.1 for more details) so we didnt have to have to write plugin code to take each parameter as an argument.




