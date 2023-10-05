using SilkroadInformationAPI;
using SilkroadInformationAPI.Client;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SRO_INGAME.Common;
using SilkroadInformationAPI.Media.DataInfo;
using System.Linq;
using System;

namespace SRO_INGAME.View
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        public Statistics()
        {
            InitializeComponent();
            RefreshNearbyBG.ImageSource = Utility.PK2GetImage("chr_stat_off.ddj");
        }

        class ItemRow
        {
            public ImageSource Item { get; set; }
            public Visibility Sox { get; set; }
            public string ItemTooltip { get; set; }
            public string Name { get; set; }
            public ImageSource ButtonSrc { get; set; }
            public uint UID { get; set; }
        }

        public void CreateNearbyItemsGrid()
        {
            try
            {
                //SroClient.PinkNotice("CreateNearbyItemsGrid has been called");
                NearbyItemsGrid.ItemsSource = null;
                NearbyItemsGrid.Items.Clear();
                foreach (var item in Client.NearbyItems.Values)
                {

                    Item mediaRow = SilkroadInformationAPI.Media.Data.MediaItems[item.ModelID];

                    Visibility showSox;
                    if (mediaRow.MediaName.Contains("RARE") || (mediaRow.MediaName.Contains("ROC") && mediaRow.MediaName.Contains("SET")))
                        showSox = Visibility.Visible;
                    else
                        showSox = Visibility.Hidden;

                    // add exception for gold
                    ItemRow row = new ItemRow 
                    { 
                        Item = mediaRow.TranslationName == "Gold" ? Utility.PK2GetImage("com_moneybutton.ddj") : Utility.PK2GetImageByURL(mediaRow.IconPath),
                        Sox = showSox,
                        ItemTooltip = mediaRow.TranslationName,
                        Name = mediaRow.TranslationName, 
                        UID = item.UniqueID,
                        ButtonSrc = Utility.PK2GetImage("com_m_button.ddj") //com_red_button
                    };

                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        NearbyItemsGrid.Items.Add(row);
                        NearbyItemsGrid.Items.Refresh();
                    });
                }
                Application.Current.Dispatcher.Invoke(delegate
                {
                    NearbyCountLabel.Content = $"Nearby Items: {Client.NearbyItems.Count} item(s)";
                    NearbyItemsGrid.Items.Refresh();
                });
            }
            catch { }
        }

        private void PickButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // add exception for gold
                Button button = (sender as Button);
                SroClient.PickItem((uint)button.Tag);
                CreateNearbyItemsGrid();
            } catch { }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNearbyItemsGrid();
        }
    }
}
