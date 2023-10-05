using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRO_INGAME.Http.Models.Stalls
{
    class CharStall
    {
        public string Owner { set; get; }
        public string Location { set; get; }
        public string Date { set; get; }
        public List<StallItem> Items { set; get; }

    }
}
