using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using System;
using System.Linq;

namespace Contollers.GameBot.Logic
{
    public class AutoPotion
    {
        /**
         * TODO:
         * Add better concept to save mp and hp
         * delay abnormal because it finishes it too quickly
         */
        bool isDebug = false;

        public void Initialise()
        {
            AutoHP();
            AutoMP();
            AutoAbnormal();
            AutoPetHGP();
        }

        private void AutoHP()
        {
            uint percentageHP = (uint)(0.5f + ((100f * Client.Info.CurrentHP) / Client.Info.MaxHP));
            
            if (isDebug)
                Console.WriteLine($"percentageHP {percentageHP}, Current HP {Client.Info.CurrentHP}, Max HP {Client.Info.MaxHP}, ItemId {BotData.PotionItems["HP"]}, isActive {BotData.PotionSettings["HP"]}, isAutoSwitchActive {BotData.PotionAutoSwitch["HP"]}");
            
            if ((percentageHP <= BotData.PotionPercentage["HP"]) && BotData.PotionSettings["HP"] == true && BotData.PotionItems["HP"] != 0)
            {
                if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemID(BotData.PotionItems["HP"]))
                {
                    if (BotData.PotionAutoSwitch["HP"])
                    {
                        if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemMultiType(ItemType.HpPotion, ItemType.VigorPotion)) // add vigor
                        {
                            SilkroadInformationAPI.Client.Actions.Utility.UseReturn(); // check the selected action from user maybe if the auto return option isn't selected so keep it hunting without preaking
                            SRCommon.botController.StopBotting(3);
                        }
                    }
                    else
                    {
                        SilkroadInformationAPI.Client.Actions.Utility.UseReturn(); // check the selected action from user maybe if the auto return option isn't selected so keep it hunting without preaking

                        SRCommon.botController.StopBotting(3);
                    }
                }
                if (isDebug)
                    Console.WriteLine("[Potion]The character is using HP");
            }
        }

        private void AutoMP()
        {
            uint percentageMP = (uint)(0.5f + ((100f * Client.Info.CurrentMP) / Client.Info.MaxMP));

            if (isDebug)
                Console.WriteLine($"percentageMP {percentageMP}, Current MP {Client.Info.CurrentMP}, Max MP {Client.Info.MaxMP}, ItemId {BotData.PotionItems["MP"]}, isActive {BotData.PotionSettings["MP"]}, isAutoSwitchActive {BotData.PotionAutoSwitch["MP"]}");

            if ((percentageMP <= BotData.PotionPercentage["MP"]) && BotData.PotionSettings["MP"] == true && BotData.PotionItems["MP"] != 0)
            {
                if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemID(BotData.PotionItems["MP"]))
                {
                    if (BotData.PotionAutoSwitch["MP"]) //&& !Client.InventoryItems.Any(i => i.Value.Type == ItemType.HpPotion || i.Value.Type == ItemType.VigorPotion)
                    {
                        if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemMultiType(ItemType.MpPotion, ItemType.VigorPotion)) // add vigor
                        {
                            SilkroadInformationAPI.Client.Actions.Utility.UseReturn();

                            SRCommon.botController.StopBotting(3);
                        }
                    }
                    else
                    {
                        SilkroadInformationAPI.Client.Actions.Utility.UseReturn();
                        SRCommon.botController.StopBotting(3);
                    }
                }
                if (isDebug)
                    Console.WriteLine("[Potion]The character is using MP");
            }
        }

        private void AutoAbnormal()
        {
            if (isDebug)
                Console.WriteLine($"Abnormal: ItemId {BotData.PotionItems["Abnormal"]}, isActive {BotData.PotionSettings["Abnormal"]}, isAutoSwitchActive {BotData.PotionAutoSwitch["Abnormal"]}");

            if (Client.Info.BadStatus && BotData.PotionSettings["Abnormal"] == true && BotData.PotionItems["Abnormal"] != 0)
            {
                if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemID(BotData.PotionItems["Abnormal"]))
                {
                    if (BotData.PotionAutoSwitch["Abnormal"])
                    {
                        if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemMultiType(ItemType.PurificationPills, ItemType.UniversalPills)) // add vigor
                        {
                            SilkroadInformationAPI.Client.Actions.Utility.UseReturn();
                            SRCommon.botController.StopBotting(3);
                        }
                    }
                    else
                    {
                        SilkroadInformationAPI.Client.Actions.Utility.UseReturn();
                        SRCommon.botController.StopBotting(3);
                    }
                }
                if (isDebug)
                    Console.WriteLine("[Potion]The character is using Abnormal");
            }
        }

        private void AutoPetHGP()
        {
            if (isDebug)
                Console.WriteLine($"PetHGP: ItemId {BotData.PotionItems["PetHGP"]}, isActive {BotData.PotionSettings["PetHGP"]}, isAutoSwitchActive {BotData.PotionAutoSwitch["PetHGP"]}");

            //Client.InventoryItems.Where(x => x.Value.Type == ItemType.AttackPet).First().Value.TranslationName;
            if (Client.NearbyCOSs.ContainsKey(0) && BotData.PotionSettings["PetHGP"] == true && BotData.PotionItems["PetHGP"] != 0) // add pet reconvert system and attack or defence
            {
                if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemID(BotData.PotionItems["PetHGP"]))
                {
                    if (BotData.PotionAutoSwitch["PetHGP"])
                    {
                        if (!SilkroadInformationAPI.Client.Actions.Utility.UseItemMultiType(ItemType.PetHGP, ItemType.PetRecoveryKit)) // add vigor
                        {
                            SilkroadInformationAPI.Client.Actions.Utility.UseReturn();

                            SRCommon.botController.StopBotting(3);
                        }
                    }
                    else
                    {
                        SilkroadInformationAPI.Client.Actions.Utility.UseReturn();

                        SRCommon.botController.StopBotting(3);
                    }
                }
                if (isDebug)
                    Console.WriteLine("[Potion]The character is using PetHGP");
            }
        }
    }
}
