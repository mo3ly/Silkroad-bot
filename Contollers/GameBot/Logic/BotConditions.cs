using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contollers.GameBot.Logic
{
    public class BotConditions
    {
        bool isDebug = true;

        public void Initialise()
        {
            Conditions();
        }

        private void Conditions()
        {
            if (SRCommon.isTeleporting || SilkroadInformationAPI.Media.Data.MediaRegions[(uint)Client.Position.RegionID].SafeZone) // fix this to work with reverse // Client.Position.RegionID //Client.State.Buffs use this for buffs
                SRCommon.botController.StopBotting(1);

            if (Client.Info.CurrentHP == 0) // fix this when die //Client.State.LifeState
            {
                SRCommon.botController.StopBotting(2);
                // return to town or use respawn silk scroll
            }

            if (SRCommon.botController.isWalking)
            {
                SRCommon.botController.isWalking = false;
                SRCommon.botController.botThread.Interval = TimeSpan.FromMilliseconds(1000);
            }

            // if same position for many times or recieved obstcle message walk // maybe we can try Client.NearbyStructures

            // use buffs
            if (BotData.BuffSkills.Count > 0 && SRCommon.botController.buffsDelay >= 60)
            {
                if(isDebug)
                    Console.WriteLine("[AUTO BUFF] Entered Auto buff Loop");
                SRCommon.botController.autoBuff.ApplyBuffs();
                SRCommon.botController.buffsDelay = 0;
            }
        }
    }
}
