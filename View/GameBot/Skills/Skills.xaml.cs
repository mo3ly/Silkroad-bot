using Contollers.GameBot;
using SilkroadInformationAPI;
using SRO_INGAME.Common;
using SRO_INGAME.View.GameBot.Skills;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfAnimatedGif;

namespace SRO_INGAME.View
{
    /***
     * [IMPORTANT] Show item when buy directly when from stall -> parse it and mall
     * Check for items that can be worn with shield // fix gears
     * Fix media images for eu shiled d11 because it has the same name as the europe one // let it get the name by full path
     * add refresh button to aviod alot of unnecessary load
     * when left click on selected gears clear the Attack and buff gears -> add guide
     * custom color for the weapon or shield that is used in both attack buff
     * when show items check for char if eu or ch
     * check item with tag not color
     * fix all the inventory operation -> buy from mall -> shop -> change slot -> storage so you the bot inventory up to date
     * select arrow randomly when char wears bow or crossbow
     * make skill attack by selcted order
     * add sox mark to selected Gear box
     * light item by id InventoryClass.hasequal isntead of color!
     * load data with navigation load and add refresh button
     * Add async and await
     * refresh arrow and bolt number
     * check error in buff shield
     */
    
    /**
     * TODO:
     *  Fix the bug related to the reload function because of the new interface
     *
     */
    public partial class Skills : Page
    {
        #region Declration

        //Classes
        Gears viewGears = new Gears();
        AttackSkills viewAttackSkills = new AttackSkills();
        BuffSkills viewBuffSkills = new BuffSkills();

        // Colors
        public SolidColorBrush emptySlotColor = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30));
        public SolidColorBrush skillSlotColor = new SolidColorBrush(Color.FromArgb(255, 125, 106, 66)); //new SolidColorBrush(Color.FromArgb(255, 140, 103, 37));
        public SolidColorBrush attackSlotColor = new SolidColorBrush(Color.FromArgb(255, 100, 14, 14));
        public SolidColorBrush buffSlotColor = new SolidColorBrush(Color.FromArgb(255, 37, 46, 140));

        // Labels
        public SolidColorBrush activeLabel = new SolidColorBrush(Color.FromArgb(255, 125, 106, 66));
        public SolidColorBrush mutedLabel = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187));

        // passive skills
        List<uint> excludedSkills = new List<uint>() {
            10001, 10106, 10234, 10521, 9956, 9951, // wizard
            692, //sword & shield 
            1098 //Bow
        };

        // Requiring Shield items
        public SubItemType[] requireShield = { SubItemType.Sword, SubItemType.Blade, SubItemType.OneHand, SubItemType.LightStaff, SubItemType.DarkStaff };
        #endregion

        #region Initialize
        public Skills()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Skills_Loaded);
        }

        void Skills_Loaded(object sender, RoutedEventArgs e)
        {
            refreshButtonBg.ImageSource = Utility.PK2GetImage("chr_stat_off.ddj");
            // LOAD GEARS
            mFrame.Content = viewGears;
            sGears.Foreground = activeLabel;
            sAttack.Foreground = mutedLabel;
            sBuff.Foreground = mutedLabel;
        }

        public void Load()
        {
            viewGears.Initialize();
            viewAttackSkills.Initialize();
            viewBuffSkills.Initialize();
        }
        #endregion

        #region Navigation
        private void Navigation(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label clicked = sender as Label;
                if (clicked.Name == "sGears")
                {
                    mFrame.Content = viewGears;
                    sGears.Foreground = activeLabel;
                    sAttack.Foreground = mutedLabel;
                    sBuff.Foreground = mutedLabel;
                }
                else if (clicked.Name == "sAttack")
                {
                    mFrame.Content = viewAttackSkills;
                    sGears.Foreground = mutedLabel;
                    sAttack.Foreground = activeLabel;
                    sBuff.Foreground = mutedLabel;
                }
                else if (clicked.Name == "sBuff")
                {
                    mFrame.Content = viewBuffSkills;
                    sGears.Foreground = mutedLabel;
                    sAttack.Foreground = mutedLabel;
                    sBuff.Foreground = activeLabel;
                }
                else { }
            } catch { }
        }
        #endregion

        #region Helper Functions
        public bool ColorEquals(SolidColorBrush brush1, SolidColorBrush brush2)
        {
            return brush1.Opacity == brush2.Opacity &&
                brush1.Color.A == brush2.Color.A &&
                brush1.Color.R == brush2.Color.R &&
                brush1.Color.B == brush2.Color.B &&
                brush1.Color.G == brush2.Color.G;
        }


        public void DisableSlot(StackPanel stackPanel)
        {
            stackPanel.Background = emptySlotColor;
            stackPanel.Opacity = 0.2;
        }

        public void ActivateSlot(StackPanel stackPanel, SolidColorBrush solidColorBrush)
        {
            stackPanel.Background = solidColorBrush;
            stackPanel.Opacity = 1;
        }

        public void SoxEffect(StackPanel parentPanel)
        {
            var Sox = new Image()
            {
                Height = 20,
                Width = 20,
                Margin = new Thickness(1, -50, 20, 0),
                Cursor = (Cursor)App.Current.Resources["Pointer"]
            };
            ImageBehavior.SetAnimatedSource(Sox, new System.Windows.Media.Imaging.BitmapImage(new Uri(@"C:\Users\mo3ly\Desktop\Personal\Silkroad\C#\SRO_INGAME\SRO_INGAME\Resources\soxglow.gif")));
            parentPanel.Children.Add(Sox);
        }

        public void AddSlotNumber(StackPanel parentPanel, int number)
        {
            Label Label = new Label()
            {
                Content = number + 1,
                Height = 13,
                Width = 13,
                Margin = new Thickness(-16, -20, 2, 0),
                FontSize = 9,
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 125, 106, 66)),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(2, 1, 0, 0),
                Background = new SolidColorBrush(Color.FromArgb(255, 30, 30, 30)),
                Foreground = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)),
            };

            parentPanel.Children.Add(Label);
        }
        #endregion

        #region Refresh
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
        #endregion
    }
}
