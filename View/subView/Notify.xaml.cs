using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using SRO_INGAME.Common;

namespace SRO_INGAME.View.subView
{
    /// <summary>
    /// Interaction logic for Notify.xaml
    /// </summary>
    public partial class Notify : Window
    {
        DispatcherTimer timer;
        int Delay;

        public Notify(string title, string content, int delay = 100)
        {
            InitializeComponent();
            nContent.Text = content;
            nTitle.Text = title;
            Delay = delay;

            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - Width) / 2);
            Top = dimensions.Y + 70;

            if (!ExternalDLL.isGameActive())
                Hide();
            else
                Show();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // 10 -> 1 second
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
                Opacity += 0.05;

            if (counter >= Delay)
            {
                timer.Stop();
                Close();
            }


            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - Width) / 2);
            Top = dimensions.Y + 70;
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
