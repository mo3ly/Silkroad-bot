﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SilkroadSecurityApi;

namespace SilkroadInformationAPI.Client.Packets.Entity
{
    public class EntitySelected
    {
        public static void Parse(Packet p)
        {
            byte success = p.ReadUInt8();
            if (success == 1)
            {
                Client.SelectedUniqueID = p.ReadUInt32();
                if (Client.NearbyMobs.ContainsKey(Client.SelectedUniqueID))
                {
                    Console.WriteLine("ENTITY SELECTED " + Client.SelectedUniqueID + " : "  + Client.NearbyMobs[Client.SelectedUniqueID].Rarity);
                }
            }
        }
    }
}
