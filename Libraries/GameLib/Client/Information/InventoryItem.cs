using System;
using System.Collections.Generic;

namespace SilkroadInformationAPI.Client.Information
{
    [Serializable]
    public class InventoryItem : Media.DataInfo.Item
    {
        public int Stack;
        public int PlusValue;
        public int Slot;
        public ulong Price;
        public bool HasAdvance;
        public Dictionary<ItemBlues, int> Blues;
        public Dictionary<string, int> Stats;

        public InventoryItem(uint ID)
        {
            //NOTE: flag from magic pop will cause an error while parsing because it has 0 blue stats while it has str and int // MAGIC POP HAS PROBLEM
            // if doesn't contain key return dummy object to aviod skipping the parsing process
            Media.DataInfo.Item mediaItem;
            if (Media.Data.MediaItems.ContainsKey(ID)) // to do keep checking item that cause a problem
            {
                if (ID == 9238) // fix magic pop -> replace it with potion
                    mediaItem = Media.Data.MediaItems[63]; // replace it with fire works since it not used 
                else
                    mediaItem = Media.Data.MediaItems[ID];
            }
            else // print couldn't find key //Console.WriteLine(Media.Data.MediaItems[4].TranslationName);//.Where(x => x.Value.Type == ItemType.Weapon).First().Value.MediaName);
            {
                mediaItem = Media.Data.MediaItems[63];// replace it with fire works since it not used 
                Console.WriteLine("[InventoryItem]Cannot add find media line: id {0}", ID);
            }
            MediaName = mediaItem.MediaName;
            TranslationName = mediaItem.TranslationName;
            ObjRefID = ID;
            Type = mediaItem.Type;
            Classes = mediaItem.Classes;
            Degree = mediaItem.Degree;
            Cooldown = mediaItem.Cooldown;
            Duration = mediaItem.Duration;
            MaxStack = mediaItem.MaxStack;
            IconPath = mediaItem.IconPath;
            Level = mediaItem.Level;
            // Console.WriteLine("[InventoryItem] Error on finding media ID: "+ ID);

            Stack = 1;
            PlusValue = 0;
            Slot = 0;
            HasAdvance = false;
            Blues = new Dictionary<ItemBlues, int>();
            Stats = new Dictionary<string, int>();
        }

        public InventoryItem Clone()
        {
            return (InventoryItem)this.MemberwiseClone();
        }
    }
}
