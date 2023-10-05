using SRO_INGAME.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace SRO_INGAME
{
    public partial class MainWindow : Window
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            ExternalDLL.SetWindowLong(helper.Handle, GWL_EXSTYLE,
            ExternalDLL.GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        // latest window position
        Point lastPosition;

        //KEYBOARD HOOK
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Dashboard controls
        private void Dashboard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastPosition = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);
        }

        private void Dashboard_MouseMove(object sender, MouseEventArgs e)
        { 
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Left += e.GetPosition(this).X - lastPosition.X;
                this.Top += e.GetPosition(this).Y - lastPosition.Y;
            }
        }


        private void exit_Button(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        #endregion

        public void startClient()
        {
            SRCommon.game.Initialize(SRCommon.clientPath);
            SRCommon.game.LoadData();
            SRCommon.game.StartProxyConnection(SRCommon.clientIP, (ushort)SRCommon.botPort, false);
            SRCommon.game.StartClient((ushort)SRCommon.botPort);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            startClient();
        }

        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {/*
            var animation = new ThicknessAnimation();
            animation.Duration = TimeSpan.FromSeconds(0.5);
            animation.DecelerationRatio = 0.5;
            animation.To = new Thickness(0, 0, 0, 0);
            if (e.NavigationMode == NavigationMode.New)
                animation.From = new Thickness(500, 0, 0, 0);
            else if (e.NavigationMode == NavigationMode.Back)
                animation.From = new Thickness(0, 0, 500, 0);

            try
            {
                (e.Content as Page).BeginAnimation(MarginProperty, animation);
            }
            catch { }*/
        }
    }
}
