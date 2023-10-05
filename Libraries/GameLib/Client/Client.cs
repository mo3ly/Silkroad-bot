﻿using System.Collections.Generic;
using SilkroadInformationAPI.Client.Information;
using SilkroadInformationAPI.Client.Information.Spells;
using SilkroadInformationAPI.Client.Information.Quests;

namespace SilkroadInformationAPI.Client
{
    public class Client
    {
        public static Info Info = new Info();
        public static Information.Party.Party Party = new Information.Party.Party();

        public static Dictionary<int, InventoryItem> InventoryItems = new Dictionary<int, InventoryItem>();
        public static Dictionary<int, InventoryItem> StorageItems = new Dictionary<int, InventoryItem>();

        public static List<InventoryItem> SoldItems = new List<InventoryItem>();
        public static List<Quest> Quests = new List<Quest>();
        public static List<Mastery> Masteries = new List<Mastery>();
        public static List<Media.DataInfo.Skill> Skills = new List<Media.DataInfo.Skill>();

        public static Information.BasicInfo.Position Position = new Information.BasicInfo.Position();
        public static Information.BasicInfo.Movement Movement = new Information.BasicInfo.Movement();
        public static Information.BasicInfo.State State = new Information.BasicInfo.State();

        public static Dictionary<uint, Information.Objects.Character> NearbyCharacters = new Dictionary<uint, Information.Objects.Character>();
        public static Dictionary<uint, Information.Objects.Base> NearbyNPCs = new Dictionary<uint, Information.Objects.Base>();
        public static Dictionary<uint, Information.Objects.Mob> NearbyMobs = new Dictionary<uint, Information.Objects.Mob>();
        public static Dictionary<uint, Information.Objects.Item> NearbyItems = new Dictionary<uint, Information.Objects.Item>();
        public static Dictionary<uint, Information.Objects.BuffArea> NearbyBuffAreas = new Dictionary<uint, Information.Objects.BuffArea>();
        public static Dictionary<uint, Information.Objects.COS> NearbyCOSs = new Dictionary<uint, Information.Objects.COS>();
        public static Dictionary<uint, Information.Objects.Structure> NearbyStructures = new Dictionary<uint, Information.Objects.Structure>();

        public static Dictionary<ChatType, List<Information.Chat.ChatMessage>> Chat = new Dictionary<ChatType, List<Information.Chat.ChatMessage>>();
        public static Information.Stalls.Stall CurrentStall = new Information.Stalls.Stall();
        public static bool CharacterInStall = false;
        public static uint SelectedUniqueID;

        public static void RefreshClient()
        {
            Info = new Info();
            Party = new Information.Party.Party();

            InventoryItems = new Dictionary<int, InventoryItem>();
            StorageItems = new Dictionary<int, InventoryItem>();

            SoldItems = new List<InventoryItem>();
            Quests = new List<Quest>();
            Masteries = new List<Mastery>();
            Skills = new List<Media.DataInfo.Skill>();

            Position = new Information.BasicInfo.Position();
            Movement = new Information.BasicInfo.Movement();
            State = new Information.BasicInfo.State();

            NearbyCharacters = new Dictionary<uint, Information.Objects.Character>();
            NearbyNPCs = new Dictionary<uint, Information.Objects.Base>();
            NearbyMobs = new Dictionary<uint, Information.Objects.Mob>();
            NearbyItems = new Dictionary<uint, Information.Objects.Item>();
            NearbyBuffAreas = new Dictionary<uint, Information.Objects.BuffArea>();
            NearbyCOSs = new Dictionary<uint, Information.Objects.COS>();
            NearbyStructures = new Dictionary<uint, Information.Objects.Structure>();

            Chat = new Dictionary<ChatType, List<Information.Chat.ChatMessage>>();
            CurrentStall = new Information.Stalls.Stall();
            CharacterInStall = false;
        }
    }
}
