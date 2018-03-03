using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.HashTagMonitor.Api.Templates;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;
using TweetSharp;

namespace Sitecore.HashTagMonitor.Api.Managers
{
    public class HashTagManager
    {
        private readonly Twitter.Twitter _twitter;

        #region Config Settings
        private const string SettingRepositoryPathParam = "HashTagMonitor.RepositoryPath";
        private const string DatabaseParam = "HashTagMonitor.Database";
        private string RepositoryPath =>
            Configuration.Settings.GetSetting(SettingRepositoryPathParam, "/sitecore/system/Modules/HashTagMonitor");
        private string DatabaseName => Configuration.Settings.GetSetting(SettingRepositoryPathParam, "master");

        private Database _database;
        private Database Database
        {
            get
            {
                if (_database != null)
                    return _database;
                _database = Database.GetDatabase(DatabaseName) ?? Context.Database;
                return _database;
            }
        }

        private Item _repositoryItem;
        private Item RepositoryItem
        {
            get
            {
                if (_repositoryItem != null)
                    return _repositoryItem;
                var repositoryItem = Database.GetItem(RepositoryPath);
                if (repositoryItem != null)
                {
                    _repositoryItem = repositoryItem;
                    return repositoryItem;
                }
                Diagnostics.Log.Error($"[HashTagMonitor] Cannot find repository item '{RepositoryPath}'", this);
                return null;
            }
        }
        #endregion

        public HashTagManager(Twitter.Twitter twitter)
        {
            _twitter = twitter;
        }

        public void ProcessAllHashTags()
        {
            if (RepositoryItem == null)
                return;

            // Get all HashTags
            var hashTagsQuery = RepositoryItem.Axes.GetDescendants()
                .Where(p => p.TemplateID == HashTag.TemplateID);
            var hashTags = hashTagsQuery as Item[] ?? hashTagsQuery.ToArray();
            if (!hashTags.Any())
                return;

            // Process all HashTags
            foreach (var hashTag in hashTags.Select(p => new HashTag(p)))
            {
                var hashTagText = hashTag.Hashtag;
                if (!hashTagText.StartsWith("#"))
                    hashTagText = "#" + hashTagText;

                ProcessHashTag(hashTagText, hashTag.PatternCard);
            }
        }

        public void ProcessHashTag(string hashtag, PatternCard patternCard)
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

                // Apply Pattern Card to the Contact if the Interaction is new
                if (isNewInteraction && patternCard != null)
                    ApplyPatternCard(contact, patternCard);
            }
        }

        private void ApplyPatternCard(Contact contact, PatternCard patternCard)
        {
            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Retrieve facet by name
                    var facet = contact.GetFacet<ContactBehaviorProfile>(ContactBehaviorProfile.DefaultFacetKey) ??
                                new ContactBehaviorProfile();

                    // Change facet properties
                    var score = new ProfileScore {
                        ProfileDefinitionId = patternCard.GetProfile().ID.ToGuid()
                    };
                    if (score.Values==null)
                        score.Values = new Dictionary<Guid, double>();

                    var patterns = patternCard.GetPatterns();
                    foreach (var pair in patterns)
                        score.Values.Add(pair.Key, pair.Value);

                    // Set the updated facet
                    client.SetFacet(contact, ContactBehaviorProfile.DefaultFacetKey, facet);
                    client.Submit();
                }
                catch (XdbExecutionException ex)
                {
                    Diagnostics.Log.Error(
                        $"[HashTagMonitor] Error applying Patter Card to Contact '{contact.Personal().Nickname}'",
                        ex, GetType());
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
            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    // Item ID of the "Twitter social community" Channel at 
                    // /sitecore/system/Marketing Control Panel/Taxonomies/Channel/Online/Social community/Twitter social community
                    var channelId = Guid.Parse("{6D3D2374-AF56-44FE-B99A-20843B440B58}");
                    var userAgent = hashtag;
                    var newInteraction = new Interaction(contact, InteractionInitiator.Brand, channelId, userAgent);
                    
                    var newEvent = new Event(Guid.NewGuid(),tweet.CreatedDate)
                    {
                        DataKey = tweet.IdStr,
                        Text = tweet.Text
                        //Url = "https://twitter.com/intent/retweet?tweet_id=" + tweet.Id
                    };

                    newInteraction.Events.Add(newEvent);
                    client.AddInteraction(newInteraction);
                    client.Submit();

                    // Has to get Contact back from xConnect
                    contact = CreateOrGetContact(hashtag, tweet);

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
            using (var client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
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