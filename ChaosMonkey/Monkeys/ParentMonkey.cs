using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey.Monkeys
{
    abstract public class ParentMonkey
    {
        protected ChaosLogger _logger;
        protected Settings _settings;
        public ParentMonkey(Settings settings, ChaosLogger logger)
        {
            _logger = logger;
            _settings = settings;
        }
        public abstract void Unleash();
    }
}
