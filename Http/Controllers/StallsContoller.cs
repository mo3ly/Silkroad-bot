using SRO_INGAME.Common;
using SRO_INGAME.Http.Models.Stalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRO_INGAME.Http.Controllers
{
    class StallsContoller
    {
        public void LoadStalls()
        {
            try
            {
                // prepare the data so you can sort the items
                SRCommon.CurrentStalls = null;
                SRCommon.CurrentStalls = SRCommon.ClientAPI.RetrieveData<Stalls>("stalls").Result;
            } catch { }
        }
    }
}
