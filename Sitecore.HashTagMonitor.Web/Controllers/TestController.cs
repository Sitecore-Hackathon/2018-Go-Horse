using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;

namespace Sitecore.HashTagMonitor.Web.Controllers
{
    public class TestController : Controller
    {
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
        public ActionResult AddContact()
        {
            using (XConnectClient client = XConnect.Client.Configuration.SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var contact = new Contact(
                        new ContactIdentifier("ad-network", "ABC123456", ContactIdentifierType.Anonymous)
                    );

                    IReadOnlyCollection<ContactIdentifier> identifiers = contact.Identifiers;

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