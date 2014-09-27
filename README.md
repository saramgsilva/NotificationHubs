Notification Hubs Demos
================

Demos I used in my talk in TechRefresh (Microsoft)

> [Slides & Demos](http://www.saramgsilva.com/index.php/2014/mobile-notification-for-any-device-using-azure-notification-hubs/)

>[[Azure] Notification Hubs – All resources you need](http://www.saramgsilva.com/index.php/2014/azure-notification-hubs-all-resources-you-need/)

____________________________________________________________________________________________________________________



Helper
================
For run the demos you should:
*1st Create a Notification Hub in Azure Portal;
*2nd Add HubName and Connection String to each demo;

> For IOS
Create Apple Certificate and add this to the project and go to the Notification Hub>Configure>Apple Certificate

> For Android
Create the Project to get the Server Key and add this key go to the Notification Hub>Configure>Google
If you are using simulator use one that has Google Apps or Google Apis and connect you google accoun;

> For Windows and Windows Phone 8.1 Runtime
Associate the demo to the store
Go to the application page in the store > details> and click the url for get WNS information and go to Notification Hub>Configure>Windows

> For Xamarin.Android
If you are using simulator and do the deploy for install the app or debug it, you can receive Push Notification while the app run, but if you stop and try to send another Push Notification the simulator will not receive it, you should run again the app without VS to get the push notification


