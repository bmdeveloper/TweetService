using System.Web.Services;
using System.Configuration;

/// <summary>
/// You want to seperate resource url username and count
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class TwitterService : System.Web.Services.WebService {

    // The below values are stored in TwitterSettings.config. These values need to be retreived by setting up an application via your twitter account.
  
    //retrieve consumer key here
    private static string consumerkey = ConfigurationManager.AppSettings["consumerkey"].ToString();

    //retrieve consumer secret here
    private static string consumersecret = ConfigurationManager.AppSettings["consumersecret"].ToString();

    //retrieve access token here 
    private static string accesstoken = ConfigurationManager.AppSettings["accesstoken"].ToString();

    //retrieve access secret here
    private static string accesssecret = ConfigurationManager.AppSettings["accesssecret"].ToString();

    public TwitterService()
    {

       
    }

    [WebMethod(CacheDuration = 15)]
    public string GetTwitterData(string resourceurl)
    {
        TwitterApiCall arsenalTweets = new TwitterApiCall(consumerkey, consumersecret, accesstoken, accesssecret);

        return arsenalTweets.GetTwitterData(resourceurl.Trim());

    }

    


    
}
