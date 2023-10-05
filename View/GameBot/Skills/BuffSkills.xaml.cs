using Contollers.GameBot;
using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using SilkroadInformationAPI;

namespace SRO_INGAME.View.GameBot.Skills
{
    /// <summary>
    /// Interaction logic for BuffSkills.xaml
    /// </summary>
    public partial class BuffSkills : Page
    {
        #region Declaration

        #endregion

        #region Initialize
        public BuffSkills()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            LoadBuffSkills();
        }
        #endregion

        #region BuffSkills OnClick Function
        private void BuffClicked(object sender, MouseEventArgs e)
        {
            try
            {
                Image skill = (sender as Image);
                //(skill.Parent as StackPanel).Background = SRCommon.pSkills.skillSlotColor;
                (skill.Parent as StackPanel).Opacity = 1;
                if (!BotData.BuffSkills.Any(buffSkill => buffSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)].ObjRefID))
                {
                    BotData.BuffSkills.Add(SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)]);
                    SRCommon.pSkills.AddSlotNumber((StackPanel)skill.Parent, BotData.BuffSkills.FindIndex(i => i.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)].ObjRefID));
                }
                selectedBuffsLabel.Content = $"Selected: [ {BotData.BuffSkills.Count} ] Skill(s)";
            }
            catch { }
        }

        private void BuffRemoveClicked(object sender, MouseEventArgs e)
        {
            try
            {
                Image skill = (sender as Image);
                //(skill.Parent as StackPanel).Background = SRCommon.pSkills.emptySlotColor;
                (skill.Parent as StackPanel).Opacity = .2;
                if (BotData.BuffSkills.Any(buffSkill => buffSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)].ObjRefID))
                {
                    BotData.BuffSkills.Remove(SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)]);
                    (skill.Parent as StackPanel).Children.Remove((skill.Parent as StackPanel).Children[1]);
                    LoadBuffSkills();
                }
                selectedBuffsLabel.Content = $"Selected: [ {BotData.BuffSkills.Count} ] Skill(s)";
            }
            catch { }
        }
        #endregion

        #region Load Buff Skills
        /// <summary>
        /// Load Buffs
        /// </summary>
        public void LoadBuffSkills() // Click select as attack - double click as buff same for weapon and shield // add BuffRootFrame under the Skill root and increase the Weapon and shield frames be 20 // add refresh button for skills instead of reloading it everytime
        {
            try
            {
                // when no skills add label no skills
                var BuffSkills = Client.Skills.Where(i => i.RequireTarget == false && i.Type != SkillType.Passive).ToList(); //&& !excludedSkills.Any(j => j == i.ObjRefID)
                if (BuffSkills.Count > 0)
                {
                    buffsLabel.Content = $"Buff skills [ {BuffSkills.ToList().Count} ] Skills(s)";
                    selectedBuffsLabel.Content = $"Selected: [ {BotData.BuffSkills.Count} ] Skill(s)";
                    BuffsRootFrame.Children.Clear();
                    int column = 0, row = 0, x = 1;
                    foreach (var skill in BuffSkills) // remove sword and force from here // order skills by level // execlude the un useable skills because it will crash the user
                    {
                        var Slot = new StackPanel()
                        {
                            Height = 38,
                            Width = 38,
                            Margin = new Thickness(5, 5, 5, 5),
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };

                        var SlotBorder = new Border()
                        {
                            BorderBrush = new SolidColorBrush(Color.FromRgb(125, 106, 66)),
                            //Padding = new Thickness(1),
                            Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                            BorderThickness = new Thickness(0,0,0,1),
                            CornerRadius = new CornerRadius(1),
                            Height = 38,
                            Width = 38,
                            //Margin = new Thickness(5, 5, 5, 5),
                            //VerticalAlignment = VerticalAlignment.Center,
                            // HorizontalAlignment = HorizontalAlignment.Center
                        };

                        var Icon = new Image()
                        {
                            Height = 32,
                            Width = 32,
                            Margin = new Thickness(2, -35, 2, 2),
                            Tag = skill.ObjRefID,
                            Cursor = (Cursor)App.Current.Resources["Pointer"]
                        };

                        // Tooltip
                        SlotToolTip slotToolTip = new SlotToolTip();
                        slotToolTip.ItemToolTipIcon();
                        slotToolTip.WriteLine(skill.TranslationName, SlotToolTip.MType.Title, true);
                        slotToolTip.WriteSpace();
                        slotToolTip.WriteLine(skill.Type.ToString(), SlotToolTip.MType.Info);
                        slotToolTip.WriteLine(SilkroadInformationAPI.Media.Utility.ItemDescription(skill.Description).ToString(), SlotToolTip.MType.Normal);
                        Icon.ToolTip = slotToolTip.Content();

                        // Events
                        Icon.MouseLeftButtonDown += new MouseButtonEventHandler(BuffClicked);
                        Icon.MouseRightButtonDown += new MouseButtonEventHandler(BuffRemoveClicked);

                        // Icon
                        if (skill.SkillIcon == null)
                            Icon.Source = Utility.PK2GetImage("icon_default.ddj");
                        else
                            Icon.Source = Utility.PK2GetImage(Path.GetFileName(skill.SkillIcon));


                        Grid.SetColumn(Slot, column);
                        if (x++ % 8 == 0)
                        {
                            Grid.SetRow(Slot, row++);
                            column = 0;
                        }
                        else
                        {
                            Grid.SetRow(Slot, row);
                            column++;
                        }

                        // appending the data
                        Slot.Children.Add(SlotBorder);
                        Slot.Children.Add(Icon);

                        if (BotData.BuffSkills.Any(buffSkill => buffSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.ObjRefID)].ObjRefID))
                        {
                            SRCommon.pSkills.AddSlotNumber(Slot, BotData.BuffSkills.FindIndex(buffSkill => buffSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.ObjRefID)].ObjRefID));
                            //Slot.Background = SRCommon.pSkills.skillSlotColor;
                            Slot.Opacity = 1;
                        }
                        else
                        {
                            //Slot.Background = SRCommon.pSkills.emptySlotColor;
                            Slot.Opacity = .2;
                        }
                        BuffsRootFrame.Children.Add(Slot);
                    }
                }
                else
                {
                    Label label = new Label()
                    {
                        Content = $"Empty",
                        VerticalAlignment = VerticalAlignment.Bottom,
                        FontFamily = new FontFamily("Times New Roman"),
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 108, 108, 108))
                    };
                    BuffsRootFrame.Children.Add(label);
                }
            }
            catch { }
        }
        #endregion
    }
}
