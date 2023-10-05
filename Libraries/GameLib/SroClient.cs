using SilkroadInformationAPI.Client.Information;
using SilkroadInformationAPI.Client.Network;
using SilkroadSecurityApi;
using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;

/// 
/// HUGE Thanks to DaxterSoul to his awesome documentation and pushedx for SilkroadSecurity.
/// And to everyone else I used their source, mostly are mentioned.
/// 

namespace SilkroadInformationAPI
{
    public class SroClient
    {

        public static Security RemoteSecurity;
        public static Security LocalSecurity;

        private static ClientlessConnection con_Clientless = new ClientlessConnection();
        public Thread ClientThread;
        private static string GamePath = "";

        public SroClient()
        {

        }

        public SroClient(string Path, string blowfish = "169841")
        {
            GamePath = Path;
            if (File.Exists(GamePath + "\\sro_client.exe") && File.Exists(GamePath + "\\Media.pk2"))
                Media.LoadData.InitializeReader(Path, blowfish);
            else
                throw new FileNotFoundException("Couldn't find all required files in that folder!", "sro_client.exe, Media.pk2");
        }

        public void Initialize(string Path, string blowfish = "169841")
        {
            GamePath = Path;
            if (File.Exists(GamePath + "\\sro_client.exe") && File.Exists(GamePath + "\\Media.pk2"))
                Media.LoadData.InitializeReader(Path, blowfish);
            else
                throw new FileNotFoundException("Couldn't find all required files in that folder!", "sro_client.exe, Media.pk2");
        }

        //TODO: REMOVE USELESS CONFIGURATION!
        public Process StartClient(ushort port)
        {
            var loader = new Client.Loader.Loader();
            return loader.StartClient(GamePath, port, Media.Data.ServerInfo.Locale);
        }

        public void StartProxyConnection(string LoginServerIP, ushort local_port, bool AutomaticClientless = true)
        {
            ProxyClient.AutoSwitchToClientless = AutomaticClientless;
            //TODO: Should check if the IP is not direct. (i.e. sro.gameserver.com)
            ClientThread = new Thread(() => ProxyClient.StartProxy(LoginServerIP, Media.Data.ServerInfo.Port, "127.0.0.1", local_port));
            ClientThread.Start();
        }

        public void StopConnection()
        {
            if (ClientThread.IsAlive)
                ClientThread.Abort();
        }

        public void LoadData() // needs to be optimized
        {
            if (!Media.LoadData.IsInitialized())
                throw new Exception("SroClient is not initialized yet!");
            else
            {
                // cancel login server and port and version from here and read it from SRComman which is assigned from launcher data
                // use LoadTextZoneNames with discord
                // add titles to discord
                Console.WriteLine("Loading login servers!");
                Media.LoadData.LoadDivisonInfo();
                Console.WriteLine("Loading login server port");
                Media.LoadData.LoadGateport();
                Console.WriteLine("Loading server version.");
                Media.LoadData.LoadServerVersion();
                Console.WriteLine("Loading zone names!");
                //Media.LoadData.LoadTextZoneNames();
                //Console.WriteLine("Loading region info.");
                Media.LoadData.LoadRefRegion();
                Console.WriteLine("Loading translation!");
                //Media.LoadData.LoadTranslation();
                Console.WriteLine("Loading models!");
                //Media.LoadData.LoadCharacterData();
                //Console.WriteLine("Loading Buildings/Portals");
                Media.LoadData.LoadTeleportBuildings();
                Console.WriteLine("Loading items!");
                Media.LoadData.LoadItems();
                Console.WriteLine("Loading skills!");
                Media.LoadData.LoadSkills();
                Console.WriteLine("Loading blue options.");
                Media.LoadData.LoadMagicOptions();
                Console.WriteLine("Loading NPC's");
                Media.LoadData.LoadRefMappingShopGroup(); //Maps the Store Group Name to Store Name
                Console.WriteLine("Mapping package items to item data.");
                Media.LoadData.LoadRefScrapOfPackageItem(); //Maps the shop package name to item media name
                Console.WriteLine("Loading shop package items.");
                Media.LoadData.LoadRefShopGoods(); //Maps the shop package items to the store tab
                Console.WriteLine("Mapping shop tabs to groups.");
                Media.LoadData.LoadRefShopTabs(); //Maps the store tabs to store group
                Console.WriteLine("Mapping shop groups to stores.");
                Media.LoadData.LoadRefMappingShopWithTab(); //Maps the store group to Store shop
                Console.WriteLine("Loading shops.");
                Media.LoadData.LoadRefShop(); //Loads the Store
                Media.LoadData.LoadRefShopGroup(); //Maps the Store Group Name to NPC Media Name
                Media.LoadData.LoadLevelData(); //Loads maximum exp
            }

            Console.WriteLine("Finished!");
        }

        /// <summary>
        /// Returns a list of all inventory items.
        /// </summary>
        /// <returns>A dictionary of Key int of the item slot and Value of SilkroadInformationAPI.Client.Information.Item> </returns>
        public Dictionary<int, InventoryItem> GetInventoryItems()
        {
            return Client.Client.InventoryItems;
        }

        /// <summary>
        /// #DEBUG# Prints out the current inventory items in a nice way, note that it prints the count of the item only.
        /// </summary>
        public void PrintInventoryCount()
        {
            if (Client.Client.InventoryItems.Count == 0)
                return;

            int current = 0;
            for (int i = 13; i < Client.Client.Info.MaxInventorySlots; i++, current++)
            {
                if (current == 4)
                {
                    Console.WriteLine();
                    current = 0;
                }

                if (Client.Client.InventoryItems.ContainsKey(i))
                    Console.Write(Client.Client.InventoryItems[i].Stats + ", ");
                else
                    Console.Write("-1, ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// #DEBUG# Print all stores data.
        /// </summary>
        public string PrintShops()
        {
            string data = "";
            foreach (var store in Media.Data.MediaShops)
            {
                data += (store.StoreName) + Environment.NewLine;
                foreach (var group in store.ShopGroups)
                {
                    data += ("\t" + group.GroupName) + Environment.NewLine;
                    foreach (var tab in group.GroupTabs)
                    {
                        data += ("\t\t" + tab.TabName) + Environment.NewLine;
                        foreach (var item in tab.TabItems)
                        {
                            data += ("\t\t\t" + item.ItemMediaName) + Environment.NewLine;
                        }
                    }
                }
            }

            Console.WriteLine(data);
            return data;
        }



        /// <summary>
        /// Returns the surrounding characters of the main account.
        /// </summary>
        /// <returns>Returns a dictionary with the key being object unique ID and value of Client.Information.Objects.Character</returns>
        public Dictionary<uint, Client.Information.Objects.Character> GetSurroundingCharacters()
        {
            return Client.Client.NearbyCharacters;
        }

        /// <summary>
        /// Returns the NPCs near the main character
        /// </summary>
        /// <returns>Returns a dictionary with the key being object unique ID and value of Client.Information.Objects.Base</returns>
        public Dictionary<uint, Client.Information.Objects.Base> GetNearbyNPCs()
        {
            return Client.Client.NearbyNPCs;
        }

        /// <summary>
        /// Returns the mobs near the main character.
        /// </summary>
        /// <returns>Returns a dictionary with the key being object unique ID and value of Client.Information.Objects.Mob</returns>
        public Dictionary<uint, Client.Information.Objects.Mob> GetSurroundingMobs()
        {
            return Client.Client.NearbyMobs;
        }

        /// <summary>
        /// Returns the structures near the main character (Portals, Dimension holes, FW Structs, etc..).
        /// Check for owner, if there exists it's a dimension hole,
        /// If it has no HP, it's a portal.
        /// TODO: Add identification filters.
        /// 
        /// </summary>
        /// <returns>Returns a dictionary with the key being object unique ID and value of Client.Information.Objects.Structure</returns>
        public Dictionary<uint, Client.Information.Objects.Structure> GetSurroundingStructures()
        {
            return Client.Client.NearbyStructures;
        }

        /// <summary>
        /// Returns nearby gold dropped on the ground.
        /// </summary>
        /// <returns>Returns a dictionary with the key being object unique ID and value of Client.Information.Objects.Item</returns>
        public Dictionary<uint, Client.Information.Objects.Item> GetDroppedGold()
        {
            var list = new Dictionary<uint, Client.Information.Objects.Item>();
            Client.Client.NearbyItems.ToList().ForEach(x =>
            {
                if (x.Value.Amount != 0)
                    list.Add(x.Key, x.Value);
            });
            return list;
        }

        /// <summary>
        /// Returns nearby items dropped on the ground.
        /// </summary>
        /// <returns>Returns a dictionary with the key being object unique ID and value of Client.Information.Objects.Item</returns>
        public Dictionary<uint, Client.Information.Objects.Item> GetDroppedItems()
        {
            var list = new Dictionary<uint, Client.Information.Objects.Item>();
            Client.Client.NearbyItems.ToList().ForEach(x =>
            {
                if (x.Value.Amount == 0)
                    list.Add(x.Key, x.Value);
            });
            return list;
        }

        #region InGame functions
        public static void BlueNotice(string message)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(1);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void PinkNotice(string message)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(2);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void GreenNotice(string message)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(3);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void SystemMessage(string message, bool isBold = false)
        {
            Packet p = new Packet(0x310C);
            if (isBold)
                p.WriteUInt8(4);
            else
                p.WriteUInt8(5);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void ChatMessage(string message, bool isBold = false)
        {
            Packet p = new Packet(0x310C);
            if (isBold)
                p.WriteUInt8(6);
            else
                p.WriteUInt8(7);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void WriteToChatTextBox(string message)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(8);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }


        public static void AppendToChatTextBox(string message)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(9);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void MessageOverUser(string message)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(10);
            p.WriteAscii(Client.Client.Info.CharacterName);
            p.WriteAscii(message);
            LocalSecurity?.Send(p);
        }

        public static void GUIVisibilty(uint id, bool isVisible)
        {
            Packet p = new Packet(0x310C);
            p.WriteUInt8(11);
            p.WriteUInt16(id);
            if (isVisible)
                p.WriteUInt8(1);
            else
                p.WriteUInt8(0);
            LocalSecurity?.Send(p);
        }
        #endregion

        public void UseItemSlot(int slot)
        {
            var p = new Packet(0x704C, true);
            p.WriteInt8(slot);
            p.WriteInt16(Client.Actions.Utility.GenerateItemType(Client.Client.InventoryItems[slot].ObjRefID));
            RemoteSecurity?.Send(p);
        }

        public static void UseItem(int slot)
        {
            var p = new Packet(0x704C, true);
            p.WriteInt8(slot);
            p.WriteInt16(Client.Actions.Utility.GenerateItemType(Client.Client.InventoryItems[slot].ObjRefID));
            RemoteSecurity?.Send(p);
        }

        public static void UseItem(int slot, uint TargetUID)
        {
            var p = new Packet(0x704C, true);
            p.WriteInt8(slot);
            p.WriteInt16(Client.Actions.Utility.GenerateItemType(Client.Client.InventoryItems[slot].ObjRefID));
            p.WriteUInt32(TargetUID);
            RemoteSecurity?.Send(p);
        }

        public static void UseItem(int slot, byte OtherItemSlot)
        {
            var p = new Packet(0x704C, true);
            p.WriteInt8(slot);
            p.WriteInt16(Client.Actions.Utility.GenerateItemType(Client.Client.InventoryItems[slot].ObjRefID));
            p.WriteUInt8(OtherItemSlot);
            RemoteSecurity?.Send(p);
        }

        public static void WearItem(int fromSlot, byte toSlot)
        {
            var p = new Packet(0x7034, true);
            p.WriteInt8(0);
            p.WriteInt8(fromSlot);
            p.WriteInt8(toSlot);
            p.WriteInt16(0);
            RemoteSecurity?.Send(p);
        }

        public static void UseSpell(uint spellRefID, uint targetUID)
        {
            //if (!Media.Data.MediaSkills[spellRefID].RequireTarget)
            //throw new Exception("WTF?");

            var p = new Packet(0x7074);
            p.WriteUInt8(0x01);
            p.WriteUInt8(0x04);
            p.WriteUInt32(spellRefID);
            p.WriteUInt8(0x01);
            p.WriteUInt32(targetUID);
            RemoteSecurity?.Send(p);
        }

        public static void UseSpell(uint spellRefID)
        {
            if (!Media.Data.MediaSkills[spellRefID].UseOnSelf && Media.Data.MediaSkills[spellRefID].RequireTarget)
                throw new Exception("WTF??");
            var p = new Packet(0x7074);
            p.WriteUInt8(0x01);
            p.WriteUInt8(0x04);
            p.WriteUInt32(spellRefID);
            p.WriteUInt8(0x00);

            if (Media.Data.MediaSkills[spellRefID].UseOnSelf && Media.Data.MediaSkills[spellRefID].RequireTarget)
                p.WriteUInt32(Client.Client.Info.UniqueID);
            RemoteSecurity?.Send(p);
        }

        public static void SelectObject(uint targetUID)
        {
            Packet p = new Packet(0x7045);
            p.WriteUInt32(targetUID);
            RemoteSecurity?.Send(p);
        }

        public void AutoAttack(uint targetUID)
        {
            var p = new Packet(0x7074);
            p.WriteUInt8(0x01);
            p.WriteUInt8(0x01);
            p.WriteUInt8(0x01);
            p.WriteUInt32(targetUID);
            RemoteSecurity?.Send(p);
        }

        public void WalkTo(int X, int Y)
        {
            Client.Actions.Utility.WalkTo(X, Y);
        }

        public void Emote(int id)
        {
            var p = new Packet(0x3091);
            p.WriteUInt8(id);
            RemoteSecurity?.Send(p);
        }

        public void BotSymbol(byte id)
        {
            Packet p = new Packet(0x7402);
            p.WriteUInt8(id);
            RemoteSecurity?.Send(p);
        }

        public static void UseZerk()
        {
            Packet p = new Packet((ushort)0x70A7);
            p.WriteInt8(1);
            RemoteSecurity?.Send(p);
        }

        #region Exchange
        public static void SendExchange(uint UID)
        {
            Packet p = new Packet(0x7081);
            p.WriteUInt32(UID);
            RemoteSecurity?.Send(p);
        }
        public static void RejectExchange(uint UID) // check this
        {
            Packet p = new Packet(0x3080);
            p.WriteUInt8(0x1);
            p.WriteUInt32(UID); // p.WriteUInt8(0x0);
            RemoteSecurity?.Send(p);
        }
        #endregion

        #region party
        public static void JoinParty(uint UID)
        {
            Packet p = new Packet(0x7060);
            p.WriteUInt32(UID);
            p.WriteUInt8(5);
            RemoteSecurity?.Send(p);
        }

        public static void AcceptParty()
        {
            // to be added
        }

        public static void RejectParty() // check this
        {
            Packet p = new Packet(0x3080);
            p.WriteUInt8(0x2);
            p.WriteUInt16(0x2C0C);
            RemoteSecurity?.Send(p);
        }

        public static void leaveParty()
        {
            RemoteSecurity?.Send(new Packet(0x7061));
        }
        #endregion

        #region stall
        public static void CreateStall(string message, bool isAvatar = false) // check this
        {
            if (isAvatar)
            {
                // avatar
                //Packet avatar = new Packet(0x7160, true);
                //avatar.WriteUInt32(0x8AEF);
                //avatar.WriteUInt32(0x0F07);
                //RemoteSecurity?.Send(avatar);
            }
            /*
            // packet1
            Packet p1 = new Packet(0x70B1, true);
            p1.WriteAscii(message);
            RemoteSecurity?.Send(p1);

            // packet2
            Packet p2 = new Packet(0x70BA, true);
            p2.WriteUInt8(0x06);
            p2.WriteAscii(welcomeMessage);
            RemoteSecurity?.Send(p2);
            */

            // first stall packet typing stall name
            Packet p1 = new Packet(0x70B1, false);
            p1.WriteAscii(message); //stal
            RemoteSecurity?.Send(p1);

            // second stall packet typing stall welcome msg
            string welcomeMessage = $"Welcome to [{Client.Client.Info.CharacterName}]'s stall";
            Packet p2 = new Packet(0x70BA, false);
            p2.WriteUInt8(0x06); //static
            p2.WriteAscii(welcomeMessage); //welcome msg
            RemoteSecurity?.Send(p2);
        }

        public static void ExitStall()
        {
            Packet p = new Packet(0x70B2);
            RemoteSecurity?.Send(p);
        }
        #endregion

        #region Skill effects
        public static void GMSkill()
        {
            Packet p = new Packet(0x7074, true);
            p.WriteUInt8(01);
            p.WriteUInt8(04);
            p.WriteUInt32(3978);
            p.WriteUInt8(0);
            RemoteSecurity?.Send(p);
        }
        #endregion

        #region
        public static void PickItem(uint UID)
        {
            var Item = Client.Client.NearbyItems.FirstOrDefault(i => i.Value.UniqueID == UID);
            if (SRCommon.game.GetDroppedItems().ContainsKey(UID))
            {
                Packet p = new Packet(0x7074);
                p.WriteUInt8(1);
                p.WriteUInt8(2);
                p.WriteUInt8(1);
                p.WriteUInt32(UID);
                RemoteSecurity?.Send(p);
            }
        }
        #endregion
    }
}
