using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using ChaosMonkey.Monkeys;

namespace ChaosMonkey
{
    internal class HardCodedMonkeyListBuilder :MonkeyListBuilder
    {
        public override IList<ParentMonkey> GetMonkeys(Settings settings, ChaosLogger logger)
        {
            logger.Log("Hard coded MonkyeListProvider returning EC2Monkey");
                return new List<ParentMonkey>() { new EC2Monkey(settings, logger) };
        }
    }
}
