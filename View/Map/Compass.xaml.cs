using SilkroadInformationAPI.Client;
using SilkroadSecurityApi;
using SRO_INGAME.Common;
using System;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Utility = SRO_INGAME.Common.Utility;

namespace SRO_INGAME.View.Map
{
    /// <summary>
    /// Interaction logic for Compass.xaml
    /// </summary>
    public partial class Compass : Window
    {
        DispatcherTimer timer;

        public Compass()
        {
            InitializeComponent();

            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - Width) / 2);
            Top = dimensions.Y + 2;

            if (!ExternalDLL.isGameActive())
                Hide();
            else
                Show();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Compass_thread;
            timer.Start();
        }

        void Compass_thread(object sender, EventArgs e)
        {
            if (!ExternalDLL.isGameActive() || SRCommon.isTeleporting)
                Hide();
            else
                Show();
            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - Width) / 2);
            Top = dimensions.Y + 2;
        }

        private void processImage()
        {
            //CompassImage.RenderTransform = new RotateTransform() { CenterX = 0.5, CenterY = 0.5, Angle = 0 };

            //MatrixTransform transformation = new MatrixTransform(0, 0, 0, 0, Client.Position.Angle / 360, 0);
            /*
            BitmapImage image = new BitmapImage(new Uri(@"C:\Users\mo3ly\Desktop\Personal\Silkroad\C#\SRO_INGAME\SRO_INGAME\Resources\Compass\Compass.png"));
            var boundingRect = new Rect(95, 10, 15, 1);
            DrawingGroup dGroup = new DrawingGroup();
            using (DrawingContext dc = dGroup.Open())
            {
                //dc.PushTransform(transformation);
                dc.DrawImage(image, boundingRect);
            }
            DrawingImage imageSource = new DrawingImage(dGroup);
            CompassImage.Source = imageSource;*/
        }

        public void Update(Packet p)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                int angel = p.ReadInt16();
                    //CompassImage.
                    //(float)Utility.RandomNumber(-910, 910)
                int ratio = 910 / 360;
                transformCompass.Matrix = new System.Windows.Media.Matrix(1, 0, 0, 1, Math.Abs(angel * ratio), 0);
                angel = Math.Abs(angel);
                switch (angel)
                {
                    case 0:
                        DegreeLabel.Content = "N";
                        break;
                    case 360:
                        DegreeLabel.Content = "N";
                        break;
                    case 45:
                        DegreeLabel.Content = "NE";
                        break;
                    case 90:
                        DegreeLabel.Content = "E";
                        break;
                    case 130:
                        DegreeLabel.Content = "SE";
                        break;
                    case 180:
                        DegreeLabel.Content = "S";
                        break;
                    case 225:
                        DegreeLabel.Content = "SW";
                        break;
                    case 270:
                        DegreeLabel.Content = "W";
                        break;
                    default:
                        DegreeLabel.Content = angel;
                        break;
                }
            });
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            ExternalDLL.SetWindowLong(helper.Handle, ExternalDLL.GWL_EXSTYLE,
            ExternalDLL.GetWindowLong(helper.Handle, ExternalDLL.GWL_EXSTYLE) | ExternalDLL.WS_EX_NOACTIVATE);
        }
    }
}
