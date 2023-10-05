using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Media.DataInfo;
using SRO_INGAME.Common;
using SRO_INGAME.Http.Models.Stalls;

namespace SRO_INGAME.View.Stalls
{
    /// <TODO>
    ///     - Logic: load all the current stalls on Nearby stalls page with pagination (exclude items part)
    ///     - when the user clicks on the row request the stall page
    /// </TODO>
    public partial class CharacterStall : Page
    {
        public CharacterStall()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                StallFrameBG.Source = Utility.PK2GetImage("stall_frame.ddj");
                CloseButtonBG.ImageSource = Utility.PK2GetImage("com_windowclose.ddj");
                WhisperButtonBG.ImageSource = Utility.PK2GetImage("com_plus_smallbutton.ddj");
            }
            catch { }
        }

        public void LoadStall(string name)
        {
            try
            {
                // check that current stalls has been loaded
                CharStall stall = SRCommon.CurrentStalls.CurrentStalls.Find(i => i.Owner == name);
                if (stall != null)
                {
                    stallTitleLabel.Content = $"{stall.Owner}'s stall.";
                    foreach (StallItem item in stall.Items)
                    {
                        StallSlot(item);
                    }
                }
            }
            catch { }
        }

        // be careful because this is not called every time so make sure you clear everything before if shows
        public void StallSlot(StallItem item)
        {
            try
            {
                Dictionary<short, object[]> Slots = new Dictionary<short, object[]>() {
                    { 0, new object[] { Slot0, SlotSox0, SlotName0 , SlotPrice0, SlotUnit0 } },
                    { 1, new object[] { Slot1, SlotSox1, SlotName1 , SlotPrice1, SlotUnit1 } },
                    { 2, new object[] { Slot2, SlotSox2, SlotName2 , SlotPrice2, SlotUnit2 } },
                    { 3, new object[] { Slot3, SlotSox3, SlotName3 , SlotPrice3, SlotUnit3 } },
                    { 4, new object[] { Slot4, SlotSox4, SlotName4 , SlotPrice4, SlotUnit4 } },
                    { 5, new object[] { Slot5, SlotSox5, SlotName5 , SlotPrice5, SlotUnit5 } },
                    { 6, new object[] { Slot6, SlotSox6, SlotName6 , SlotPrice6, SlotUnit6 } },
                    { 7, new object[] { Slot7, SlotSox7, SlotName7 , SlotPrice7, SlotUnit7 } },
                    { 8, new object[] { Slot8, SlotSox8, SlotName8 , SlotPrice8, SlotUnit8 } },
                    { 9, new object[] { Slot9, SlotSox9, SlotName9 , SlotPrice9, SlotUnit9 } },
                };

                if (item.ItemID == 0)
                {
                    (Slots[item.Slot][0] as Image).Source = null;
                    (Slots[item.Slot][0] as Image).ToolTip = null;
                    (Slots[item.Slot][1] as Image).Visibility = Visibility.Hidden;
                    (Slots[item.Slot][2] as Label).Content = null;
                    (Slots[item.Slot][3] as Label).Content = null;
                    (Slots[item.Slot][4] as Label).Content = null;
                }
                else
                {
                    Item mediaItem = SilkroadInformationAPI.Media.Data.MediaItems[item.ItemID];
                    item.Quantity = (item.Quantity <= 0) ? (short)1 : item.Quantity;

                    (Slots[item.Slot][0] as Image).Source = Utility.PK2GetImageByURL(mediaItem.IconPath);
                    (Slots[item.Slot][2] as Label).Content = mediaItem.TranslationName;
                    (Slots[item.Slot][3] as Label).Content = $"{item.Price["formatted"]} Gold";
                    (Slots[item.Slot][4] as Label).Content = $"{item.Quantity} Unit";

                    // sox
                    if (mediaItem.MediaName.Contains("RARE") || (mediaItem.MediaName.Contains("ROC") && mediaItem.MediaName.Contains("SET")))
                        (Slots[item.Slot][1] as Image).Visibility = Visibility.Visible;
                    else
                        (Slots[item.Slot][1] as Image).Visibility = Visibility.Hidden;

                    // tooltip
                    SlotToolTip SlotToolTip = new SlotToolTip();
                    SlotToolTip.ItemToolTipIcon();
                    SlotToolTip.WriteLine($"{mediaItem.TranslationName} (+{item.Plus})", SlotToolTip.MType.SealTitle, true);
                    SlotToolTip.WriteSpace();
                    SlotToolTip.WriteLine("Seal of Nova", SlotToolTip.MType.Seal, true);
                    SlotToolTip.WriteLine("Power", SlotToolTip.MType.SubSeal, true);
                    SlotToolTip.WriteSpace();
                    SlotToolTip.WriteLine("Sort of item: Two handed staff", SlotToolTip.MType.Info);
                    SlotToolTip.WriteLine("Degree: 11 degrees", SlotToolTip.MType.Info);
                    SlotToolTip.WriteSpace();
                    SlotToolTip.WriteLine("Phy.atk.pwr. 2437.0 ~ 2780.0 (+80%)");
                    SlotToolTip.WriteLine("Durability 138/91 (+0%)");
                    SlotToolTip.WriteSpace();
                    SlotToolTip.WriteLine("Required Level: 100");
                    SlotToolTip.WriteLine("European");
                    SlotToolTip.WriteLine("Max. no. of magic options: 9 Unit", SlotToolTip.MType.Info);
                    SlotToolTip.WriteSpace();
                    SlotToolTip.WriteLine("Str 7 Increase", SlotToolTip.MType.Blues);
                    SlotToolTip.WriteLine("Int 7 Increase", SlotToolTip.MType.Blues);
                    SlotToolTip.WriteLine("Advanced elixir is in effect [+2]", SlotToolTip.MType.Advance, true);
                    SlotToolTip.WriteLine("Max durabillity 400%", SlotToolTip.MType.Warning);
                    SlotToolTip.WriteLine(item.Description, SlotToolTip.MType.Warning);


                    (Slots[item.Slot][0] as Image).ToolTip = SlotToolTip.Content();
                }
            }
            catch { }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            // hide overlay in Nearby stalls
            //SRCommon.n
        }

        private void WhisperButton_Click(object sender, RoutedEventArgs e)
        {
            SroClient.WriteToChatTextBox("$Exoria ");
        }
    }
}
