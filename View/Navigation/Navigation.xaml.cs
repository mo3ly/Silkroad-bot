using Contollers.GameBot;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using SRO_INGAME.View.GameBot;
using SRO_INGAME.View.Stalls;
using SRO_INGAME.View.ViewCharacter;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace SRO_INGAME.View.Navigation
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : Window
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;



        /// <summary>
        /// keyboard hooks
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        private static readonly int VK_ESCAPE = 0x1B;
        private static readonly int VK_B = 0x42;

        private void keystate()
        {
            short keyState = GetAsyncKeyState(VK_ESCAPE);

            if (((keyState >> 15) & 0x0001) == 0x0001)
            {
                SRCommon.mainFrame.Hide();
                if (SRCommon.mainFrame.IsVisible)
                    SroClient.GUIVisibilty(98, false);
            }

            if (((keyState >> 0) & 0x0001) == 0x0001)
            {
                SRCommon.mainFrame.Hide();
                //SroClient.GUIVisibilty(98, false);
            }
            /*
            short bState = GetAsyncKeyState(VK_B);

            if (((bState >> 15) & 0x0001) == 0x0001 && ExternalDLL.isGameActive())
                if(SRCommon.mainFrame.IsVisible)
                    SRCommon.mainFrame.Hide();
                else
                    SRCommon.mainFrame.Show();

            if (((bState >> 0) & 0x0001) == 0x0001 && ExternalDLL.isGameActive())
                if (SRCommon.mainFrame.IsVisible)
                    SRCommon.mainFrame.Hide();
                else
                    SRCommon.mainFrame.Show();
            */
        }

        public Navigation()
        {
            InitializeComponent();
            System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
            Left = (dimensions.X + dimensions.Width) - (Width + 10);
            Top = (dimensions.Y + dimensions.Height) - (Height + 350);
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();
        }

        bool isShown = false;
        private void dispatcherTimer_Tick(object sender, EventArgs e) // make one ticker for the hut window and the wgole window
        {
            // exit app if the sro_client is not active make this in the begining before laucher in case the game close before reaches here and you may add this to packed cycle
            //if (!Process.GetProcesses().Any(x => x.Id == SRCommon.SRClientID))
            //Environment.Exit(0);
            keystate();
            // add chat command to hide it
            if (!ExternalDLL.isGameActive() || SRCommon.isTeleporting)
            {
                Hide();
                SRCommon.mainFrame.Hide();
                isShown = false;
            }
            else
            {
                if (!isShown)
                {
                    // add full screen part
                    Show();
                    isShown = true;
                }
                // store the client position and keep checking if changed change position!
                // update positions every type -> you might check if full screen allow drag
                System.Drawing.Rectangle dimensions = SRCommon.DUtillity.SRDimensions();
                Left = (dimensions.X + dimensions.Width) - (Width + 10);
                Top = (dimensions.Y + dimensions.Height) - (Height + 350);
                //Console.WriteLine("[SRO] X: {0}, Y: {1}, WIDTH: {2}, HEIGHT: {3}",dimensions.X, dimensions.Y, dimensions.Width  , dimensions.Height );
                // main window diminsions
                //SRCommon.mainFrame.Left = Math.Max(dimensions.X, dimensions.X + (dimensions.Width - SRCommon.mainFrame.Width) / 2);
                //SRCommon.mainFrame.Top = Math.Max(dimensions.Y, dimensions.Y + (dimensions.Height - SRCommon.mainFrame.Height) / 2);
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            //Set the window style to noactivate.
            WindowInteropHelper helper = new WindowInteropHelper(this);
            ExternalDLL.SetWindowLong(helper.Handle, GWL_EXSTYLE,
            ExternalDLL.GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        Point lastPosition;
        private void Navigation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastPosition = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);
        }

        private void Navigation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Left += e.GetPosition(this).X - lastPosition.X;
                Top += e.GetPosition(this).Y - lastPosition.Y;
            }
        }

        bool isExpanded = true;
        private void visibiltyButton_Click(object sender, RoutedEventArgs e)
        {
            if (isExpanded)
            {
                visibiltyButton.Content = "⮝";
                isExpanded = false;
                try
                {
                    mWindow.Visibility = Visibility.Hidden;
                    nPanel.Visibility = Visibility.Hidden;
                    //wBackground.Source = Utility.PK2GetImage("panel_not_expanded.ddj");
                    mWindow.Visibility = Visibility.Visible;
                    mWindow.Height = 63;
                }
                catch { }
            }
            else
            {
                visibiltyButton.Content = "⮟";
                isExpanded = true;
                try
                {
                    mWindow.Visibility = Visibility.Hidden;
                    nPanel.Visibility = Visibility.Visible;
                    //wBackground.Source = Utility.PK2GetImage("panel_expanded.ddj");
                    mWindow.Visibility = Visibility.Visible;
                    mWindow.Height = 180;
                }
                catch { }
            }
        }

        #region navigation button
        // declare views
        BotFrame BotFrame = new BotFrame();
        ViewCharacterFrame ViewCharacterFrame;
        NearbyStalls NearbyStalls;
        Guide.Guide GameGuide = new Guide.Guide();

        private void HuntButton_Click(object sender, RoutedEventArgs e)
        {
            SRCommon.mainFrame.windowTitle.Content = "GAME AUTO-HUNT";
            SRCommon.mainFrame.mContent.Content = BotFrame;
            // Load bot data when enter
            SRCommon.pSkills.Load();
            SRCommon.pPotion.Load();
            SRCommon.mainFrame.Show();
        }
        #endregion

        private void ViewCharButton_Click(object sender, RoutedEventArgs e)
        {
            if(true)//if (Client.NearbyCharacters.ContainsKey(Client.SelectedUniqueID)) // and charname != opened charname
            {
                ViewCharacterFrame = new ViewCharacterFrame();
                ViewCharacterFrame.LoadCharacter(Client.SelectedUniqueID);
                SRCommon.mainFrame.windowTitle.Content = $"Character Details";
                SRCommon.mainFrame.mContent.Content = ViewCharacterFrame;
                SRCommon.mainFrame.Show();
            }
            /*
            Console.WriteLine("Nearby characters count: " + Client.NearbyCharacters.Count);
            foreach (KeyValuePair<uint, SilkroadInformationAPI.Client.Information.Objects.Character> kvp in Client.NearbyCharacters)
            {
                Console.WriteLine("Key = {0}, Name = {1}, UniqueID = {2}", kvp.Key, kvp.Value.Name, kvp.Value.UniqueID);
            }


            //Console.WriteLine("Selected unique id: " + Client.SelectedUniqueID);
            Console.WriteLine("Nearby Mobs count: " + Client.NearbyMobs.Count);
            foreach (KeyValuePair<uint, SilkroadInformationAPI.Client.Information.Objects.Mob> kvp in Client.NearbyMobs)
            {
                Console.WriteLine("Key = {0}, CurrentHP = {1}, MaxHP = {2}, Appearance = {2}", kvp.Key, kvp.Value.CurrentHP, kvp.Value.MaxHP, kvp.Value.Appearance);
            }


            Console.WriteLine("Skills count: " + Client.Skills.Count);
            foreach( var skill in Client.Skills)
            {
                Console.WriteLine("Name = {0}, UseOnslef? = {1}, UseOnEnemy? = {2}, UseOnAlly? = {3},desc = {4}, icon = {5}", skill.TranslationName, skill.UseOnSelf, skill.UseOnEnemy, skill.UseOnAlly, skill.Description, skill.SkillIcon);
            }

            Console.WriteLine("Masteries count: " + Client.Masteries.Count);
            foreach (SilkroadInformationAPI.Client.Information.Spells.Mastery Mastery in Client.Masteries)
            {
                Console.WriteLine("ID {0}, Level {1}", Mastery.ID, Mastery.Level);
            }

            Console.WriteLine("InventoryItems count: " + Client.InventoryItems.Count);
            //Client.InventoryItems.Select(i => $"{i.Key}: Blues: {i.Value.Blues.Count} - {i.Value.Blues.Select(x => $"{x.Value} {x.Key}")} - {i.Value.Blues.Values.Select(x => $"{x}")} - \n stats: {i.Value.Stats.Values.Select(x => $"{x}")} - stats keys: {i.Value.Stats.Keys.Select(x => $"{x}")} - \n Desc: {i.Value.Description}").ToList().ForEach(Console.WriteLine);
            foreach (KeyValuePair<int, SilkroadInformationAPI.Client.Information.InventoryItem> kvp in Client.InventoryItems)
            {
                Console.WriteLine("Key = {0}, Degree = {1}, Sox = {2}, TranslationName = {3}, PlusValue = {4}, Stack = {5}, Slot = {6}, Type = {7}, Advance = {8}, SubType= {9}, IconPath = {10}",
                    kvp.Value.Classes.B.ToString() + " " +
                    kvp.Value.Classes.C.ToString() + " " +
                    kvp.Value.Classes.D.ToString() + " " +
                    kvp.Value.Classes.E.ToString() + " " +
                    kvp.Value.Classes.F.ToString(), 
                    kvp.Value.Degree, kvp.Value.SOX, kvp.Value.TranslationName, kvp.Value.PlusValue, kvp.Value.Stack, kvp.Value.Slot, kvp.Value.Type, kvp.Value.HasAdvance, kvp.Value.SubType, kvp.Value.IconPath);
                Console.WriteLine("[Blues]");
                foreach (KeyValuePair <SilkroadInformationAPI.ItemBlues, int> keyValuePair in kvp.Value.Blues)
                {
                    Console.WriteLine("{0} {1}", keyValuePair.Key, keyValuePair.Value);
                }
                Console.WriteLine("[White Stats]");
                foreach (KeyValuePair<string, int> keyValuePair in kvp.Value.Stats)
                {
                    Console.WriteLine("{0} {1}", keyValuePair.Key, keyValuePair.Value);
                }
                Console.WriteLine("\n");
            }

            Console.WriteLine("NearbyStructures count: " + Client.NearbyStructures.Count);
            Client.NearbyStructures.Select(x => $"HP {x.Value.HP} , State {x.Value.State}, OwnerName {x.Value.OwnerName}, ModelID {x.Value.ModelID}").ToList().ForEach(Console.WriteLine);
            Console.WriteLine("Character Race: " + Client.Info.Race);*/
        }

        private void BotAction_Click(object sender, RoutedEventArgs e)
        {
            if (BotData.isBotting)
            {
                if (SRCommon.botController.StopBotting())
                {
                    botStatusLabel.Content = "OFF";
                    BotActionButton.Content = "START BOT";
                }
            }
            else
            {
                if (SRCommon.botController.StartBotting())
                {
                    botStatusLabel.Content = "ON";
                    BotActionButton.Content = "STOP BOT";
                }
            }
        }

        private void mWindow_GotMouseCapture(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        private void mWindow_LostMouseCapture(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.3;
        }

        private void GuideButton_Click(object sender, RoutedEventArgs e)
        {
            SRCommon.mainFrame.windowTitle.Content = "GAME GUIDE";
            SRCommon.mainFrame.mContent.Content = GameGuide;
            SRCommon.mainFrame.Show();
        }

        private void NearStallsButton_Click(object sender, RoutedEventArgs e)
        {
            NearbyStalls = new NearbyStalls();
            NearbyStalls.Load();
            SRCommon.mainFrame.windowTitle.Content = "NEARBY STALLS";
            SRCommon.mainFrame.mContent.Content = NearbyStalls;
            SRCommon.mainFrame.Show();
        }
    }
}
