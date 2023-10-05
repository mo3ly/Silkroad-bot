using Contollers.Notifications;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using SRO_INGAME.View.Map;
using SRO_INGAME.View.Navigation;
using SRO_INGAME.View.subView;
using System;
using System.Threading;
using System.Windows;

namespace Game.Controller
{
    class ViewController
    {
        public static void ShowNavigation()
        {
            // show in game navigation window  // make external class for this
            if (!SRCommon.isNavigationShown)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    SRCommon.Navigation = new Navigation();
                    SRCommon.Navigation.Show();
                    //SRCommon.Compass = new Compass();
                });
                SRCommon.isNavigationShown = true;
                // fix this one to show only once
                Notifier.BannerNotification($"Hey {Client.Info.CharacterName}", $"Welcome {Client.Info.CharacterName} to the server we hope you enjoy it!");
                //SroClient.BlueNotice($"Welcome {Client.Info.CharacterName} to the server we hope you enjoy it!");
                //SroClient.SystemMessage($"Welcome {Client.Info.CharacterName} to the server we hope you enjoy it!", false);
            }
        }

        public static void ShowTermsAndServices()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                new SRO_INGAME.View.Terms.TermsAndServices();
            });
        }

        public static void ExitApp()
        {
            Thread.Sleep(1000);
            try
            {
                Console.WriteLine("Killing the SRO_Client");
                System.Diagnostics.Process.GetProcessById(SRCommon.SRClientID).Kill();
            }
            catch { }
            Environment.Exit(0);
        }
    }
}
