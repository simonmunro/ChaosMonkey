using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey.Monkeys
{
    /// <summary>
    /// IIS Monkey who randomly restart IIS servers / IIS AppPools in a deployment.
    /// </summary>
    /// <remarks>Move this class to different assembly if it needs extra dll references</remarks>
    public class IISServerMonkey :ParentMonkey
    {
        public IISServerMonkey(Settings settings,ChaosLogger logger) :base(settings,logger)
        {

        }
        public override void Unleash()
        {
            _logger.Log("Restarting local IIS");
            RestartLocalIIS("localhost");
        }
        private void RestartLocalIIS(string serverName)
        {
            new CommandExecuter().ExecuteCommandSync("iisreset");
        }
    }
}
