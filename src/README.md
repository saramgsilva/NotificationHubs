# Windows and Xamarin applications - Source Code 
## :white_medium_square: Table of contents
* [Introduction](https://github.com/saramgsilva/NotificationHubs/blob/master/src/README.md#Introduction)
* [Case 1 - Devices register directly in Azure Notification Hubs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/README.md#case-1---devices-register-directly-in-azure-notification-hubs)
* [Case 2 - Devices register in Azure Notification Hubs through backends](https://github.com/saramgsilva/NotificationHubs/blob/master/src/README.md#case-2---devices-register-in-azure-notification-hubs-through-backends)
* [Azure Notification Hubs API by platform](https://github.com/saramgsilva/NotificationHubs/blob/master/src/README.md#azure-notification-hubs-api-by-platform)
* [Push Notification Templates by platform](https://github.com/saramgsilva/NotificationHubs/blob/master/src/README.md#push-notification-templates-by-platform)



## :white_medium_square: Introduction

The solution has three main folders:

* Shared: contain the files shared between projects
* Case 1 - Devices register directly in Azure Notification Hubs: contain all projects related with the Case 1
* Case 2 - Devices register in Azure Notification Hubs through backend: contain all projects related with the Case 2

<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/FinalSolution-Close.png"> 
</MTMarkdownOptions>  


:warning: *Change required*

The [Contants.cs file](https://github.com/saramgsilva/NotificationHubs/blob/master/src/Shared/Constants.cs) should be defined because it is required to the sample works:


* *SenderID* - define the ProjectID used in Xamarin Android and Android projects to request a registrationId from Google Cloud Messaging (GCM)
* *HubName* - define the Notification Hub's name created in Azure Portal 
* *ConnectionString* - define the Notification Hub's connection string for the client applications connect with Notification Hub.
* *BackEndConnectionString* - define the Notification Hub's connection string for the backend connect with Notification Hub.
* *AMSEndpoint* - define the url from the Azure Mobile Services, used by the client application when create the object to connect with Azure Mobile Services
* *AMSKey*  - define the admin key used by the client application when create the object to connect with Azure Mobile Services.


## :white_medium_square: Case 1 - Devices register directly in Azure Notification Hubs 

<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/FinalSolution-Case1.png"> 
</MTMarkdownOptions>  
> All projects for the Case 1


* Windows 
   * Windows Phone 8.1 (WinRT)
   * Windows Store Apps (WinRT)
   * Windows Phone 8.1 (SL)
* Xamarin 
   * Xamarin.Android
   * Xamarin.IOS


In this case, is provided the client applications that will connect to the Azure Notification Hubs, to register the device (the registration will happen each time the application runs).



> To test the Push Notifications, you can use the Debug feature for Azure Notification Hubs in Azure Portal or in Visual Studio



## :white_medium_square:  Case 2 - Devices register in Azure Notification Hubs through backends

<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/FinalSolution-Case2-closed.png"> 
</MTMarkdownOptions>  



>  The "Version from January 2015 (v2)" and "*Version from June 2014 (v1)*" are two different versions from the backend, followed the MSDN Documentation, which the implementation are different but very similar

* Version from January 2015 (v2)

  * *Using Azure Mobile Services*
    * *Backend* - define the Azure Mobile Service BackEnd (.Net Backend). See [NotificationHandler](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.AMS/NotificationHandler.cs) and [CarController.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.AMS/Controllers/CarController.cs).
    * *Windows* - define all windows applications
       * *Universal App* - define the Universal application that connect to the Azure Mobile Service. See [App.xaml.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.WinUsingAMS/NotificationHubsSample.WinUsingAMS.Shared/App.xaml.cs) and [MainPage.xaml.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.WinUsingAMS/NotificationHubsSample.WinUsingAMS.Shared/MainPage.xaml.cs)
       * *Windows Phone 8.1 (SL)* - define the WP 8.1 (SL) application that connect to the Azure Mobile Service. See [NotificationService.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.WPSLUsingAMS/NotificationService.cs), [MainPage.xaml.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.WPSLUsingAMS/MainPage.xaml.cs) and [App.xaml.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.WPSLUsingAMS/App.xaml.cs)
    * *Xamarin* - define all xamarin applications
       * *Xamarin Android* - define the Android application that connect to the Azure Mobile Service. See [MyBroadcastReceiver.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.DroidUsingAMS/MyBroadcastReceiver.cs) and [MainActivity.cs](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.DroidUsingAMS/MainActivity.cs) 
       * *Xamarin IOS* - define the IOS application that connect to the Azure Mobile Service

> In each client application is made a request to the backend to register the device, using the Azure Mobile Service API, and is possible to change the registration in the backend through the NotificationHandler. To test the Push Notification is only required to insert a Car and two notifications will be send because client application define one tag and in [NotificationHandler](https://github.com/saramgsilva/NotificationHubs/blob/master/src/NotificationHubsSample.AMS/NotificationHandler.cs) is defined another as example.

* *Version from January 2015 (v2)*
  * *Using WebApi*
    * *Backend* - define the ASP.Net Backend. See [NotificationHubController.cs](http://bit.ly/17AXl7W)
    * *Windows* - define all windows applications
       * *Universal App* - define the Universal application that connect to the backend (ASP.Net WebAPI). See [App.xaml.cs](http://bit.ly/1FM5hTl) and [MainPage.xaml.cs](http://bit.ly/1w9WlfF)
       * *Windows Phone 8.1 (SL)* - define the WP 8.1 (SL) application that connect  to the backend (ASP.Net WebAPI). See [App.xaml.cs](http://bit.ly/1IEIc0Q), [MainPage.xaml.cs](http://bit.ly/1xhv3Er) and [NotificationService.cs](http://bit.ly/1C906XA)
    * *Xamarin* - define all xamarin applications
       * *Xamarin Android* - define the Android application that connect to the backend (ASP.Net WebAPI). See [MyBroadcastReceiver.cs](http://bit.ly/1z0V7dG), [MainActivity.cs](http://bit.ly/1IEIm8l) and [MainPage.cs](http://bit.ly/1AITX4z)
       * *Xamarin IOS* - define the IOS application that connect to the backend (ASP.Net WebAPI)
        
> To test the Push Notification, in each client application, is required to do a login (user and pass shoud be equal) and clicking in the button is made a request to register the device in Notification Hubs and a Push Notification is send.   

* *Version from June 2014 (v1)*
    * *Backend* -  define the ASP.Net Backend. See [RegisterController.cs](http://bit.ly/1A8slDa) or [RegisterUsingTemplatesController.cs](http://bit.ly/1C90BRk)
    * *Windows*
      * *Universal App* - define the Universal application that connect to the backend (ASP.Net WebAPI). See [App.xaml.cs](http://bit.ly/1AIUh3a) and [MainPage.xaml.cs](http://bit.ly/14M9xla)
      * *Windows Phone 8.1 (SL)* - define the WP 8.1 (SL) application that connect  to the backend (ASP.Net WebAPI). See [App.xaml.cs](http://bit.ly/1y55b5s), [MainPage.xaml.cs](http://bit.ly/1sqXcxa) and [NotificationService.cs](http://bit.ly/1C906XA)
    * *Xamarin* - define all xamarin applications
       * *Xamarin Android* - define the Android application that connect to the backend (ASP.Net WebAPI). See [MyBroadcastReceiver.cs](http://bit.ly/1IltjlR), [MainActivity.cs](http://bit.ly/1DBdQ10) and [MainPage.cs](http://bit.ly/1y55AVz)
       * *Xamarin IOS* - define the IOS application that connect to the backend (ASP.Net WebAPI)


> To test the Push Notification, in each client application, is required to do a login (user and pass shoud be equal) and clicking in the button is made a request to register the device in Notification Hubs and a Push Notification is send.      



<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/FinalSolution-Case2-opened.png"> 
</MTMarkdownOptions>  

> All projects for the Case 2


## :white_medium_square: Azure Notification Hubs API by platform

Libraries used in the Case 1 are:

Platform | Azure Notification Hubs API 
:---------- | :------------------------
Windows Store 8.1 (WinRT) | [WindowsAzure.Messaging.Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed/)
Windows Phone 8.1 (WinRT) | [WindowsAzure.Messaging.Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed/)
Windows Phone 8.1 (SL)| [WindowsAzure.Messaging.Managed](https://www.nuget.org/packages/WindowsAzure.Messaging.Managed/)
Xamarin Android | [ByteSmith.WindowsAzure.Messaging.Android.dll](https://github.com/saramgsilva/NotificationHubs/tree/master/src/NotificationHubsSample.Droid/_external) 
Xamarin IOS | ByteSmith.WindowsAzure.Messaging.IOS.dll




## :white_medium_square: Push Notification Templates by platform

#### Windows Phone 8.1 and Windows Store apps (WinRT)

```
var payload = new XElement("toast",
                         new XElement("visual",
                            new XElement("binding",
                                new XAttribute("template", "ToastText01"),
                                new XElement("text",
                                    new XAttribute("id", "1"), message)))).ToString(SaveOptions.DisableFormatting);
```

#### Windows Phone 8.1 SL

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

#### IOS
```
var alert =new JObject(
                new JProperty("aps", new JObject(new JProperty("alert", notificationText))),
                new JProperty("inAppMessage", notificationText))
                .ToString(Newtonsoft.Json.Formatting.None);
```

#### Android
```
var payload = new JObject(
                    new JProperty("data", new JObject(new JProperty("message", notificationText))))
                    .ToString(Newtonsoft.Json.Formatting.None);
```
