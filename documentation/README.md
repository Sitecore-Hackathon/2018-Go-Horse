# Documentation

Module develop by GO HORSE TEAM , Sitecore Hackathon 2018

## Summary

**Category:** xConnect
Purpose: The Module automatically tracks all tweets for a particular #hashtag and identify accounts that interact with it and import them to xDB. This module helps marketers 
to identify the profile of each user along with their engagement with the #hashtag, e.g. if someone twitts on #schackthon, we can match the user to a Geek persona.

## Pre-requisites
- xConnect must be properly installed and configured (Sitecore 9 post build steps)

## Installation

Provide detailed instructions on how to install the module, and include screenshots where necessary.
1. Install Sitecore 9
2. Run Sitecore9 post build installation Steps 
3. Use the Sitecore Installation wizard to install the Sitecore HashTagMonitor module [package](#link-to-package)
4. Use the Sitecore Installation wizard to install the Example Web Sitecore [package](#link-to-package)
5. Install Sitecore PowerShell [OPTIONAL STEP] : This can help you to Force start the TASK without waiting the necessary amout of time
6. Configure HashTag, the Standard #HashTag is configured as #SCHackathon
7. Go to(Control Panel ->  Index Manager Rebuild all indexes )
8. Go to(Control Panel ->  Database -> Rebuild link Database )
9. Go to(Control Panel ->  Analytics  -> Deploy Marketing Definitions )

## Configuration

How do you configure your module once it is installed? Are there items that need to be updated with settings, or maybe config files need to have keys updated?

Remember you are using Markdown, you can provide code samples too:

```xml
<?xml version="1.0"?>
<!--
  Purpose: Configuration settings for my hackathon module
-->
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <settings>
      <setting name="MyModule.Setting" value="Hackathon" />
    </settings>
  </sitecore>
</configuration>
```

## Usage

Provide documentation  about your module, how do the users use your module, where are things located, what do icons mean, are there any secret shortcuts etc.

Please include screenshots where necessary. You can add images to the `./images` folder and then link to them from your documentation:

You can embed images of different formats too:
The image below, shows all the Contacts created after the Task was runned
![Contacts](images/contacts.png?raw=true "Contacts")

The image below the scheduler configured to be executed every 5 minutes
![Task Scheduler](images/taskscheduler.png?raw=true "Task Scheduler")

The image below demonstrates how to Force the Task to be run
![Task Scheduler](images/powershellforcetask.png?raw=true "Powershell Task")

The image below demonstrates the website
![Visit Us](images/VisitUs.png?raw=true "Visit Us")

The image below show the configured HashTag
/sitecore/system/Modules/HashTagMonitor/Test/SCHackathon
![HashTag](images/configurehashtag.png?raw=true "Configure HashTag")

## Video

Please provide a video highlighing your Hackathon module submission and provide a link to the video. Either a [direct link](https://www.youtube.com/watch?v=EpNhxW4pNKk) to the video, upload it to this documentation folder or maybe upload it to Youtube...

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/EpNhxW4pNKk/0.jpg)](https://www.youtube.com/watch?v=EpNhxW4pNKk)
