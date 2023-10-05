using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SilkroadInformationAPI.Client.Information.Objects;
using SilkroadInformationAPI.Media;
using SilkroadSecurityApi;
using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Contollers.GameBot
{
    public static class ChatCommands
    {
        public static async void ChatFilter(Packet packet)
        {
            try
            {
                /**
                 * TODO:
                 *  maybe add fortress commands later
                 *  if chat contains ! don't send it // print all commands in the beginning
                 *  create Action class for all these tasks
                 *  riding horse function isnot working
                 *  add charm pet in emotes
                 *  fix emotes order because it is corret
                 *  add auto select option
                 */
                string message = packet.ReadAscii();
                Console.WriteLine("Message: {0}, Lenght: {1}", message, message.Length);

                #region windows
                if (message == "/bot")
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SRCommon.Navigation.HuntButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    });
                }
                else if (message == "/info")
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SRCommon.Navigation.ViewCharButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    });
                }
                else if (message == "/guide" || message == "/chatcommands" || message == "/commands")
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SRCommon.Navigation.GuideButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    });
                }
                #endregion

                #region bot
                else if (message == "/bot on")
                {
                    SRCommon.botController.StartBotting();
                }
                else if (message == "/bot off")
                {
                    SRCommon.botController.StopBotting();
                }
                #endregion

                #region emotes
                else if (message == "/hi")
                {
                    SRCommon.game.Emote(0);
                }
                else if (message == "/smile")
                {
                    SRCommon.game.Emote(1);
                }
                else if (message == "/greeting")
                {
                    SRCommon.game.Emote(3);
                }
                else if (message == "/yes")
                {
                    SRCommon.game.Emote(4);
                }
                else if (message == "/rush")
                {
                    SRCommon.game.Emote(5);
                }
                else if (message == "/joy")
                {
                    SRCommon.game.Emote(6);
                }
                else if (message == "/no")
                {
                    SRCommon.game.Emote(7);
                }
                else if (message == "/sit")
                {
                    Packet p = new Packet(0x704F);
                    p.WriteUInt8(4);
                    SroClient.RemoteSecurity.Send(p);
                }
                else if (message == "/stand")
                {
                    Packet p = new Packet(0x704F);
                    p.WriteUInt8(4);
                    SroClient.RemoteSecurity.Send(p);
                }
                #endregion

                #region potion
                else if (message == "/hp")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.HpPotion);
                }
                else if (message == "/mp")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.MpPotion);
                }
                else if (message == "/vigor")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.VigorPotion);
                }
                else if (message == "/pill")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemMultiType(ItemType.PurificationPills, ItemType.UniversalPills);
                }
                else if (message == "/recover pet")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemMultiType(ItemType.PetHGP, ItemType.PetRecoveryKit);
                }
                #endregion

                #region pet
                else if (message == "/ride")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.RidePet);
                }
                else if (message == "/ride trade")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.TradeRidePet);
                }
                else if (message == "/spawn attack pet")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.AttackPet);
                }
                else if (message == "/spawn pick pet")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseItemType(ItemType.PickupPet);
                }
                #endregion

                #region stats
                else if (message == "/speed")
                {
                    SRCommon.game.UseItemSlot(Client.InventoryItems.Where(x => x.Value.MediaName.Trim().StartsWith("_SPEED_")).First().Value.Slot);
                }
                else if (message == "/str")
                {
                    if (Client.Info.StatPoints > 0)
                        SroClient.RemoteSecurity.Send(new Packet(0x7050));
                    SroClient.ChatMessage($"(Action) a strength point has been added.");
                }
                else if (message == "/int")
                {
                    if (Client.Info.StatPoints > 0)
                        SroClient.RemoteSecurity.Send(new Packet(0x7051));
                    SroClient.ChatMessage($"(Action) an intellect point has been added.");
                }
                else if (message == "/auto str")
                {
                    SroClient.ChatMessage($"(Action) [{Client.Info.StatPoints}] avaliable state point(s), type /yes str to activate the auto strength points system.");
                }
                else if (message == "/yes str")
                {
                    SroClient.ChatMessage($"(Action) auto strength point(s) system has been activated.");
                    for (int x = 0; x <= Client.Info.StatPoints; x++)
                    {
                        SroClient.RemoteSecurity.Send(new Packet(0x7050));
                        await Task.Delay(100);
                    }
                    SroClient.ChatMessage($"(Action) [{Client.Info.StatPoints}] strength point(s) has been added.");
                }
                else if (message == "/auto int")
                {
                    SroClient.ChatMessage($"(Action) [{Client.Info.StatPoints}] avaliable state point(s), type /yes int to activate the auto intellect points system.");
                }
                else if (message == "/yes int")
                {
                    // keep adding int -> fix the auto update for the stat points
                    SroClient.ChatMessage($"(Action) auto intellect point(s) system has been activated.");
                    for (int x = 0; x <= Client.Info.StatPoints; x++)
                    {
                        SroClient.RemoteSecurity.Send(new Packet(0x7051));
                        await Task.Delay(100);
                    }
                    SroClient.ChatMessage($"(Action) [{Client.Info.StatPoints}] intellect point(s) has been added.");
                }
                #endregion

                #region pvp
                else if (message.Trim().StartsWith("/pvp")) // change it to names like red blue etc instead of number
                {
                    if (Client.Info.PVPCape == FRPVPMode.None)
                    {
                        Dictionary<string, FRPVPMode> PVPCaps = new Dictionary<string, FRPVPMode>()
                        {
                            { "red" , FRPVPMode.Red },
                            { "black" , FRPVPMode.Black },
                            { "blue" , FRPVPMode.Blue },
                            { "white" , FRPVPMode.White },
                            { "yellow" , FRPVPMode.Yellow },
                        };
                        string[] args = message.Split(' ');
                        if (PVPCaps.Keys.ToList().Contains(args[1]))
                        {
                            Packet p = new Packet(0x7516);
                            p.WriteUInt8(PVPCaps[args[1]]);
                            SroClient.RemoteSecurity.Send(p);
                            SroClient.ChatMessage($"(PVP) wearing {args[1]} cap.");
                        }
                        else
                        {
                            Packet p = new Packet(0x7516);
                            p.WriteUInt8(1);
                            SroClient.RemoteSecurity.Send(p);
                            SroClient.ChatMessage("(PVP) no valid option, wearing a red cap.");
                        }
                    }
                    else
                        SroClient.ChatMessage("(PVP) This character is already in a pvp mode");
                }
                else if (message == "/cpvp")
                {
                    if (Client.Info.PVPCape != FRPVPMode.None)
                    {
                        Packet p = new Packet(0x7516);
                        p.WriteUInt8(0);
                        SroClient.RemoteSecurity.Send(p);
                        SroClient.ChatMessage("(PVP) unwearing the pvp cap.");
                    }
                    else
                        SroClient.ChatMessage("(PVP) This character is not in a pvp mode");
                }
                #endregion

                #region exhange
                else if (message.Trim().StartsWith("/exc"))
                {
                    Character Char = null;
                    string[] args = message.Split(' ');

                    if ((args.Length == 1) && Client.SelectedUniqueID == 0)
                    {
                        SroClient.ChatMessage($"(Exchange) select a character or type its name after exc command to send an exchange it.");
                        return;
                    }

                    if ((args.Length > 1) && args[1].Length > 0)
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.Name == args[1]))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.Name == args[1]).Value;
                        else
                            Char = null;
                    }
                    else
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.UniqueID == Client.SelectedUniqueID))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.UniqueID == Client.SelectedUniqueID).Value;
                        else
                            Char = null;
                    }


                    if (Char != null)
                    {
                        SroClient.SendExchange(Char.UniqueID);
                        SroClient.ChatMessage($"(Exchange) has been sent to {Char.Name}.");
                    }
                    else
                        SroClient.ChatMessage($"(Exchange) the entered character name might be invalid or not in in your range.");
                }
                else if (message.Trim().StartsWith("/banexc")) // don't allow exchange send or accept when char is blocked
                {
                    Character Char = null;
                    string[] args = message.Split(' ');

                    if ((args.Length == 1) && Client.SelectedUniqueID == 0)
                    {
                        SroClient.ChatMessage($"(Exchange) select a character or type its name after unblock command to unmute it.");
                        return;
                    }

                    if ((args.Length > 1) && args[1].Length > 0)
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.Name == args[1]))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.Name == args[1]).Value;
                        else
                            Char = null;
                    }
                    else
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.UniqueID == Client.SelectedUniqueID))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.UniqueID == Client.SelectedUniqueID).Value;
                        else
                            Char = null;
                    }


                    if (Char != null)
                    {
                        if (SRCommon.SessionData.blockedExchange.ContainsKey(Char.UniqueID))
                            SroClient.ChatMessage($"(Exchange) {Char.Name} is already blocked.");
                        else
                        {
                            SRCommon.SessionData.blockedExchange.Add(Char.UniqueID, Char.Name);
                            SroClient.ChatMessage($"(Exchange) {Char.Name} has been blocked.");
                        }
                    }
                    else
                    {
                        SroClient.ChatMessage($"(Exchange) the entered character name might be invalid or not in in your range.");
                    }
                }
                else if (message.Trim().StartsWith("/banexc"))
                {
                    Character Char = null;
                    string[] args = message.Split(' ');

                    if ((args.Length == 1) && Client.SelectedUniqueID == 0)
                    {
                        SroClient.ChatMessage($"(Exchange) select a character or type its name after block command to mute it.");
                        return;
                    }

                    if ((args.Length > 1) && args[1].Length > 0)
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.Name == args[1]))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.Name == args[1]).Value;
                        else
                            Char = null;
                    }
                    else
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.UniqueID == Client.SelectedUniqueID))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.UniqueID == Client.SelectedUniqueID).Value;
                        else
                            Char = null;
                    }


                    if (Char != null)
                    {
                        if (SRCommon.SessionData.blockedExchange.ContainsKey(Char.UniqueID))
                        {
                            SRCommon.SessionData.blockedExchange.Remove(Char.UniqueID);
                            SroClient.ChatMessage($"(Exchange) {Char.Name} has been unblocked.");
                        }
                        else
                            SroClient.ChatMessage($"(Exchange) {Char.Name} is not in the block list.");
                    }
                    else
                    {
                        SroClient.ChatMessage($"(Exchange) the entered character name might be invalid or not in in your range.");
                    }
                }
                #endregion

                #region party
                else if (message.Trim().StartsWith("/pt"))//
                {
                    // send party to select char check if it real user
                    if (Client.Party.PartyMaster != null)
                    {
                        SroClient.ChatMessage($"(Party) This character is already joining party.");
                    }
                    else
                    {

                        Character Char = null;
                        string[] args = message.Split(' ');

                        if ((args.Length == 1) && Client.SelectedUniqueID == 0)
                        {
                            SroClient.ChatMessage($"(Party) select a character or type its name after pt command to send party request.");
                            return;
                        }

                        if ((args.Length > 1) && args[1].Length > 0)
                        {
                            if (Client.NearbyCharacters.Any(i => i.Value.Name == args[1]))
                                Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.Name == args[1]).Value;
                            else
                                Char = null;
                        }
                        else
                        {
                            if (Client.NearbyCharacters.Any(i => i.Value.UniqueID == Client.SelectedUniqueID))
                                Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.UniqueID == Client.SelectedUniqueID).Value;
                            else
                                Char = null;
                        }


                        if (Char != null)
                        {
                            SroClient.JoinParty(Char.UniqueID);
                            SroClient.ChatMessage($"(Party) {Char.Name} has been recieved party request from you.");
                        }
                        else
                            SroClient.ChatMessage($"(Party) the entered character name might be invalid or not in in your range.");
                    }
                }
                else if (message.Trim().StartsWith("/banpt"))
                {
                    Character Char = null;
                    string[] args = message.Split(' ');

                    if ((args.Length == 1) && Client.SelectedUniqueID == 0)
                    {
                        SroClient.ChatMessage($"(Party) select a character or type its name after blockpt command to unmute it.");
                        return;
                    }

                    if ((args.Length > 1) && args[1].Length > 0)
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.Name == args[1]))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.Name == args[1]).Value;
                        else
                            Char = null;
                    }
                    else
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.UniqueID == Client.SelectedUniqueID))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.UniqueID == Client.SelectedUniqueID).Value;
                        else
                            Char = null;
                    }


                    if (Char != null)
                    {
                        if (SRCommon.SessionData.blockedParty.ContainsKey(Char.UniqueID))
                            SroClient.ChatMessage($"(Party) {Char.Name} is already blocked.");
                        else
                        {
                            SRCommon.SessionData.blockedParty.Add(Char.UniqueID, Char.Name);
                            SroClient.ChatMessage($"(Party) {Char.Name} has been blocked.");
                        }
                    }
                    else
                    {
                        SroClient.ChatMessage($"(Party) the entered character name might be invalid or not in in your range.");
                    }
                }
                else if (message.Trim().StartsWith("/unbanpt"))
                {
                    Character Char = null;
                    string[] args = message.Split(' ');

                    if ((args.Length == 1) && Client.SelectedUniqueID == 0)
                    {
                        SroClient.ChatMessage($"(Party) select a character or type its name after unblockpt command to mute it.");
                        return;
                    }

                    if ((args.Length > 1) && args[1].Length > 0)
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.Name == args[1]))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.Name == args[1]).Value;
                        else
                            Char = null;
                    }
                    else
                    {
                        if (Client.NearbyCharacters.Any(i => i.Value.UniqueID == Client.SelectedUniqueID))
                            Char = Client.NearbyCharacters.FirstOrDefault(i => i.Value.UniqueID == Client.SelectedUniqueID).Value;
                        else
                            Char = null;
                    }


                    if (Char != null)
                    {
                        if (SRCommon.SessionData.blockedParty.ContainsKey(Char.UniqueID))
                        {
                            SRCommon.SessionData.blockedParty.Remove(Char.UniqueID);
                            SroClient.ChatMessage($"(Party) {Char.Name} has been unblocked.");
                        }
                        else
                            SroClient.ChatMessage($"(Party) {Char.Name} is not in the block list.");
                    }
                    else
                    {
                        SroClient.ChatMessage($"(Party) the entered character name might be invalid or not in in your range.");
                    }
                }
                else if (message == "/rejectpt")
                {
                    SroClient.ChatMessage($"(Party) reject the party successfully.");
                    SroClient.RejectParty();
                }
                else if (message == "/leavept")
                {
                    if (Client.Party.PartyMaster != null) // check 0x7061 in dispatcher to update this
                    {
                        SroClient.leaveParty();
                        SroClient.ChatMessage($"(Party) left the party successfully.");
                    }
                    else
                        SroClient.ChatMessage($"(Party) you are not joining any party.");
                }
                #endregion party

                #region stall
                else if (message.Trim().StartsWith("/stall")) //add close stall // check safe zone and not in battle // check already in stall // add close stall
                {
                    if (Data.MediaRegions[(uint)Client.Position.RegionID].SafeZone) // add delays
                    {
                        string arg = message.Substring(7);
                        Console.WriteLine("Stall message: {0}", arg);
                        //if (arg == null || arg.Length < 1)
                        //{
                        string stallMessage = $"[{Client.Info.CharacterName}]'s stall";
                        SroClient.CreateStall(stallMessage, true);
                        //}
                        //else if (arg.Length > 2)
                        //{
                        //   SroClient.CreateStall(arg, false);
                        //}
                        SroClient.ChatMessage("(Stall) the stall has been created successfully");
                    }
                    else
                    {
                        SroClient.ChatMessage("(Stall) you should be in a safe zone to create a stall.");
                    }
                }
                else if (message == "/exitstall") // check if on stall
                {
                    SroClient.ExitStall();
                    SroClient.ChatMessage($"(Stall) has beed exited successffully.");
                }
                #endregion

                #region action
                else if (message == "/pick")
                {
                    //trace
                    Client.NearbyItems.Select(i => $"UnqiueID: {i.Value.UniqueID}, Amount: {i.Value.Amount}, ModelID: {i.Value.ModelID}, , Name: { Data.MediaItems[i.Value.ModelID].MediaName}").ToList().ForEach(Console.WriteLine);
                    if (Client.NearbyItems.Count != 0)
                    {
                        Packet p = new Packet(0x7074);
                        p.WriteUInt8(1);
                        p.WriteUInt8(2);
                        p.WriteUInt8(1);
                        p.WriteUInt32(Client.NearbyItems.First().Value.UniqueID);
                        SroClient.RemoteSecurity.Send(p);
                        SroClient.ChatMessage($"(Action) picked an item.");
                    }
                }
                else if (message == "/trace")
                {
                    if (Client.SelectedUniqueID != 0)
                    {
                        Packet p = new Packet(0x7074);
                        p.WriteUInt8(1);
                        p.WriteUInt8(3);
                        p.WriteUInt8(1);
                        p.WriteUInt32(Client.SelectedUniqueID);
                        SroClient.RemoteSecurity.Send(p);
                        SroClient.ChatMessage($"(Action) trace has been actiavted.");
                    }
                }

                else if (message.Trim().StartsWith("/walk"))
                {
                    string[] args = message.Split(' ');
                    SRCommon.game.WalkTo(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]));
                }
                else if (message.Trim().StartsWith("/useitem"))
                {
                    if (int.Parse(message.Split(' ')[1]) > 13)
                        SRCommon.game.UseItemSlot(Convert.ToInt32(message.Split(' ')[1]));
                }
                else if (message == "/return")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.UseReturn();
                }
                else if (message == "/wear weapon")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.WearItem(ItemType.Weapon, 6);
                }
                else if (message == "/wear shield")
                {
                    SilkroadInformationAPI.Client.Actions.Utility.WearItem(ItemType.Shield, 7);
                }
                else if (message == "/attack")
                {
                    SRCommon.game.AutoAttack(SRCommon.game.GetSurroundingMobs().First().Value.UniqueID);
                }
                else if (message == "/exit")
                {
                    SRCommon.botController.StopBotting();
                    Packet p = new Packet(0x7005);
                    p.WriteUInt8(1);
                    SroClient.RemoteSecurity.Send(p);
                    SroClient.ChatMessage($"(System) exiting the game, good bye.");
                }
                else if (message == "/restart")
                {
                    SRCommon.botController.StopBotting();
                    Packet p = new Packet(0x7005);
                    p.WriteUInt8(2);
                    SroClient.RemoteSecurity.Send(p);
                    SroClient.ChatMessage($"(System) restarting the game.");
                }
                #endregion

                #region test packets
                else if (message == "/test")
                {
                    SroClient.ChatMessage($"(test) packet has been sent.");
                    SroClient.ChatMessage($"(test) Bold packet has been sent.", true);
                    SroClient.SystemMessage($"(test) packet has been sent.");
                    SroClient.SystemMessage($"(test) Bold packet has been sent.", true);
                    /*
                    SroClient.GreenNotice("This is GreenNotice");
                    SroClient.PinkNotice("This is PinkNotice");
                    SroClient.BlueNotice("This is BlueNotice");
                    SroClient.SystemMessage("This is System Message");*/
                    //Packet p = new Packet(0x30CF);
                    //SroClient.LocalSecurity?.Send(p);
                    SroClient.GUIVisibilty(16, false);
                    SroClient.GUIVisibilty(10, false);
                    //SroClient.WriteToChatTextBox("Hello iam selling this item < Long Spear (8) >");

                    const string UNDERLINE = "\x1B[4m";
                    const string RESET = "\x1B[0m";
                    string s = " < Long Spear (8) >";
                    SroClient.AppendToChatTextBox($"\x1B[4m{s}\x1B[24m");
                    SroClient.AppendToChatTextBox("̲s̲s̲s̲s̲s̲s̲s̲s̲s̲s̲s̲");
                    SroClient.MessageOverUser("[Test msg] Hello iam selling this item < Long Spear (8) >");
                }
                else if (message == "/test2")
                {
                    // be careful with this and make it on the sro_client better because you can easily bybased
                    SroClient.GUIVisibilty(16, true); // one entity select check if character hide it on battle royal
                    SroClient.GUIVisibilty(10, true); // same here hide it on every select
                    //SroClient.WriteToChatTextBox("Hello iam selling this item < Long Spear (Plus 8) (Level 101) (Sun) (FB) >");
                    SroClient.AppendToChatTextBox("< Long Spear (Sun) (Plus 8) (Lvl 101) >");
                }
                else if (message.Trim().StartsWith("/hide"))
                {
                    SroClient.GUIVisibilty(uint.Parse(message.Split(' ')[1]), false);
                    SroClient.SystemMessage($"(GUI) hidden.", true);
                }
                else if (message.Trim().StartsWith("/show"))
                {
                    SroClient.GUIVisibilty(uint.Parse(message.Split(' ')[1]), true);
                    SroClient.SystemMessage($"(GUI) shown.", true);
                }
                else if (message == "/skill")
                {
                    SroClient.GMSkill();
                    //Packet p = new Packet(0xB072);
                    //p.WriteUInt8(01);
                    //p.WriteUInt32(0x0242);
                    //SroClient.LocalSecurity?.Send(p);
                    //Packet p = new Packet(0x3057);
                    //p.WriteUInt32(0x01948F);
                    //p.WriteUInt16(0x04);
                    //p.WriteUInt8(0x02);
                    //p.WriteUInt32(0x8923);
                    //SroClient.LocalSecurity?.Send(p);
                    //SroClient.ChatMessage($"(test) skill packet has been sent.");
                }
                else if (message == "/c")
                {
                    int rows = 10;
                    Packet p = new Packet(0x310C);
                    p.WriteUInt16(22);// identifier
                    p.WriteUInt16(rows);
                    for (int x = 0; x < rows; x++)
                    {
                        p.WriteAscii($"item {x}");
                    }
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(chest) packet has been sent.");
                }
                else if (message == "/c2")
                {
                    int rows = 12;
                    Packet p = new Packet(0x310C);
                    p.WriteUInt16(22);// identifier
                    p.WriteUInt16(rows);
                    for (int x = 0; x < rows; x++)
                    {
                        p.WriteAscii($"chest {x}");
                    }
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(chest) packet has been sent.");
                }
                else if (message == "/c3")
                {
                    int rows = 36;
                    Packet p = new Packet(0x310C);
                    p.WriteUInt16(22);// identifier
                    p.WriteUInt16(rows);
                    for (int x = 0; x < rows; x++)
                    {
                        p.WriteAscii($"lollll 36 {x}");
                    }
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(chest) packet has been sent.");
                }
                else if (message == "/c4")
                {
                    int rows = 45;
                    Packet p = new Packet(0x310C);
                    p.WriteUInt16(22);// identifier
                    p.WriteUInt16(rows);
                    for (int x = 0; x < rows; x++)
                    {
                        p.WriteAscii($"haha 45 {x}");
                    }
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(chest) packet has been sent.");
                }
                else if (message == "/c5")
                {
                    int rows = 500;
                    Packet p = new Packet(0x310C);
                    p.WriteUInt16(22);// identifier
                    p.WriteUInt16(rows);
                    for (int x = 0; x < rows; x++)
                    {
                        p.WriteAscii($"haha heavy {x}");
                    }
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(chest) packet has been sent.");
                }
                else if (message == "/ba")
                {
                    Packet p = new Packet(0x34D2);
                    // registeration result
                    //p.WriteUInt16(0xFF);
                    //p.WriteUInt16(0x1);
                    // registeration period has been ended
                    //p.WriteUInt16(0X3);
                    //p.WriteUInt16(0);
                    // Player cannot register if...
                    //p.WriteUInt16(0XD);
                    //p.WriteUInt16(0);
                    // battle arena 1 minute
                    //p.WriteUInt16(0XE);
                    // p.WriteUInt16(0);
                    // battle arena error
                    //p.WriteUInt16(0X4);
                    //p.WriteUInt16(0x1);
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(packet) BA packet has been sent.");
                }
                else if (message == "/ba2")
                {
                    Packet p = new Packet(0x34E1);
                    p.WriteInt8(1);
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(packet) BA packet has been sent.");
                }
                else if (message == "/rm guild")
                {
                    Packet p = new Packet(0x3100);
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(packet) guild name removal packet has been sent.");
                }
                else if (message == "/afk")
                {
                    SroClient.UseSpell(33788);
                    SroClient.ChatMessage($"(spell) packet has been sent.");
                }
                else if (message == "/dmg")
                {
                    SroClient.UseSpell(33790);
                    SroClient.ChatMessage($"(dmg) packet has been sent.");
                }
                else if (message == "/killme")
                {
                    for (int x = 0; x < 20; x++)
                    {
                        SroClient.UseSpell(33790);
                        await Task.Delay(1000); // minus 10% hp
                    }

                    for (int x = 0; x < 40; x++)
                    {
                        SroClient.UseSpell(33795);
                        await Task.Delay(500); // minus 1% hp
                    }
                    for (int x = 0; x < 20; x++)
                    {
                        SroClient.UseSpell(33794); // minus 1 hp
                        await Task.Delay(500);
                    }
                    SroClient.ChatMessage($"(kill) packet has been sent.");
                }
                else if (message == "/dmg final")
                {
                    SroClient.UseSpell(33794);
                    SroClient.ChatMessage($"(dmg final) packet has been sent.");
                }
                else if (message == "/d1")
                {
                    SroClient.UseSpell(33791);
                    SroClient.ChatMessage($"(dmg) packet has been sent.");
                }
                else if (message == "/d2")
                {
                    SroClient.UseSpell(33792);
                    SroClient.ChatMessage($"(dmg) packet has been sent.");
                }
                else if (message.Trim().StartsWith("/weather"))
                {
                    Packet p = new Packet(0x3809);
                    p.WriteUInt8(Convert.ToUInt32(message.Split(' ')[1])); //Clear = 1,  Rain = 2,  Snow = 3,
                    p.WriteUInt8(Convert.ToUInt32(message.Split(' ')[2])); //Clear = 1,  Rain = 2,  Snow = 3,
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(weather) packet has been sent.");
                }
                else if (message.Trim().StartsWith("/fguide"))
                {
                    Packet p = new Packet(0xB0EA);
                    p.WriteUInt8(1);
                    p.WriteUInt64(Convert.ToUInt32(message.Split(' ')[1])); //Clear = 1,  Rain = 2,  Snow = 3,
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(Guide flag) packet has been sent.");
                }
                else if (message.Trim().StartsWith("/s"))
                {
                    if (int.Parse(message.Split(' ')[1]) > 13)
                        SroClient.UseSpell(Convert.ToUInt32(message.Split(' ')[1]));
                }
                else if (message.Trim().StartsWith("/dm"))
                {
                    Packet p = new Packet(0x310C);
                    p.WriteUInt8(50);// identifier
                    p.WriteInt32(Convert.ToInt32(message.Split(' ')[1]));
                    p.WriteInt32(Convert.ToInt32(message.Split(' ')[2]));
                    p.WriteInt32(Convert.ToInt32(message.Split(' ')[3]));
                    p.WriteInt32(Convert.ToInt32(message.Split(' ')[4]));
                    SroClient.LocalSecurity?.Send(p);
                    SroClient.ChatMessage($"(dm) packet has been sent. " + Convert.ToInt32(message.Split(' ')[1]) + " " + Convert.ToInt32(message.Split(' ')[2]) + " " + Convert.ToInt32(message.Split(' ')[3]) + " " + Convert.ToInt32(message.Split(' ')[4]));
                } else if (message == "/item")
                {
                    for (int x = 0; x <= 1; x++)
                    {
                        Packet p2 = new Packet(0xB034);
                        p2.WriteUInt8(01);
                        p2.WriteUInt8(06);
                        p2.WriteUInt8(0x19);
                        p2.WriteUInt32(0);
                        p2.WriteUInt32(0x605D);
                        p2.WriteUInt16(0x12);
                        SroClient.LocalSecurity?.Send(p2);

                        SroClient.WriteToChatTextBox("test");

                        // delete packet
                        Packet p = new Packet(0xB034);
                        p.WriteUInt8(01);
                        p.WriteUInt8(07);
                        p.WriteUInt8(0x19);
                        SroClient.LocalSecurity?.Send(p);
                    }

                } 
                else if (message == "/nearids")
                {
                    foreach (var character in SRCommon.game.GetSurroundingCharacters())
                    {
                        SroClient.SystemMessage($"UID: {character.Value.UniqueID}, Name: {character.Value.Name}");
                    }
                }
                else if (message.Trim().StartsWith("/openstall"))
                {
                    Packet p = new Packet(0x70B3);
                   // p.WriteUInt32(SRCommon.game.GetSurroundingCharacters().FirstOrDefault().Value.UniqueID);
                    p.WriteUInt32(Convert.ToUInt32(message.Split(' ')[1]));
                    SroClient.RemoteSecurity?.Send(p);
                }
                else if (message.Trim().StartsWith("/bstallslot"))
                {
                    Packet p = new Packet(0x70B4);
                    p.WriteUInt8(byte.Parse(message.Split(' ')[1]));
                    SroClient.RemoteSecurity?.Send(p);
                }
                #endregion
            }
            catch { }
        }
    }
}
