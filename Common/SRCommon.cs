using PK2Reader;
using SilkroadInformationAPI;
using System.Diagnostics;
using SRO_INGAME.View.Navigation;
using System.Linq;
using System;
using SRO_INGAME.View;
using Contollers.GameBot;
using SRO_INGAME.View.Map;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;
using SRO_INGAME.Libraries.WebLib;
using SRO_INGAME.Http.Models.Stalls;

namespace SRO_INGAME.Common
{
    /**
     * NOTE:
     *  Fix skills unload when teleport
     *  fix item count
     *  update invnetory when by item form f10
     *  on restatt reset everthing here 
     *  send welcome notification again reset everything
     * */
    class SRCommon
    {
        // add path handlers
        public static string clientPath = @"C:\Users\mo3ly\Desktop\Personal\Silkroad\Clients\Flare Online\"; //Directory.GetCurrentDirectory();
        public static string BlowfishKey = "169841";
        public static Reader PK2 = new Reader($"{clientPath}/Media.pk2", BlowfishKey);

        //Launcher
        public static string clientIP;
        public static int clientPort;
        public static int clientVersion;

        // login data
        public static bool hasLoggedOnce = false; // turn this true after login first time and add condition for auto login if this is true don't login again
        public static bool autoLogin = false;
        public static string Username = null;
        public static string Password = null;
        public static string loginCharacter = null;

        // session data
        public static Session.SessionData SessionData = new Session.SessionData();

        // Main window
        public static MainWindow mainFrame;
        public static string botIP = "127.0.0.1";
        public static int botPort = 20001; // don't forget to create new poert checker
        public static uint botVersion = 188;
        public static byte botLocal = 22;

        // sro_client
        public static Process SRClientProcess;
        public static int SRClientID;

        // Navigation window
        // hide this when travel and show it after spawn and all the other windows
        // save the sro_client pid to link this bot and navigation page to it in case the use opened mor than one page
        public static Navigation Navigation;

        // compass
        public static Compass Compass;

        // External dll
        public static ExternalDLL DUtillity = new ExternalDLL();

        // Images
        public static Dictionary<string, ImageSource> Images = new Dictionary<string, ImageSource>(); // to do any image used in the session add it to this place so you don't reload it everytime -> in different meaning re use it

        //user
        //public static string Username;
        //public static string Password;

        //data
        public static bool isNavigationShown = false;
        public static bool isTeleporting = false;

        // declare game
        public static SroClient game = new SroClient(clientPath);
        // declare bot controller
        public static BotController botController = new BotController();

        // bot pages
        public static Statistics pStatistics = new Statistics();
        public static Potion pPotion = new Potion();
        public static Skills pSkills = new Skills();
        public static Hunting pHunting = new Hunting();

        // API
        public static APIClient ClientAPI = new APIClient("http://localhost/api");
        // API Data -> Create another class for storing the api data
        public static Stalls CurrentStalls;

        // timer
        public static  System.Windows.Threading.DispatcherTimer ClientTimer = new System.Windows.Threading.DispatcherTimer(TimeSpan.FromMilliseconds(100), System.Windows.Threading.DispatcherPriority.Background, new EventHandler(ClintTimer_Tick), System.Windows.Threading.Dispatcher.CurrentDispatcher);
        
        private static void ClintTimer_Tick(object sender, EventArgs e) // make one ticker for the hut window and the wgole window
        {
            // exit app if the sro_client is not active make this in the begining before laucher in case the game close before reaches here and you may add this to packed cycle
            if (!Process.GetProcesses().Any(x => x.Id == SRClientID))
                Environment.Exit(0);
        }
    }
}
