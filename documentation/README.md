# Documentation

Module develop by GO HORSE TEAM , Sitecore Hackathon 2018

## Summary

**Category:** xConnect
Purpose: The Module automatically tracks all tweets for a specified #hashtag and identify accounts that interact with it and import them to xDB. 
This module helps marketers to identify the profile of each user along with their engagement with the #hashtag, 
e.g. if someone twitts on #schackthon, visit the website , and fill in a VisitUs Form , then we can match the user to a given persona
and show personalized content.

## Pre-requisites
- Sitecore 9 Update 1
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

The user has the ability of changing the #hashtah on the sitecore item /sitecore/system/Modules/HashTagMonitor/Test/SCHackathon

The image below show the configured HashTag
/sitecore/system/Modules/HashTagMonitor/Test/SCHackathon
![HashTag](images/configurehashtag.png?raw=true "Configure HashTag")

You can embed images of different formats too:
The image below, shows all the Contacts created after the Task was runned
![Contacts](images/contacts.png?raw=true "Contacts")

The image below the scheduler configured to be executed every 5 minutes
![Task Scheduler](images/taskscheduler.png?raw=true "Task Scheduler")

The image below demonstrates how to Force the Task to be run
![Task Scheduler](images/powershellforcetask.png?raw=true "Powershell Task")

The image below demonstrates the website
![Visit Us](images/VisitUs.png?raw=true "Visit Us")



## Video

Please provide a video highlighing your Hackathon module submission and provide a link to the video. Either a [direct link](https://www.youtube.com/watch?v=EpNhxW4pNKk) to the video, upload it to this documentation folder or maybe upload it to Youtube...

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/EpNhxW4pNKk/0.jpg)](https://www.youtube.com/watch?v=EpNhxW4pNKk)
