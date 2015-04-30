using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using ChaosMonkey.Monkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey
{
    public class MonkeyKeeper
    {
        IList<ParentMonkey> _monkeys;
        Settings _settings;
        ChaosLogger _logger;
        MonkeyListBuilder _builder;
        Random random;
        public MonkeyKeeper(Settings setting,ChaosLogger logger, MonkeyListBuilder listBuilder) {
            _settings = setting;
            _logger = logger;
            _builder = listBuilder;
        }
        public void UnleashRandomMonkey()
        {
            BuildMonkeysListAndInitializeRandomSeedIfNotDoneAlready();
            int nextRandomNumber = random.Next(_monkeys.Count);
            ParentMonkey monkey = _monkeys[nextRandomNumber];
            _logger.Log(string.Format("Unleashing random Monkey named {0}",monkey));
            monkey.Unleash();
        }

        private void BuildMonkeysListAndInitializeRandomSeedIfNotDoneAlready()
        {
            if (_monkeys == null)
            {
                _logger.Log("Building Monkey's List");
                _monkeys = _builder.GetMonkeys(_settings, _logger);
                _logger.Log("Initializing Random as the Monkey's count changes");
                random = new Random(_monkeys.Count);
            }
        }
    }
}
