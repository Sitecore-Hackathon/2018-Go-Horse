# Documentation

Module develop by GO HORSE TEAM , Sitecore Hackathon 2018

## Summary

**Category:** xConnect

Purpose :The Module gets all tweets for a particular #hashtagh, add import this information to the xDB , this will help marketers 
to identify the Profile of each user, Examples: If someone post things on #schackthon, we can inferir that this particular user is a Geek. 
So it will add points to this category

## Pre-requisites

Does your module rely on other Sitecore modules or frameworks?

- List any dependencies: 
- xConnect must be properly installed and configured(Sitecore 9 post build steps)
- Sitecore PowerShell it not necessary, but may help to start the Sitecore TASK

## Installation

Provide detailed instructions on how to install the module, and include screenshots where necessary.
1. Install Sitecore 9
2. Run Sitecore9 post build installation Steps 
3. Go to(Control Panel ->  Index Manager (rebuild all indexes) )
4. Go to(Control Panel ->  Database -> Rebuild link Database )
5. Go to(Control Panel ->  Analytics  -> Deploy Marketing Definitions )
6. Use the Sitecore Installation wizard to install the [package](#link-to-package)
7. InstallSitecore PowerShell (OPTIONAL STEP) : If you don't wanna wait some minutes to the task to be runned, 
you can install PowerShell to Fire
8. Profit

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

![Hackathon Logo](images/hackathon.png?raw=true "Hackathon Logo")

You can embed images of different formats too:

![Deal With It](images/contacts.png?raw=true "Deal With It")

And you can embed external images too:

![Random](https://placeimg.com/480/240/any "Random")

## Video

Please provide a video highlighing your Hackathon module submission and provide a link to the video. Either a [direct link](https://www.youtube.com/watch?v=EpNhxW4pNKk) to the video, upload it to this documentation folder or maybe upload it to Youtube...

[![Sitecore Hackathon Video Embedding Alt Text](https://img.youtube.com/vi/EpNhxW4pNKk/0.jpg)](https://www.youtube.com/watch?v=EpNhxW4pNKk)
