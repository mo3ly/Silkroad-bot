using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SRO_INGAME.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        /**
         * KNOWN ERRORS:
         *      Restart error
         *      When character is already opened error
         */
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Console.WriteLine("[SHOPS]");
            //SRCommon.game.PrintShops();

            Console.WriteLine("[Inventory]");
            //SRCommon.game.PrintInventoryCount();

            Console.WriteLine("[Dropped Gold]");
            foreach (KeyValuePair<uint, SilkroadInformationAPI.Client.Information.Objects.Item> kvp in SRCommon.game.GetDroppedGold())
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value.Amount);
            }


            Console.WriteLine("[USED ITEM IN 14 SLOT]");
            //SRCommon.game.UseItemSlot(1);
            //SRCommon.game.WalkTo(Convert.ToInt32(username.Text), Convert.ToInt32(password.Password));

            //Packet NewPacket = new Packet(7050, false);
            //Proxy.ag_remote_security.Send(NewPacket);
            //new Notify("This character has recieved a new str point.");
        }



    }
}
