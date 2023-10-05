using Contollers.GameBot;
using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRO_INGAME.View.GameBot.Potion
{
    /// <summary>
    /// Interaction logic for PetPotion.xaml
    /// </summary>
    public partial class PetPotion : Page
    {
        public PetPotion()
        {
            InitializeComponent();
        }


        #region Settings
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (sender as Slider);

            if (slider.Name == "petHGPSlider")
            {
                petHGPValueBox.Text = petHGPSlider.Value.ToString();
                BotData.PotionPercentage["PetHGP"] = (short)petHGPSlider.Value;
            }
        }

        private void SliderChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (sender as CheckBox);

            if (checkBox.Name == "petHGPCheckBox" && BotData.PotionItems["PetHGP"] != 0 && Client.InventoryItems.Any(i => i.Value.Type == ItemType.PetHGP || i.Value.Type == ItemType.PetRecoveryKit))
            {
                petHGPSlider.IsEnabled = petHGPCheckBox.IsChecked == true ? true : false;
                BotData.PotionSettings["PetHGP"] = petHGPCheckBox.IsChecked == true ? true : false;
            }
        }

        // Auto switch type
        private void AutoSwitchChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (sender as CheckBox);
            if (checkBox.Name == "sHGPCheckBox")
                BotData.PotionAutoSwitch["PetHGP"] = sHGPCheckBox.IsChecked == true ? true : false;
        }

        // click events
        public void PetPotionSelect(object sender, MouseEventArgs e)
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

                if (Slot.Name == "PetHGP")
                {
                    SlotHgpPet.Source = Slot.Source;
                    petHGPCheckBox.IsEnabled = true;
                    BotData.PotionItems["PetHGP"] = Convert.ToUInt32(Slot.Tag);
                }
            }
            catch { }
        }

        public void PetPotionDSelect(object sender, MouseEventArgs e)
        {
            try
            {
                Image Slot = (sender as Image);
                (Slot.Parent as StackPanel).Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
                (Slot.Parent as StackPanel).Opacity = .2;

                if (Slot.Name == "PetHGP")
                {
                    SlotHgpPet.Source = null;
                    petHGPCheckBox.IsEnabled = false;
                    petHGPCheckBox.IsChecked = false;
                }
            }
            catch { }
        }
        #endregion
    }
}
