using SilkroadInformationAPI.Media.DataInfo;
using SRO_INGAME.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contollers.GameBot.Logic
{
    /**
     * TODO:
     *  Be aware for the case when the the selected buffs requie two weapon -> easy solution -> run all buff loop with buff weapon and attack weapon check the current used skills to skip quickly
     */
    public class AutoBuff
    {
        public void Initialise()
        {
            ApplyBuffs();
        }


        public async void ApplyBuffs()
        {
            SRCommon.botController.botThread.Stop();
            SRCommon.botController.isUsingBuffs = true;
            foreach (Skill buff in BotData.BuffSkills)
            {
                try
                {
                    Console.WriteLine("[APPLYING BUFF] " + buff.MediaName); // check duration
                    // wear buff weapon and shield
                    SilkroadInformationAPI.Client.Actions.Utility.UseSkillName(buff.MediaName);
                    // wear attack weapon and shield
                    await Task.Delay(1000);
                }
                catch
                {
                    SRCommon.botController.botThread.Start();
                }
            }
            SRCommon.botController.isUsingBuffs = false;
            SRCommon.botController.buffsDelay = 0;
            SRCommon.botController.botThread.Start();
        }
    }
}
