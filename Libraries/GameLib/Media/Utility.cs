﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using SilkroadInformationAPI.Media.DataInfo;

namespace SilkroadInformationAPI.Media
{
    class Utility
    {
        public static ModelType GetModelType(DataInfo.MediaModel obj)
        {
            ModelType type;
            string seq = obj.Classes.A.ToString() +
                obj.Classes.B.ToString() +
                obj.Classes.C.ToString() +
                obj.Classes.D.ToString() +
                obj.Classes.E.ToString() +
                obj.Classes.F.ToString();

            if (seq == "011234")
                type = ModelType.PickupPet;
            else if (seq == "011233")
                type = ModelType.AttackPet;
            else if (seq == "011231")
                type = ModelType.NormalPet;
            else if (seq == "011232")
                type = ModelType.JobPet;
            else if (seq == "111232")
                type = ModelType.JobSilkPet;
            else if (seq == "011220")
                type = ModelType.NPC;
            else if (seq.StartsWith("01121"))
                type = ModelType.Mob;
            else if (seq == "011100")
                type = ModelType.Character;
            else if (seq == "003350")
                type = ModelType.Gold;
            else if (seq == "003390")
                type = ModelType.QuestItem;
            else if (seq.StartsWith("0033111") || seq.StartsWith("0033112"))
                type = ModelType.StonesType;
            else if (seq.StartsWith("0031"))
                type = ModelType.PlusItem;
            else if (seq.StartsWith("0033"))
                type = ModelType.CountableItem;
            else if (seq.StartsWith("1033"))
                type = ModelType.MallCountItem;
            else if (seq.StartsWith("1031"))
                type = ModelType.MallPlusItem;
            else if (seq == "011212")
                type = ModelType.NPCMob;
            else if (seq.StartsWith("00411"))
                type = ModelType.Portal1;
            else if (seq == "004120")
                type = ModelType.Portal2;
            else if (seq.StartsWith("01125"))
                type = ModelType.Structure;
            else
            {
                type = ModelType.Unknown;
            }

            return type;
        }
        public static ItemType GetItemType(Item item)
        {
            ItemType type;
            string seq = item.Classes.B.ToString() + " " +
                item.Classes.C.ToString() + " " +
                item.Classes.D.ToString() + " " +
                item.Classes.E.ToString() + " " +
                item.Classes.F.ToString();
            
            string seq2 = item.Classes.B.ToString() + " " +
                item.Classes.C.ToString() + " " +
                item.Classes.D.ToString() + " " +
                item.Classes.E.ToString() + " " +
                item.Classes.F.ToString();
            
            if (seq == "0 3 2 1 2")
                type = ItemType.PickupPet;
            else if (seq == "0 3 2 1 1")
                type = ItemType.AttackPet;
            else if (seq == "0 3 3 3 2") //This could be Trade or normal pet.
                type = ItemType.RidePet;
            else if (seq.StartsWith("0 3 2 2"))
                type = ItemType.MonsterMask;
            else if (seq.StartsWith("0 3 2 3"))
                type = ItemType.MagicCube;
            else if (seq.StartsWith("0 3 3 14")) //Not sure whether to add another class? {1, 2}
                type = ItemType.ItemExchangeCoupon;
            else if (seq.StartsWith("0 3 3 11"))
                type = ItemType.Stones;
            else if (seq == "0 3 3 1 1")
                type = ItemType.HpPotion;
            else if (seq == "0 3 3 1 2")
                type = ItemType.MpPotion;
            else if (seq == "0 3 3 1 3")
                type = ItemType.VigorPotion;
            else if (seq == "0 3 3 1 4")
                type = ItemType.PetRecoveryKit;
            else if (seq == "0 3 3 1 6")
                type = ItemType.PetRevival;
            else if (seq == "0 3 3 1 9")
                type = ItemType.PetHGP;
            else if (seq == "0 3 3 4 1")
                type = ItemType.Arrows;
            else if (seq == "0 3 3 4 2")
                type = ItemType.Bolts;
            else if (seq == "0 3 3 2 6")
                type = ItemType.UniversalPills;
            else if (seq == "0 3 3 2 7")
                type = ItemType.PetASRP;
            else if (seq == "0 3 3 2 1")
                type = ItemType.PurificationPills;
            else if (seq == "0 3 3 3 1" && item.MediaName.Contains("THIEF") == false)
                type = ItemType.ReturnScroll;
            else
                type = ItemType.None;

            return type;
        }

        public static SubItemType GetSubItemType(Item item)
        {
            SubItemType type;
            string seq = item.Classes.B.ToString() + " " +
                item.Classes.C.ToString() + " " +
                item.Classes.D.ToString() + " " +
                item.Classes.F.ToString();

            string seq2 = item.Classes.B.ToString() + " " +
                item.Classes.C.ToString() + " " +
                item.Classes.D.ToString() + " " +
                item.Classes.E.ToString() + " " +
                item.Classes.F.ToString();


            if (seq2 == "0 3 1 5 2")
                type = SubItemType.ChinaNecklace;
            else if (seq2 == "0 3 1 12 2")
                type = SubItemType.EuropeNecklace;
            else if (seq2 == "0 3 1 5 1")
                type = SubItemType.ChinaEarring;
            else if (seq2 == "0 3 1 12 1")
                type = SubItemType.EuropeEarring;
            else if (seq2 == "0 3 1 5 3")
                type = SubItemType.ChinaRing;
            else if (seq2 == "0 3 1 12 3")
                type = SubItemType.EuropeRing;

            else if (seq2 == "0 3 1 4 1")
                type = SubItemType.ChinaShield;
            else if (seq2 == "0 3 1 6 2")
                type = SubItemType.Sword;
            else if (seq2 == "0 3 1 6 3")
                type = SubItemType.Blade;
            else if (seq2 == "0 3 1 6 4")
                type = SubItemType.Spear;
            else if (seq2 == "0 3 1 6 5")
                type = SubItemType.Glaive;
            else if (seq2 == "0 3 1 6 6")
                type = SubItemType.Bow;
            else if (seq2 == "0 3 1 4 2")
                type = SubItemType.EuroupeShield;
            else if (seq2 == "0 3 1 6 7")
                type = SubItemType.OneHand;
            else if (seq2 == "0 3 1 6 8")
                type = SubItemType.TwoHanded;
            else if (seq2 == "0 3 1 6 9")
                type = SubItemType.Axe;
            else if (seq2 == "0 3 1 6 10")
                type = SubItemType.DarkStaff;
            else if (seq2 == "0 3 1 6 11")
                type = SubItemType.Staff;
            else if (seq2 == "0 3 1 6 12")
                type = SubItemType.Crossbow;
            else if (seq2 == "0 3 1 6 13")
                type = SubItemType.Dagger;
            else if (seq2 == "0 3 1 6 14")
                type = SubItemType.Bard;
            else if (seq2 == "0 3 1 6 15")
                type = SubItemType.LightStaff;
            
            // china clothes
            else if (seq == "0 3 1 1")
                type = SubItemType.Head;
            else if (seq == "0 3 1 2")
                type = SubItemType.Shoulders;
            else if (seq == "0 3 1 3")
                type = SubItemType.Chest;
            else if (seq == "0 3 1 4")
                type = SubItemType.Legs;
            else if (seq == "0 3 1 5")
                type = SubItemType.Hands;
            else if (seq == "0 3 1 6")
                type = SubItemType.Foot;

            // default
            else
                type = SubItemType.None;

            return type;
        }

        public static byte GetItemDegree(byte lvl) //TODO: Add degree 12, 13, 14, 15
        {
            byte Degree = 0;

            if (lvl >= 101)
                Degree = 11;
            else if (lvl >= 90)
                Degree = 10;
            else if (lvl >= 76)
                Degree = 9;
            else if (lvl >= 64)
                Degree = 8;
            else if (lvl >= 52)
                Degree = 7;
            else if (lvl >= 42)
                Degree = 6;
            else if (lvl >= 32)
                Degree = 5;
            else if (lvl >= 24)
                Degree = 4;
            else if (lvl >= 16)
                Degree = 3;
            else if (lvl >= 8)
                Degree = 2;
            else if (lvl >= 1)
                Degree = 1;

            return Degree;
        }

        public static void CharacterRace(uint ObjID)
        {
            uint[] ChinaObjects = { 1907, 1908, 1909, 1910, 1911, 1912, 1913, 1914, 1915, 1916, 1917, 1918, 1919, 1920, 1921, 1922, 1923, 1924, 1925, 1926, 1927, 1928, 1929, 1930, 1931, 1932 };
            uint[] EuropeObjects = { 14875, 148576, 14877, 14878, 14879, 14880, 14881, 14882, 14883, 14884, 14885, 14886, 14887, 14888, 14889, 14890, 14891, 14892, 14893, 14894, 14895, 14896, 14897, 14898, 14899, 14900};

            if (ChinaObjects.Contains(ObjID)) // if it had any bug just check if it begins with 19 so it chines 148 it euro
                Client.Client.Info.Race = SilkroadInformationAPI.CharacterRace.Chinese;
            else if (EuropeObjects.Contains(ObjID))
                Client.Client.Info.Race = SilkroadInformationAPI.CharacterRace.Europe;
        }

        public static CharacterRace GetCharacterRace(uint ObjID)
        {
            uint[] ChinaObjects = { 1907, 1908, 1909, 1910, 1911, 1912, 1913, 1914, 1915, 1916, 1917, 1918, 1919, 1920, 1921, 1922, 1923, 1924, 1925, 1926, 1927, 1928, 1929, 1930, 1931, 1932 };
            uint[] EuropeObjects = { 14875, 148576, 14877, 14878, 14879, 14880, 14881, 14882, 14883, 14884, 14885, 14886, 14887, 14888, 14889, 14890, 14891, 14892, 14893, 14894, 14895, 14896, 14897, 14898, 14899, 14900};

            if (ChinaObjects.Contains(ObjID)) // if it had any bug just check if it begins with 19 so it chines 148 it euro
                return SilkroadInformationAPI.CharacterRace.Chinese;
            else
                return SilkroadInformationAPI.CharacterRace.Europe;
        }

        public static string TranslateWhiteStats(string attribute, int value)
        {
            string whiteStatement = null;
            switch (attribute)
            {
                case ("DURABILITY"):
                    whiteStatement = $"Durabillity {value}/{value}";
                    break;
                default:
                    whiteStatement = $"{attribute} (+{ToPercentage(value)}%)";
                    break;
            }
            return whiteStatement;
        }


        // use this later for variance
        private static string ToPercentage(int stat)
        {
            return String.Format("{0}", stat * 100 / 31);
        }

        public static string TranslateBlueStats(ItemBlues attribute, int value)
        {
            string blueStatement = null;
            switch (attribute)
            {
                case (ItemBlues.MATTR_STR):
                    blueStatement = $"Str {value} Increase";
                    break;
                case (ItemBlues.MATTR_INT):
                    blueStatement = $"Int {value} Increase";
                    break;
                case (ItemBlues.MATTR_HP):
                    blueStatement = $"HP {value} Increase";
                    break;
                case (ItemBlues.MATTR_MP):
                    blueStatement = $"MP {value} Increase";
                    break;
                default:
                    blueStatement = $"{attribute} {value}";
                    break;
            }
            return blueStatement;
        }

        public static StringBuilder ItemDescription(string desc, int perline = 7)
        {
            // check if word is less than per line maybe there is an error here!
            if (string.IsNullOrEmpty(desc))
                return default;

            StringBuilder finalDesc = new StringBuilder();
            finalDesc.AppendLine();

            int x = 1;
            foreach (string word in GetWords(desc))
            {
                finalDesc.Append($"{word} ");
                if (x++ % perline == 0)
                    finalDesc.AppendLine();
            }

            return finalDesc;
        }

        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }

        static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }

        #region Sro Enc Skills Decrypter By superkhung@vnsecurity.net

        static int[] Hash_Table_1 =
        {
            0x07, 0x83, 0xBC, 0xEE, 0x4B, 0x79, 0x19, 0xB6, 0x2A, 0x53, 0x4F, 0x3A, 0xCF, 0x71, 0xE5, 0x3C,
            0x2D, 0x18, 0x14, 0xCB, 0xB6, 0xBC, 0xAA, 0x9A, 0x31, 0x42, 0x3A, 0x13, 0x42, 0xC9, 0x63, 0xFC,
            0x54, 0x1D, 0xF2, 0xC1, 0x8A, 0xDD, 0x1C, 0xB3, 0x52, 0xEA, 0x9B, 0xD7, 0xC4, 0xBA, 0xF8, 0x12,
            0x74, 0x92, 0x30, 0xC9, 0xD6, 0x56, 0x15, 0x52, 0x53, 0x60, 0x11, 0x33, 0xC5, 0x9D, 0x30, 0x9A,
            0xE5, 0xD2, 0x93, 0x99, 0xEB, 0xCF, 0xAA, 0x79, 0xE3, 0x78, 0x6A, 0xB9, 0x02, 0xE0, 0xCE, 0x8E,
            0xF3, 0x63, 0x5A, 0x73, 0x74, 0xF3, 0x72, 0xAA, 0x2C, 0x9F, 0xBB, 0x33, 0x91, 0xDE, 0x5F, 0x91,
            0x66, 0x48, 0xD1, 0x7A, 0xFD, 0x3F, 0x91, 0x3E, 0x5D, 0x22, 0xEC, 0xEF, 0x7C, 0xA5, 0x43, 0xC0,
            0x1D, 0x4F, 0x60, 0x7F, 0x0B, 0x4A, 0x4B, 0x2A, 0x43, 0x06, 0x46, 0x14, 0x45, 0xD0, 0xC5, 0x83,
            0x92, 0xE4, 0x16, 0xD0, 0xA3, 0xA1, 0x13, 0xDA, 0xD1, 0x51, 0x07, 0xEB, 0x7D, 0xCE, 0xA5, 0xDB,
            0x78, 0xE0, 0xC1, 0x0B, 0xE5, 0x8E, 0x1C, 0x7C, 0xB4, 0xDF, 0xED, 0xB8, 0x53, 0xBA, 0x2C, 0xB5,
            0xBB, 0x56, 0xFB, 0x68, 0x95, 0x6E, 0x65, 0x00, 0x60, 0xBA, 0xE3, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x9C, 0xB5, 0xD5, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2E, 0x3F, 0x41, 0x56,
            0x43, 0x45, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x64, 0xBB, 0xE3, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        };

        static int[] Hash_Table_2 =
        {
            0x0D, 0x05, 0x90, 0x41, 0xF9, 0xD0, 0x65, 0xBF, 0xF9, 0x0B, 0x15, 0x93, 0x80, 0xFB, 0x01, 0x02, 
            0xB6, 0x08, 0xC4, 0x3C, 0xC1, 0x49, 0x94, 0x4D, 0xCE, 0x1D, 0xFD, 0x69, 0xEA, 0x19, 0xC9, 0x57, 
            0x9C, 0x4D, 0x84, 0x62, 0xE3, 0x67, 0xF9, 0x87, 0xF4, 0xF9, 0x93, 0xDA, 0xE5, 0x15, 0xF1, 0x4C, 
            0xA4, 0xEC, 0xBC, 0xCF, 0xDD, 0xB3, 0x6F, 0x04, 0x3D, 0x70, 0x1C, 0x74, 0x21, 0x6B, 0x00, 0x71, 
            0x31, 0x7F, 0x54, 0xB3, 0x72, 0x6C, 0xAA, 0x42, 0xC1, 0x78, 0x61, 0x3E, 0xD5, 0xF2, 0xE1, 0x27, 
            0x36, 0x71, 0x3A, 0x25, 0x36, 0x57, 0xD1, 0xF8, 0x70, 0x86, 0xBD, 0x0E, 0x58, 0xB3, 0x76, 0x6D, 
            0xC3, 0x50, 0xF6, 0x6C, 0xA0, 0x10, 0x06, 0x64, 0xA2, 0xD6, 0x2C, 0xD4, 0x27, 0x30, 0xA5, 0x36, 
            0x1C, 0x1E, 0x3E, 0x58, 0x9D, 0x59, 0x76, 0x9D, 0xA7, 0x42, 0x5A, 0xF0, 0x00, 0xBC, 0x69, 0x31, 
            0x40, 0x1E, 0xFA, 0x09, 0x1D, 0xE7, 0xEE, 0xE4, 0x54, 0x89, 0x36, 0x7C, 0x67, 0xC8, 0x65, 0x22, 
            0x7E, 0xA3, 0x60, 0x44, 0x1E, 0xBC, 0x68, 0x6F, 0x15, 0x2A, 0xFD, 0x9D, 0x3F, 0x36, 0x6B, 0x28, 
            0x06, 0x67, 0xFE, 0xC6, 0x49, 0x6B, 0x9B, 0x3F, 0x80, 0x2A, 0xD2, 0xD4, 0xD3, 0x20, 0x1B, 0x96, 
            0xF4, 0xD2, 0xCA, 0x8C, 0x74, 0xEE, 0x0B, 0x6A, 0xE1, 0xE9, 0xC6, 0xD2, 0x6E, 0x33, 0x63, 0xC0, 
            0xE9, 0xD0, 0x37, 0xA9, 0x3C, 0xF7, 0x18, 0xF2, 0x4A, 0x74, 0xEC, 0x41, 0x61, 0x7A, 0x19, 0x47, 
            0x8F, 0xA0, 0xBB, 0x94, 0x8F, 0x3D, 0x11, 0x11, 0x26, 0xCF, 0x69, 0x18, 0x1B, 0x2C, 0x87, 0x6D, 
            0xB3, 0x22, 0x6C, 0x78, 0x41, 0xCC, 0xC2, 0x84, 0xC5, 0xCB, 0x01, 0x6A, 0x37, 0x00, 0x01, 0x65, 
            0x4F, 0xA7, 0x85, 0x85, 0x15, 0x59, 0x05, 0x67, 0xF2, 0x4F, 0xAB, 0xB7, 0x88, 0xFA, 0x69, 0x24, 
            0x9E, 0xC6, 0x7B, 0x3F, 0xD5, 0x0E, 0x4D, 0x7B, 0xFB, 0xB1, 0x21, 0x3C, 0xB0, 0xC0, 0xCB, 0x2C, 
            0xAA, 0x26, 0x8D, 0xCC, 0xDD, 0xDA, 0xC1, 0xF8, 0xCA, 0x7F, 0x6A, 0x3F, 0x2A, 0x61, 0xE7, 0x60, 
            0x5C, 0xCE, 0xD3, 0x4C, 0xAC, 0x45, 0x40, 0x62, 0xEA, 0x51, 0xF1, 0x66, 0x5D, 0x2C, 0x45, 0xD6, 
            0x8B, 0x7D, 0xCE, 0x9C, 0xF5, 0xBB, 0xF7, 0x52, 0x24, 0x1A, 0x13, 0x02, 0x2B, 0x00, 0xBB, 0xA1, 
            0x8F, 0x6E, 0x7A, 0x33, 0xAD, 0x5F, 0xF4, 0x4A, 0x82, 0x76, 0xAB, 0xDE, 0x80, 0x98, 0x8B, 0x26, 
            0x4F, 0x33, 0xD8, 0x68, 0x1E, 0xD9, 0xAE, 0x06, 0x6B, 0x7E, 0xA9, 0x95, 0x67, 0x60, 0xEB, 0xE8, 
            0xD0, 0x7D, 0x07, 0x4B, 0xF1, 0xAA, 0x9A, 0xC5, 0x29, 0x93, 0x9D, 0x5C, 0x92, 0x3F, 0x15, 0xDE, 
            0x48, 0xF1, 0xCA, 0xEA, 0xC9, 0x78, 0x3C, 0x28, 0x7E, 0xB0, 0x46, 0xD3, 0x71, 0x6C, 0xD7, 0xBD, 
            0x2C, 0xF7, 0x25, 0x2F, 0xC7, 0xDD, 0xB4, 0x6D, 0x35, 0xBB, 0xA7, 0xDA, 0x3E, 0x3D, 0xA7, 0xCA, 
            0xBD, 0x87, 0xDD, 0x9F, 0x22, 0x3D, 0x50, 0xD2, 0x30, 0xD5, 0x14, 0x5B, 0x8F, 0xF4, 0xAF, 0xAA, 
            0xA0, 0xFC, 0x17, 0x3D, 0x33, 0x10, 0x99, 0xDC, 0x76, 0xA9, 0x40, 0x1B, 0x64, 0x14, 0xDF, 0x35, 
            0x68, 0x66, 0x5B, 0x49, 0x05, 0x33, 0x68, 0x26, 0xC8, 0xBA, 0xD1, 0x8D, 0x39, 0x2B, 0xFB, 0x3E, 
            0x24, 0x52, 0x2F, 0x9A, 0x69, 0xBC, 0xF2, 0xB2, 0xAC, 0xB8, 0xEF, 0xA1, 0x17, 0x29, 0x2D, 0xEE, 
            0xF5, 0x23, 0x21, 0xEC, 0x81, 0xC7, 0x5B, 0xC0, 0x82, 0xCC, 0xD2, 0x91, 0x9D, 0x29, 0x93, 0x0C, 
            0x9D, 0x5D, 0x57, 0xAD, 0xD4, 0xC6, 0x40, 0x93, 0x8D, 0xE9, 0xD3, 0x35, 0x9D, 0xC6, 0xD3, 0x00, 
        };

        public static string DecryptSkills(byte[] buffer)
        {
            int encrypted = 0;
            uint key = 0x8c1f;
            byte buff;

            if (buffer[0] == 0xe2 && buffer[1] == 0xb0)
                encrypted = 1;

            for(uint i=0;i<buffer.Length;i++)
            {
                buff = (byte)(Hash_Table_1[key % 0xa7] - Hash_Table_2[key % 0x1ef]);
                ++key;

                if (encrypted == 1)
                    buffer[i] += buff;
                else
                    buffer[i] -= buff;

            }

            return Encoding.Unicode.GetString(buffer);
        }
        #endregion
    }
}
