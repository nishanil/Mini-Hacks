using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BugSweeper
{
    enum TileStatus
    {
        Hidden,
        Flagged,
        Exposed
    }

    class Tile : Frame
    {
        TileStatus tileStatus = TileStatus.Hidden;
        Label label;
        Image flagImage, bugImage;
        static ImageSource flagImageSource;
        static ImageSource bugImageSource;
        bool doNotFireEvent;

        public event EventHandler<TileStatus> TileStatusChanged;

        static Tile()
        {
            flagImageSource = ImageSource.FromFile("BugSweeper/Images/Xamarin120.png");
            bugImageSource = ImageSource.FromFile("BugSweeper/Images/RedBug.png");
        }

        public Tile(int row, int col)
        {
            this.Row = row;
            this.Col = col;

            this.BackgroundColor = Color.Yellow;
            this.OutlineColor = Color.Blue;
            this.Padding = 3;

            label = new Label
            {
                Text = " ",
                TextColor = Color.Yellow,
                BackgroundColor = Color.Blue,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            flagImage = new Image
            {
                Source = flagImageSource,

            };

            bugImage = new Image
            {
                Source = bugImageSource
            };

            TapGestureRecognizer singleTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1
            };
            singleTap.Tapped += OnSingleTap;
            this.GestureRecognizers.Add(singleTap);



            TapGestureRecognizer doubleTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 2
            };
            doubleTap.Tapped += OnDoubleTap;
            this.GestureRecognizers.Add(doubleTap);

//#if FIX_WINDOWS_DOUBLE_TAPS

       //     }

//#endif

        }

        public int Row { private set; get; }

        public int Col { private set; get; }

        public bool IsBug { set; get; }

        public int SurroundingBugCount { set; get; }

        public TileStatus Status
        {
            set
            {
                if (tileStatus != value)
                {
                    tileStatus = value;

                    switch (tileStatus)
                    {
                        case TileStatus.Hidden:
                            this.Content = new Label { Text = " " };

//#if FIX_WINDOWS_PHONE_NULL_CONTENT

//                            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows) {
//                                this.Content = new Label { Text = " " };
//                            }

//#endif
                            break;

                        case TileStatus.Flagged:
                            this.Content = flagImage;
                            break;

                        case TileStatus.Exposed:
                            if (this.IsBug)
                            {
                                this.Content = bugImage;
                            }
                            else
                            {
                                this.Content = label;
                                label.Text =
                                        (this.SurroundingBugCount > 0) ?
                                            this.SurroundingBugCount.ToString() : " ";
                            }
                            break;
                    }

                    if (!doNotFireEvent && TileStatusChanged != null)
                    {
                        TileStatusChanged(this, tileStatus);
                    }
                }
            }
            get
            {
                return tileStatus;
            }
        }

        // Does not fire TileStatusChanged events.
        public void Initialize()
        {
            doNotFireEvent = true;
            this.Status = TileStatus.Hidden;
            this.IsBug = false;
            this.SurroundingBugCount = 0;
            doNotFireEvent = false;
        }

//#if FIX_WINDOWS_DOUBLE_TAPS

        bool lastTapSingle;
        DateTime lastTapTime;

//#endif

        void OnSingleTap(object sender, object args)
        {

//#if FIX_WINDOWS_DOUBLE_TAPS

        //    if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                if (lastTapSingle && DateTime.Now - lastTapTime < TimeSpan.FromMilliseconds (500)) {
                    OnDoubleTap (sender, args);
                    lastTapSingle = false;
                } else {
                    lastTapTime = DateTime.Now;
                    lastTapSingle = true;
                }
        	}

//#endif

            switch (this.Status)
            {
                case TileStatus.Hidden:
                    this.Status = TileStatus.Flagged;
                    break;

                case TileStatus.Flagged:
                    this.Status = TileStatus.Hidden;
                    break;

                case TileStatus.Exposed:
                    // Do nothing
                    break;
            }
        }

        void OnDoubleTap(object sender, object args)
        {
            this.Status = TileStatus.Exposed;
        }
    }
}
