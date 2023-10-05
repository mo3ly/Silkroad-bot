using SilkroadInformationAPI.Client;
using SRO_INGAME.Common;
using Utility = SRO_INGAME.Common.Utility;

namespace Contollers.GameBot.Logic
{
    public class AutoWalk
    {
        public void randomWalk(int xRange, int yRange)
        {
            int randomX = Utility.RandomNumber(-1 * xRange, xRange);
            int randomY = Utility.RandomNumber(-1 * yRange, yRange);

            if (Utility.RandomNumber(-1 * xRange, xRange) < 0)
            {
                System.Console.WriteLine("[Positive] Randomlly Generated X:{0} and Y:{1}", Client.Position.GetRealX() + randomX, Client.Position.GetRealY() + randomY);

                SRCommon.game.WalkTo(Client.Position.GetRealX() + randomX, Client.Position.GetRealY() + randomY);
            }
            else
            {
                System.Console.WriteLine("[Negative] Randomlly Generated X:{0} and Y:{1}", Client.Position.GetRealX() - randomX, Client.Position.GetRealY() - randomY);
                SRCommon.game.WalkTo(Client.Position.GetRealX() - randomX, Client.Position.GetRealY() - randomY);
            }

        }
    }
}
