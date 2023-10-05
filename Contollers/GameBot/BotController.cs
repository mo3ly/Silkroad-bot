using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SilkroadInformationAPI.Media.DataInfo;
using System.Windows.Threading;
using System;
using System.Linq;
using SRO_INGAME.Common;
using System.Threading;
using System.Threading.Tasks;
using Contollers.GameBot.Logic;
using System.Windows;

namespace Contollers.GameBot
{
    /**
     * TODO:
     * auto stop bot when log out button clicked
     * Add bot on and off
     * mob counter based on kill owner packet
     * pet attack and protection
     * don't walk if monster is selected
     * auto-pick
     * arrow witch bow and crossbow
     */

    public class BotController
    {
        // Bot thread - Change this to thread
        public DispatcherTimer botThread;

        // Bot Logic
        public BotConditions botConditions = new BotConditions();
        public AutoPotion autoPotion = new AutoPotion();
        public AutoAttack autoAttack = new AutoAttack();
        public AutoBuff autoBuff = new AutoBuff();
        public AutoWalk autoWalk = new AutoWalk();

        // client start positon
        public SilkroadInformationAPI.Client.Information.BasicInfo.Position StartPosition;
        public SilkroadInformationAPI.Client.Information.BasicInfo.Position curPosition;
        public int buffsDelay;
        public bool isWalking;
        public bool isUsingBuffs;

        public BotController()
        {
            botThread = new DispatcherTimer();
            botThread.Interval = TimeSpan.FromMilliseconds(1000);
            botThread.Tick += BotThread_Tick;
        }

        public bool StartBotting()
        {
            if (BotData.isBotting == true)
            {
                return false;
            }
            else if (SRCommon.isTeleporting || SilkroadInformationAPI.Media.Data.MediaRegions[(uint)Client.Position.RegionID].SafeZone)
            {
                Notifications.Notifier.BannerNotification("Auto hunt", "Cannot start auto-hunting because no you are in a safe zone", 30);
                return false;
            }
            else if (BotData.Gears["AttackWeapon"] == null) // strict it to check for first weapon
            {
                Notifications.Notifier.BannerNotification("Auto hunt", "Cannot start auto-hunting because no weapon has been selected", 30); // maybe auto hunt if no skills??
                return false;
            } // add cannot start when dead
            else
            {
                try
                {
                    BotData.isBotting = true;
                    buffsDelay = 0;
                    // use botting skill effect to be added
                    SroClient.UseSpell(33788);
                    //SRCommon.game.BotSymbol(2);
                    // use speed
                    //if (BotData.useDrugSpeed) // error here when no drug it doesn't attack
                    //    SRCommon.game.UseItemSlot(Client.InventoryItems.Where(x => x.Value.MediaName.Contains("_SPEED_")).First().Value.Slot); // fix this to be more accurate
                    //wear weapon
                    SroClient.WearItem(Client.InventoryItems.Where(x => x.Value.ObjRefID == BotData.Gears["AttackWeapon"].ObjRefID && x.Value.PlusValue == BotData.Gears["AttackWeapon"].PlusValue).First().Value.Slot, 6);
                    //shield weapon
                    if (BotData.Gears["AttackShield"] != null)
                        SroClient.WearItem(Client.InventoryItems.Where(x => x.Value.ObjRefID == BotData.Gears["AttackShield"].ObjRefID && x.Value.PlusValue == BotData.Gears["AttackShield"].PlusValue).First().Value.Slot, 7);
                    StartPosition = Client.Position;
                    if (BotData.BuffSkills.Count > 0)
                        autoBuff.ApplyBuffs();
                    botThread.Start();
                    Notifications.Notifier.BannerNotification("Auto hunt", "Auto hunt system has been activated", 30);
                    SroClient.ChatMessage("(Bot) auto-hunt mode has been activated.");
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        SRCommon.Navigation.botStatusLabel.Content = "ON";
                        SRCommon.Navigation.BotActionButton.Content = "STOP BOT";
                    });
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public bool StopBotting(int code = 0)
        {
            if (BotData.isBotting == false)
                return false;

            string message = null;
            switch (code)
            {
                case 1:
                    message = "because you are in a safe zone";
                    break;
                case 2:
                    message = "because the character is dead";
                    break;
                case 3:
                    message = "because there isn't enough potion, returning.";
                    break;
            }

            SRCommon.game.BotSymbol(0);
            BotData.isBotting = false;
            botThread.Stop();
            Notifications.Notifier.BannerNotification("Auto hunt", "Auto hunt system has been deactivated " + message, 30);
            Application.Current.Dispatcher.Invoke(delegate
            {
                SRCommon.Navigation.botStatusLabel.Content = "OFF";
                SRCommon.Navigation.BotActionButton.Content = "START BOT";
            });
            return true;
        }

        void BotThread_Tick(object sender, EventArgs e) // check for weapon and skills and monster rarety unqiue then giant then .. // walk randomly if invalid target // check safe zone
        {
            try
            {
                //if (Client.State.Buffs)
                //Console.WriteLine($"Active skills count: {Client.State.Buffs.Values}"); 
                //Console.WriteLine($"Active skills count: {Client.State.BuffCount}");
                // Update
                // store previous position and compare it to the new one
                curPosition = Client.Position;

                botConditions.Initialise();
                autoPotion.Initialise();
                Console.WriteLine("Entered bot loop");
                if (!isUsingBuffs)
                {
                    if (Client.NearbyMobs.Where(x => x.Value.CurrentHP == 0).ToList().Count != 0 || Client.NearbyMobs.Count != 0)
                    {
                        SkillAttack();
                        Console.WriteLine("Entered attack loop");
                    }
                    else
                    {
                        Console.WriteLine("[Auto Walk]Entered auto walk loop");
                        isWalking = true;
                        int range = 100;
                        SRCommon.game.WalkTo(StartPosition.GetRealX() + Utility.RandomNumber(-1 * range, range), StartPosition.GetRealY() + Utility.RandomNumber(-1 * range, range));
                        botThread.Interval = TimeSpan.FromMilliseconds(4000);
                    }
                }
                buffsDelay++;
            }
            catch(Exception ex) { Console.WriteLine("BotThread_Tick error:" + ex); }
        }



        private void SkillAttack()
        {
            // use select first fire or ice or sword attack buff
            // check attack weapon is worn
            // if zerk is ready and monster is champion or unique use it
            // if stay in same place try to walk // maybe you you can parse trigger cannot attack due and obstcle message and take walk action based on it
            // invalid target issue
            // if buff has the sword fire or cold do it every 5 seconds
            // Mob_Type.Normal order mob priority from UI 
            SilkroadInformationAPI.Client.Information.Objects.Mob nerbayMob = Client.NearbyMobs.OrderBy(x => Math.Abs((long)x.Value.Position.GetRealX() - Client.Position.GetRealX())).ThenBy(x => x.Value.Rarity == (byte)Mob_Type.Normal).ThenBy(x => x.Value.Rarity == (byte)Mob_Type.Champion).ThenBy(x => x.Value.Rarity == (byte)Mob_Type.Unique).FirstOrDefault().Value;// then add distance check here!! between mob and you // add where MobCurrent HP != 0//.Where(x => x.Value.CurrentHP != 0)//.OrderBy(x => x.Value.CurrentHP)
            //Console.WriteLine($"[{nerbayMob.UniqueID}]Selected MOB: CurrentHP {nerbayMob.CurrentHP}, Rarity: {nerbayMob.Rarity}, X: {nerbayMob.Position.GetRealX()} Y: {nerbayMob.Position.GetRealY()}");
            //if(nerbayMob.CurrentHP == 0)
            //randomWalk(50, 50);

            if (nerbayMob.UniqueID != 0)
            {
                // use zerk
                byte[] zerkPriority = { (byte)Mob_Type.Unique, (byte)Mob_Type.GiantParty, (byte)Mob_Type.Giant, (byte)Mob_Type.Elite, (byte)Mob_Type.Champion };
                if (Client.Info.Zerk && zerkPriority.Contains(nerbayMob.Rarity) && !isUsingBuffs)
                {
                    SroClient.UseZerk();
                    Console.WriteLine("[Zerk] Entered zerk loop");
                }

                if (BotData.AttackSkills.Count > 0)
                {
                    SroClient.SelectObject(nerbayMob.UniqueID);
                    Skill selectedSkill = BotData.AttackSkills[Utility.RandomNumber(0, BotData.AttackSkills.Count)];
                    SilkroadInformationAPI.Client.Actions.Utility.UseSkillName(selectedSkill.MediaName, nerbayMob.UniqueID);
                    //Console.WriteLine($"Skill used {Client.NearbyMobs.First().Value.UniqueID}, {selectedSkill.TranslatedName}");
                }
                else
                {
                    SroClient.SelectObject(nerbayMob.UniqueID);
                    SRCommon.game.AutoAttack(nerbayMob.UniqueID);
                }
            }

            //if (nerbayMob.CurrentHP == 0) // maybe be you can use Client.Movement
                //autoWalk.randomWalk(100, 100); // maybe add counter
        }

    }
}
