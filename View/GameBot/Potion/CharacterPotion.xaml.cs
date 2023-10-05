using Contollers.GameBot;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SRO_INGAME.View.GameBot.Potion
{
    /// <summary>
    /// Interaction logic for CharacterPotion.xaml
    /// </summary>
    public partial class CharacterPotion : Page
    {
        public CharacterPotion()
        {
            InitializeComponent();
        }

        #region Settings
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (sender as Slider);
            if (slider.Name == "hpSlider")
            {
                hpValueBox.Text = hpSlider.Value.ToString();
                BotData.PotionPercentage["HP"] = (short)hpSlider.Value;
            }
            else if (slider.Name == "mpSlider")
            {
                mpValueBox.Text = mpSlider.Value.ToString();
                BotData.PotionPercentage["MP"] = (short)mpSlider.Value;
            }
        }

        private void SliderChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (sender as CheckBox);

            if (checkBox.Name == "hpCheckBox" && BotData.PotionItems["HP"] != 0 && Client.InventoryItems.Any(i => i.Value.Type == ItemType.HpPotion || i.Value.Type == ItemType.VigorPotion)) // maybe check for avaliable item count!!
            {
                hpSlider.IsEnabled = hpCheckBox.IsChecked == true ? true : false;
                BotData.PotionSettings["HP"] = hpCheckBox.IsChecked == true ? true : false;
            }
            else if (checkBox.Name == "mpCheckBox" && BotData.PotionItems["MP"] != 0 && Client.InventoryItems.Any(i => i.Value.Type == ItemType.MpPotion || i.Value.Type == ItemType.VigorPotion))
            {
                mpSlider.IsEnabled = mpCheckBox.IsChecked == true ? true : false;
                BotData.PotionSettings["MP"] = mpCheckBox.IsChecked == true ? true : false;
            }
        }

        private void AbnormalStatus(object sender, RoutedEventArgs e)
        {
            //if (BotData.PotionItems["Abnormal"] != 0 && Client.InventoryItems.Any(i => i.Value.Type == ItemType.UniversalPills || i.Value.Type == ItemType.PurificationPills))
            BotData.PotionSettings["Abnormal"] = abnormalCheckBox.IsChecked == true ? true : false;
        }

        // Auto switch type
        private void AutoSwitchChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (sender as CheckBox);
            if (checkBox.Name == "sHPCheckBox")
                BotData.PotionAutoSwitch["HP"] = sHPCheckBox.IsChecked == true ? true : false;
            else if (checkBox.Name == "sMPCheckBox")
                BotData.PotionAutoSwitch["MP"] = sMPCheckBox.IsChecked == true ? true : false;
            else if (checkBox.Name == "sAbnormalCheckBox")
                BotData.PotionAutoSwitch["Abnormal"] = sAbnormalCheckBox.IsChecked == true ? true : false;
        }

        // click events
        public void CharacterPotionSelect(object sender, MouseEventArgs e)
        {
            try
            {
                Image Slot = (sender as Image);
                // disbale all the other items
                Grid grid = (Grid)(Slot.Parent as StackPanel).Parent;
                foreach (Panel oSlot in grid.Children)
                {
                    (oSlot as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                    (oSlot as StackPanel).Opacity = 0.2;
                }
                // activate the clicked
                (Slot.Parent as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 125, 106, 66));
                (Slot.Parent as StackPanel).Opacity = 1;

                if (Slot.Name == "HP")
                {
                    slotHpIcon.Source = Slot.Source;
                    hpCheckBox.IsEnabled = true;
                    BotData.PotionItems["HP"] = Convert.ToUInt32(Slot.Tag);
                }
                else if (Slot.Name == "MP")
                {
                    slotMpIcon.Source = Slot.Source;
                    mpCheckBox.IsEnabled = true;
                    BotData.PotionItems["MP"] = Convert.ToUInt32(Slot.Tag);
                }
                else if (Slot.Name == "Abnormal")
                {
                    SlotAbnormal.Source = Slot.Source;
                    abnormalCheckBox.IsEnabled = true;
                    BotData.PotionItems["Abnormal"] = Convert.ToUInt32(Slot.Tag);
                }
            }
            catch { }
        }

        public void CharacterPotionDSelect(object sender, MouseEventArgs e)
        {
            try
            {
                Image Slot = (sender as Image);
                (Slot.Parent as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                (Slot.Parent as StackPanel).Opacity = .2;

                if (Slot.Name == "HP")
                {
                    slotHpIcon.Source = null;
                    hpCheckBox.IsEnabled = false;
                    hpCheckBox.IsChecked = false;
                }
                else if (Slot.Name == "MP")
                {
                    slotMpIcon.Source = null;
                    mpCheckBox.IsEnabled = false;
                    mpCheckBox.IsChecked = false;
                }
                else if (Slot.Name == "Abnormal")
                {
                    SlotAbnormal.Source = null;
                    abnormalCheckBox.IsEnabled = false;
                    abnormalCheckBox.IsChecked = false;
                }
            }
            catch { }
        }
        #endregion
    }
}
