using Contollers.GameBot;
using ImageLibrary.GDImageLibrary;
using System;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace SRO_INGAME.Common
{
    public class Utility
    {
        public static int RandomNumber(int min, int max)
        {
            return BotData.random.Next(min, max);
        }

        public static ImageSource PK2GetImage(string name)
        {
            try
            {
                if(SRCommon.Images.ContainsKey(name))
                    return SRCommon.Images[name];
                else
                {
                    byte[] imageBytes = SRCommon.PK2.GetFileBytes(name);
                    ArraySegment<byte> toDDS = new ArraySegment<byte>(imageBytes, 20, imageBytes.Length - 20);
                    System.Drawing.Bitmap srcImage = _DDS.LoadImage(toDDS.ToArray());
                    SRCommon.Images.Add(name, ExternalDLL.ImageSourceFromBitmap(srcImage));
                    return ExternalDLL.ImageSourceFromBitmap(srcImage);
                }
            }
            catch { return default; }
        }

        public static ImageSource PK2GetImageByURL(string url)
        {
            try
            {
                if (SRCommon.Images.ContainsKey(url))
                    return SRCommon.Images[url];
                else
                {

                    string[] path = url.Split('\\');
                    byte[] imageBytes = SRCommon.PK2.GetFileBytes(Path.GetFileName(url), path[path.Length - 3], path[path.Length - 2]);
                    ArraySegment<byte> toDDS = new ArraySegment<byte>(imageBytes, 20, imageBytes.Length - 20);
                    System.Drawing.Bitmap srcImage = _DDS.LoadImage(toDDS.ToArray());
                    SRCommon.Images.Add(url, ExternalDLL.ImageSourceFromBitmap(srcImage));
                    return ExternalDLL.ImageSourceFromBitmap(srcImage);
                }
            }
            catch { return default; }
        }
    }
}
