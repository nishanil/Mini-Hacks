# BugSweeper

This is a familiar game with a new twist. Ten bugs are hidden in a 9-by-9 grid of tiles. To win, you must find and flag all ten bugs.

Begin playing by double tapping any tile. That first double tap is always safe. Thereafter, numbers indicate the number of surrounding tiles with bugs. If you know that a tile has a bug, flag it (or unflag it) with a single tap. Avoid double-tapping a tile with a bug!

Original Author :
Charles Petzold

## The Challenge
In this challenge, you will develop a game named BugSweeper using Xamarin forms. The goal is to get the game running on any two platforms. (Android, IOS or Windows).

Below steps should help you to complete this challenge.For queries, get in touch with [@AparnaChinya](https://twitter.com/AparnaChinya) 

This challenge requires:

* Microsoft Visual Studio 2015 Update 3 on Windows (with Xamarin tools installed)
* Xamarin Studio on Mac
* Download the BugSweeper folder from [here](http://github.com/AparnaChinya/Mini-Hacks/BugSweeper)


### Challenge  Walkthrough

####Step 1: Open Visual Studio 2015 > File > New Project > Choose Blank App (Xamarin.Forms.Portable) under the Cross-Platform Visual C# template > Name it *BugSweeper*.

Note: Choose the target platform and minimum platform versions that your Universal Windows application will support. Click *OK* on this dialogue box.

#### Step 2: Right Click on BugSweeper (Portable) > Add > Forms Xaml Page under Visual C# - Cross-Platform > Name it BugSweeperPage.cs.

####Step 3: Go to App.cs and call the BugSweeperPage class which you just created, inside the constructor.

    


```chsarp
 public App()
        {
            // The root page of your application
            MainPage = new BugSweeperPage();
        }

```

####Step 4: Add the [downloaded](http://github.com/AparnaChinya/Mini-Hacks/BugSweeper) resources into your project.
* Right Click BugSweeper (Portable) > Add > Existing Item > Board.cs AND Tile.cs
* Right Click BugSweeper (Portable) > Add > Create New Folder> Name it Images > Copy and paste the 2 images from the Downloaded folder into this.

####Step 5: Lets prepare the UI for our game! Go to BugSweeperPage.xaml and copy-paste the below code inside the ContentPage node.

````XAML
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:BugSweeper;assembly=BugSweeper"
             x:Class="BugSweeper.BugSweeperPage">
  <Label Text="{Binding MainText}" VerticalOptions="Center" HorizontalOptions="Center" />
  <ContentView SizeChanged="OnMainContentViewSizeChanged">
    <Grid x:Name="mainGrid" ColumnSpacing="0" RowSpacing="0">
      <Grid.RowDefinitions>
        <RowDefinition Height="7*" />
        <RowDefinition Height="4*" />
      </Grid.RowDefinitions>
      <Grid.ColumnDefinitions>
        <ColumnDefinition Width="0" />
        <ColumnDefinition Width="*" />
      </Grid.ColumnDefinitions>
      <StackLayout x:Name="textStack" Grid.Row="0" Grid.Column="1" Spacing="0">
        <StackLayout HorizontalOptions="Center" Spacing="0">
          <Label Text="BugSweeper" Font="Bold, Large" TextColor="Accent" />
          <BoxView Color="Accent" HeightRequest="3" />
        </StackLayout>
        <Label Text="Tap to flag/unflag a potential bug." VerticalOptions="CenterAndExpand" HorizontalTextAlignment="Center" />
        <Label Text="Double-tap if you're sure it's not a bug.&#xA;The first double-tap is always safe!" VerticalOptions="CenterAndExpand" HorizontalTextAlignment="Center" />
        <StackLayout Orientation="Horizontal" Spacing="0" VerticalOptions="CenterAndExpand" HorizontalOptions="Center">
          <Label BindingContext="{x:Reference board}" Text="{Binding FlaggedTileCount, StringFormat='Flagged {0} '}" />
          <Label BindingContext="{x:Reference board}" Text="{Binding BugCount, StringFormat=' out of {0} bugs.'}" />
        </StackLayout>
        <!-- Make this a binding??? -->
        <Label x:Name="timeLabel" Text="0:00" VerticalOptions="CenterAndExpand" HorizontalTextAlignment="Center" />
      </StackLayout>
      <ContentView Grid.Row="1" Grid.Column="1" SizeChanged="OnBoardContentViewSizeChanged">
        <!-- Single-cell Grid for Board and overlays. -->
        <Grid>
          <Grid.RowDefinitions>
            <RowDefinition Height="*" />
          </Grid.RowDefinitions>
          <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
          </Grid.ColumnDefinitions>
          <local:Board x:Name="board" />

          <StackLayout x:Name="congratulationsText" Orientation="Horizontal" HorizontalOptions="Center" VerticalOptions="Center" Spacing="0">
            <Label Text="C" TextColor="Red" />
            <Label Text="O" TextColor="Red" />
            <Label Text="N" TextColor="Red" />
            <Label Text="G" TextColor="Red" />
            <Label Text="R" TextColor="Red" />
            <Label Text="A" TextColor="Red" />
            <Label Text="T" TextColor="Red" />
            <Label Text="U" TextColor="Red" />
            <Label Text="L" TextColor="Red" />
            <Label Text="A" TextColor="Red" />
            <Label Text="T" TextColor="Red" />
            <Label Text="I" TextColor="Red" />
            <Label Text="O" TextColor="Red" />
            <Label Text="N" TextColor="Red" />
            <Label Text="S" TextColor="Red" />
            <Label Text="!" TextColor="Red" />
          </StackLayout>
          <StackLayout x:Name="consolationText" Orientation="Horizontal" Spacing="0" HorizontalOptions="Center" VerticalOptions="Center">
            <Label Text="T" TextColor="Red" />
            <Label Text="O" TextColor="Red" />
            <Label Text="O" TextColor="Red" />
            <Label Text=" " TextColor="Red" />
            <Label Text="B" TextColor="Red" />
            <Label Text="A" TextColor="Red" />
            <Label Text="D" TextColor="Red" />
            <Label Text="!" TextColor="Red" />
          </StackLayout>
          <Button x:Name="playAgainButton" Text=" Play Another Game? " HorizontalOptions="Center" VerticalOptions="Center" Clicked="OnplayAgainButtonClicked"
                  BorderColor="Black" BorderWidth="2" BackgroundColor="White" TextColor="Black" />
        </Grid>
      </ContentView>
    </Grid>
  </ContentView>
</ContentPage>
```

####Step 6: Let's now write our Game logic. Got to BugSweeper.xaml.cs and follow the below steps.
* Declare the below three variables inside your BugSweeper class.

````csharp
        const string timeFormat = @"%m\:ss";

        bool isGameInProgress;
        DateTime gameStartTime;

```

*    Inside the constructor, call the GameStarted and GameEnded eventHandlers as below. Add the function PrepareForNewGame() as well.

```chsarp
            board.GameStarted += (sender, args) =>
            {
                isGameInProgress = true;
                gameStartTime = DateTime.Now;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    timeLabel.Text = (DateTime.Now - gameStartTime).ToString(timeFormat);
                    return isGameInProgress;
                });
            };

            board.GameEnded += (sender, hasWon) =>
            {
                isGameInProgress = false;

                if (hasWon)
                {
                    DisplayWonAnimation();
                }
                else
                {
                    DisplayLostAnimation();
                }
            };
        PrepareForNewGame();
```

####Step 7: Add the below function for Preparing for a New Game.

````charp
        void PrepareForNewGame()
        {
            board.NewGameInitialize();

            congratulationsText.IsVisible = false;
            consolationText.IsVisible = false;
            playAgainButton.IsVisible = false;
            playAgainButton.IsEnabled = false;

            timeLabel.Text = new TimeSpan().ToString(timeFormat);
            isGameInProgress = false;
        }
```

####Step 8: Add the function for Winning Condition of the game.

````charp
        async void DisplayWonAnimation()
        {
            congratulationsText.Scale = 0;
            congratulationsText.IsVisible = true;

            // Because IsVisible has been false, the text might not have a size yet, 
            //  in which case Measure will return a size.
            double congratulationsTextWidth = congratulationsText.GetSizeRequest(Double.PositiveInfinity,                 Double.PositiveInfinity).Request.Width;

            congratulationsText.Rotation = 0;
            await congratulationsText.RotateTo(3 * 360, 1000, Easing.CubicOut);

            double maxScale = 0.9 * board.Width / congratulationsTextWidth;
            await congratulationsText.ScaleTo(maxScale, 1000);

            foreach (View view in congratulationsText.Children)
            {
                view.Rotation = 0;
                await view.RotateTo(180);
                await view.ScaleTo(3, 100);
                await view.RotateTo(360);
                await view.ScaleTo(1, 100);
            }

            await DisplayPlayAgainButton();
        }
```

####Step 9: Add the function for loosing Condition of the game.
````charp
        async void DisplayLostAnimation()
        {
            consolationText.Scale = 0;
            consolationText.IsVisible = true;

            // (See above for rationale)
            double consolationTextWidth = consolationText.GetSizeRequest(Double.PositiveInfinity, Double.PositiveInfinity).Request.Width;

            double maxScale = 0.9 * board.Width / consolationTextWidth;
            await consolationText.ScaleTo(maxScale, 1000);
            await Task.Delay(1000);
            await DisplayPlayAgainButton();
        }
```

####Step 10: Write the Task for displaying the PlayAnimation once the game finishes.

````chsarp
        async Task DisplayPlayAgainButton()
        {
            playAgainButton.Scale = 0;
            playAgainButton.IsVisible = true;
            playAgainButton.IsEnabled = true;

            // (See above for rationale)
            double playAgainButtonWidth = playAgainButton.GetSizeRequest(Double.PositiveInfinity, Double.PositiveInfinity).Request.Width;

            double maxScale = board.Width / playAgainButtonWidth;
            await playAgainButton.ScaleTo(maxScale, 1000, Easing.SpringOut);
        }

```

Step 11: Call the NewGame function when a user clicks on the OnPlayAgain button.

````chsarp
     void OnplayAgainButtonClicked(object sender, object EventArgs)
        {
            PrepareForNewGame();
        }
```

####Step 12: Add function for OnMainContentViewSizeChanged AND OnBoardContentViewSizeChanged to take care of the UI for different screen sizes for all the mobile applications.

````csharp
void OnMainContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            double width = contentView.Width;
            double height = contentView.Height;

            bool isLandscape = width > height;

            if (isLandscape)
            {
                mainGrid.RowDefinitions[0].Height = 0;
                mainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

                mainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(textStack, 1);
                Grid.SetColumn(textStack, 0);
            }
            else // portrait
            {
                mainGrid.RowDefinitions[0].Height = new GridLength(3, GridUnitType.Star);
                mainGrid.RowDefinitions[1].Height = new GridLength(5, GridUnitType.Star);

                mainGrid.ColumnDefinitions[0].Width = 0;
                mainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                Grid.SetRow(textStack, 0);
                Grid.SetColumn(textStack, 1);
            }
        }

        // Maintains a square aspect ratio for the board.
        void OnBoardContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            double width = contentView.Width;
            double height = contentView.Height;
            double dimension = Math.Min(width, height);
            double horzPadding = (width - dimension) / 2;
            double vertPadding = (height - dimension) / 2;
            contentView.Padding = new Thickness(horzPadding, vertPadding);
        }
```

##Your final Game looks like this ! :)


