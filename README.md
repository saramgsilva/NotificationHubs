# Azure Notification Hubs Sample

## :white_medium_square: Table of contents
* [What is this?](https://github.com/saramgsilva/NotificationHubs#white_medium_square-what-is-this)
* [Requirements](https://github.com/saramgsilva/NotificationHubs#white_medium_square-requirements)
* [Sample Status](https://github.com/saramgsilva/NotificationHubs#white_medium_square-sample-status)
* [The Source Code](https://github.com/saramgsilva/NotificationHubs#white_medium_square-the-source-code)
* [Build project](https://github.com/saramgsilva/NotificationHubs#white_medium_square-build-projects)
* [Screenshots](https://github.com/saramgsilva/NotificationHubs#white_medium_square-screenshots)
* [Resources](https://github.com/saramgsilva/NotificationHubs#white_medium_square-resources)
* [Common Issues](https://github.com/saramgsilva/NotificationHubs#white_medium_square-common-issues)
* [Tips](https://github.com/saramgsilva/NotificationHubs#white_medium_square-tips)
* [Contributors](https://github.com/saramgsilva/NotificationHubs#white_medium_square-contributors)
* [Contribute](https://github.com/saramgsilva/NotificationHubs#white_medium_square-contribute)
* [License](https://github.com/saramgsilva/NotificationHubs#white_medium_square-license)

## :white_medium_square: What is this?

The sample provided has the goal to help developers add Push Notifications to their applications, 
through [Azure Notification Hubs](http://azure.microsoft.com/en-us/documentation/services/notification-hubs/) 
and this sample supports:

* The two main ways to manage devices in Azure Notification Hubs, using the Registration Model:
	* Case 1 - Devices register directly in Azure Notification Hubs 
	* Case 2 - Devices register in Azure Notification Hubs through backend
	
* Azure Mobile Services;


## :white_medium_square: Requirements


* Create the Azure Notification Hubs in Azure Portal (Azure Mobile Service create it by default)
* Create the required data, in Push Notification Service (WNS, GCM, APNs...), to support push notification for each Platform
* Configure the Azure Notification Hub, in Azure Portal, with the data from Push Notification Services (WNS, GCM, APNs...)
* Add connection string and hub name from the Azure Notification Hub created (in the sample it is defined in Contants.cs file

## :white_medium_square: Sample Status


* **Key:** :white_check_mark: = Supported,  :x: = Not Supported, :wrench: = In development 

##### Windows Applications - Using XAML / CSharp

Platform |  Azure Mobile Services|  Registration Model (Case1) |  Registration Model (Case2)  |
:---------- | :------------------------ | :------------------------ | :------------------------ | 
Windows Store 8.1 (WinRT) | :white_check_mark:  | :white_check_mark: | :white_check_mark: 
Windows Phone 8.1 (WinRT) | :white_check_mark: | :white_check_mark:   | :white_check_mark:   
Windows Phone 8.1 (SL)| :white_check_mark: | :white_check_mark: | :white_check_mark:


##### Windows Applications - Using XAML / Visual Basic

Platform |  Azure Mobile Services|  Registration Model (Case1) |  Registration Model (Case2)  |
:---------- | :------------------------ | :------------------------ | :------------------------ |
Windows Store 8.1 (WinRT) | :white_check_mark:  | :white_check_mark: | :white_check_mark: 
Windows Phone 8.1 (WinRT) | :white_check_mark: | :white_check_mark:   | :white_check_mark:   
Windows Phone 8.1 (SL)| :white_check_mark: | :white_check_mark: | :white_check_mark:

##### Xamarin Applications

Platform |  Azure Mobile Services|  Registration Model (Case1) |  Registration Model (Case2)  |
:---------- | :------------------------ | :------------------------ | :------------------------ | 
Xamarin Android | :white_check_mark: |  :white_check_mark:  | :white_check_mark:   | 
Xamarin IOS | :white_check_mark:  |  :white_check_mark: | :white_check_mark: | 


#### Others

Platform |  Azure Mobile Services|  Registration Model (Case1) |  Registration Model (Case2)  |
:---------- | :------------------------ | :------------------------ | :------------------------ | 
Android Native | :white_check_mark: | :x:| :x: | 
IOS Native | :x:  |:white_check_mark:| :x:| 
Cordova |  :x:  | :x: | :x: |  


:warning: Notes:

1.  The Windows Phone 8.0 implementation is similar to Windows Phone 8.1 SL implementation, this way it will not be provided.

2. For Cordova apps, see the article [MSDN Magazine - Push Notifications to Cordova Apps with Microsoft Azure](http://msdn.microsoft.com/en-us/magazine/dn879353.aspx) by [Glenn Gailey](http://msdn.microsoft.com/en-us/magazine/dn879353.aspx).



## :white_medium_square: The Source Code

The sample has the following project's struture:

* Shared
* BackEnd
* Windows 
  * CSharp
     * Windows Phone 8.1 (WinRT)
     * Windows Store Apps (WinRT)
     * Windows Phone 8.1 (SL)
  * Visual Basic
     * Windows Phone 8.1 (WinRT)
     * Windows Store Apps (WinRT)
     * Windows Phone 8.1 (SL)
* Xamarin 
   * Xamarin.Android
   * Xamarin.IOS


## :white_medium_square: Build the projects


To develop on this project, just clone the project to your computer, package restore is enable so build the solution first, if you get any errors try to build again and if necessary close the solution and open again to load the references.


:warning: Change required before run each sample

The Contants.cs file should be defined, which:

- SenderID: define the ProjectID used in Xamarin Android and Android projects to request a registrationId from Google Cloud Messaging (GCM)
- HubName: define the Notification Hub's name created in Azure Portal
- ConnectionString: define the Notification Hub's connection string for the client applications connect with Notification Hub.
- BackEndConnectionString: define the Notification Hub's connection string for the backend connect with Notification Hub.

- AMSEndpoint: define the url from the Azure Mobile Services, used by the client application when create the object to connect with Azure Mobile Services

- AMSKey: define the admin key used by the client application when create the object to connect with Azure Mobile Services.


## :white_medium_square: Resources


* [MSDN Documentation] (http://msdn.microsoft.com/en-us/library/jj891130.aspx)

* [Curah! Azure Notification Hubs] (https://curah.microsoft.com/72603/notification-hubs)

* [Azure Mobile Services: Add support for Push Notification](http://www.saramgsilva.com/index.php/2014/microsofts-windows-appstudio-add-support-for-push-notification/)

* [Azure Notification Hubs - Diagnosis guidelines](http://azure.microsoft.com/en-us/documentation/articles/notification-hubs-diagnosing/)

* [Debugging Notification Hubs](https://msdn.microsoft.com/library/azure/dn530751.aspx)

## :white_medium_square: Common Issues

The following list provide some common issues I found when I did the sample or even when help others developers.

### In General  

* The connection string from Notification Hubs is wrong;
* The xml/json that define the template are not well defined;
* When debug the notification is not using the correct tag or forget to use the tag;
* Is missing the configurations in Azure Portal for each application;
* In Azure Mobile Services developers uses the Notification Hubs API to manage devices and it is not necessary because Azure Mobile Service give us it out-of-box;
* When developers implement the registration in devices do not use the debug feature to verify if the devices was registered correctly; 
* Developers implements the Case 1 when they want to implement the Case 2 and mixes the two cases;

### In Windows (WinRT) apps:

* In manifest should be defined the Toast capable;
* In manifest should be defined the Internet capability;
* Associate with store;

### In Windows Phone (SL) apps:

* In manifest should be defined Internet capability, Toast capable and the ID_CAP_PushNotification
* Should be handled the notification when the app is running

### In Android apps:

* The Project Id is wrong;
* The GCM component is missing;
* In debug mode, after stop the debug be aware that is need to run again the app to receive the notifications;
* The manifest file must have the package's name starting with lower case;
* The key used in the payload is not the same in the application;

### In iOS apps:

* To enable push notification is required the certificates that enable it. The process is not simple to create it and it is easy to do a mess between steps;
* Developers should confirm which version of the certificate is on Azure Portal and it is used by application. This means if in Azure Portal has the production certificate the application must have the production certificate;
* The plist must have the push notification checked and the bundle must match with the bounded created;


## :white_medium_square: Tips

### Azure Notification Hubs API used in Case 2 sample

Platform | Azure Notification Hubs API 
:---------- | :------------------------
Windows Store 8.1 (WinRT) | [WindowsAzure.Messaging.Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed/)
Windows Phone 8.1 (WinRT) | [WindowsAzure.Messaging.Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed/)
Windows Phone 8.1 (SL)| [WindowsAzure.Messaging.Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed/)
Xamarin Android | [Azure Messaging](https://components.xamarin.com/view/azure-messaging)
Xamarin IOS | [Azure Messaging](https://components.xamarin.com/view/azure-messaging)

### Azure Notification Hubs API used in Azure Mobile Service sample

[Windows Azure Mobile Services](https://www.nuget.org/packages/WindowsAzure.MobileServices/) 

### Push Notification Service by Platform

Platform | Push Notification Service 
:---------- | :------------------------ 
Windows Store 8.1 (WinRT) | Windows Push Notification Services (WNS) 
Windows Phone 8.1 (WinRT) | Windows Push Notification Services (WNS)
Windows Phone 8.1 (SL)| Microsoft Push Notification Service (MPNS)
Windows Phone 8.0 (SL)| Microsoft Push Notification Service (MPNS)
Android | Google Cloud Service (GCM)
IOS | Apple Push Notification Service (APNs)

#### Push Notification Templates by platform

##### Windows Phone 8.1 and Windows Store apps (WinRT)

```
var payload = new XElement("toast",
                         new XElement("visual",
                            new XElement("binding",
                                new XAttribute("template", "ToastText01"),
                                new XElement("text",
                                    new XAttribute("id", "1"), message)))).ToString(SaveOptions.DisableFormatting);
```

##### Windows Phone 8.1 SL

```
var mpnsPushMessage  = new MpnsPushMessage(toast);
        XNamespace wp = "WPNotification";
        XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", null),
            new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"),
                new XElement(wp + "Toast",
                    new XElement(wp + "Text1",
                         "Notification Hubs Sample"),
                    new XElement(wp + "Text2", message))));

var xmlPayload = string.Concat(doc.Declaration, doc.ToString(SaveOptions.DisableFormatting));
```

##### IOS
```
var alert =new JObject(
                new JProperty("aps", new JObject(new JProperty("alert", notificationText))),
                new JProperty("inAppMessage", notificationText))
                .ToString(Newtonsoft.Json.Formatting.None);
```

##### Android
```
var payload = new JObject(
                    new JProperty("data", new JObject(new JProperty("message", notificationText))))
                    .ToString(Newtonsoft.Json.Formatting.None);
```

### Development In Xamarin.Android

If you are using simulator and do the deploy for install the app or debug it, you can receive Push Notification while the app run, but if you stop and try to send another Push Notification the simulator will not receive it, you should run again the app without VS to get the push notification


## :white_medium_square: Contributors

<MTMarkdownOptions output='html4'>
<a href="https://twitter.com/saramgsilva"><img src="http://saramgsilva.github.io/NotificationHubs/images/Eu_400x400.png" height="50"/></a>
<a href="https://twitter.com/paulomorgado"><img src="http://saramgsilva.github.io/NotificationHubs/images/PauloMorgado_320x240_400x400.jpg" height="50"/></a>
<a href="https://twitter.com/adpead"><img src="http://saramgsilva.github.io/NotificationHubs/images/gn8frj8ipi0rsntcvcd0_400x400.jpeg"/ height="50"></a>
<a href="https://twitter.com/clerigo"><img src="http://saramgsilva.github.io/NotificationHubs/images/EbslN-rW_400x400.jpeg"/ height="50"></a>
</MTMarkdownOptions><a href="https://twitter.com/Totemika"><img src="http://s20.postimg.org/77ylj1vj1/Nz2_Jo_RA2_400x400.jpg"/ height="50"></a>	</p>
</MTMarkdownOptions>  	</p>


## :white_medium_square: Contribute


Everbody is welcome to contribute.

Twitter hashtag : [#notificationhubs](https://twitter.com/search?q=%23notificationhubs&src=typd)


## :white_medium_square: License


MIT License (MIT), read more about it in the [LICENSE file](https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/LICENSE.txt).


