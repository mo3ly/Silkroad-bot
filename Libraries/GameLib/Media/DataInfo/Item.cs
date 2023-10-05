using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkroadInformationAPI.Media.DataInfo
{
    public class Item : Base
    {
        public int MaxStack;
        public ItemType Type;
        public SubItemType SubType;
        public int Degree;
        public long Duration;
        public long Cooldown;
        public string IconPath;
        public string Description;
        public int Level;
        public bool SOX = false;
        public bool Plusable = false;

        public Item()
        {
            Degree = 1;
        }
    }
}
