# Microsoft Emotion API with Xamarin Mobile Apps #

### The Challenge ###

In this challenge, you will use one of the [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services) API's to bring emotions detection service to your cross-platform Xamarin mobile application. The goal is to use [Microsoft Cognition Services Translator API](https://www.microsoft.com/cognitive-services/en-us/translator-api) to detect happiness or sadness of the user. 

Below steps should help you to complete this challenge. For queries, get in touch with [@mayur_tendulkar](https://twitter.com/mayur_tendulkar) or [@nishanil](https://twitter.com/nishanil) 

This challenge requires:

* Active Microsoft Azure Subscription. Trial is available [here](https://azure.microsoft.com/en-in/free/)
* Microsoft Visual Studio 2015 Update 3 on Windows (with Xamarin tools installed)
* Xamarin Studio on Mac

### Challenge Walkthrough ###

#### Step 01: Get Translation API Key ####

Visit [Azure Portal](https://portal.azure.com/) and click on 'New' button. In the search box, search for 'Cognitive Services'. Click on the result, which will bring up details pane. There will be option to 'create' a service.



Give this service a name, select subscription and make sure API Type selected is '**Translator Text API**' 



Here, select the pricing tier. For this hack, F0 Free tier will work.



Create a new Resource Group and name it 'Xamarin-Mini-Hack' and click on 'Create' button.



Once the service is created, click on Keys section and note down the keys.



#### Step 02: Create Xamarin Mobile Application ####