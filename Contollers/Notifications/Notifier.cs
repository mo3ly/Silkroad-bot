using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Contollers.Notifications
{
    public class Notifier
    {
        public static void ItemNotificaion(string icon)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                new SRO_INGAME.View.subView.SoxNotify(icon);
            });
        }

        public static void BannerNotification(string title, string message, int delay = 100)
        {

            Application.Current.Dispatcher.Invoke((Action)delegate {
                new SRO_INGAME.View.subView.Notify(title, message, delay);
            });
        }
    }
}
