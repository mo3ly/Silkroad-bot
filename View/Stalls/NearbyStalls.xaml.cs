using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using SilkroadInformationAPI.Client.Information;
using SilkroadInformationAPI.Client.Information.Objects;
using SilkroadInformationAPI.Media;
using SRO_INGAME.Common;
using SRO_INGAME.Http.Controllers;
using SRO_INGAME.Http.Models.Stalls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Utility = SRO_INGAME.Common.Utility;

namespace SRO_INGAME.View.Stalls
{
    /// <summary>
    /// Interaction logic for NearbyStalls.xaml
    /// </summary>
    public partial class NearbyStalls : Page
    {
        /**
         * Lets the client store all the item based on LOGID's and request the shop data from an api update evey 5 minutes
         * or lets the huard store all that
         * 
         * When fetch the data
         * 1- parse Desc-> if it doesn't contain [ or ] means that is quanitiy else it will be item info
         * 2- parsing item info from desc -> split the sec by space final bracets should be blues split it by ,
         * 3- varaince and opt level will be easily in desc after spilting it
         * 
         * Simulate purchasing from stall by packet maybe it will work
         * Another Logic: maybe we can get the nerbay characters from the packets when the user clicks the row it loads the api data for this specific stall
         */
        public NearbyStalls()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NextPageButtonBG.ImageSource = Utility.PK2GetImage("com_right_arrow.ddj");
            PrevPageButtonBG.ImageSource = Utility.PK2GetImage("com_left_arrow.ddj");
        }

        class StallRow
        {
            public string Name { get; set; }
            public ImageSource Icon { get; set; }
            public ImageSource ButtonSrc { get; set; }
            public string ItemsCount { get; set; }
            public string Location { get; set; }
            public string Date { get; set; }
            public string StallID { get; set; }
        }

        public void Load()
        {
            try
            {
                new StallsContoller().LoadStalls();
                var stalls = SRCommon.CurrentStalls.CurrentStalls;
                SroClient.ChatMessage($"(Stalls) current stalls count {stalls.Count}");
                labelStallsCount.Content = $"Nearby stalls: {stalls.Count}";
                if (stalls.Count > 0)
                {
                    EmptyStallLabel.Visibility = Visibility.Collapsed;
                    foreach (CharStall stall in stalls)
                    {
                        StallRow row = new StallRow
                        {
                            Name = stall.Owner,
                            Icon = Utility.PK2GetImage("char_ch_man1.ddj"),
                            ButtonSrc = Utility.PK2GetImage("com_m_button.ddj"),
                            ItemsCount = stall.Items.Count.ToString() + " Item(s)",
                            Location = stall.Location,
                            Date = stall.Date,
                            StallID = stall.Owner,
                        };

                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            NearbyStallsGrid.Items.Add(row);
                            NearbyStallsGrid.Items.Refresh();
                        });
                    }
                }
                else
                {
                    EmptyStallLabel.Visibility = Visibility.Visible;
                }
            } catch { }
        }

        private void StallDetails_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (sender as Button);
                Overlay.Visibility = Visibility.Visible;
                CharacterStall characterStall = new CharacterStall();
                stallFrame.Content = characterStall;
                characterStall.LoadStall((string)button.Tag);
            }
            catch { }
        }

        public void Load(string old)
        {
            try
            {
                new StallsContoller().LoadStalls();
                var stalls = SRCommon.CurrentStalls.CurrentStalls;
                SroClient.ChatMessage($"(Stalls) current stalls count {stalls.Count}");
                labelStallsCount.Content = $"Nearby stalls: {stalls.Count}";
                if (stalls.Count > 0)
                {
                    foreach (CharStall stall in stalls)
                    {
                        Console.WriteLine(stall.Owner);
                        // stall label maybe you can make it toggelable
                        Label label = new Label()
                        {
                            Content = $"{stall.Owner} [ {stall.Items.Count} ] Items(s)",
                            FontSize = 14,
                            FontFamily = new FontFamily("Times New Roman"),
                            Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                            ToolTip = $"Location: {stall.Location}\nCreation Date [{stall.Date}]"

                        };
                        //StallsPanel.Children.Add(label);

                        foreach (StallItem item in stall.Items)
                        {
                            // create class for this shit
                            var mediaRow = Data.MediaItems[item.ItemID];
                            Console.WriteLine(mediaRow.TranslationName);
                            StackPanel itemSlot = new StackPanel()
                            {
                                Margin = new Thickness(5, 5, 5, 5),
                                Height = 40,
                                Width = 40,
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left,
                            };

                            Image bgSlot = new Image();
                            //bgSlot.Source = Utility.PK2GetImage("com_close_icon_box.ddj");//com_close_icon_box

                            // item icon
                            Image icon = new Image()
                            {
                                Height = 32,
                                Width = 32,
                                Margin = new Thickness(2, 3, 2, 2),
                                ToolTip = $"{mediaRow.TranslationName} \n{SilkroadInformationAPI.Media.Utility.ItemDescription(item.Description)}", // geni problem
                                Cursor = (Cursor)App.Current.Resources["Pointer"]

                            };
                            if (mediaRow.IconPath == null)
                                icon.Source = Utility.PK2GetImage("icon_default.ddj");
                            else
                                icon.Source = Utility.PK2GetImageByURL(mediaRow.IconPath);


                            itemSlot.Children.Add(bgSlot);
                            itemSlot.Children.Add(icon);
                            //StallsPanel.Children.Add(itemSlot);
                        }
                    }
                }
                else
                {
                    Label label = new Label()
                    {
                        Content = $"No avaliable items(s) currently in the stalls.",
                        FontSize = 15,
                        FontFamily = new FontFamily("Times New Roman"),
                        Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255))

                    };
                   // StallsPanel.Children.Add(label);
                }
            }
            catch { }
        }

        public void Load_old()
        {
            try
            {
                var StallChars = Client.NearbyCharacters.Values.ToList();//.Where(i => i.Stall.StallItems.Count > 0)
                SroClient.ChatMessage($"stalls {StallChars.Count}");
                labelStallsCount.Content = $"Nearby stalls: {Client.NearbyCharacters.Values.Where(i => i.Stall.StallCreated).ToList().Count}";
                if (StallChars.Count > 0)
                {
                    foreach (Character Character in StallChars) // show only stall chars
                    {

                        Label label = new Label()
                        {
                            Content = $"{Character.Name} [ {Character.Stall.StallItems.Count} ] Items(s)",
                            FontSize = 15,
                            FontFamily = new FontFamily("Times New Roman"),
                            Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255)),
                            //ToolTip = $"Message: {Character.Stall.Message}\nPeople in stall [{Character.Stall.PeopleInStall.Count}]:\n{Character.Stall.PeopleInStall.Select(i =>  $"→ {i.Value}\n")}"

                        };

                        SlotToolTip SlotToolTip = new SlotToolTip();
                        SlotToolTip.ItemToolTipIcon();
                        SlotToolTip.WriteLine("Witacheon Headgear (+12)", SlotToolTip.MType.SealTitle, true);
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


                        label.ToolTip = SlotToolTip.Content();
                       // StallsPanel.Children.Add(label);

                        foreach (InventoryItem item in Client.InventoryItems.Values)//Character.Stall.StallItems.Values
                        {
                            Console.WriteLine(item.TranslationName);
                            StackPanel itemSlot = new StackPanel()
                            {
                                Margin = new Thickness(5, 5, 5, 5),
                                Height = 40,
                                Width = 40,
                                VerticalAlignment = VerticalAlignment.Top,
                                HorizontalAlignment = HorizontalAlignment.Left,
                            };

                            Image bgSlot = new Image();
                            bgSlot.Source = Utility.PK2GetImage("am_block_02.ddj");//com_close_icon_box

                            // item icon
                            Image icon = new Image()
                            {
                                Height = 32,
                                Width = 32,
                                Margin = new Thickness(2, 3, 2, 2),
                                ToolTip = $"{item.TranslationName} \n{SilkroadInformationAPI.Media.Utility.ItemDescription(item.Description)}\nPrice:{item.Price}",
                                Tag = item.ObjRefID,
                                Cursor = (Cursor)App.Current.Resources["Pointer"]

                            };
                            if (item.IconPath == null)
                                icon.Source = Utility.PK2GetImage("icon_default.ddj");
                            else
                                icon.Source = Utility.PK2GetImageByURL(item.IconPath);


                            itemSlot.Children.Add(bgSlot);
                            itemSlot.Children.Add(icon);
                           // StallsPanel.Children.Add(itemSlot);
                        }
                    }
                }
                else
                {
                    Label label = new Label()
                    {
                        Content = $"No avaliable items(s) currently in the stalls.",
                        FontSize = 15,
                        FontFamily = new FontFamily("Times New Roman"),
                        Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255))

                    };
                   // StallsPanel.Children.Add(label);
                }
            }
            catch { }
        }

    }
}
