using SRO_INGAME.Common;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRO_INGAME.View.Main.LauncherCompnents
{
    /// <summary>
    /// Interaction logic for ItemsSlider.xaml
    /// </summary>
    public partial class ItemsSlider : Page
    {
        // maybe you can do that and add all the image to array so you don't call it every time and reload images! do that for the skills and the items
        private List<ImageSource> Images = new List<ImageSource>();
        private WrapPanel[] ImageControls;
        private static string[] TransitionEffects = new[] { "SlideRight", "SlideLeft" };
        private string TransitionType;
        private int CurrentCtrlIndex = 0;

        public ItemsSlider()
        {
            InitializeComponent();
            LoadElements();
            LeftArrow.Source = Utility.PK2GetImage("com_left_bigarrow.ddj");
            RightArrow.Source = Utility.PK2GetImage("com_right_bigarrow.ddj");
        }

        //Load images from folder
        private void LoadElements()
        {
            try
            {
                Images.Clear();
                List<string[]> imgsInfo = new List<string[]>
                {
                    new string[] { @"item\china\weapon\bow_11.ddj", "Strong Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_10.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_09.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_08.ddj", "Strong Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_07.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_06.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_05.ddj", "Strong Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_04.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_03.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_01.ddj", "Strong Bow (+10)" },
                    new string[] { @"item\china\weapon\bow_01.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\tblade_06.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\spear_08.ddj", "Strong Bow (+10)" },
                    new string[] { @"item\china\weapon\sword_07.ddj", "Weak Bow (+10)" },
                    new string[] { @"item\china\weapon\spear_06.ddj", "Weak Bow (+10)" },
                };
                foreach (string[] path in imgsInfo)
                    Images.Add(Utility.PK2GetImageByURL(path[0]));

                double originalCount = Convert.ToDouble(Images.Count / 4.0);
                int totalCount = Convert.ToInt32(Images.Count / 4);
                int finalCount;
                if (originalCount == totalCount)
                    finalCount = totalCount;
                else
                    finalCount = totalCount + 1;

                ImageControls = new WrapPanel[finalCount];
                for (int j = 0; j < finalCount; j++)
                {
                    WrapPanel myWrapPanel = new WrapPanel()
                    {
                        Name = "sWrapImage" + j,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Orientation = Orientation.Horizontal,
                        Visibility = Visibility.Hidden,
                    };
                    myWrapPanel.RenderTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 1, CenterX = 0, CenterY = 0 };
                    myWrapPanel.RenderTransform = new SkewTransform() { AngleX = 0, AngleY = 0, CenterX = 0, CenterY = 0 };
                    myWrapPanel.RenderTransform = new RotateTransform() { Angle = 0, CenterX = 0, CenterY = 0 };
                    myWrapPanel.RenderTransform = new TranslateTransform() { X = 0, Y = 0 };
                    CanvasGrid.Children.Add(myWrapPanel);
                    ImageControls[j] = myWrapPanel;
                }
                ImageControls[0].Visibility = Visibility.Visible;

                int s = 0, intCnt = 0;
                for (int i = 0; i < Images.Count; i++)
                {
                    Border SlotBorder = new Border()
                    {
                        BorderBrush = new SolidColorBrush(Color.FromRgb(125, 105, 66)),
                        Padding = new Thickness(1),
                        Margin = new Thickness(10, 0, 10, 0),
                        Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                        BorderThickness = new Thickness(1),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Cursor = (Cursor)App.Current.Resources["Pointer"]
                    };

                    Image ItemIcon = new Image()
                    {
                        Name = "ItemIcon" + i,
                        Source = Images[i],
                        Cursor = (Cursor)App.Current.Resources["Pointer"],
                        ToolTip = imgsInfo[i][1]
                    };

                    SlotBorder.Child = ItemIcon;

                    if (intCnt < 4)
                    {
                        ImageControls[s].Children.Add(SlotBorder);
                        intCnt++;
                    }
                    else
                    {
                        s++;
                        intCnt = 1;
                        ImageControls[s].Children.Add(SlotBorder);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //Slide images in Left or Right direction
        private void Play_Image_Slider(string strDirection)
        {
            try
            {
                if (ImageControls.Length == 1)
                    return;
                var oldCtrlIndex = CurrentCtrlIndex;

                if (strDirection == "LeftSide")
                {
                    TransitionType = TransitionEffects[1].ToString();
                    if (CurrentCtrlIndex == 0)
                        CurrentCtrlIndex = (ImageControls.Length - 1);
                    else
                        CurrentCtrlIndex--;
                }
                else
                {
                    TransitionType = TransitionEffects[0].ToString();
                    if (CurrentCtrlIndex == (ImageControls.Length - 1))
                        CurrentCtrlIndex = 0;
                    else
                        CurrentCtrlIndex++;
                }

                WrapPanel oldImage = ImageControls[oldCtrlIndex];
                WrapPanel newImage = ImageControls[CurrentCtrlIndex];

                //For Animation of opacity......
                Storyboard DefaultPosition = (Resources["SlideAndFadeIn"] as Storyboard).Clone();
                DefaultPosition.Begin(newImage);

                //For Sliding....
                Storyboard hidePage = (Resources[string.Format("{0}Out", TransitionType.ToString())] as Storyboard).Clone();
                hidePage.Begin(oldImage);
                Storyboard showNewPage = Resources[string.Format("{0}In", TransitionType.ToString())] as Storyboard;
                showNewPage.Begin(newImage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        //Images slide left
        private void imgUp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int tempIndex;
                if ((CurrentCtrlIndex - 1) == -1)
                    tempIndex = (ImageControls.Length - 1);
                else
                    tempIndex = (CurrentCtrlIndex - 1);
                if (ImageControls[tempIndex].Visibility == System.Windows.Visibility.Hidden)
                    ImageControls[tempIndex].Visibility = System.Windows.Visibility.Visible;

                Play_Image_Slider("LeftSide");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        //Images slide right
        private void imgDown_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int tempIndex;
                if (CurrentCtrlIndex == ImageControls.Length - 1)
                    tempIndex = 0;
                else
                    tempIndex = (CurrentCtrlIndex + 1);
                if (ImageControls[tempIndex].Visibility == System.Windows.Visibility.Hidden)
                    ImageControls[tempIndex].Visibility = System.Windows.Visibility.Visible;

                Play_Image_Slider("RightSide");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }
    }
}
