using Amazon.EC2.Model;
using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey.Monkeys
{
    /// <summary>
    /// Randomly terminate AWS instance.
    /// </summary>
    class EC2Monkey :ParentMonkey
    {
        public EC2Monkey(Settings settings, ChaosLogger logger) : base(settings, logger) { }
        public override void Unleash()
        {
            var random = new Random();
            var ec2Factory = new Ec2Factory(_settings.AwsAccessKey, _settings.AwsSecretKey, _settings.ServiceUrl, _logger);
            _logger.Log(string.Format("Looking for instances in {0} with tag {1}={2}", _settings.ServiceUrl, _settings.Tagkey, _settings.TagValue));

            List<Reservation> instances;
            try
            {
                instances = ec2Factory.ListInstancesByTag(_settings.Tagkey, _settings.TagValue);
            }
            catch (Exception ex)
            {
                _logger.Log("ERROR: " + ex.Message);
                return;
            }

            if (instances.Count == 0)
            {
                _logger.Log("No instances found");
            }
            else
            {
                _logger.Log(string.Format("Found {0} candidate instance(s) for chaos!", instances.Count));
                var victimIndex = random.Next(0, instances.Count);
                var instanceId = instances[victimIndex].RunningInstance[0].InstanceId;
                _logger.Log(string.Format("Randomly chosen instance {0} ({1}) as the chaos victim.", instanceId, instances[victimIndex].RunningInstance[0].PublicDnsName));
                _logger.Log(string.Format("Terminating {0}...", instanceId));

                ec2Factory.TerminateInstance(instanceId);
                _logger.Log(string.Format("{0} terminated", instanceId));
            }
        }
    }
}
