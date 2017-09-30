# Microsoft Translator API with Xamarin.Forms #

### The Challenge ###

In this challenge, you will use one of the [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services) API's to bring translation service to your cross-platform Xamarin.Forms application. The goal is to use [Microsoft Cognition Services Translator API](https://www.microsoft.com/cognitive-services/en-us/translator-api) to translate text from one language to another. 

Below steps should help you to complete this challenge. For queries, get in touch with [@mayur_tendulkar](https://twitter.com/mayur_tendulkar) or [@nishanil](https://twitter.com/nishanil) 

This challenge requires:

* Active Microsoft Azure Subscription. Trial is available [here](https://azure.microsoft.com/en-in/free/)
* Visual Studio 2017 on Mac or Windows (with Mobile Development for .NET)

### Challenge Walkthrough ###

#### Step 01: Get Translation API Key ####

Visit [Azure Portal](https://portal.azure.com/) and click on 'New' button. In the search box, search for 'Cognitive Services'. Click on the result, which will bring up details pane. There will be option to 'create' a service.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Translator%20API/Images/01-Cognitive-Services-Creation.png)

Give this service a name, select subscription and make sure API Type selected is '**Translator Text API**' 

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Translator%20API/Images/02-Cognitive-Services-Translator.png)

Here, select the pricing tier. For this hack, F0 Free tier will work.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Translator%20API/Images/03-Cognitive-Services-Pricing-Tier.png)

Create a new Resource Group and name it 'Xamarin-Mini-Hack' and click on 'Create' button.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Translator%20API/Images/04-Cognitive-Services-Create.png)

Once the service is created, click on Keys section and note down the keys.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Translator%20API/Images/05-Cognitive-Services-Keys.png)

#### Step 02: Create Xamarin.Forms Application ####

* Create a new Xamarin.Forms application and name it TranslatorClient.
* Add `TranslateService.cs` class, which will translate the text from one language to another. In this class, replace [insert-translator-service-key] with your Key from above step.

```csharp
static class TranslateService
{
	private const string SecretKey = "[insert-translator-service-key]";
    private static string AccessToken = string.Empty;
    public static async Task<string> TranslateText(string textToTranslate, string languageCode)
    {
    	var textToReturn = string.Empty;
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SecretKey);         
        var data = await client.PostAsync("https://api.cognitive.microsoft.com/sts/v1.0/issueToken", new StringContent(""));
        AccessToken = data.Content.ReadAsStringAsync().Result;
        client.DefaultRequestHeaders.Clear();
        var authHeader = "Bearer " + AccessToken;
        var translateEndpoint = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text='" + textToTranslate +"'&to= " + languageCode;
		client.DefaultRequestHeaders.Add("Authorization", authHeader);
        var result = await client.GetStringAsync(translateEndpoint);
        var xTranslation = XDocument.Parse(result);
        textToReturn = xTranslation.Root?.FirstNode.ToString();
        return textToReturn;
	}
}
```

* Modify `MainPage.xaml` to allow user to enter the text to translate, click on button which will translate the text and label to display the translated text.

```xml
<StackLayout HorizontalOptions="Center" VerticalOptions="Center" Padding="30" Spacing="20">
	<Entry x:Name="TextToTranslate" Placeholder="Enter text to translate" />
    <Button x:Name="Authenticate" Text="Authenticate" Clicked="Authenticate_Clicked"/>
    <Label x:Name="TranslatedText" />
</StackLayout>
```

* For button click event handler, modify `MainPage.xaml.cs`

```csharp
private async void Authenticate_Clicked(object sender, EventArgs e)
{
	TranslatedText.Text = await TranslateService.TranslateText(TextToTranslate.Text, "hi");
}
```

Run the apps and notice the output. Display the result to judges to mark your hack as complete.
