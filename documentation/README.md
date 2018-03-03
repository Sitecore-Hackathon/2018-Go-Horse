# Documentation

Module develop by GO HORSE TEAM , Sitecore Hackathon 2018

## Category 

xConnect

## Purpose 
Identify personas and fire goals based on a twitter HashTag

## Detailed description
 The Module automatically tracks all tweets for a specified #hashtag and identify accounts that interact with it and import them to xDB. 
This module helps marketers to identify the profile of each user along with their engagement with the #hashtag, 
e.g. if someone tweets on #schackthon, visit the website, and fill in a "Visit Us" Form, then we can match the user to a given persona
and show personalized content.

## Pre-requisites
- Sitecore 9 Update-1
- xConnect must be properly installed and configured (Sitecore 9 post build steps)

## Installation

Provide detailed instructions on how to install the module, and include screenshots where necessary.
1. Install Sitecore 9
2. Use the Sitecore Installation wizard to install the Sitecore HashTagMonitor module [package](#link-to-package)
3. Use the Sitecore Installation wizard to install the Example Web Sitecore [package](#link-to-package)
4. Install Sitecore PowerShell [OPTIONAL STEP] : This can help you to Force start the TASK without waiting the necessary amout of time
5. Publish web 
6. Configure HashTag on the sitecore item (/sitecore/system/Modules/HashTagMonitor/Test/SCHackathon), the Standard #HashTag is configured as #SCHackathon
7. Rebuild all indexes (Control Panel ->  Index Manager -> Rebuild all indexes)
8. Rebuild Link Database (Control Panel ->  Database -> Rebuild link Database)
9. Deploy Marketing Definitions (Control Panel ->  Analytics  -> Deploy Marketing Definitions)

## Usage

The user has the ability of changing the #hashtah on the sitecore item /sitecore/system/Modules/HashTagMonitor/Test/SCHackathon.

The image below show the configured HashTag
/sitecore/system/Modules/HashTagMonitor/Test/SCHackathon
![HashTag](images/configurehashtag.png?raw=true "Configure HashTag")

After the HashTag is defined, the user can run the Task Scheduler in order to import all the tweets into the xDB.
The image below the scheduler configured to be executed every 5 minutes.

![Task Scheduler](images/TaskScheduler.png?raw=true "Task Scheduler")

If you can't wait, Force the importation procedure by Firing the following url
http://myur/api/sitecore/hash/Process

After you run the Process, go to "Experience Profile" and check the List of Contacts. The image below, shows all the Contacts created after the Task was run.

![Contacts](images/contacts.png?raw=true "Contacts")

Acessing the Website:
User should access the home at e.g. "gohorse.local", click on visit us call to action button, fill the form with his personal data and click on Submit. 

![Visit Us](images/VisitUs.png?raw=true "Visit Us")

He will be redirected to the home page and get a personalized message.

![Thanks for tweeting](images/ThanksForTweeting.jpg?raw=true "Thanks for tweeting")

## Video
Link to the video

[Click here to watch the module presentation on YouTube](https://youtu.be/2lEAazVlHUQ) 


