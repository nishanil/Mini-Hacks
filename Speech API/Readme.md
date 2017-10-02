# Microsoft Speech API with Xamarin.Forms #

### The Challenge ###

In this challenge, you will use one of the [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services) API's to bring Speech Recognition service to your cross-platform Xamarin.Forms application. The goal is to use [Microsoft Cognition Services Speech API]https://azure.microsoft.com/en-in/services/cognitive-services/speaker-recognition/) to translate text from one language to another.

Below steps should help you to complete this challenge. For queries, get in touch with [@mayur_tendulkar](https://twitter.com/mayur_tendulkar) or [@nishanil](https://twitter.com/nishanil) or [@sarthakm](https://twitter.com/sarthakm)

This challenge requires:

* Active Microsoft Azure Subscription. Trial is available [here](https://azure.microsoft.com/en-in/free/)
* Visual Studio 2017 on Mac or Windows (with Mobile Development for .NET)

### Challenge Walkthrough ###

#### Step 01: Get Translation API Key ####

Visit [Azure Portal](https://portal.azure.com/) and click on 'New' button. In the search box, search for 'Bing Speech API'. Click on the result, which will bring up details pane. There will be option to 'create' a service.

![](https://github.com/sarthakmahapatra/Mini-Hacks/blob/master/Speech%20API/Images/01-Cognitive-Services-Selection.png)

Give this service a name and select the subscription.

Select the pricing tier. For this hack, F0 Free tier will work.

Create a new Resource Group  and click on 'Create' button.

![](https://github.com/sarthakmahapatra/Mini-Hacks/blob/master/Speech%20API/Images/02-Cognitive-Services-Creation.png)


Once the service is created, click on Keys section and note down the keys.

![](https://github.com/sarthakmahapatra/Mini-Hacks/blob/master/Speech%20API/Images/03-Cognitive-Services-Keys.png)

#### Step 02: Create Xamarin.Forms Application ####

* Create a new Xamarin.Forms application and name it SpeechToText.

* Add `SpeechResult.cs` class, this will be Model that will be to returned from the Bing Speech API.

```csharp
    [JsonObject("result")]
    public class SpeechResult
    {
      public string Scenario { get; set; }
      public string Name { get; set; }
      public string Lexical { get; set; }
      public string Confidence { get; set; }
    }

    public class SpeechResults
    {
      public List<SpeechResult> results { get; set; }
    }
```

* Add `BingSpeechService.cs` class, which will translate the speech to text using the Bing Speech API. In this class, replace [insert-translator-service-key] with your Key created earlier.

```csharp
    public class BingSpeechService
    {

        private static readonly string authenticationTokenEndpoint = "https://api.cognitive.microsoft.com/sts/v1.0";
        private static readonly string bingSpeechApiKey = "9bd67ca367fa4f2d9e4874eea4e4f5d6";
        private static readonly string speechRecognitionEndpoint = "https://speech.platform.bing.com/recognize";
        private static readonly string audioContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

        private string _operatingSystem;
        private string _token;


        public BingSpeechService(string os)
        {
            _operatingSystem = os;
        }

        public async Task<SpeechResult> RecognizeSpeechAsync(string filename)
        {
            if (string.IsNullOrWhiteSpace(_token))
            {
                _token = await FetchTokenAsync(authenticationTokenEndpoint, bingSpeechApiKey);
            }

            // Read audio file to a stream
            var file = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(filename);
            var fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read);

            // Send audio stream to Bing and deserialize the response
            string requestUri = GenerateRequestUri(speechRecognitionEndpoint);
            string accessToken = _token;
            var response = await SendRequestAsync(fileStream, requestUri, accessToken, audioContentType);
            var speechResults = JsonConvert.DeserializeObject<SpeechResults>(response);

            fileStream.Dispose();
            return speechResults.results.FirstOrDefault();
        }

        string GenerateRequestUri(string speechEndpoint)
        {
            string requestUri = speechEndpoint;
            requestUri += @"?scenarios=ulm";                                    // websearch is the other option
            requestUri += @"&appid=a4f08ca2-3eff-4eaf-ac2a-2634a14a0074";       // You must use this ID.
            requestUri += @"&locale=en-US";                                     // Other languages supported.
            requestUri += string.Format("&device.os={0}", _operatingSystem);     // Open field
            requestUri += @"&version=3.0";                                      // Required value
            requestUri += @"&format=json";                                      // Required value
            requestUri += @"&instanceid=2becfc37-fac9-4c40-a6e8-06a13f68944c";  // GUID for device making the request
            requestUri += @"&requestid=" + Guid.NewGuid().ToString();           // GUID for the request
            return requestUri;
        }

        async Task<string> SendRequestAsync(Stream fileStream, string url, string bearerToken, string contentType)
        {
            var content = new StreamContent(fileStream);
            content.Headers.TryAddWithoutValidation("Content-Type", contentType);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                var response = await httpClient.PostAsync(url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }


        async Task<string> FetchTokenAsync(string fetchUri, string apiKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
                UriBuilder uriBuilder = new UriBuilder(fetchUri);
                uriBuilder.Path += "/issueToken";

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
```

* Modify `MainPage.xaml` to allow user to record speech and display the result.

```xml
<?xml version="1.0" encoding="utf-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" xmlns:local="clr-namespace:SpeechAPI" x:Class="SpeechAPI.SpeechAPIPage" x:Name="page" Title="Bing API Sample">
    <ContentPage.Resources>
        <ResourceDictionary>
            <local:NegateBooleanConverter x:Key="converter" />
        </ResourceDictionary>
    </ContentPage.Resources>
    <StackLayout>
        <Label x:Name="txtSpeech" HeightRequest="200" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" Text="{Binding SpeechText}" VerticalOptions="Center" HorizontalOptions="CenterAndExpand" />
        <Label x:Name="lblStatus" Text="Status" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" VerticalOptions="Center" HorizontalOptions="CenterAndExpand" />
        <Label x:Name="txtStatus" Text="{Binding Status, Mode=OneWay}" VerticalTextAlignment="Center" HorizontalTextAlignment="Center" VerticalOptions="Center" HorizontalOptions="CenterAndExpand" />
        <ActivityIndicator HorizontalOptions="Center" VerticalOptions="Center" IsRunning="{Binding IsProcessing}" />
        <Button Text="Start Record" x:Name="btnStartRecord" VerticalOptions="End" Command="{Binding StartRecordingCommand}" IsEnabled="{Binding IsNotRecodingOrProcessing}" />
        <Button Text="Stop Record" x:Name="btnStopRecord" VerticalOptions="End" Command="{Binding StopRecordingCommand}" IsEnabled="{Binding IsRecodingAndNotProcessing}" />
    </StackLayout>
</ContentPage>
```

* Modify `MainPage.xaml.cs`. Bind the SpeechViewModel the View

```csharp
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new SpeechViewModel();
        }
    }
```

Run the apps and notice the output. Display the result to judges to mark your hack as complete.
