using Contollers.GameBot;
using ImageLibrary.GDImageLibrary;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using SRO_INGAME.View.GameBot.Potion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SRO_INGAME.View
{
    /// <summary>
    /// Interaction logic for Potion.xaml
    /// </summary>
    public partial class Potion : Page
    {
        /**
         * Fix paurchase packet from all and npc so you update the inventory immediatey
         * add petrecovery thing and fix it !!
         * add pet abnormal
         * fix the click event
         */

        #region Declaration
        CharacterPotion viewCharacterPotion = new CharacterPotion();
        PetPotion viewPetPotion = new PetPotion();
        // Labels
        public SolidColorBrush activeLabel = new SolidColorBrush(Color.FromArgb(255, 125, 106, 66));
        public SolidColorBrush mutedLabel = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));
        #endregion

        #region Initialize
        public Potion()
        {
            InitializeComponent();
        }

        public void Load()
        {
            // make it only select 1 of each type and when you hover it print all the quantitnty from this item
            LoadPotionItems(viewCharacterPotion.HpRootFrame, BotData.PotionItems["HP"], ItemType.HpPotion, ItemType.VigorPotion, "HP");
            LoadPotionItems(viewCharacterPotion.MpRootFrame, BotData.PotionItems["MP"], ItemType.MpPotion, ItemType.VigorPotion, "MP");
            LoadPotionItems(viewCharacterPotion.AbnormalRootFrame, BotData.PotionItems["Abnormal"], ItemType.UniversalPills, ItemType.PurificationPills, "Abnormal"); //PurificationPills and UniversalPills
            LoadPotionItems(viewPetPotion.PetRootFrame, BotData.PotionItems["PetHGP"], ItemType.PetHGP, ItemType.PetRecoveryKit, "PetHGP");

            // set the init view
            potionFrame.Content = viewCharacterPotion;
            pCharacterLabel.Foreground = activeLabel;
            pPetLabel.Foreground = mutedLabel;
        }
        #endregion

        #region Load Potions
        public void LoadPotionItems(Grid parent, uint itemID, ItemType type1, ItemType type2, string name) // Click select as attack - double click as buff same for weapon and shield
        {
            try
            {
                parent.Children.Clear();
                int column = 0, row = 0;
                var filteredList = Client.InventoryItems.GroupBy(i => i.Value.MediaName, (Key, group) => group.First().Value).Where(i => i.Type == type1 || i.Type == type2).ToList();
                Console.WriteLine(name + " " + filteredList.Count);
                if (filteredList.Count > 0)
                {
                    foreach (SilkroadInformationAPI.Client.Information.InventoryItem item in filteredList) // check if we are causing a problem to the main Client.InventoryItems because we are't asigging it clearly
                    {
                        // total quantity for item type
                        int quantity = 0;
                        Client.InventoryItems.Where(i => i.Value.ObjRefID == item.ObjRefID).ToList().ForEach(i => quantity += i.Value.Stack);

                        // stack panel
                        var Slot = new StackPanel()
                        {
                            Margin = new Thickness(5, 5, 5, 5),
                            Height = 38,
                            Width = 38,
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                        };
                        if (item.ObjRefID == itemID && itemID != 0)
                        {
                            Slot.Background = new SolidColorBrush(Color.FromArgb(255, 125, 106, 66));
                            Slot.Opacity = 1;
                        }
                        else
                        {
                            Slot.Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                            Slot.Opacity = .2;
                        }

                        // image
                        var Icon = new Image()
                        {
                            Height = 32,
                            Width = 32,
                            Margin = new Thickness(2, 3, 2, 2),
                            Name = name,
                            Tag = item.ObjRefID,// to be saved in bot data
                            Cursor = (Cursor)App.Current.Resources["Pointer"],
                        };

                        if (item.IconPath == null)
                            Icon.Source = Utility.PK2GetImage("icon_default.ddj");
                        else
                            Icon.Source = Utility.PK2GetImage(Path.GetFileName(item.IconPath));

                        SlotToolTip slotToolTip = new SlotToolTip();
                        slotToolTip.ItemToolTipIcon();
                        slotToolTip.WriteLine(item.TranslationName, SlotToolTip.MType.Title, true);
                        slotToolTip.WriteLine($"Quanitity ({quantity})", SlotToolTip.MType.Normal);
                        Icon.ToolTip = slotToolTip.Content();

                        //functions
                        if (new string[] { "HP", "MP", "Abnormal" }.Contains(name))
                        {
                            Icon.MouseLeftButtonDown += new MouseButtonEventHandler(viewCharacterPotion.CharacterPotionSelect);
                            Icon.MouseRightButtonDown += new MouseButtonEventHandler(viewCharacterPotion.CharacterPotionDSelect);
                        }
                        else
                        {
                            Icon.MouseLeftButtonDown += new MouseButtonEventHandler(viewPetPotion.PetPotionSelect);
                            Icon.MouseRightButtonDown += new MouseButtonEventHandler(viewPetPotion.PetPotionDSelect);
                        }

                        Grid.SetRow(Slot, row);
                        Grid.SetColumn(Slot, column);
                        column++;

                        Slot.Children.Add(Icon);
                        parent.Children.Add(Slot);
                    }
                }
                else
                {
                    Label label = new Label()
                    {
                        Content = $"No avaliable items(s).",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontFamily = new FontFamily("Times New Roman"),
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 108, 108, 108))
                    };
                    parent.Children.Add(label);
                }
            }
            catch { }
        }
        #endregion

        #region Navigation
        private void Navigation(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Border clicked = sender as Border;
                if (clicked != null)
                {
                    if (clicked.Name == "pCharacter")
                    {
                        potionFrame.Content = viewCharacterPotion;
                        pCharacterLabel.Foreground = activeLabel;
                        pPetLabel.Foreground = mutedLabel;
                    }
                    else if (clicked.Name == "pPet")
                    {
                        potionFrame.Content = viewPetPotion;
                        pCharacterLabel.Foreground = mutedLabel;
                        pPetLabel.Foreground = activeLabel;
                    }
                }
            }
            catch { }
        }
        #endregion
    }
}
