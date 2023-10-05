using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using SilkroadInformationAPI.Media.DataInfo;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System;
using SilkroadInformationAPI.Client.Information.Objects;
using Item = SilkroadInformationAPI.Media.DataInfo.Item;
using System.Windows;
using System.Windows.Media;

namespace SRO_INGAME.View.ViewCharacter
{
    /// <TODO>
    ///     - Disable this feature in battle royale
    ///     - Add slot 8 which is related to cape or job suit
    /// </TODO>
    public partial class ViewCharacterFrame : Page
    {
        uint CharUniqueID;
        Character CharacterRow = null;
        CharacterRace characterRace;
        bool isEquipActive = true;

        public ViewCharacterFrame()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GearsFrameBG.Source = Utility.PK2GetImage("equip_frame.ddj");
            SwitchGearsButtonBG.ImageSource = Utility.PK2GetImage("chr_stat_off.ddj");
            MasteryOne.Source = Utility.PK2GetImage("mastery_bow.ddj");
            MasteryTwo.Source = Utility.PK2GetImage("mastery_cold.ddj");

            // Data row test
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Tiger Girl", TotalKills = "2 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Demon Shitan", TotalKills = "10 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Ictyra", TotalKills = "4 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Tiger Girl", TotalKills = "6 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Tiger Girl", TotalKills = "2 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Demon Shitan", TotalKills = "10 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Ictyra", TotalKills = "4 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Tiger Girl", TotalKills = "6 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Demon Shitan", TotalKills = "10 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Ictyra", TotalKills = "4 Kill(s)" });
            GameLogGrid.Items.Add(new DataRow { Image = Utility.PK2GetImage("char_ch_man1.ddj"), Unique = "Tiger Girl", TotalKills = "6 Kill(s)" });
        }

        // Data row test
        class DataRow
        {
            public ImageSource Image { get; set; }
            public string Unique { get; set; }
            public string TotalKills { get; set; }
        }

        public void LoadCharacter(uint UniqueID)
        {
            CharUniqueID = UniqueID;

            Dictionary<string, Image[]> ItemFrames = new Dictionary<string, Image[]>() {
                { "Weapon", new Image[] {weaponFrame,  weaponFrameSox} },
                { "Shield", new Image[] {ShieldFrame,  ShieldFrameSox} },
                { "Head", new Image[] { HeadFrame, HeadFrameSox } },
                { "Shoulders", new Image[] { ShouldersFrame, ShouldersFrameSox } },
                { "Chest", new Image[] { ChestFrame, ChestFrameSox } },
                { "Hands", new Image[] { HandsFrame, HandsFrameSox } },
                { "Legs", new Image[] { LegsFrame, LegsFrameSox } },
                { "Foot", new Image[] { BootsFrame, BootsFrameSox } },
            };

            // click on the equipment button

            try
            {
                CharacterRow = Client.NearbyCharacters[Client.SelectedUniqueID];
                if (Client.NearbyCharacters.ContainsKey(Client.SelectedUniqueID) && Client.Info.UniqueID != UniqueID) // and charname != opened charname
                {
                    // load data
                    CharacterLabel.Content = CharacterRow.Name;
                    LevelLabel.Content = "Job Level " + CharacterRow.JobLevel.ToString();
                    // Add Char Level Also
                    characterRace = SilkroadInformationAPI.Media.Utility.GetCharacterRace(Client.NearbyCharacters[Client.SelectedUniqueID].ModelID);

                    if(CharacterRow.JobType == JOB_Type.Thief)
                        CharacterJobIcon.Source = Utility.PK2GetImage("com_job_thief.ddj");
                    else if (CharacterRow.JobType == JOB_Type.Trader)
                        CharacterJobIcon.Source = Utility.PK2GetImage("com_job_merchant.ddj");
                    else if (CharacterRow.JobType == JOB_Type.Hunter)
                        CharacterJobIcon.Source = Utility.PK2GetImage("com_job_hunter.ddj");
                    else
                        CharacterJobIcon.Source = null;


                    if (characterRace == CharacterRace.Chinese)
                        CharacterRaceIcon.Source = Utility.PK2GetImage("com_kindred_china.ddj");
                    if(characterRace == CharacterRace.Europe)
                        CharacterRaceIcon.Source = Utility.PK2GetImage("com_kindred_europe.ddj");

                    // char image
                    CharacterImage.Source = Utility.PK2GetImage("char_ch_man1.ddj");

                    // Load items
                    foreach (var item in CharacterRow.Inventory)
                    {
                        Item mediaItem = SilkroadInformationAPI.Media.Data.MediaItems[item.ModelID];
                        Console.WriteLine(mediaItem.TranslationName);
                        if (new SubItemType[] { SubItemType.EuroupeShield, SubItemType.ChinaShield }.Contains( mediaItem.SubType))
                        {
                            Image[] Frames = ItemFrames.FirstOrDefault(i => i.Key == "Shield").Value;
                            Frames[0].Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            if (mediaItem.MediaName.Contains("RARE"))
                                Frames[1].Visibility = Visibility.Visible;
                            else
                                Frames[1].Visibility = Visibility.Hidden;

                            // data
                            SetItemToolTip(mediaItem, item.PlusValue, Frames[0]);
                        }
                        else if (new string[] { "_AA_", "_HA_", "_SA_", "_LA_", "_FA_", "_BA_" }.Any(mediaItem.MediaName.Contains)) // check by contaitning _AA_ _HA_ _SA_ _FA_ BA_ _LA_ //new SubItemType[] { SubItemType.Head, SubItemType.Shoulders, SubItemType.Chest, SubItemType.Hands, SubItemType.Foot, SubItemType.Legs }.Contains(mediaItem.SubType)
                        {
                            Image[] Frames = ItemFrames.FirstOrDefault(i => i.Key == mediaItem.SubType.ToString()).Value;
                            Frames[0].Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            if (mediaItem.MediaName.Contains("RARE") || (mediaItem.MediaName.Contains("ROC") && mediaItem.MediaName.Contains("SET")))
                                Frames[1].Visibility = Visibility.Visible;
                            else
                                Frames[1].Visibility = Visibility.Hidden;

                            // data
                            SetItemToolTip(mediaItem, item.PlusValue, Frames[0]);
                        }
                        else if (new string[] { "_HAT" }.Any(mediaItem.MediaName.Contains))
                        {
                            AvatarHeadFrame.Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            SetItemToolTip(mediaItem, item.PlusValue, AvatarHeadFrame);
                        }
                        else if (new string[] { "_FLAG", "_PLAG", "FLAG" }.Any(mediaItem.MediaName.Contains))
                        {
                            AvatarFlagFrame.Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            SetItemToolTip(mediaItem, item.PlusValue, AvatarFlagFrame);
                        }
                        else if (new string[] { "_ATTACH" }.Any(mediaItem.MediaName.Contains))
                        {
                            AvatarAttachFrame.Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            SetItemToolTip(mediaItem, item.PlusValue, AvatarAttachFrame);
                        }
                        else if (new string[] { "_NASRUN" }.Any(mediaItem.MediaName.Contains))
                        {
                            AvatarDevilFrame.Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            SetItemToolTip(mediaItem, item.PlusValue, AvatarDevilFrame);
                        }
                        else if (new string[] { "_DRESS", "_AVATAR" }.Any(mediaItem.MediaName.Contains))
                        {
                            AvatarDressFrame.Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            SetItemToolTip(mediaItem, item.PlusValue, AvatarDressFrame);
                        }
                        else if (mediaItem.SubType != SubItemType.None)
                        {
                            Image[] Frames = ItemFrames.FirstOrDefault(i => i.Key == "Weapon").Value;
                            Frames[0].Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                            if (mediaItem.MediaName.Contains("RARE") || (mediaItem.MediaName.Contains("ROC") && mediaItem.MediaName.Contains("SET")))
                                Frames[1].Visibility = Visibility.Visible;
                            else
                                Frames[1].Visibility = Visibility.Hidden;

                            // data
                            SetItemToolTip(mediaItem, item.PlusValue, Frames[0]);
                        }
                    }
                }
            }
            catch { }
        }

        private void SetItemToolTip(Item item, uint plusValue, Image image)
        {
            // Item info
            System.Text.StringBuilder itemInfo = new System.Text.StringBuilder();
            if (plusValue > 0)
                itemInfo.AppendLine($"{ item.TranslationName} (+{plusValue})");
            else
                itemInfo.AppendLine($"{ item.TranslationName}");

            itemInfo.AppendLine();

            // add this to utillity
            if (item.MediaName.Contains("SET_A_RARE") && item.Degree == 11)
                itemInfo.AppendLine($"Seal Of Nova");
            else if (item.MediaName.Contains("SET_B_RARE") && item.Degree == 11)
                itemInfo.AppendLine($"Seal Of Nova");
            else if (item.MediaName.Contains("A_RARE") && item.Degree == 11)
                itemInfo.AppendLine($"Seal Of Nova");
            else if (item.MediaName.Contains("C_RARE"))
                itemInfo.AppendLine($"Seal Of Sun");
            else if (item.MediaName.Contains("B_RARE"))
                itemInfo.AppendLine($"Seal Of Moon");
            else if (item.MediaName.Contains("A_RARE"))
                itemInfo.AppendLine($"Seal Of Star");

            if (item.MediaName.Contains("SET_B") && item.Type == ItemType.Weapon && item.Degree == 11)
                itemInfo.AppendLine($"Fight");
            else if (item.MediaName.Contains("SET_A") && item.Type == ItemType.Weapon && item.Degree == 11)
                itemInfo.AppendLine($"Power");

            if (item.MediaName.Contains("SET_B") && item.Type == ItemType.Shield && item.Degree == 11)
                itemInfo.AppendLine($"Guard");
            else if (item.MediaName.Contains("SET_A") && item.Type == ItemType.Shield && item.Degree == 11)
                itemInfo.AppendLine($"Shield");

            if (item.MediaName.Contains("RARE"))
                itemInfo.AppendLine();

            itemInfo.AppendLine($"Sort of item: {item.SubType}");
            if(item.Type == ItemType.Protector)
                itemInfo.AppendLine($"Mount part: {item.SubType}");
            itemInfo.AppendLine($"Degree: {item.Degree} degrees");

            itemInfo.AppendLine();
            itemInfo.AppendLine($"Required Level: {item.Level}");
            itemInfo.AppendLine($"{characterRace}");
            itemInfo.AppendLine();
            itemInfo.AppendLine($"Max. no. of magic options: 9 Unit");
            itemInfo.Append($"Able to use Advanced elixir.");

            image.ToolTip = itemInfo.ToString();
        }

        private void SwitchGears_OnClick(object sender, RoutedEventArgs args)
        {
            if (isEquipActive)
            {
                GearsFrameBG.Source = Utility.PK2GetImage("avatar_frame.ddj");
                SwitchGearsButton.ToolTip = "View Equipment Gears.";
                GearsFrameLabel.Content = "AVATAR";
                isEquipActive = false;

                // hide Equipment
                EquipmentFrame.Visibility = Visibility.Hidden;

                // show avatar
                AvatarHeadFrame.Visibility = Visibility.Visible;
                AvatarFlagFrame.Visibility = Visibility.Visible;
                AvatarDressFrame.Visibility = Visibility.Visible;
                AvatarAttachFrame.Visibility = Visibility.Visible;
                AvatarDevilFrame.Visibility = Visibility.Visible;

            } 
            else
            {
                GearsFrameBG.Source = Utility.PK2GetImage("equip_frame.ddj");
                SwitchGearsButton.ToolTip = "View Avatas Gears.";
                GearsFrameLabel.Content = "EQUIPMENT";
                isEquipActive = true;

                // show Equipment
                EquipmentFrame.Visibility = Visibility.Visible;

                // hide avatar
                AvatarHeadFrame.Visibility = Visibility.Hidden;
                AvatarFlagFrame.Visibility = Visibility.Hidden;
                AvatarDressFrame.Visibility = Visibility.Hidden;
                AvatarAttachFrame.Visibility = Visibility.Hidden;
                AvatarDevilFrame.Visibility = Visibility.Hidden;
            }
        }
    }
}
