using System;
using System.Linq;
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
                var isNewInteraction = false;
                var interaction = CreateOrGetInteraction(hashtag, contact, tweet, out isNewInteraction);
                if (interaction == null)
                    continue;

                // Create Event for tweet (if not created yet)
                //if (!isNewInteraction)
                    //CreateOrGetEvent(hashtag, interaction, tweet);
            }
        }

        private Event CreateOrGetEvent(string hashtag, Interaction interaction, TwitterStatus tweet)
        {
            // Get Event if it does exists
            var tweetEvent = interaction.Events.FirstOrDefault(p=>p.DataKey==tweet.IdStr);
            if (tweetEvent != null)
                return tweetEvent;

            // Create event if does not exists
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var newEvent = new Event(Guid.NewGuid(), DateTime.Now)
                    {
                        DataKey = tweet.IdStr,
                        Text = tweet.Text
                    };

                    interaction.Events.Add(newEvent);
                    client.AddInteraction(interaction);
                    client.Submit();
                    return CreateOrGetEvent(hashtag, interaction, tweet);
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error creating or retrieving interaction for interaction id '{interaction.Id}' with hashtag '{hashtag}'",
                        ex, GetType());
                    return null;
                }
            }
        }

        private Interaction CreateOrGetInteraction(string hashtag, Contact contact, TwitterStatus tweet, out bool isNewInteraction)
        {
            // Get interaction if it does exists
            isNewInteraction = false;
            var interaction = contact.Interactions?.FirstOrDefault(p => p.UserAgent == hashtag);
            if (interaction != null)
                return interaction;

            // Create if does not exists
            isNewInteraction = true;
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Item ID of the "Twitter social community" Channel at 
                    // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Online/Social community/Twitter social community
                    var channelId = Guid.Parse("{6D3D2374-AF56-44FE-B99A-20843B440B58}");
                    var userAgent = hashtag;
                    var newInteraction = new Interaction(contact, InteractionInitiator.Brand, channelId, userAgent);
                    
                    var newEvent = new PageViewEvent(tweet.CreatedDate , Guid.NewGuid(),1 , tweet.Language)
                    {
                        DataKey = tweet.IdStr,
                        Text = tweet.Text,
                        Url = "https://twitter.com/intent/retweet?tweet_id=" + tweet.Id
                    };

                    newInteraction.Events.Add(newEvent);
                    client.AddInteraction(newInteraction);
                    client.Submit();
                    return newInteraction;
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error creating or retrieving interaction for contact '{contact.Personal().Nickname}' with hashtag '{hashtag}'",
                        ex, GetType());
                    return null;
                }
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
                        new ContactIdentifier("twitter", authorAccount, ContactIdentifierType.Known)
                    };
                    var newContact = new Contact(identifiers);

                    // Personal Info
                    var nameSplit = tweet.User.Name.Split(' ');
                    var firstName = nameSplit.Length>0 ? nameSplit[0] : "";
                    var lastName = nameSplit.Length>1 ?  nameSplit[1] : "";
                    var personalInfoFacet = new PersonalInformation
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Nickname = authorAccount
                    };
                    client.SetFacet<PersonalInformation>(newContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    // E-mail
                    //var emailFacet = new EmailAddressList(new EmailAddress("longhorn@taco.com", true), "twitter");
                    //client.SetFacet<EmailAddressList>(newContact, EmailAddressList.DefaultFacetKey, emailFacet);

                    // Add contact to XConnect
                    client.AddContact(newContact);
                    client.Submit();
                    return CreateOrGetContact(hashtag, tweet);
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