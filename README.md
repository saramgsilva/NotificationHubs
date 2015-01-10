Azure Notification Hubs Sample
================

## :white_medium_square: What is this?

The sample provided has the goal to help developers implement Push Notification in mobile applications and this sample supports the two main ways to manage devices in Notification Hubs:

* Case 1 - Devices register directly in Notification Hubs 
* Case 2 - Devices register in Notification Hubs through backend


:white_medium_square: Requirements
================

* Create the Notification Hubs at Azure Portal (Azure Mobile Service create it by default)
* Create the required data, in Push Notification Service (WNS, GCM, APNs...), to support push notification in each Platform
* Configure the Notification Hub, in Azure Portal, with the data from Push Notification Services (WNS, GCM, APNs...)
* Add connectiong string and hub name from the Notification Hub created (in the sample it is defined in [Contants.cs file](https://github.com/saramgsilva/NotificationHubs/blob/master/Shared/Constants.cs))


:white_medium_square: Sample Status
================

* **Key:** :white_check_mark: = Supported,  :x: = Not Supported, :wrench: = In development 


Platform | Case 1 | Case 2 using WebAPI (V1) | Case 2 using WebAPI (V2) | Case 2 using AMS (V2) 
:---------- | :------------------------ | :------------------------ | :------------------------ | :------------------------ |
Windows Store 8.1 (WinRT) | :white_check_mark:  | :white_check_mark: | :white_check_mark: | :white_check_mark: 
Windows Phone 8.1 (WinRT) | :white_check_mark: | :white_check_mark:   | :white_check_mark:   | :white_check_mark: 
Windows Phone 8.1 (SL)| :x: | :x: | :x:| :x:
Windows Phone 8.0 (SL)| :x: | :x:| :x:| :x:
Xamarin Android | :white_check_mark: | :white_check_mark:  | :white_check_mark:   | :white_check_mark: 
Xamarin IOS | :wrench: | :wrench:| :wrench:| :wrench:
Android Native |  :x:  | :x:| :x:| :x:
IOS Native | :white_check_mark:   | :x:| :x:| :x:
Cordova |  :x: | :x:| :x:| :x:

:warning: Notes:


1. The Case V1 was created in June 2014 following the documentation, but it changed and the Case 2 V2 was created to show the new version. At the end all version will work and for example the Azure Mobile Services will uses a solution similar to the Case 2 V1.

2. The IOS Native Demo was created for a presentation by [Edgar Clérigo](https://twitter.com/clerigo)

3. For Cordova apps, is possible to read about it in the article [MSDN Magazine - Push Notifications to Cordova Apps with Microsoft Azure](http://msdn.microsoft.com/en-us/magazine/dn879353.aspx) by [Glenn Gailey](http://msdn.microsoft.com/en-us/magazine/dn879353.aspx).


:white_medium_square: The Solution
=============

The solution have two main cases:

* Case 1 - Devices register directly in Notification Hubs 
* Case 2 - Devices register in Notification Hubs through backend

> Solution with folders minimized

<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/FinalSolution-Close.png"> 
</MTMarkdownOptions>  


> Solution with folders expanded

<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/FinalSolution-Open.png"> 
</MTMarkdownOptions>  


:warning: The [Contants.cs file](https://github.com/saramgsilva/NotificationHubs/blob/master/Shared/Constants.cs) should be define because it is required to the sample works.


:white_medium_square: Screenshots
================

The [SceenShots folder](https://github.com/saramgsilva/NotificationHubs/tree/master/ScreenShots) contains image for each platform provided.

<MTMarkdownOptions output='html4'>
<img align="middle" src="https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/ScreenShots/Windows%20Sample%20-%201%20-%20Registration%20&%20Notification.png"> 
</MTMarkdownOptions>  


:white_medium_square: Resources
================

> Presentation

<MTMarkdownOptions output='html4'>
<a href="http://www.saramgsilva.com/index.php/2014/mobile-notification-for-any-device-using-azure-notification-hubs/" target="_blank"><img align="middle" src="http://s20.postimg.org/xt7iab6jh/presentation.png"> </a>
</MTMarkdownOptions>  

> [Documentation at MSDN] (http://msdn.microsoft.com/en-us/library/jj891130.aspx)

> [Curah! Notification Hubs] (https://curah.microsoft.com/72603/notification-hubs)

> [Azure Notification Hubs – All resources you need](http://www.saramgsilva.com/index.php/2014/azure-notification-hubs-all-resources-you-need/)



:white_medium_square: Common Issues
================

> In General  

* The connection string from Notification Hubs is wrong;
* The xml/json that define the template are not well defined;
* When debug the notification is not using the correct tag or forget to use the tag;
* Is missing the configurations in Azure Portal for each application;
* In Azure Mobile Services developers uses the Notification Hubs API to manage devices and it is not necessary because Azure Mobile Service give us it out-of-box;
* When developers implement the registration in devices do not use the debug feature to verify if the devices was registered correctly; 
* Developers implements the Case 1 when they want to implement the Case 2 and mixes the two cases;

> In Windows apps:

* In manifest should be defined the Toast capable;
* In manifest should be defined the Internet capability;
* Associate with store;

> In Android apps:

* The Project Id is wrong;
* The GCM component is missing;
* In debug mode, after stop the debug be aware that is need to run again the app to receive the notifications;
* The manifest file must have the package's name starting with lower case;
* The key used in the payload is not the same in the application;


:white_medium_square: Tips
================

> Push Notification Service by Platform


Platform | Push Notification Service 
:---------- | :------------------------ |
Windows Store 8.1 (WinRT) | Windows Push Notification Services (WNS) 
Windows Phone 8.1 (WinRT) | Windows Push Notification Services (WNS)
Windows Phone 8.1 (SL)| Microsoft Push Notification Service (MPNS)
Windows Phone 8.0 (SL)| Microsoft Push Notification Service (MPNS)
Android | Google Cloud Service (GCM)
IOS | Apple Push Notification Service (APNs)



> Development In Xamarin.Android

If you are using simulator and do the deploy for install the app or debug it, you can receive Push Notification while the app run, but if you stop and try to send another Push Notification the simulator will not receive it, you should run again the app without VS to get the push notification



:white_medium_square: Build the project
================

To develop on this project, just clone the project to your computer, package restore is enable so build the solution first, if you get any errors try to build again and if necessary close the solution and open again to load the references.



:white_medium_square: Contributions
================


[Sara Silva aka saramgsilva](https://twitter.com/saramgsilva)

[Edgar Clérigo](https://twitter.com/clerigo)


:white_medium_square: Contribute
================

Everbody is welcome to contribute, only is required to provide all cases to manage devices in Azure Notification Hubs.

Twitter hashtag : [#notificationhubs](https://twitter.com/search?q=%23notificationhubs&src=typd)


:white_medium_square: License
================
MIT License (MIT), read more about it in the [LICENSE file](https://raw.githubusercontent.com/saramgsilva/NotificationHubs/master/LICENSE.txt).
