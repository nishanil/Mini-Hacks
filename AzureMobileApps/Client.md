# AzureMobileApps - Client
In order to complete this step of the AzureMobileApps challenge, you have to develop Client end code for atleast two of the platforms - Windows , Android , or IOS and connect it to the Azure Mobile apps created under [Step 1](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Server.md) of this challenge.

##Challenge Walkthrough
###Step 1: 
Open Visual Studio 2015 > File > New Project > Choose *Blank XAML App* (Xamarin.Forms.Portable) under the Cross-Platform Visual C# template > Name it *AzureMobileApp*.

Note: Choose the target platform and minimum platform versions that your Universal Windows application will support. Click OK on this dialogue box.


###Step 2: 
Right Click on AzureMobileApp(Portable) shared Project > Add > Class > Name it myTable.
Add the below code in your class. This class is required in order to match the table schema from your Azure Mobile App table which you created in [Step 1](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Server.md) of this challenge of the challenge.
#####NOTE: The class name should match the Table name in your easy tables on Azure Mobile App.


````csharp
    public class myTable
    {
        string desc;
        string name;
        string url;
        string id;

        [JsonProperty(PropertyName = "ID")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "NAME")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty(PropertyName = "DESC")]
        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        [JsonProperty(PropertyName = "URL")]
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

    }

```

NOTE: Add the NugetPackage called ["Newtonsoft.Json"](https://www.nuget.org/packages/Newtonsoft.Json/ ) to the shared project.
Resolve the reference issues in your myTable class by adding the below namespace.

```chsarp
using Newtonsoft.Json;
```

###Step 3: Let's create the UI for our client Project.
Got to MainPage.XAML page and add the below code under the *ContentPage* node.

```XML
 <ContentPage.Content>
    <Grid>
      <Grid.RowDefinitions>

      <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
        <RowDefinition Height="Auto" />
         <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
      </Grid.RowDefinitions>

      <Button Text="Google" x:Name="Google"
               Clicked="OnAdd" BackgroundColor="#df4a32" Grid.Row="0"/>

      <Button Text="Twitter"
                 
              Clicked="OnAdd"  BackgroundColor="#1da1f2" Grid.Row="1"  />
      <Button Text="Microsoft"
                  
              Clicked="OnAdd"  BackgroundColor="#a030cb"  Grid.Row="2"/>
      <Button Text="Facebook"
              MinimumHeightRequest="30" MinimumWidthRequest ="30"
              Clicked="OnAdd"  BackgroundColor="#3b5998" Grid.Row="3" />

 
      <Label Text="Login to see your Role Model! " HorizontalOptions="Center" Grid.Row="4"></Label>
      <ListView x:Name="DisplayList" Grid.Row="5" HasUnevenRows="true" >
        <ListView.ItemTemplate>
          <DataTemplate>
            <ViewCell Height="300">
             
              <Image Source="{Binding URL}" Aspect="Fill"/>
          
            </ViewCell>
          </DataTemplate>
        </ListView.ItemTemplate>
      </ListView>

    </Grid>
  </ContentPage.Content> 
  
  ```

###Step 4

Let's start to add the code for Authentication of Facebook,Google,Twitter,Microsoft. Since, authentication is platform specific, we have to create an interface which can be called from each of the platforms.

Go to MainPage.xaml.cs under your shared project and create an *interface*. Add the below code to implement this.

````chsarp
        public interface IAuthenticate
        {
            Task<bool> Authenticate(int Provider);
        }

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }
```


###Step 5:
Add the below variables under MainPage.XAML.cs

````csharp
        public static string ApplicationURL = @"https://<ReplaceName>.azurewebsites.net";
        MobileServiceClient client;
        IMobileServiceTable<myTable> myList;
        string serviceProvider = "";

        int Provider;
```

Note:

1. Replace the Application URL with the AzureMobileApp which you have created under [Step 1](https://github.com/nishanil/Mini-Hacks/blob/master/AzureMobileApps/Server.md) of the challenge.

2. Add the NugetPackage called ["Microsoft.Azure.Mobile.Client"](https://www.nuget.org/packages/Microsoft.Azure.Mobile.Client/ ) to the SOLUTION and resolve the reference issues. Make sure you have checked all the projects (AzureMobileApp.Droid, AzureMobileApp.IOS and AzureMobileApp.UWP) while installing the nuget package. You will require this later as well.

###Step 6:
Add the below code to implement the ButtonClick handler 

````csharp
//Checks which of the authentication button is clicked.
        private async void OnAdd(object sender, EventArgs e)
        {

            var b = sender as Button;
            bool result = false;
            switch (b.Text.ToString())
            {
                case "Google":
                    {
                        Provider = 0;
                        serviceProvider = "Google";
                        result = await Authenticator.Authenticate(Provider);
                        break;
                    }
                case "Twitter":
                    {
                        Provider = 1;
                        serviceProvider = "Twitter";
                        result = await Authenticator.Authenticate(Provider);
                        break;
                    }
                case "Microsoft":
                    {
                        Provider = 2;
                        serviceProvider = "Microsoft";
                        result = await Authenticator.Authenticate(Provider);
                        break;
                    }
                case "Facebook":
                    {
                        Provider = 3;
                        serviceProvider = "Facebook";
                        result = await Authenticator.Authenticate(Provider);
                        break;
                    }
            }



            if (result)
            {
                this.client = new MobileServiceClient(ApplicationURL);
                this.myList = client.GetTable<myTable>();

                //Get the items from the myTable from your Azure Mobile apps table at Azure
                DisplayList.ItemsSource = await myList.Where(myList => myList.Name == serviceProvider).ToEnumerableAsync();
            }


        }
```

####Let's add the platform Specific codes to implement the client end scenarios.

##Android
Go to AzureMobileApp.Droid > MainActivity.cs page and add the below code.
Implement the IAuthenticate interface which you created by changing the below line as.

````csharp
public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
```

Call the below function inside the OnCreate function. It should be added before the Xamarin forms is initiated.

````csharp
MainPage.Init((IAuthenticate)this);

```

NOTE : Resolve the references errors by adding the appropriate namespaces.

Add the below code to inside the MainActivity.cs to implement the authentication function.

````csharp
private MobileServiceUser user;
        MobileServiceClient client = new MobileServiceClient("https://<Repace Name>.azurewebsites.net");

        public MobileServiceAuthenticationProvider ServiceProvider { get; private set; }
        public async Task<bool> Authenticate(int Provider)
        {
            var success = false;
            var message = string.Empty;

            switch (Provider)
            {
                case 0:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Google;
                        break;
                    }
                case 1:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Twitter;
                        break;
                    }
                case 2:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.MicrosoftAccount;
                        break;
                    }
                case 3:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Facebook;
                        break;
                    }

            }



            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await client.LoginAsync(this,
                    ServiceProvider);
                if (user != null)
                {
                    message = string.Format("you are now signed-in as {0}.",
                        user.UserId);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }
```


##Windows

Go to AzureMobileApp.UWP> MainPage.XAML.cs page and add the below code.
Implement the IAuthenticate interface which you created by changing the below line as.

````csharp
 public sealed partial class MainPage:IAuthenticate
```

Call the below function inside the Constructor. It should be added before the LoadApplication line.

````csharp
  AzureMobileApps.MainPage.Init(this);

```

NOTE : Resolve the references errors by adding the appropriate namespaces.

Add the below code to inside the MainPage.XAML.cs to implement the authentication function.

````chsarp
private MobileServiceUser user;
        MobileServiceClient client = new MobileServiceClient("https://<Replace Name>.azurewebsites.net");

        public MobileServiceAuthenticationProvider ServiceProvider { get; private set; }

        public async Task<bool> Authenticate(int Provider)
        {
            string message = string.Empty;
            var success = false;

            switch(Provider)
            {
                case 0:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Google;
                        break;
                    }
                case 1:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Twitter;
                        break;
                    }
                case 2:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.MicrosoftAccount;                        break;
                    }
                case 3:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Facebook;
                        break;
                    }

            }

           

            try
            {
               // Sign in with the respective Service Provider login using a server-managed flow.
                if (user == null)
                {
                    user = await client.LoginAsync(ServiceProvider);
                    if (user != null)
                    {
                        success = true;
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                    }
                }

            }
            catch (Exception ex)
            {
                message = string.Format("Authentication Failed: {0}", ex.Message);
            }

          //  Display the success or failure message.
            await new MessageDialog(message, "Sign-in result").ShowAsync();

            return success;
        }
```

##iOS
Go to AzureMobileApp.IOS> AppDelegate.cs page and add the below code.
Implement the IAuthenticate interface which you created by changing the below line as.

````csharp
public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate

```

Call the below function inside the Constructor. It should be added before the LoadApplication line.

````csharp
 MainPage.Init((IAuthenticate)this);

```

NOTE : Resolve the references errors by adding the appropriate namespaces.

Add the below code to inside the MainPage.XAML.cs to implement the authentication function.

````chsarp
 // Define a authenticated user.
        private MobileServiceUser user;
        MobileServiceClient client = new MobileServiceClient("https://apchin-mobileapp.azurewebsites.net");

        public MobileServiceAuthenticationProvider ServiceProvider { get; private set; }

        public async Task<bool> Authenticate(int Provider)
        {
            var success = false;
            var message = string.Empty;

            switch (Provider)
            {
                case 0:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Google;
                        break;
                    }
                case 1:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Twitter;
                        break;
                    }
                case 2:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.MicrosoftAccount;
                        break;
                    }
                case 3:
                    {
                        ServiceProvider = MobileServiceAuthenticationProvider.Facebook;
                        break;
                    }

            }

            try
            {
                // Sign in with Facebook login using a server-managed flow.
                if (user == null)
                {
                    user = await client.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController,
                       ServiceProvider);
                    if (user != null)
                    {
                        message = string.Format("You are now signed-in as {0}.", user.UserId);
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }
```

### Compile the Project and RUN it. After authenticating with one of the Service Provider, you should be able to retrieve an image of your RoleModel. ;)


##You have Completed the Challenge!


#Bonus!
1. Try to retrieve other details like the EmailID, UserName after authentication from either Facebook, Twitter, Microsoft LIVE, Google is complete and display them in your client app.
2. Display the Description of your Role Model in the client app.



