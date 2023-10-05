using Contollers.GameBot;
using SilkroadInformationAPI.Client;
using SilkroadInformationAPI.Client.Information.Spells;
using SilkroadInformationAPI.Media.DataInfo;
using SRO_INGAME.Common;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SRO_INGAME.View.GameBot.Skills
{
    /// <summary>
    /// Interaction logic for AttackSkills.xaml
    /// </summary>
    public partial class AttackSkills : Page
    {
        #region Declarion

        #endregion

        #region Init
        public AttackSkills()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            LoadAttackSkills();
        }
        #endregion

        #region Load Attack Skills
        /// <summary>
        /// Load Attack buffs
        /// </summary>
        public void LoadAttackSkills() // Click select as attack - double click as buff same for weapon and shield // add BuffRootFrame under the Skill root and increase the Weapon and shield frames be 20 // add refresh button for skills instead of reloading it everytime
        {
            try
            {
                var Skills = Client.Skills.Where(i => i.RequireTarget == true).ToList();
                if (Skills.Count > 0)
                {
                    skillsLabel.Content = $"Accured skills: [ {Client.Skills.Where(i => i.RequireTarget == true).ToList().Count} ] Skill(s)";
                    selectedSkillsLabel.Content = $"Selected: [ {BotData.AttackSkills.Count} ] Skill(s)";
                    SkillsRootFrame.Children.Clear();
                    int column = 0, row = 0, x = 1;
                    foreach (var skill in Skills) // add sword and force from here // order skills by level
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
                            BorderThickness = new Thickness(1),
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

                        Icon.MouseLeftButtonDown += new MouseButtonEventHandler(SkillClicked);
                        Icon.MouseRightButtonDown += new MouseButtonEventHandler(SkillRemoveClicked);

                        if (skill.SkillIcon == null)
                            Icon.Source = Utility.PK2GetImage("icon_default.ddj");
                        else
                            Icon.Source = Utility.PK2GetImage(Path.GetFileName(skill.SkillIcon));


                        Grid.SetColumn(Slot, column);
                        if (x++ % 8 == 0) // fix this
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

                        if (BotData.AttackSkills.Any(attackSkill => attackSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.ObjRefID)].ObjRefID))
                        {
                            SRCommon.pSkills.AddSlotNumber(Slot, BotData.AttackSkills.FindIndex(attackSkill => attackSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.ObjRefID)].ObjRefID));
                            Slot.Background = SRCommon.pSkills.skillSlotColor;
                            Slot.Opacity = 1;
                        }
                        else
                        {
                            Slot.Background = SRCommon.pSkills.emptySlotColor;
                            Slot.Opacity = .2;
                        }

                        SkillsRootFrame.Children.Add(Slot);
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
                    SkillsRootFrame.Children.Add(label);
                }
            }
            catch { }
        }

        #endregion

        #region Attack-Skill OnClick Functions
        // click events
        private void SkillClicked(object sender, MouseEventArgs e)
        {
            try
            {
                Image skill = (sender as Image);
                //(skill.Parent as StackPanel).Background = SRCommon.pSkills.skillSlotColor;
                (skill.Parent as StackPanel).Opacity = 1;
                if (!BotData.AttackSkills.Any(attackSkill => attackSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)].ObjRefID))
                {
                    BotData.AttackSkills.Add(SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)]);
                    SRCommon.pSkills.AddSlotNumber((StackPanel)skill.Parent, BotData.AttackSkills.FindIndex(i => i.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)].ObjRefID));
                }
                selectedSkillsLabel.Content = $"Selected: [ {BotData.AttackSkills.Count} ] Skill(s)";
            }
            catch { }
        }

        private void SkillRemoveClicked(object sender, MouseEventArgs e)
        {
            try
            {
                Image skill = (sender as Image);
                //(skill.Parent as StackPanel).Background = SRCommon.pSkills.emptySlotColor;
                (skill.Parent as StackPanel).Opacity = .2;
                if (BotData.AttackSkills.Any(attackSkill => attackSkill.ObjRefID == SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)].ObjRefID))
                {
                    BotData.AttackSkills.Remove(SilkroadInformationAPI.Media.Data.MediaSkills[Convert.ToUInt32(skill.Tag)]);
                    (skill.Parent as StackPanel).Children.Remove((skill.Parent as StackPanel).Children[1]);
                    LoadAttackSkills();
                }
                selectedSkillsLabel.Content = $"Selected: [ {BotData.AttackSkills.Count} ] Skill(s)";
            }
            catch { }
        }
        #endregion
    }
}
