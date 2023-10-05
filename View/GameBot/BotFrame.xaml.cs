using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRO_INGAME.View.GameBot
{
    /// <summary>
    /// Interaction logic for BotFrame.xaml
    /// </summary>
    public partial class BotFrame : Page
    {

        // navigation pages

        // home buttons
        Brush activeButtonColor;
        Brush normalButtonColor;

        public BotFrame()
        {
            InitializeComponent();
            activeButtonColor = Statistics.Background;
            normalButtonColor = Potion.Background;
            Statistics.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        Button cHomeButton;
        private void Navigation(object sender, RoutedEventArgs e)
        {
            Button button = (sender as Button);

            // active the clicked button
            if (cHomeButton != null)
            {
                cHomeButton.Background = normalButtonColor;
            }
            cHomeButton = button;
            button.Background = activeButtonColor;

            switch (button.Name)
            {
                case "Statistics":
                    mContent.Content = SRCommon.pStatistics;
                    break;
                case "Potion":
                    mContent.Content = SRCommon.pPotion;
                    break;
                case "Skills":
                    mContent.Content = SRCommon.pSkills;
                    break;
                case "Hunting":
                    mContent.Content = SRCommon.pHunting;
                    break;
            }
        }

        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            /*var animation = new ThicknessAnimation();
            animation.Duration = TimeSpan.FromSeconds(0.5);
            animation.DecelerationRatio = 0.5;
            animation.To = new Thickness(0, 0, 0, 0);
            if (e.NavigationMode == NavigationMode.New)
            {
                animation.From = new Thickness(500, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back)
            {
                animation.From = new Thickness(0, 0, 500, 0);
            }
            (e.Content as Page).BeginAnimation(MarginProperty, animation);*/
        }
    }
}
