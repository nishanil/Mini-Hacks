# Microsoft Emotion API with Xamarin Mobile Apps #

### The Challenge ###

In this challenge, you will use one of the [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services) API's to bring emotions detection service to your cross-platform Xamarin mobile application. The goal is to use [Microsoft Cognition Services Translator API](https://www.microsoft.com/cognitive-services/en-us/translator-api) to detect happiness or sadness of the user. 

Below steps should help you to complete this challenge. For queries, get in touch with [@mayur_tendulkar](https://twitter.com/mayur_tendulkar), [@AparnaChinya](https://twitter.com/AparnaChinya) or [@nishanil](https://twitter.com/nishanil) 

This challenge requires:

* Active Microsoft Azure Subscription. Trial is available [here](https://azure.microsoft.com/en-in/free/)
* Microsoft Visual Studio 2015 Update 3 on Windows (with Xamarin tools installed)
* Xamarin Studio on Mac

### Challenge Walkthrough ###

#### Step 01: Get Translation API Key ####

Visit [Azure Portal](https://portal.azure.com/) and click on 'New' button. In the search box, search for 'Cognitive Services'. Click on the result, which will bring up details pane. There will be option to 'create' a service.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Emotion%20API/Images/01-Cognitive-Services-Creation.png)

Give this service a name, select subscription and make sure API Type selected is '**Translator Text API**' 

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Emotion%20API/Images/02-Cognitive-Services-Emotion-API.png)

Here, select the pricing tier. For this hack, F0 Free tier will work.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Emotion%20API/Images/03-Cognitive-Services-Pricing-Tier.png)

Create a new Resource Group and name it 'Xamarin-Mini-Hack' and click on 'Create' button.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Emotion%20API/Images/04-Cognitive-Services-Resource-Group.png)

Once the service is created, click on Keys section and note down the keys.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Emotion%20API/Images/05-Cognitive-Services-Keys.png)

#### Step 02: Create Xamarin Mobile Application ####

* In Visual Studio create a blank solution as 'EmotionClient'
* Add new Android Blank App and name it as 'EmotionClient.Droid'
* Add new iPhone Single View App and name it as 'EmotionClient.iOS'
* Add new UWP Blank App name it 'EmotionClient.UWP'
* Add new Shared Project and name it as 'EmotionClient.Shared'
* Add reference to this Shared Project in iOS, Windows and Android projects.

##### Add required NuGet Packages #####

For iOS, Windows and Android projects add `'Microsoft.ProjectOxford.Emotion'` NuGet package. 

Note: If you get any error, add `'Microsoft.Bcl.Build NuGet'` package first and then try to add `'Microsoft.ProjectOxford.Emotion'`

##### Build EmotionClient.Shared #####

In this project add `'Core.cs'` class which will consume Emotion API and will be shared across iOS, Windows and Android applications.

```csharp
public class Core
{
	private static async Task<Emotion[]> GetHappiness(Stream stream)
    {
    	string emotionKey = "[insert-emotion-service-key]";
        EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);
        var emotionResults = await emotionClient.RecognizeAsync(stream);
        if (emotionResults == null || emotionResults.Count() == 0)
        {
        	throw new Exception("Can't detect face");
		}
        return emotionResults;
	}
    public static async Task<float> GetAverageHappinessScore(Stream stream)
    {
    	Emotion[] emotionResults = await GetHappiness(stream);
        float score = 0;
        foreach (var emotionResult in emotionResults)
        {
        	score = score + emotionResult.Scores.Happiness;
		}
        return score / emotionResults.Count();
	}
	public static string GetHappinessMessage(float score)
    {
    	score = score * 100;
        double result = Math.Round(score, 2);
        if (score >= 50)
        	return result + " % :-)";
		else
        	return result + "% :-(";
	}
}
```

##### Build EmotionClient.Droid #####

In this project, modify `'Main.axml'` file to create UI for the screen.

```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
    android:orientation="vertical"
    android:layout_width="fill_parent"
    android:layout_height="fill_parent">
    <ImageView
    android:src="@android:drawable/ic_menu_gallery"
    android:layout_width="fill_parent"
    android:layout_height="300.0dp"
    android:id="@+id/imageView1"
    android:adjustViewBounds="true" />
    <TextView
    android:textAppearance="?android:attr/textAppearanceLarge"
    android:layout_width="fill_parent"
    android:layout_height="wrap_content"
    android:id="@+id/resultText"
    android:textAlignment="center" />
    <Button
    android:id="@+id/GetPictureButton"
    android:layout_width="fill_parent"
    android:layout_height="wrap_content"
    android:text="Take Picture" />
</LinearLayout>
```

Add `'BitmapHelpers.cs'` class to this project to make sure photographs are in right orientation when they are taken.

```csharp
public static class BitmapHelpers
{
	public static Bitmap GetAndRotateBitmap(string fileName)
    {
    	Bitmap bitmap = BitmapFactory.DecodeFile(fileName);
        using (Matrix mtx = new Matrix())
        {
        	if (Android.OS.Build.Product.Contains("Emulator"))
            {
            	mtx.PreRotate(90);
			}
            else
            {
            	ExifInterface exif = new ExifInterface(fileName);
                var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);
                switch (orientation)
                {
                	case Orientation.Rotate90:
                    	mtx.PreRotate(90);
                    	break;
					case Orientation.Rotate180:
                    	mtx.PreRotate(180);
                        break;
					case Orientation.Rotate270:
                    	mtx.PreRotate(270);
                        break;
					case Orientation.Normal:
                    	// Normal, do nothing
                        break;
					default:
                    	break;
                    }
                }
			if (mtx != null)
            	bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
		}
		return bitmap;
    }
}
```
In `'MainActivity.cs'` define variables which will hold the controls and photographs taken.

```csharp
public static Java.IO.File _file;
public static Java.IO.File _dir;
public static Bitmap _bitmap;
private ImageView _imageView;
private Button _pictureButton;
private TextView _resultTextView;
private bool _isCaptureMode = true;
```

Write a method to store captured photograpghs. Name it as `'CreateDirectoryForPictures()'`

```csharp
private void CreateDirectoryForPictures()
{
	_dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
	if (!_dir.Exists())
	{
    	_dir.Mkdirs();
	}
}
```

Check if there is already an app to take pictures and lauch it as an `Intent`

```csharp
private bool IsThereAnAppToTakePictures()
{
	Intent intent = new Intent(MediaStore.ActionImageCapture);
    IList<ResolveInfo> availableActivities =
    	PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
    return availableActivities != null && availableActivities.Count > 0;
}
```

In `OnCreate()` method set the view and assign controls to the variables along with event handlers

```csharp
 protected override void OnCreate(Bundle bundle)
{
	base.OnCreate(bundle);
	SetContentView(Resource.Layout.Main);
	if (IsThereAnAppToTakePictures())
    {
    	CreateDirectoryForPictures();
		_pictureButton = FindViewById<Button>(Resource.Id.GetPictureButton);
        _pictureButton.Click += OnActionClick;
		_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
		_resultTextView = FindViewById<TextView>(Resource.Id.resultText);
	}
}
```

Add event handler for button click as `OnActionClick`

```csharp
private void OnActionClick(object sender, EventArgs eventArgs)
{
	if (_isCaptureMode == true)
    {
    	Intent intent = new Intent(MediaStore.ActionImageCapture);
        _file = new Java.IO.File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
        intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
        StartActivityForResult(intent, 0);
    }
	else
	{
    	_imageView.SetImageBitmap(null);
        if (_bitmap != null)
        {
        	_bitmap.Recycle();
        	_bitmap.Dispose();
        	_bitmap = null;
		}
        	_pictureButton.Text = "Take Picture";
            _resultTextView.Text = "";
            _isCaptureMode = true;
	}
}
```

Once photograph is taken, to get control back on current activity use `'OnActivityResult()'`

```csharp
protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
{
	base.OnActivityResult(requestCode, resultCode, data);
    try
    {
    	_bitmap = BitmapHelpers.GetAndRotateBitmap(_file.Path);
        _bitmap = Bitmap.CreateScaledBitmap(_bitmap, 2000, (int)(2000 * _bitmap.Height / _bitmap.Width), false);
        _imageView.SetImageBitmap(_bitmap);
        _resultTextView.Text = "Loading...";
        using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
        {
        	_bitmap.Compress(Bitmap.CompressFormat.Jpeg, 90, stream);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            float result = await Core.GetAverageHappinessScore(stream);
            _resultTextView.Text = Core.GetHappinessMessage(result);
		}
	}
    catch (Exception ex)
    {
    	_resultTextView.Text = ex.Message;
	}
    finally
    {
    	_pictureButton.Text = "Reset";
        _isCaptureMode = false;
	}
}
```

##### Build EmotionClient.iOS #####

When you're connected to Mac open Main.Storyboard from iOS application. On this storyboard, drag and drop controls from toolbox and design the UI as shown below.

![](https://github.com/mayur-tendulkar/Mini-Hacks/raw/master/Emotion%20API/Images/06-Cognitive-Services-iOS-UI.png)

Name the controls as:

* UILabelView: DetailsText
* UIImageView: ThePhoto
* UIButton:TakePhotoButton

Double click `TakePhotoButton` and add an event handler for the same. Write below code in that event handler.

```csharp
 partial void TakePhotoButton_TouchUpInside(UIButton sender)
{
	TakePhotoButton.Enabled = false;
    UIImagePickerController picker = new UIImagePickerController();
    picker.SourceType = UIImagePickerControllerSourceType.Camera;
    picker.FinishedPickingMedia += async (o, e) => {
    ThePhoto.Image = e.OriginalImage;
    DetailsText.Text = "Processing...";
    ((UIImagePickerController)o).DismissViewController(true, null);
    	using (var stream = e.OriginalImage.AsJPEG(.5f).AsStream())
    	{
    		try
        	{
        		float happyValue = await Core.GetAverageHappinessScore(stream);
            	DetailsText.Text = Core.GetHappinessMessage(happyValue);
			}
        	catch (Exception ex)
			{
        		DetailsText.Text = ex.Message;	
			}
        	TakePhotoButton.Enabled = true;
		}
	};
	PresentModalViewController(picker, true);
}
```


##### Build EmotionClient.UWP#####
In this project, modify 'MainPage.XAML' file to create UI for the screen.


````XAML
        <Grid.RowDefinitions>
            <RowDefinition Height="2*"></RowDefinition>
            <RowDefinition Height="1*"></RowDefinition>
            <RowDefinition Height="1*"></RowDefinition>
        </Grid.RowDefinitions>

        <Image x:Name="imageControl" HorizontalAlignment="Center" VerticalAlignment="Center" Grid.Row="0"/>
        <Button x:Name="_pictureButton" Content="Take Picture" HorizontalAlignment="Center" VerticalAlignment="Center" Grid.Row="1" Click="_pictureButton_Click"/>
        <TextBlock x:Name="_result" HorizontalAlignment="Center" TextWrapping="Wrap" Text="" VerticalAlignment="Center" Grid.Row="2"/>
```

Double click _pictureButton add an event handler for the same. Write below code in that event handler.

````chsarp
 	    CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }

            // Display the photo which was taken.
            IRandomAccessStream streams = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(streams);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
            BitmapPixelFormat.Bgra8,BitmapAlphaMode.Premultiplied);
            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);
            imageControl.Source = bitmapSource;

            var randomAccessStream = await photo.OpenReadAsync();

            Stream stream = randomAccessStream.AsStreamForRead();

            // Get Happiness Score
            _result.Text =  Core.GetHappinessMessage(await Core.GetAverageHappinessScore(stream));
```


Run the apps and notice the output. Display the result to judges to mark your hack as complete.
