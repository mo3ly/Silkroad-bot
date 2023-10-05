using ImageLibrary.GDImageLibrary;
using SRO_INGAME.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace SRO_INGAME.View.Terms
{
    /// <summary>
    /// Interaction logic for TermsAndServices.xaml
    /// </summary>
    public partial class TermsAndServices : Window
    {

        private DispatcherTimer timer;
        System.Drawing.Rectangle dimensions;

        public TermsAndServices() // add bool isShown so it doesn't appear again
        {
            InitializeComponent();
            dimensions = SRCommon.DUtillity.SRDimensions();
            Width = dimensions.Width;
            Height = dimensions.Height;
            Left = dimensions.X;
            Top = dimensions.Y;

            try
            {
                byte[] ddjBytes = SRCommon.PK2.GetFileBytes("qno_tq_baekryoung_16_3.ddj"); // pk2 reader if file not found check file == null instead of file position = 0
                ArraySegment<byte> toDDS = new ArraySegment<byte>(ddjBytes, 20, ddjBytes.Length - 20);
                System.Drawing.Bitmap Image = _DDS.LoadImage(toDDS.ToArray());
                wBackground.Width = dimensions.Width;
                wBackground.Height = dimensions.Height;
                wBackground.Source = ExternalDLL.ImageSourceFromBitmap(Image);
            } catch { }

            if (!ExternalDLL.isGameActive())
                Hide();
            else
                Show();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // 10 -> 1 second
            timer.Tick += notify_timer;
            timer.Start();
        }

        void notify_timer(object sender, EventArgs e)
        {
            try
            {
                if (!ExternalDLL.isGameActive())
                    Hide();
                else
                    Show();

                dimensions = SRCommon.DUtillity.SRDimensions();
                Width = dimensions.Width;
                Height = dimensions.Height;// - 30
                Left = dimensions.X;
                Top = dimensions.Y;// + 30
            }
            catch { 
                timer.Stop();
                Close(); 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            Close();
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
