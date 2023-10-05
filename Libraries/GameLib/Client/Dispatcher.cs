using System;
using System.Threading;
using System.Windows;
using Contollers.GameBot;
using SilkroadSecurityApi;
using SRO_INGAME.Common;
using SRO_INGAME.View.Terms;

namespace SilkroadInformationAPI.Client
{
    class Dispatcher
    {
        public static void Process(Packet p)
        {
            try
            {
                #region GATEWAY
                if (p.Opcode == 0xA101)
                {
                    //Game.Controller.ViewController.ShowTermsAndServices();
                    Packets.Gateway.ShardResponse.Parse(p);
                }
                else if (p.Opcode == 0xA100)
                {
                    Packets.Gateway.PatchResponse.Parse(p);
                }
                else if (p.Opcode == 0xA102)
                {
                    Packets.Gateway.LoginResponse.Parse(p);
                }
                else if (p.Opcode == 0xA323)
                {
                    Packets.Gateway.CaptchaCodeResponse.Parse(p); 
                }
                else if (p.Opcode == 0xA103)
                {
                    Packets.Gateway.AgentAuthResponse.Parse(p);
                }
                #endregion

                #region CHARACTER SELECTION
                else if (p.Opcode == 0xB001)
                {
                    Packets.CharacterSelection.CharacterJoinResponse.Parse(p);
                }
                else if (p.Opcode == 0xB007)
                {
                    Packets.CharacterSelection.CharacterListResponse.Parse(p);
                }
                #endregion

                #region CHARACTER
                else if (p.Opcode == 0x3013)
                {
                    Packets.Character.CharacterData.CharData(p);
                }
                else if (p.Opcode == 0x34A5)
                {
                    Packets.Character.CharacterData.CharDataStart();
                }
                else if (p.Opcode == 0x34A6)
                {
                    Packets.Character.CharacterData.CharDatEnd();
                }
                else if (p.Opcode == 0x304E)
                {
                    Packets.Character.InfoUpdate.Parse(p); // update stat points
                }
                else if (p.Opcode == 0x3056)
                {
                    Packets.Character.ExpSpUpdate.Parse(p);
                }
                else if (p.Opcode == 0x303D)
                {
                    //Console.WriteLine("StatsUpdate 0x303D");
                    Game.Controller.ViewController.ShowNavigation();
                    Packets.Character.StatsUpdate.Parse(p);
                    SRCommon.isTeleporting = false;
                    // reload bot data
                    //SRCommon.pSkills.Load();
                    //SRCommon.pPotion.Load();
                }
                #endregion

                #region SPAWN
                else if (p.Opcode == 0xB05A)
                {
                    SRCommon.isTeleporting = true;
                    Console.WriteLine("isTeleporting = true 0xB05A");
                }
                else if (p.Opcode == 0x3017)
                {
                    Packets.Spawn.GroupSpawn.GroupSpawnStart(p);
                }
                else if (p.Opcode == 0x3019)
                {
                    Packets.Spawn.GroupSpawn.GroupSpawnData(p);
                }
                else if (p.Opcode == 0x3018)
                {
                    Packets.Spawn.GroupSpawn.GroupSpawnEnd(p);
                }
                else if (p.Opcode == 0x3015)
                {
                    Packets.Spawn.SingleSpawn.Parse(p);
                    // update CreatNearbyItemsGrid after 
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        SRCommon.pStatistics.RefreshButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    });
                }
                else if (p.Opcode == 0x3016)
                {
                    Packets.Spawn.Despawn.Parse(p);
                    // update CreatNearbyItemsGrid after 
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        SRCommon.pStatistics.RefreshButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
                    });
                }
                #endregion

                #region INVENTORY
                else if (p.Opcode == 0x3040)
                {
                    Packets.Inventory.ItemCountUpdatedDueAlchemy.Parse(p);
                }
                else if (p.Opcode == 0xB034)
                {
                    Packets.Inventory.InventoryOperation.Parse(p);
                    // reload bot data
                    //SRCommon.pSkills.Load();
                    //SRCommon.pPotion.Load();

                }
                else if (p.Opcode == 0xB04C)
                {
                    Packets.Inventory.ItemCountUpdated.Parse(p);
                    // reload bot data
                    //SRCommon.pSkills.Load();
                    //SRCommon.pPotion.Load();
                }
                else if (p.Opcode == 0x3052)
                {
                    Packets.Inventory.UpdateItemDurability.Parse(p);
                    // reload bot data
                    //SRCommon.pSkills.Load();
                }
                #endregion

                #region STORAGE
                else if (p.Opcode == 0x3049)
                {
                    Packets.Inventory.StorageInfoResponse.StorageInfoData(p);
                    // reload bot data
                    //SRCommon.pSkills.Load();
                    //SRCommon.pPotion.Load();
                }
                else if (p.Opcode == 0x3047)
                {
                    Packets.Inventory.StorageInfoResponse.StorageInfoStart();
                    // reload bot data
                    //SRCommon.pSkills.Load();
                    //SRCommon.pPotion.Load();
                }
                else if (p.Opcode == 0x3048)
                {
                    Packets.Inventory.StorageInfoResponse.StorageInfoEnd();
                    // reload bot data
                    //SRCommon.pSkills.Load();
                    //SRCommon.pPotion.Load();
                }
                #endregion

                #region COS
                else if (p.Opcode == 0xB0CB)
                {
                    Packets.COS.RideState.Parse(p);
                }
                else if (p.Opcode == 0x30C8)
                {
                    Packets.COS.COSData.Parse(p);
                }
                #endregion

                #region Chat
                else if (p.Opcode == 0x3026)
                {
                    Packets.Chat.ChatUpdated.ParseServer(p);
                }
                else if (p.Opcode == 0x7025)
                {
                    Packets.Chat.ChatUpdated.ParseClient(p);
                }
                #endregion

                #region Spells
                else if (p.Opcode == 0xB0A1)
                {
                    Packets.Spells.ClientSkillLearn.Parse(p);
                }
                else if (p.Opcode == 0xB202)
                {
                    Packets.Spells.ClientSkillWithdraw.Parse(p);
                }
                else if (p.Opcode == 0xB070)
                {
                    Packets.Spells.SpellUsed.Parse(p);
                }
                else if (p.Opcode == 0xB072)
                {
                    Packets.Spells.BuffEnded.Parse(p);
                }
                else if (p.Opcode == 0xB0BD)
                {
                    Packets.Spells.BuffStarted.Parse(p);
                }
                else if (p.Opcode == 0xB074)
                {
                    Packets.Spells.ClientSkillChannel.Parse(p);
                }
                #endregion

                #region PARTY
                else if (p.Opcode == 0xB06C)
                {
                    Packets.Party.PartyMatchingResponse.Parse(p);
                }
                else if (p.Opcode == 0x3065)
                {
                    Packets.Party.EnteredParty.Parse(p);
                }
                else if (p.Opcode == 0x3864)
                {
                    Packets.Party.PartyUpdate.Parse(p);
                }
                else if (p.Opcode == 0xB069)
                {
                    Packets.Party.PartyMatchingEntryFormed.Parse(p);
                }
                else if (p.Opcode == 0xB06B)
                {
                    Packets.Party.PartyMatchingEntryDeleted.Parse(p);
                }
                else if (p.Opcode == 0x706D)
                {
                    Packets.Party.PartyMatchingRequestJoin.Parse(p);
                }
                #endregion

                #region ENTITY
                else if (p.Opcode == 0xB021)
                {
                    Packets.Entity.PositionUpdate.Parse(p);
                }
                else if (p.Opcode == 0x30BF)
                {
                    Packets.Entity.StateChange.Parse(p);
                }
                else if (p.Opcode == 0x30D0)
                {
                    Packets.Entity.SpeedUpdate.Parse(p);
                }
                else if (p.Opcode == 0x3054)
                {
                    Packets.Entity.LevelUpAnimation.Parse(p);
                }
                else if (p.Opcode == 0x3057)
                {
                    Packets.Entity.HPMPUpdate.Parse(p);
                }
                else if (p.Opcode == 0xB516)
                {
                    Packets.Entity.PVPUpdate.Parse(p);
                }
                else if (p.Opcode == 0x3020)
                {
                    Packets.Entity.CelestialPosition.Parse(p);
                }
                else if (p.Opcode == 0x3038)
                {
                    Packets.Entity.ItemEquip.Parse(p);
                }
                else if (p.Opcode == 0x3039)
                {
                    Packets.Entity.ItemUnequip.Parse(p);
                }
                else if (p.Opcode == 0xB045)
                {
                    Packets.Entity.EntitySelected.Parse(p);
                }
                #endregion

                #region STALL
                else if (p.Opcode == 0xB0B3)
                {
                    Packets.Stall.Entered.Parse(p);
                }
                else if (p.Opcode == 0xB0B5)
                {
                    Packets.Stall.Leave.Parse();
                }
                else if (p.Opcode == 0x30B7)
                {
                    Packets.Stall.Action.Parse(p);
                }
                else if (p.Opcode == 0x30B8)
                {
                    Packets.Stall.Created.Parse(p);
                }
                else if (p.Opcode == 0x30B9)
                {
                    Packets.Stall.Closed.Parse(p);
                }
                else if (p.Opcode == 0xB0BA)
                {
                    Packets.Stall.Updated.Parse(p);
                }
                else if (p.Opcode == 0x30BB)
                {
                    Packets.Stall.NameUpdated.Parse(p);
                }
                else if (p.Opcode == 0xB0B1)
                {
                    //TODO: Client opened stall
                }
                else if (p.Opcode == 0xB0B2)
                {
                    //TODO: Client closed stall
                }
                #endregion

                #region LOGOUT
                else if (p.Opcode == 0x300A)
                {
                    // finx this to work with exit only not restart
                    //Game.Controller.ViewController.ExitApp();
                }
                #endregion

                #region Commands
                //else if (p.Opcode == 0x7608)
                //{
                //    int flag = p.ReadUInt8();
                //    if (flag == 0x1)
                //        ChatCommands.ChatFilter(p);
                //    else if (flag == 0x2)
                //    {
                //        if (SRCommon.Compass != null)
                //            SRCommon.Compass.Update(p);
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
