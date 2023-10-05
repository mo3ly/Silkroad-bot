using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRO_INGAME.Http.Models.Stalls
{
    public class StallItem
    {
        public UInt32 ItemID { get; set; }
        public UInt64 Variance { get; set; }
        public short Plus { get; set; }
        public short Advance { get; set; }
        public short Quantity { get; set; }
        public Dictionary<string, UInt64> Price { get; set; }
        public short Slot { get; set; }
        public string Description { get; set; }

    }
}
