using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// this is an object where you can make an api call to twitter
/// it will contain a constructor with the consumer key, consumer secret, access token and access secret tokens. All of these values are obtained from creating an application within your twitter account
/// 
/// </summary>
/// 

public class TwitterApiCall
{
    private string consumerkey;

    private string consumersecret;

    private string accesstoken;

    private string accesssecret;

    //store oauth version here
    private static string version = "1.0";
    //store signature method here
    private static string signaturemethod = "HMAC-SHA1";

    //Constructor takes user keys as arguments. These values need to be retreived by setting up an application via your twitter account
	public TwitterApiCall(string ck,string cs, string ak,string acs)
	{
        this.consumerkey = ck;
        this.consumersecret = cs;
        this.accesstoken = ak;
        this.accesssecret = acs;
	}

    //this gets the json data from twitter api
    public string GetTwitterData(string resourceurl)
    {
        //create parameter list
        List<string> parameterlist;
        //check for query string
        if (resourceurl.Contains("?"))
        {
            parameterlist = getparameterlistfromurl(resourceurl);
            resourceurl = resourceurl.Substring(0, resourceurl.IndexOf('?'));
        }

        else
        {
            parameterlist = null;
        }
        //build the oauth header
        string authheader = buildheader(resourceurl,parameterlist);

        //make the request to the twitter api and get the JSON response
        string jsonresponse = TwitterWebRequest(resourceurl, authheader,parameterlist);

        return jsonresponse;


    }

    //retreive a list if parameters from the resource url. This will be used when making the request to the twitter api and in generating the signature
    private List<string> getparameterlistfromurl(string resourceurl)
    {

        //Uri MyUrl = new Uri(resourceurl);
        string querystring = resourceurl.Substring(resourceurl.IndexOf('?')+1);

        

        List<string> listtoreturn = new List<string>();


        NameValueCollection nv = HttpUtility.ParseQueryString(querystring);

        foreach(string parameter in nv)
        {
            listtoreturn.Add(parameter + "="+ Uri.EscapeDataString(nv[parameter].ToString()));

        }
        return listtoreturn;
    }



    //this gets the timeline data from twitter api
    private string buildheader(string resourceurl, List<string> parameterlist)
    {

        string nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
        TimeSpan timespan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        string timestamp = Convert.ToInt64(timespan.TotalSeconds).ToString();

        string signature = getSignature(nonce, timestamp, resourceurl, parameterlist);

        // build the authentication header with all information collected

        var HeaderFormat = "OAuth " +
        "oauth_consumer_key=\"{0}\", " +
        "oauth_nonce=\"{1}\", " +
        "oauth_signature=\"{2}\", " +
        "oauth_signature_method=\"{3}\", " +
        "oauth_timestamp=\"{4}\", " +
        "oauth_token=\"{5}\", " +
        "oauth_version=\"{6}\"";

        string authHeader = string.Format(HeaderFormat,
        Uri.EscapeDataString(consumerkey),
        Uri.EscapeDataString(nonce),
        Uri.EscapeDataString(signature),
        Uri.EscapeDataString(signaturemethod),
        Uri.EscapeDataString(timestamp),
        Uri.EscapeDataString(accesstoken),
        Uri.EscapeDataString(version)
        );

        return authHeader;

    }



    private string getSignature(string nonce, string timestamp, string resourceurl, List<string> parameterlist)
    {
        // generate the base string for the signature
        
        string baseString = generatebasestring(nonce,timestamp,resourceurl,parameterlist);
        
        baseString = string.Concat("GET&", Uri.EscapeDataString(resourceurl), "&", Uri.EscapeDataString(baseString));


        // generate the signature using the base string, consumer secret and access secret from the application api. Using the HMAC-SHA1 signature method

        var signingKey = string.Concat(Uri.EscapeDataString(consumersecret), "&", Uri.EscapeDataString(accesssecret));
        string signature;

        //generate hash using signing key
        HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(signingKey));

        signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
        //get signature signature using the hash

        return signature;

    }

    private string generatebasestring(string nonce, string timestamp, string resourceurl, List<string> parameterlist)
    {

        string basestring="";
        //create list with all the security parameters
        List<string> baseformat = new List<string>();
        baseformat.Add("oauth_consumer_key="+consumerkey);
        baseformat.Add("oauth_nonce="+ nonce);
        baseformat.Add("oauth_signature_method="+signaturemethod);
        baseformat.Add("oauth_timestamp="+timestamp);
        baseformat.Add("oauth_token="+accesstoken);
        baseformat.Add("oauth_version=" + version);


        //append parameter list as twitter requires the parameters to be in alphabetical order
        if(parameterlist!=null)
        {
            baseformat.AddRange(parameterlist);

        }
        //sort list alphabetically
        baseformat.Sort();


        //loop through list and generate base string

        foreach (string value in baseformat)
        {
            basestring += value +"&";
        }

        basestring= basestring.TrimEnd('&');

        return basestring;
       
        
    }

    //makes the request to twitter and returns a string of JSON data
  
    private string TwitterWebRequest(string resourceurl, string authheader,List<string> parameterlist)
    {

        //build  the http web request to the twitter api
        ServicePointManager.Expect100Continue = false;

        string postBody;
        
        if (parameterlist != null)
        {
            postBody = GetPostBody(parameterlist);
        }
        else
        {
            postBody = "";
        }
        resourceurl += "?" + postBody;
        
        

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceurl);
        request.Headers.Add("Authorization", authheader);
        request.Method = "GET";
        request.ContentType = "application/x-www-form-urlencoded";

        // Retrieve the response json data
        WebResponse response = request.GetResponse();

        //json reponse data
        string responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();

        return responseData;

    }

    private string GetPostBody(List<string> parameterlist)
    {
        string stringtoreturn="";
        
        foreach(string item in parameterlist)
        {
            stringtoreturn += item + "&"; 

        }
        stringtoreturn = stringtoreturn.TrimEnd('&');
        return stringtoreturn;

    }

 
   
}