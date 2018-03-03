using Sitecore.Data.Items;
using Sitecore.HashTagMonitor.Api.Managers;

namespace Sitecore.HashTagMonitor.Api
{
    public class Task
    {
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            HashTagManager hashTagManager= new HashTagManager(new Twitter.Twitter());

            hashTagManager.ProcessHashTag("#SCHackathon");
        }
    }
}
