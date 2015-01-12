# Azure Notification Hubs Sample - Android Native - Source Code

The solution has three main folders:

* Shared: contain the files shared between projects;
* Case 1: contain all projects related with the Case 1 - Devices register directly in Azure Notification Hubs:
* Case 2: contain all projects related with the Case 2 - Devices register in Azure Notification Hubs through backend;

<MTMarkdownOptions output='html4'>
<img align="middle" src=""> 
</MTMarkdownOptions>  


:warning: *Change required*

The :interrobang: Contants file should be defined because it is required to the sample works:


* *SenderID* - define the ProjectID used in Xamarin Android and Android projects to request a registrationId from Google Cloud Messaging (GCM)
* *HubName* - define the Notification Hub's name created in Azure Portal 
* *ConnectionString* - define the Notification Hub's connection string for the client applications connect with Notification Hub.
* *BackEndConnectionString* - define the Notification Hub's connection string for the backend connect with Notification Hub.
* *AMSEndpoint* - define the url from the Azure Mobile Services, used by the client application when create the object to connect with Azure Mobile Services
* *AMSKey*  - define the admin key used by the client application when create the object to connect with Azure Mobile Services.


### Case 1 - Devices register directly in Azure Notification Hubs 



> To test the Push Notifications, you can use the Debug feature for Azure Notification Hubs in Azure Portal or in Visual Studio



###  Case 2 - Devices register in Azure Notification Hubs through backends


>  The "Version from January 2015 (v2)" and "*Version from June 2014 (v1)*" are two different versions from the backend, followed the MSDN Documentation, which the implementation are different but very similar

* Version from January 2015 (v2)
  * Using Azure Mobile Services
  * Using WebApi
* Version from June 2014 (v1)

> To test the Push Notification, in each client application, is required to do a login (user and pass shoud be equal) and clicking in the button is made a request to register the device in Notification Hubs and a Push Notification is send.      



### Azure Notification Hubs API used
:interrobang:
