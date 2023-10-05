using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRO_INGAME.Common.Session
{
    class SessionData
    {
        public Dictionary<uint, string> blockedExchange = new Dictionary<uint, string>();
        public Dictionary<uint, string> blockedParty = new Dictionary<uint, string>();
    }
}
