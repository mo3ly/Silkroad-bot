using SilkroadInformationAPI.Client.Information;
using System;
using System.Collections.Generic;

namespace Contollers.GameBot
{
    public class BotData
    {
        // Bot skills
        public static List<SilkroadInformationAPI.Media.DataInfo.Skill> AttackSkills = new List<SilkroadInformationAPI.Media.DataInfo.Skill>();
        public static List<SilkroadInformationAPI.Media.DataInfo.Skill> BuffSkills = new List<SilkroadInformationAPI.Media.DataInfo.Skill>();

        // utillity
        public static Random random = new Random();

        // Bot condition
        public static bool isBotting;

        // hunt settings
        public static bool useDrugSpeed = true;
        public static bool attackWithPet = false;

        // bot potion item
        public static Dictionary<string, InventoryItem> Gears = new Dictionary<string, InventoryItem>()
        {
            { "AttackWeapon", null },
            { "AttackShield", null },
            { "BuffWeapon", null },
            { "BuffShield", null }
        };

        // bot potion item
        public static Dictionary<string, uint> PotionItems = new Dictionary<string, uint>()
        {
            { "HP", 0 },
            { "MP", 0 },
            { "Abnormal", 0 },
            { "PetHGP", 0 }
        };

        // bot skill items
        /*public static string AttackWeaponID;
        public static string AttackShieldID;
        public static string BuffWeaponID;
        public static string BuffShieldID;*/


        //potion settings
        public static Dictionary<string, bool> PotionSettings = new Dictionary<string, bool>()
        {
            { "HP", false },
            { "MP", false },
            { "Abnormal", false },
            { "PetHGP", false }
        };

        //potion auto switch type settings
        public static Dictionary<string, bool> PotionAutoSwitch = new Dictionary<string, bool>()
        {
            { "HP", false },
            { "MP", false },
            { "Abnormal", false },
            { "PetHGP", false }
        };

        // potion percentage
        public static Dictionary<string, short> PotionPercentage = new Dictionary<string, short>()
        {
            { "HP", 0 },
            { "MP", 0 },
            { "PetHGP", 0 }
        };


    }
}
