using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Color = System.Windows.Media.Color;

namespace SRO_INGAME.Common
{
    public class SlotToolTip
    {
        StackPanel ToolTipStack;
        public SlotToolTip()
        {
            ToolTipStack = new StackPanel();
        }

        public ToolTip Content()
        {
            ToolTip ToolTip = new ToolTip()
            {
                Content = ToolTipStack
            };
            return ToolTip;
        }

        public void ItemToolTipIcon()
        {
            // add item tooltip circle
            System.Windows.Controls.Image icon = new System.Windows.Controls.Image()
            {
                Source = Utility.PK2GetImage("com_itemsign.ddj"),
                Margin = new Thickness(0, -4, 0, -22),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            ToolTipStack.Children.Add(icon);
        }

        public void WriteLine(string content, MType color = MType.Normal, bool isBold = false)
        {
            // add drop shadow Color="#000000" BlurRadius="1" Direction="-20" ShadowDepth="1"
            SolidColorBrush fColor = null;
            switch (color)
            {
                case (MType.Title):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case (MType.SealTitle):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 255, 217, 83));
                    break;
                case (MType.Seal):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 255, 217, 83));
                    break;
                case (MType.SubSeal):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 108, 230, 117));
                    break;
                case (MType.Info):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 239, 218, 167));
                    break;
                case (MType.Normal):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
                case (MType.Blues):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 0, 234, 255));
                    break;
                case (MType.Warning):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 255, 47, 81));
                    break;
                case (MType.Advance):
                    fColor = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                    break;
                default:
                    fColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    break;
            }

            Label Line = new Label() {
                FontSize = 12,
                Content = content,
                Margin = new Thickness(-4),
                Foreground = fColor,
                Effect = new DropShadowEffect
                {
                    Color = new Color { A = 255, R = 0, G = 0, B = 0 },
                    Direction = -20,
                    BlurRadius = 1,
                    ShadowDepth = 1,
                    Opacity = 1,
                }
            };

            if (color == MType.Title)
                Line.Margin = new Thickness(14, -4, 0, 0);

            if (isBold)
                Line.FontWeight = FontWeights.Bold;

            ToolTipStack.Children.Add(Line);
        }

        public void WriteSpace()
        {
            Label Line = new Label()
            {
                Content = " ",
                Margin = new Thickness(-4),
            };
            ToolTipStack.Children.Add(Line);
        }

        public enum MType
        {
            Normal,
            Title,
            SealTitle,
            Seal,
            SubSeal,
            Blues,
            Info,
            Advance,
            Warning // durabillity
        }
    }
}
