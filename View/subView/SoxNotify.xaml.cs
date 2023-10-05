using ImageLibrary.GDImageLibrary;
using SRO_INGAME.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
namespace SRO_INGAME.View.subView
{
    /// <summary>
    /// Interaction logic for SoxNotify.xaml
    /// </summary>
    public partial class SoxNotify : Window
    {
        DispatcherTimer timer;
        public SoxNotify(string icon)
        {
            InitializeComponent();

            try
            {
                //byte[] ddjBytes = SRCommon.PK2.GetFileBytes(System.IO.Path.GetFileName(icon)); // pk2 reader if file not found check file == null instead of file position = 0
                //ArraySegment<byte> toDDS = new ArraySegment<byte>(ddjBytes, 20, ddjBytes.Length - 20);
                //System.Drawing.Bitmap Image = _DDS.LoadImage(toDDS.ToArray());
                iconFrame.Source = Utility.PK2GetImageByURL(icon);
            } catch { 
                Close();
            }


            Opacity = .5;

            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - Width) / 2);
            Top = dimensions.Y + 100;

            if (!ExternalDLL.isGameActive())
                Hide();
           else
                Show();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += notify_timer;
            timer.Start();
        }

        int counter = 0;
        void notify_timer(object sender, EventArgs e)
        {
            counter++;

            if (!ExternalDLL.isGameActive() || SRCommon.isTeleporting)
                Hide();
            else
                Show();

            if (Opacity <= 1)
            {
                Opacity += 0.1;
            }

            if (counter >= 100)
            {
                timer.Stop();
                Close();
            }


            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - Width) / 2);
            Top = dimensions.Y + 100;
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
