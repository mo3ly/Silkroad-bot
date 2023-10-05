using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SilkroadInformationAPI.Media;
using SRO_INGAME.Common;
using SRO_INGAME.GameLauncher;
using SRO_INGAME.Http.Models;
using SRO_INGAME.Http.Models.Stalls;
using SRO_INGAME.Libraries.WebLib;
using SRO_INGAME.View.Main.LauncherCompnents;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SRO_INGAME
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        // Select the worn weapon and shield as first attack items
        // fix restart issue and character is already opened
        // check for how many client
        // add login from launcher
        // add register page
        // add transiation in the baackground ()
        // Add CPSQuickLogin -> save the login params in the config file before loading the sro_client -> if the client finds it active loads the supercharge if not skip lol

        Point lastPosition;

        public Launcher()
        {
            InitializeComponent();
            // to keep tool tip focus
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            SliderFrame.Content = new ItemsSlider();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LauncherData.LoadData();
            Console.WriteLine("IP " + SRCommon.clientIP);
            Console.WriteLine("Port " + SRCommon.clientPort);
            Console.WriteLine("Version " + SRCommon.clientVersion);
        }

        private void exit_Button(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //APIAsync();
            //    using (APIClient APIClient = new APIClient("http://localhost/api/"))
            //    {
            //        // dynamic json
            //        //dynamic character = APIClient.RetrieveData<dynamic>("character").Result;
            //        //Console.WriteLine(character.CharName16);

            //        Stalls Stalls = APIClient.RetrieveData<Stalls>("stalls").Result;
            //        Console.WriteLine(Stalls.CurrentStalls.Count);
            //        foreach (var stall in Stalls.CurrentStalls)
            //        {
            //            Console.WriteLine(stall.Owner);
            //            Console.WriteLine(stall.Date);
            //            Console.WriteLine(stall.Location);
            //            foreach(var item in stall.Items)
            //            {
            //                Console.WriteLine(item.ItemID);
            //            }
            //        }

            //        // retriereving string
            //        //Console.WriteLine(APIClient.RetrieveData("stalls").Result);
            //        //Console.WriteLine(APIClient.RetrieveData("character").Result);

            //    }
            //} catch (Exception er)
            //{
            //    Console.WriteLine(er);
            //}
            Close();
        }

        private void Launcher_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastPosition = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);
        }

        private void Launcher_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Left += e.GetPosition(this).X - lastPosition.X;
                Top += e.GetPosition(this).Y - lastPosition.Y;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text != null)
                SRCommon.Username = Username.Text;

            if (Password.Password != null)
                SRCommon.Password = Password.Password;

            if (LoginChar.Text != null)
                SRCommon.loginCharacter = LoginChar.Text;

            //run bot headless when launcher has everything
            SRCommon.mainFrame = new MainWindow(); // rname to mainFrame
            SRCommon.mainFrame.Owner = this;
            Hide();
            // load and hide window bot
            SRCommon.mainFrame.Show();
            SRCommon.mainFrame.Hide();
        }

        private void AutoLogin_Click(object sender, RoutedEventArgs e)
        {
            AutoLoginFrame.Visibility = Visibility.Visible;
            SettingsFrame.Visibility = Visibility.Hidden;
        }

        private void settings_Button(object sender, RoutedEventArgs e)
        {
            SettingsFrame.Visibility = Visibility.Visible;
            AutoLoginFrame.Visibility = Visibility.Hidden;
        }

    }
}
