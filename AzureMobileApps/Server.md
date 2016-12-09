#AzureMobileApps - Server

This is the STEP 1 of the Azure Mobile App challenge. In this, you will be creating an Azure Mobile app and integrating it with a SQL Server and adding authentication in the backend Node.js server.
　
You must add Authentication (Service Providers) in your app backend by at least two of these providers: Facebook, Google, Microsoft and Twitter. 

##Challenge Walkthrough:
You can begin by  creating a Mobile app on Azure. Go to the *Quick Start* section and integrate it with a SQL database. Later you can follow steps for registering your applications with your favourite providers and adding the details of the same to Azure app.

* Create Azure Mobile app
* Register your app with a provider 
   
##Create Azure Mobile App
　
    1. Go to Azure Portal. Under "New", select Mobile App.
　
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/a.png)


    2. Add required details and click on Create.

　
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/B.png)

    3. Go to the *QuickStart* section and connect the AzureMobileApp to the following
*    SQL Database Server.
*    Node.js backend - You can create this easily by following the second step under the QuickStart section.

At the end of this step, you will see something similar to the below image.
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/SQL.PNG)

4.Go under the *Easy Tables* tab under your Azure Mobile App > Click on ADD FROM CSV > Upload the myTable.csv which is present  [here](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/myTable.csv)


##Register your app with a provider (Any 2)
Note: in this process, you will come across some keys/passwords/client secrets specific to the provider you choose and your app. Do not share this secret with anyone or distribute it within a client application.  
　
##Register your application with Facebook
1. Log on to the Azure portal, and navigate to your application. Copy your URL. You will use this to configure your Facebook app.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/C.png)

2. In another browser window, navigate to the Facebook Developers website and sign-in with your Facebook account credentials.
3.	(Optional) If you have not already registered, click Apps > Register as a Developer, then accept the policy and follow the registration steps.
4. Click My Apps > Add a New App > Website > Skip and Create App ID. 
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/D.png)


5. In Display Name, type a unique name for your app, type your Contact Email, choose a Category for your app, then click Create App ID and complete the security check. This takes you to the developer dashboard for your new Facebook app.
6.	Under "Facebook Login," click Get Started. Add your application's Redirect URI to Valid OAuth redirect URIs, then click Save Changes. 
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/E.png)


![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/F.png)

7.	In the left-hand navigation, click Settings. On the App Secret field, click Show, provide your password if requested, then make a note of the values of App ID and App Secret. You use these later to configure your application in Azure.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureAuthentication/Images/G.png)
8.	The Facebook account which was used to register the application is an administrator of the app. At this point, only administrators can sign into this application. To authenticate other Facebook accounts, click App Review and enable Make public to enable general public access using Facebook authentication.


![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/H.png)


##Add Facebook information to your application

Back in the Azure portal, navigate to your application. Click Settings > Authentication / Authorization, and make sure that App Service Authentication is On.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/I.png)

Click Facebook, paste in the App ID and App Secret values which you obtained previously, optionally enable any scopes needed by your application, then click OK.
　
　

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/J.png)
By default, App Service provides authentication but does not restrict authorized access to your site content and APIs. You must authorize users in your app code.
4.	(Optional) To restrict access to your site to only users authenticated by Facebook, set Action to take when request is not authenticated to Facebook. This requires that all requests be authenticated, and all unauthenticated requests are redirected to Facebook for authentication.
5.	When done configuring authentication, click Save.

You are now ready to use Facebook for authentication in your app.
　
　
　
##Register your application with Twitter

To complete the procedure in this topic, you must have a Twitter account that has a verified email address and phone number. To create a new Twitter account, go to twitter.com.
Log on to the Azure portal, and navigate to your application. Copy your URL. You will use this to configure your Twitter app.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/K.png)

3.	Navigate to the Twitter Developers website, sign in with your Twitter account credentials, and click Create New App.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/L.png)

4.	Type in the Name and a Description for your new app. Paste in your application's URL for the Website value. Then, for the Callback URL, paste the Callback URL you copied earlier. This is your Mobile App gateway appended with the path, /.auth/login/twitter/callback. For example, https://contoso.azurewebsites.net/.auth/login/twitter/callback. Make sure that you are using the HTTPS scheme.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/M.png)

5.	At the bottom the page, read and accept the terms. Then click Create your Twitter application. This registers the app displays the application details.
6.	Click the Settings tab, check Allow this application to be used to sign in with Twitter, then click Update Settings.
7.	Select the Keys and Access Tokens tab. Make a note of the values of Consumer Key (API Key) and Consumer secret (API Secret). 

##Add Twitter information to your application

Back in the Azure portal, navigate to your application. Click Settings, and then Authentication / Authorization.
If the Authentication / Authorization feature is not enabled, turn the switch to On.
Click Twitter. Paste in the App ID and App Secret values which you obtained previously. Then click OK.
　


![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/N.png)


By default, App Service provides authentication but does not restrict authorized access to your site content and APIs. You must authorize users in your app code.
4.	(Optional) To restrict access to your site to only users authenticated by Twitter, set Action to take when request is not authenticated to Twitter. This requires that all requests be authenticated, and all unauthenticated requests are redirected to Twitter for authentication.
5.	Click Save.

You are now ready to use Twitter for authentication in your app.
　
　
　
To complete the procedure in this topic, you must have a Google account that has a verified email address. To create a new Google account, go to accounts.google.com.+ 
##Register your application with Google

To complete the procedure in this topic, you must have a Google account that has a verified email address. To create a new Google account, go to accounts.google.com.
Log on to the Azure portal, and navigate to your application. Copy your URL, which you use later to configure your Google app.

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/O.png)

3.	Navigate to the Google apis website, sign in with your Google account credentials, click Create Project, provide a Project name, then click Create.
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/P.png)

　
5.	Under Social APIs click Google+ API and then Enable.


![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/Q.png)


6.	In the left navigation, Credentials > OAuth consent screen, then select your Email address, enter a Product Name, and click Save. 
7.	

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/R.png)


8.	In the Credentials tab, click Create credentials > OAuth client ID, then select Web application.
　

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/S.png)



9.	Paste the App Service URL you copied earlier into Authorized JavaScript Origins, then paste your redirect URI into Authorized Redirect URI. The redirect URI is the URL of your application appended with the path, /.auth/login/google/callback. For example, https://contoso.azurewebsites.net/.auth/login/google/callback. Make sure that you are using the HTTPS scheme. Then click Create.
10.	On the next screen, make a note of the values of the client ID and client secret.

##Add Google information to your application

Back in the Azure portal, navigate to your application. Click Settings, and then Authentication / Authorization.
If the Authentication / Authorization feature is not enabled, turn the switch to On.
Click Google. Paste in the App ID and App Secret values which you obtained previously, and optionally enable any scopes your application requires. Then click OK.
![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/T.png)


By default, App Service provides authentication but does not restrict authorized access to your site content and APIs. You must authorize users in your app code.
4.	Click Save.

##Register your app with Microsoft Account

Log on to the Azure portal, and navigate to your application. Copy your URL, which later you use to configure your app with Microsoft Account.
Navigate to the My Applications page in the Microsoft Account Developer Center, and log on with your Microsoft account, if required.

　
3.	Click Add an app, then type an application name, and click Create application.
4.	Make a note of the Application ID, as you will need it later. 

5.	Under "Platforms," click Add Platform and select "Web".

6.	Under "Redirect URIs" supply the endpoint for your application, then click Save. 

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/U.png)



Note
Your redirect URI is the URL of your application appended with the path, /.auth/login/microsoftaccount/callback. For example, https://contoso.azurewebsites.net/.auth/login/microsoftaccount/callback.
Make sure that you are using the HTTPS scheme.
7.	Under "Application Secrets," click Generate New Password. Make note of the value that appears. Once you leave the page, it will not be displayed again.
[AZURE.IMPORTANT] The password is an important security credential. Do not share the password with anyone or distribute it within a client application.

+ 
##Add Microsoft Account information to your App Service application

Back in the Azure portal, navigate to your application, click Settings > Authentication / Authorization.
If the Authentication / Authorization feature is not enabled, switch it On.
Click Microsoft Account. Paste in the Application ID and Password values which you obtained previously, and optionally enable any scopes your application requires. Then click OK.
　

![IMAGE](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Images/V.png)



By default, App Service provides authentication but does not restrict authorized access to your site content and APIs. You must authorize users in your app code.
4.	(Optional) To restrict access to your site to only users authenticated by Microsoft account, set Action to take when request is not authenticated to Microsoft Account. This requires that all requests be authenticated, and all unauthenticated requests are redirected to Microsoft account for authentication.
5.	Click Save.

You are now ready to use Microsoft Account for authentication in your app.
##Content Reference: 
https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-how-to-configure-facebook-authentication
　

　

　
　
　
　
　
　
　

　
