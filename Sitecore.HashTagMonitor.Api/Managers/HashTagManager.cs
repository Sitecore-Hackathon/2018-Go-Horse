using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using TweetSharp;

namespace Sitecore.HashTagMonitor.Api.Managers
{
    public class HashTagManager
    {
        private readonly Twitter.Twitter _twitter;

        public HashTagManager(Twitter.Twitter twitter)
        {
            _twitter = twitter;
        }

        public void ProcessHashTag(string hashtag)
        {
            // Get all tweets from hashtag
            var tweets = _twitter.GetTweets(hashtag, TwitterSearchResultType.Recent, 200);

            foreach (var tweet in tweets)
            {
                // Create Contact + Identifier + Personal Info
                var contact = CreateOrGetContact(hashtag, tweet);
                if (contact == null)
                    continue;

                // Create Interaction
                //contact.Interactions.Any(p=>p.)


                // Create Event for each tweet                
            }
        }

        private Contact CreateOrGetContact(string hashtag, TwitterStatus tweet)
        {
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var authorAccount = tweet.User.ScreenName;

                    // Get Contact
                    var contactReference = new IdentifiedContactReference("twitter", authorAccount);
                    var contact = client.Get(contactReference, new ExpandOptions { FacetKeys = { "Personal" } });
                    if (contact != null)
                        return contact;

                    // Create Contact                    
                    var identifiers = new[] {
                        new ContactIdentifier("twitter", hashtag, ContactIdentifierType.Known)
                    };                                 
                    var newContact = new Contact(identifiers);

                    // Personal Info
                    var nameSplit = tweet.User.Name.Split(' ');
                    var firstName = nameSplit.Length>0 ? nameSplit[0] : "";
                    var lastName = nameSplit.Length>1 ?  nameSplit[1] : "";
                    var personalInfoFacet = new PersonalInformation
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };
                    client.SetFacet<PersonalInformation>(newContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    // E-mail
                    var emailFacet = new EmailAddressList(new EmailAddress("longhorn@taco.com", true), "twitter");
                    client.SetFacet<EmailAddressList>(newContact, EmailAddressList.DefaultFacetKey, emailFacet);

                    // Add contact to XConnect
                    client.AddContact(newContact);
                    client.Submit();

                    return newContact;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error creating or retrieving contact '{tweet.User.ScreenName}' for hashtag '{hashtag}'",
                        ex, GetType());
                    return null;
                }
            }
        }
    }
}