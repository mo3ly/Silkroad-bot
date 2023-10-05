using Contollers.GameBot;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SilkroadInformationAPI.Client.Information;
using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SRO_INGAME.View.GameBot.Skills
{
    /// <summary>
    /// Interaction logic for Gears.xaml
    /// </summary>
    public partial class Gears : Page
    {

        #region Declaration
        ImageSource[] imageSources;
        #endregion

        #region Init
        public Gears()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            LoadGears(WeaponsRootFrame, ItemType.Weapon);//, ItemType.Arrows, ItemType.Bolts
            LoadGears(ShieldRootFrame, ItemType.Shield);
        }
        #endregion

        #region Reload
        public void ReloadWeapons()
        {
            LoadGears(WeaponsRootFrame, ItemType.Weapon);//, ItemType.Arrows, ItemType.Bolts
        }

        public void ReloadShields()
        {
            LoadGears(ShieldRootFrame, ItemType.Shield);
        }
        #endregion

        #region Clear Gears Function
        /// <summary>
        /// TODO: add check for wearing shield with other items!
        /// Clear selected gears slot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearGear(object sender, MouseEventArgs e)
        {
            Image slot = (sender as Image);
            if (slot.Name == "weaponAttackImage")
            {
                // remove bolt or arrow
                if (BotData.Gears["AttackShield"] != null && new ItemType[] { ItemType.Bolts, ItemType.Arrows }.Contains(BotData.Gears["AttackShield"].Type) && new SubItemType[] { SubItemType.Bow, SubItemType.Crossbow }.Contains(BotData.Gears["AttackWeapon"].SubType))
                    shieldAttackImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });

                BotData.Gears["AttackWeapon"] = null;
                slot.Source = null;
                weaponAttackImageSox.Visibility = Visibility.Hidden;
                foreach (Panel weaponSlot in WeaponsRootFrame.Children)
                {
                    if (SRCommon.pSkills.ColorEquals((SolidColorBrush)(weaponSlot as StackPanel).Background, SRCommon.pSkills.attackSlotColor))
                    {
                        (weaponSlot as StackPanel).Background = SRCommon.pSkills.emptySlotColor;
                        (weaponSlot as StackPanel).Opacity = 0.2;
                    }
                }
            }
            else if (slot.Name == "shieldAttackImage")
            {
                BotData.Gears["AttackShield"] = null;
                slot.Source = null;
                shieldAttackImageSox.Visibility = Visibility.Hidden;
                foreach (Panel shieldSlot in ShieldRootFrame.Children)
                {
                    if (SRCommon.pSkills.ColorEquals((SolidColorBrush)(shieldSlot as StackPanel).Background, SRCommon.pSkills.attackSlotColor))
                    {
                        (shieldSlot as StackPanel).Background = SRCommon.pSkills.emptySlotColor;
                        (shieldSlot as StackPanel).Opacity = 0.2;
                    }
                }
            }
            else if (slot.Name == "weaponBuffImage")
            {
                // remove bolt or arrow
                if (BotData.Gears["BuffShield"] != null && new ItemType[] { ItemType.Bolts, ItemType.Arrows }.Contains(BotData.Gears["BuffShield"].Type) && new SubItemType[] { SubItemType.Bow, SubItemType.Crossbow }.Contains(BotData.Gears["BuffWeapon"].SubType))
                    shieldBuffImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });

                BotData.Gears["BuffWeapon"] = null;
                slot.Source = null;
                weaponBuffImageSox.Visibility = Visibility.Hidden;
                foreach (Panel weaponSlot in WeaponsRootFrame.Children)
                {

                    if (SRCommon.pSkills.ColorEquals((SolidColorBrush)(weaponSlot as StackPanel).Background, SRCommon.pSkills.buffSlotColor))
                    {
                        (weaponSlot as StackPanel).Background = SRCommon.pSkills.emptySlotColor;
                        (weaponSlot as StackPanel).Opacity = 0.2;
                    }
                }
            }
            else if (slot.Name == "shieldBuffImage")
            {
                BotData.Gears["BuffShield"] = null;
                slot.Source = null;
                shieldBuffImageSox.Visibility = Visibility.Hidden;
                foreach (Panel shieldSlot in ShieldRootFrame.Children)
                {
                    if (SRCommon.pSkills.ColorEquals((SolidColorBrush)(shieldSlot as StackPanel).Background, SRCommon.pSkills.buffSlotColor))
                    {
                        (shieldSlot as StackPanel).Background = SRCommon.pSkills.emptySlotColor;
                        (shieldSlot as StackPanel).Opacity = 0.2;
                    }
                }
            }
        }
        #endregion

        #region Gears OnClick Function
        /// <summary>
        /// Weapon attack gear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeaponAttackClicked(object sender, MouseEventArgs e) // cannot wear spear with shield and so on
        {
            try
            {
                Image weapon = (sender as Image);
                InventoryItem item = (InventoryItem)weapon.Tag;
                if (item.Type != ItemType.Arrows && item.Type != ItemType.Bolts) // exclude arrow and bolts
                {
                    weaponAttackImage.Source = weapon.Source;
                    weaponAttackImage.ToolTip = weapon.ToolTip;
                    if ((weapon.Tag as InventoryItem).MediaName.Contains("RARE"))
                        weaponAttackImageSox.Visibility = Visibility.Visible;
                    else
                        weaponAttackImageSox.Visibility = Visibility.Hidden;

                    // restore all the slots
                    foreach (Panel weaponSlot in WeaponsRootFrame.Children)
                        if (!SRCommon.pSkills.ColorEquals((SolidColorBrush)(weaponSlot as StackPanel).Background, SRCommon.pSkills.buffSlotColor)) // to keep both buff and attack active
                            SRCommon.pSkills.DisableSlot((StackPanel)weaponSlot);

                    // activate the new slot
                    SRCommon.pSkills.ActivateSlot((StackPanel)weapon.Parent, SRCommon.pSkills.attackSlotColor);
                    BotData.Gears["AttackWeapon"] = item;

                    // remove shield with non shield items
                    if (BotData.Gears["AttackShield"] != null && !SRCommon.pSkills.requireShield.Contains(BotData.Gears["AttackWeapon"].SubType))
                        shieldAttackImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });

                    // remove bolt or arrow
                    else if (BotData.Gears["AttackShield"] != null && new ItemType[] { ItemType.Bolts, ItemType.Arrows }.Contains(BotData.Gears["AttackShield"].Type) && !new SubItemType[] { SubItemType.Bow, SubItemType.Crossbow }.Contains(BotData.Gears["AttackWeapon"].SubType))
                        shieldAttackImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });


                    // auto wear arrow or bolt
                    if (item.SubType == SubItemType.Bow || item.SubType == SubItemType.Crossbow) // this is not working check later // select arrow or bolt randomly
                    {
                        BotData.Gears["AttackShield"] = null;
                        ItemType itemType = Client.Info.Race == CharacterRace.Chinese ? ItemType.Arrows : ItemType.Bolts;
                        InventoryItem arrowOrBolt = Client.InventoryItems.OrderByDescending(i => i.Value.Stack).Where(i => i.Value.Type == itemType).FirstOrDefault().Value;
                        if (arrowOrBolt != null)
                        {
                            BotData.Gears["AttackShield"] = arrowOrBolt;
                            shieldAttackImage.Source = Utility.PK2GetImageByURL(arrowOrBolt.IconPath);
                            shieldAttackImage.ToolTip = $"{arrowOrBolt.TranslationName}\nSort of item: {arrowOrBolt.Type}\nQuantity({arrowOrBolt.Stack})";
                        }
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// Weapon buff gear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeaponBuffClicked(object sender, MouseEventArgs e)
        {
            try
            {
                Image weapon = (sender as Image);
                InventoryItem item = (InventoryItem)weapon.Tag;
                if (item.Type != ItemType.Arrows && item.Type != ItemType.Bolts) // exclude arrow and bolts
                {
                    weaponBuffImage.Source = weapon.Source;
                    weaponBuffImage.ToolTip = weapon.ToolTip;
                    if ((weapon.Tag as InventoryItem).MediaName.Contains("RARE"))
                        weaponBuffImageSox.Visibility = Visibility.Visible;
                    else
                        weaponBuffImageSox.Visibility = Visibility.Hidden;

                    // restore all the slots
                    foreach (Panel weaponSlot in WeaponsRootFrame.Children)
                        if (!SRCommon.pSkills.ColorEquals((SolidColorBrush)(weaponSlot as StackPanel).Background, SRCommon.pSkills.attackSlotColor)) // to keep both buff and attack active
                            SRCommon.pSkills.DisableSlot((StackPanel)weaponSlot);

                    // activate the new slot
                    SRCommon.pSkills.ActivateSlot((StackPanel)weapon.Parent, SRCommon.pSkills.buffSlotColor);
                    BotData.Gears["BuffWeapon"] = (InventoryItem)weapon.Tag; // BotData.BuffWeaponID = weapon.Tag.ToString();
                }

                // remove shield
                if (BotData.Gears["BuffShield"] != null && !SRCommon.pSkills.requireShield.Contains(BotData.Gears["BuffWeapon"].SubType))
                    shieldBuffImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });


                // remove bolt or arrow
                else if (BotData.Gears["BuffShield"] != null && new ItemType[] { ItemType.Bolts, ItemType.Arrows }.Contains(BotData.Gears["BuffShield"].Type) && !new SubItemType[] { SubItemType.Bow, SubItemType.Crossbow }.Contains(BotData.Gears["BuffWeapon"].SubType))
                    shieldBuffImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });


                // auto wear arrow or bolt
                if (item.SubType == SubItemType.Bow || item.SubType == SubItemType.Crossbow) // this is not working check later // select arrow or bolt randomly
                {
                    BotData.Gears["BuffShield"] = null;
                    ItemType itemType = Client.Info.Race == CharacterRace.Chinese ? ItemType.Arrows : ItemType.Bolts;
                    InventoryItem arrowOrBolt = Client.InventoryItems.OrderByDescending(i => i.Value.Stack).Where(i => i.Value.Type == itemType).FirstOrDefault().Value;
                    if (arrowOrBolt != null)
                    {
                        BotData.Gears["BuffShield"] = arrowOrBolt;
                        shieldBuffImage.Source = Utility.PK2GetImageByURL(arrowOrBolt.IconPath);
                        shieldBuffImage.ToolTip = $"{arrowOrBolt.TranslationName}\nSort of item: {arrowOrBolt.Type}\nQuantity({arrowOrBolt.Stack})";
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Select Attack shield
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShieldAttackClicked(object sender, MouseEventArgs e)
        {
            try
            {
                if (BotData.Gears["AttackWeapon"] != null)
                {
                    if (!SRCommon.pSkills.requireShield.Contains(BotData.Gears["AttackWeapon"].SubType))
                    {
                        weaponAttackImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });
                    }

                }
                Image shield = (sender as Image);
                shieldAttackImage.Source = shield.Source;
                shieldAttackImage.ToolTip = shield.ToolTip;
                if ((shield.Tag as InventoryItem).MediaName.Contains("RARE"))
                    shieldAttackImageSox.Visibility = Visibility.Visible;
                else
                    shieldAttackImageSox.Visibility = Visibility.Hidden;

                // restore all the slots
                foreach (Panel shieldSlot in ShieldRootFrame.Children)
                    if (!SRCommon.pSkills.ColorEquals((SolidColorBrush)(shieldSlot as StackPanel).Background, SRCommon.pSkills.buffSlotColor))
                        SRCommon.pSkills.DisableSlot((StackPanel)shieldSlot);

                // activate the new slot
                SRCommon.pSkills.ActivateSlot((StackPanel)shield.Parent, SRCommon.pSkills.attackSlotColor);
                BotData.Gears["AttackShield"] = (InventoryItem)shield.Tag;
            }
            catch { }
        }

        /// <summary>
        /// Select Buff Shield
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShieldBuffClicked(object sender, MouseEventArgs e)
        {
            try
            {
                if (BotData.Gears["BuffWeapon"] != null)
                {
                    if (!SRCommon.pSkills.requireShield.Contains(BotData.Gears["BuffWeapon"].SubType))
                    {
                        weaponBuffImage.RaiseEvent(new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Right) { RoutedEvent = MouseRightButtonDownEvent });
                    }

                }
                Image shield = (sender as Image);
                shieldBuffImage.Source = shield.Source;
                shieldBuffImage.ToolTip = shield.ToolTip;
                if ((shield.Tag as InventoryItem).MediaName.Contains("RARE"))
                    shieldBuffImageSox.Visibility = Visibility.Visible;
                else
                    shieldBuffImageSox.Visibility = Visibility.Hidden;

                // restore all the slots
                foreach (Panel shieldSlot in ShieldRootFrame.Children)
                    if (!SRCommon.pSkills.ColorEquals((SolidColorBrush)(shieldSlot as StackPanel).Background, SRCommon.pSkills.buffSlotColor))
                        SRCommon.pSkills.DisableSlot((StackPanel)shieldSlot);

                // activate the new slot
                SRCommon.pSkills.ActivateSlot((StackPanel)shield.Parent, SRCommon.pSkills.buffSlotColor);
                BotData.Gears["BuffShield"] = (InventoryItem)shield.Tag;
            }
            catch { }
        }
        #endregion

        #region Load Gears
        /// <summary>
        /// Load Gears on Grid
        /// </summary>
        /// <param name="RootGrid"></param>
        /// <param name="type"></param>
        public void LoadGears(Grid RootGrid, ItemType type) // Click select as attack - double click as buff same for weapon and shield
        {
            try
            {
                string itemRace = Client.Info.Race == CharacterRace.Chinese ? "_CH_" : "_EU_";
                var items = Client.InventoryItems.OrderByDescending(i => i.Value.Level).Where(i => i.Value.Type == type && i.Value.MediaName.Contains(itemRace) && i.Value.Level <= Client.Info.Level).ToList();
                if (items.Count > 0)
                {
                    RootGrid.Children.Clear();
                    //ItemType secondType = Client.Info.Race == CharacterRace.Chinese ? type2 : type3; // for arrow
                    int column = 0;
                    int row = 0;
                    int x = 1;
                    foreach (KeyValuePair<int, InventoryItem> item in items) // add items below char level only
                    {
                        // stack panel
                        var stackPanel = new StackPanel();
                        stackPanel.Margin = new Thickness(5, 5, 5, 5);
                        stackPanel.Height = 38;
                        stackPanel.Width = 38;
                        stackPanel.VerticalAlignment = VerticalAlignment.Top;
                        stackPanel.HorizontalAlignment = HorizontalAlignment.Left;

                        if (type == ItemType.Weapon)
                        {
                            if (BotData.Gears["AttackWeapon"] != null && item.Value.ObjRefID == BotData.Gears["AttackWeapon"].ObjRefID && item.Value.PlusValue == BotData.Gears["AttackWeapon"].PlusValue) // maybe add where type is shield
                                SRCommon.pSkills.ActivateSlot(stackPanel, SRCommon.pSkills.attackSlotColor);
                            else if (BotData.Gears["BuffWeapon"] != null && item.Value.ObjRefID == BotData.Gears["BuffWeapon"].ObjRefID && item.Value.PlusValue == BotData.Gears["BuffWeapon"].PlusValue) // maybe add where type is shield
                                SRCommon.pSkills.ActivateSlot(stackPanel, SRCommon.pSkills.buffSlotColor);
                            else
                                SRCommon.pSkills.DisableSlot(stackPanel);
                        }
                        else if (type == ItemType.Shield)
                        {
                            // add here when used in both add custom color
                            if (BotData.Gears["AttackShield"] != null && item.Value.ObjRefID == BotData.Gears["AttackShield"].ObjRefID && item.Value.PlusValue == BotData.Gears["AttackShield"].PlusValue)
                                SRCommon.pSkills.ActivateSlot(stackPanel, SRCommon.pSkills.attackSlotColor);
                            else if (BotData.Gears["BuffShield"] != null && item.Value.ObjRefID == BotData.Gears["BuffShield"].ObjRefID && item.Value.PlusValue == BotData.Gears["BuffShield"].PlusValue) // maybe add where type is shield
                                SRCommon.pSkills.ActivateSlot(stackPanel, SRCommon.pSkills.buffSlotColor);
                            else
                                SRCommon.pSkills.DisableSlot(stackPanel);
                        }

                        // image
                        var imageControl = new Image();
                        if (item.Value.IconPath == null)
                            imageControl.Source = Utility.PK2GetImage("icon_default.ddj");
                        else
                            imageControl.Source = Utility.PK2GetImageByURL(item.Value.IconPath);//

                        imageControl.Height = 32;
                        imageControl.Width = 32;
                        imageControl.Margin = new Thickness(2, 3, 2, 2);
                        imageControl.Cursor = (Cursor)App.Current.Resources["Pointer"];
                        imageControl.Tag = item.Value;// item.Value.ObjRefID + "-" + item.Value.PlusValue;

                        // Item info
                        System.Text.StringBuilder itemInfo = new System.Text.StringBuilder();
                        if (item.Value.PlusValue > 0)
                            itemInfo.AppendLine($"{ item.Value.TranslationName} (+{ item.Value.PlusValue})");
                        else
                            itemInfo.AppendLine($"{ item.Value.TranslationName}");

                        itemInfo.AppendLine();

                        // add this to utillity
                        if (item.Value.MediaName.Contains("SET_A_RARE") && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Seal Of Nova");
                        else if (item.Value.MediaName.Contains("SET_B_RARE") && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Seal Of Nova");
                        else if (item.Value.MediaName.Contains("A_RARE") && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Seal Of Nova");
                        else if (item.Value.MediaName.Contains("C_RARE"))
                            itemInfo.AppendLine($"Seal Of Sun");
                        else if (item.Value.MediaName.Contains("B_RARE"))
                            itemInfo.AppendLine($"Seal Of Moon");
                        else if (item.Value.MediaName.Contains("A_RARE"))
                            itemInfo.AppendLine($"Seal Of Star");

                        if (item.Value.MediaName.Contains("SET_B") && item.Value.Type == ItemType.Weapon && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Fight");
                        else if (item.Value.MediaName.Contains("SET_A") && item.Value.Type == ItemType.Weapon && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Power");

                        if (item.Value.MediaName.Contains("SET_B") && item.Value.Type == ItemType.Shield && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Guard");
                        else if (item.Value.MediaName.Contains("SET_A") && item.Value.Type == ItemType.Shield && item.Value.Degree == 11)
                            itemInfo.AppendLine($"Shield");

                        if (item.Value.MediaName.Contains("RARE"))
                            itemInfo.AppendLine();

                        itemInfo.AppendLine($"Sort of item: {item.Value.SubType}");
                        itemInfo.AppendLine($"Degree: {item.Value.Degree} degrees");

                        if (item.Value.Stats.Count > 0)
                            itemInfo.AppendLine();

                        foreach (var whiteState in item.Value.Stats)
                        {
                            itemInfo.AppendLine(SilkroadInformationAPI.Media.Utility.TranslateWhiteStats(whiteState.Key, whiteState.Value));
                        }

                        if (item.Value.Blues.Count > 0)
                            itemInfo.AppendLine();

                        foreach (var blueState in item.Value.Blues)
                        {
                            itemInfo.AppendLine(SilkroadInformationAPI.Media.Utility.TranslateBlueStats(blueState.Key, blueState.Value)); // repleace it with real
                        }

                        itemInfo.AppendLine();
                        itemInfo.AppendLine($"Required Level: {item.Value.Level}");
                        itemInfo.AppendLine($"{Client.Info.Race}");
                        itemInfo.AppendLine();
                        itemInfo.AppendLine($"Max. no. of magic options: 9 Unit");
                        if (item.Value.HasAdvance)
                            itemInfo.Append($"Advanced elixir is in effect [+2]");
                        else
                            itemInfo.Append($"Able to use Advanced elixir.");

                        if (item.Value.Type == ItemType.Arrows || item.Value.Type == ItemType.Bolts)
                            itemInfo.AppendLine($"nQuantity: {item.Value.Stack}");

                        imageControl.ToolTip = itemInfo.ToString();

                        if (type == ItemType.Weapon)
                        {
                            imageControl.MouseLeftButtonDown += new MouseButtonEventHandler(WeaponAttackClicked);
                            imageControl.MouseRightButtonDown += new MouseButtonEventHandler(WeaponBuffClicked);
                        }
                        else if (type == ItemType.Shield)
                        {
                            imageControl.MouseLeftButtonDown += new MouseButtonEventHandler(ShieldAttackClicked);
                            imageControl.MouseRightButtonDown += new MouseButtonEventHandler(ShieldBuffClicked);
                        }


                        Grid.SetColumn(stackPanel, column);
                        if (x++ % 3 == 0 && x != 0)
                        {
                            Grid.SetRow(stackPanel, row++);
                            column = 0;
                        }
                        else
                        {
                            Grid.SetRow(stackPanel, row);
                            column++;
                        }

                        stackPanel.Children.Add(imageControl);

                        if (item.Value.MediaName.Contains("RARE"))
                            SRCommon.pSkills.SoxEffect(stackPanel);

                        RootGrid.Children.Add(stackPanel);
                    }
                }
                else
                {
                    Label label = new Label()
                    {
                        Content = "Empty",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontFamily = new FontFamily("Times New Roman"),
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 108, 108, 108))
                    };
                    RootGrid.Children.Add(label);
                }
            }
            catch { }
        }
        #endregion
    }
}
