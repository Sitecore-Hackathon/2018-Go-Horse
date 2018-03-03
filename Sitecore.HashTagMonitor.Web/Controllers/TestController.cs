using System;
using System.Web.Mvc;
using Sitecore.HashTagMonitor.Api.Managers;
using Sitecore.HashTagMonitor.Api.Twitter;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;

namespace Sitecore.HashTagMonitor.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly HashTagManager _hashTagManager;

        public TestController() : this(new HashTagManager(new Twitter())) { }
        public TestController(HashTagManager hashTagManager)
        {
            _hashTagManager = hashTagManager;
        }

        [HttpGet]
        public ActionResult Process()
        {
            _hashTagManager.ProcessHashTag("#SCHackathon");
            return null;
        }

        [HttpGet]
        public ActionResult Index()
        {
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var firstContact = new Contact();
                    client.AddContact(firstContact); // Extension found in Sitecore.XConnect.Operations

                    // Submits the batch, which contains two operations
                    client.Submit();
                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                }
            }

            return null;
        }

        [HttpGet]
        public ActionResult GetContact()
        {
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                var contactReference = new IdentifiedContactReference("twitter", "longhorntaco");
                var contact = client.Get(contactReference, new ExpandOptions() { FacetKeys = { "Personal" } });

                if (contact != null)
                {
                    Console.WriteLine($"{contact.Personal().FirstName} {contact.Personal().LastName}");
                }
            }

            return null;
        }


        [HttpGet]
        public ActionResult AddContact()
        {
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    //var contact = new Contact(
                    //    new ContactIdentifier("ad-network", "ABC123456", ContactIdentifierType.Known)
                    //);

                    //IReadOnlyCollection<ContactIdentifier> identifiers = contact.Identifiers;

                    //client.Submit();


                    var identifiers = new ContactIdentifier[]
                    {
                        new ContactIdentifier("twitter", "longhorntaco", ContactIdentifierType.Known),
                        new ContactIdentifier("domain", "longhorn.taco", ContactIdentifierType.Known)
                    };
                    var contact = new Contact(identifiers);

                    var personalInfoFacet = new PersonalInformation
                    {
                        FirstName = "Longhorn",
                        LastName = "Taco"
                    };
                    client.SetFacet<PersonalInformation>(contact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    var emailFacet = new EmailAddressList(new EmailAddress("longhorn@taco.com", true), "twitter");
                    client.SetFacet<EmailAddressList>(contact, EmailAddressList.DefaultFacetKey, emailFacet);

                    client.AddContact(contact);
                    client.Submit();


                }
                catch (XdbExecutionException ex)
                {
                    // Manage exception
                }
            }

            return null;
        }

        [HttpGet]
        public ActionResult AnalyticsRefresh()
        {
            if (Analytics.Tracker.Current != null)
            {
                Analytics.Tracker.Current.EndTracking();
                Session.Abandon();
            }
            return null;
        }
    }
}