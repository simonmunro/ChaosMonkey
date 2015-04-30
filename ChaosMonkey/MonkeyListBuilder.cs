using System;
using System.Collections.Generic;
using ChaosMonkey.Monkeys;
using ChaosMonkey.Infrastructure;
using ChaosMonkey.Domain;

namespace ChaosMonkey
{
    abstract public class MonkeyListBuilder
    {
        public MonkeyListBuilder()
        {
        }
        /// <summary>
        /// Gives the list of Monkeys
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public abstract IList<ParentMonkey> GetMonkeys(Settings settings,ChaosLogger logger);
    }
}