using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace GoHorseLib
{
    public class Twitter
    {
        public static string _consumerKey = "jTZHoDGisacVsNoEV3tlyH8uF"; // Add your Key
        public static string _consumerSecret = "SXmCpmA064MyBsSF6Jrb8io4AAjV6HraYYHpopmz9w1FVdq3Sh"; // Add your Key
        public static string _accessToken = "845015154790223872-oEkmRQMlW3YLObMeGgmPX1JjXoEAN4W"; // Add your Key
        public static string _accessTokenSecret = "E1j98jiZ0sOUwVassZxTUCOSgVcsTtWsBy1H6oJRucxAG"; // Add your Key
        public List<TwitterStatus> GetTweets(string hashtag, TwitterSearchResultType twitterSearchResultType, int count )
        {
            TwitterService twitterService = new TwitterService(_consumerKey, _consumerSecret);
            twitterService.AuthenticateWith(_accessToken, _accessTokenSecret);

            int tweetcount = 1;
            //var tweets_search = twitterService.Search(new SearchOptions { Q = "#SCHackathon",Resulttype = TwitterSearchResultType.Popular, Count = 100 });
            var tweets_search = twitterService.Search(new SearchOptions { Q = hashtag, Resulttype = twitterSearchResultType, Count = count });

            //Resulttype can be TwitterSearchResultType.Popular or TwitterSearchResultType.Mixed or TwitterSearchResultType.Recent
            List<TwitterStatus> resultList = new List<TwitterStatus>(tweets_search.Statuses);

            return resultList;
        }
    }
}
