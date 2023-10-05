using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contollers.Notifications;
using SilkroadInformationAPI.Client.Information.BasicInfo;
using SilkroadSecurityApi;

namespace SilkroadInformationAPI.Client.Actions
{
    public class Utility
    {
        public static Dictionary<string, int> CalculateWhiteStats(ulong variance, ItemType type) //By stratii
        {
            Dictionary<string, int> whiteStats = new Dictionary<string, int>();

            var stats = new List<byte>();

            while (variance > 0)
            {
                var stat = (byte)(variance & 0x1f);
                variance >>= 5;

                stats.Add(stat);
            }

            try
            {
                switch (type)
                {
                    case ItemType.Shield:
                        whiteStats.Add("Durability", stats[0]);
                        whiteStats.Add("Phy. reinforce", stats[1]);
                        whiteStats.Add("Mag. reinforce", stats[2]);
                        whiteStats.Add("Blocking rate", stats[3]);
                        whiteStats.Add("Phy. def. pwr.", stats[4]);
                        whiteStats.Add("Mag. def. pwr.", stats[5]);
                        break;
                    case ItemType.Protector:
                        whiteStats.Add("Durability", stats[0]);
                        whiteStats.Add("Phy. reinforce", stats[1]);
                        whiteStats.Add("Mag. reinforce", stats[2]);
                        whiteStats.Add("Phy. def. pwr.", stats[3]);
                        whiteStats.Add("Mag. def. pwr.", stats[4]);
                        whiteStats.Add("Parry rate", stats[5]);
                        break;
                    case ItemType.Weapon:
                        whiteStats.Add("Durability", stats[0]);
                        whiteStats.Add("Phy. reinforce", stats[1]);
                        whiteStats.Add("Mag. reinforce", stats[2]);
                        whiteStats.Add("Attack rating", stats[3]);
                        whiteStats.Add("Phy. atk. pwr.", stats[4]);
                        whiteStats.Add("Mag. atk. pwr.", stats[5]);
                        whiteStats.Add("Critical", stats[6]);
                        break;
                    case ItemType.Accessory:
                        whiteStats.Add("Phy. absorption", stats[0]);
                        whiteStats.Add("Mag. absorption", stats[1]);
                        break;
                }
            } catch
            {

            }
            return whiteStats;

        }

        public static void WalkTo(int X, int Y)
        {
            uint xPos = 0;
            uint yPos = 0;

            if (X > 0 && Y > 0)
            {
                xPos = (uint)((X % 192) * 10);
                yPos = (uint)((Y % 192) * 10);
            }
            else
            {
                if (X < 0 && Y > 0)
                {
                    xPos = (uint)((192 + (X % 192)) * 10);
                    yPos = (uint)((Y % 192) * 10);
                }
                else
                {
                    if (X > 0 && Y < 0)
                    {
                        xPos = (uint)((X % 192) * 10);
                        yPos = (uint)((192 + (Y % 192)) * 10);
                    }
                }
            }

            byte xSector = (byte)((X - (int)(xPos / 10)) / 192 + 135);
            byte ySector = (byte)((Y - (int)(yPos / 10)) / 192 + 92);
            ushort xPosition = (ushort)((X - (int)((xSector - 135) * 192)) * 10);
            ushort yPosition = (ushort)((Y - (int)((ySector - 92) * 192)) * 10);

            var p = new Packet(0x0);

            if(Client.Info.TransportUniqueID == 0)
            {
                p = new Packet(0x7021);
            } else
            {
                p = new Packet(0x70C5);
                p.WriteUInt32(Client.Info.TransportUniqueID);
                p.WriteUInt8(0x01);
            }
            p.WriteUInt8(0x01);
            p.WriteUInt8(xSector);
            p.WriteUInt8(ySector);
            p.WriteUInt16(xPosition);
            p.WriteUInt16(0x0000);
            p.WriteUInt16(yPosition);
            SroClient.RemoteSecurity?.Send(p);
        }

        public static double CalculateRealDistance(Position Object_1, Position Object_2)
        {
            return Math.Sqrt(Math.Pow(Object_2.GetRealX() - Object_1.GetRealX(), 2) + Math.Pow(Object_2.GetRealY() - Object_1.GetRealY(), 2));
        }

        public static double CalculateRealDistance(Position Object_1, int realX, int realY)
        {
            return Math.Sqrt(Math.Pow(realX - Object_1.GetRealX(), 2) + Math.Pow(realY - Object_1.GetRealX(), 2));
        }

        public static double CalculateDistance(Position Object_1, Position Object_2)
        {
            return Math.Sqrt(Math.Pow(Object_2.X - Object_1.X, 2) + Math.Pow(Object_2.Y - Object_1.Y, 2));
        }

        public static int TimeToDistance(int x, int y, int DesX, int DesY)
        {
            float time = 0f;
            float speed = 0; 
            const float PacketToMeterSeconds_Transformation = 0.0768f;

            //if (Global.Player.Transport.UniqueID != 0)
            speed = 90.0f * PacketToMeterSeconds_Transformation;
            //else
            //   speed = Global.Player.Speeds.RunSpeed * Math.PacketToMeterSeconds_Transformation;

            double tx = (DesX - x);
            double ty = (DesY - y);
            double distance = System.Math.Sqrt(tx * tx + ty * ty);

            time = (float)distance / speed;

            decimal intervalTime = (decimal)time;

            Console.WriteLine("| Walk speed " + Client.State.RunSpeed + "+ Time: " + System.Math.Round(intervalTime) + " spot X: " + DesX + " Y: " + DesY + "Des: " + distance);

            return (int)(intervalTime * 1000);
        }

        public static void UseReturn()
        {
            if (Client.State.Returning == false)
            {
                if (!UseItemType(ItemType.ReturnScroll))
                    Notifier.BannerNotification("System", "No return scroll is found!", 40);
                else
                    Notifier.BannerNotification("System", "There character is returing to town!", 40);
            } else
            {
                Notifier.BannerNotification("System", "Client is already returning!", 40);
            }
        }

        public static bool UseItemID(uint ObjRefID)
        {
            if (Client.InventoryItems.Where(x => x.Value.ObjRefID == ObjRefID).Count() > 0)
            {
                SroClient.UseItem(Client.InventoryItems.First(x => x.Value.ObjRefID == ObjRefID).Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UseItemType(ItemType type)
        {
            if (Client.InventoryItems.Where(x => x.Value.Type == type).Count() > 0)
            {
                SroClient.UseItem(Client.InventoryItems.First(x => x.Value.Type == type).Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UseItemMultiType(ItemType type1, ItemType type2)
        {
            if (Client.InventoryItems.Where(x => x.Value.Type == type1 || x.Value.Type == type2).Count() > 0)
            {
                SroClient.UseItem(Client.InventoryItems.First(x => x.Value.Type == type1 || x.Value.Type == type2).Key);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UseItemType(ItemType type, byte otherItemSlot)
        {
            if (Client.InventoryItems.Where(x => x.Value.Type == type).Count() > 0)
            {
                SroClient.UseItem(Client.InventoryItems.First(x => x.Value.Type == type).Key, otherItemSlot);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UseItemType(ItemType type, uint targetUID)
        {
            if (Client.InventoryItems.Where(x => x.Value.Type == type).Count() > 0)
            {
                SroClient.UseItem(Client.InventoryItems.First(x => x.Value.Type == type).Key, targetUID);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool WearItem(ItemType type, byte toSlot)
        {
            if (Client.InventoryItems.Where(x => x.Value.Type == type).Count() > 0)
            {
                SroClient.WearItem(Client.InventoryItems.First(x => x.Value.Type == type).Value.Slot, toSlot);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool UseSkillName(string name)
        {
            if (Client.Skills.Where(x => x.MediaName == name).Count() > 0)
            {
                SroClient.UseSpell(Client.Skills.Single(x => x.MediaName == name).ObjRefID);
                return true;
            }
            else
                return false;
        }

        public static bool UseSkillName(string name, uint target)
        {
            if (Client.Skills.Where(x => x.MediaName == name).Count() > 0)
            {
                SroClient.UseSpell(Client.Skills.Single(x => x.MediaName == name).ObjRefID, target);
                return true;
            }
            else
                return false;
        }

        public static ushort GenerateItemType(uint itemID)
        {
            var item = Media.Data.MediaItems[itemID];
            return (ushort)(1 * item.Classes.A + 2 * item.Classes.B + 4 * item.Classes.C + 32 * item.Classes.D + 128 * item.Classes.E + 2048 * item.Classes.F);
        }
    }
}
