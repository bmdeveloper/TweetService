<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content runat="server" ID="header" ContentPlaceHolderID="HeadContent">
<%--    <script type="text/javascript" src="Scripts/twitter.js"></script>--%>
    <script src="Scripts/jquery.tweetservice.js"></script>
    <script src="Scripts/script.js"></script>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <p>Add your twitter search or timeline query in the textbox below</p> 
    <p> e.g. https://api.twitter.com/1.1/search/tweets.json?q=#WorldCup2014&count=3 or https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=arsenal&count=4</p> 
    <input type="text" id ="TwitterQuery" value="https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=arsenal&count=4"/>
    <a href="javascript:void(0)" id="QueryButton">Submit</a>


    <div class="twitterbox"></div>
       
</asp:Content>