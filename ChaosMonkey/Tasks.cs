namespace ChaosMonkey
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using Amazon.EC2.Model;
    using Domain;
    using Infrastructure;

    public class Tasks
    {
        public static void SaveSettings(string filename, Settings settings, ChaosLogger logger)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            try
            {
                TextWriter writer = new StreamWriter(filename);
                serializer.Serialize(writer, settings);
                writer.Close();
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex.Message);
                throw new Exception("Cannot save settings");
            }
        }

        public static Settings LoadSettings(string filename, ChaosLogger logger)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            try
            {
                logger.Log(string.Format("Loading settings from file {0}", filename));
                var fileStream = new FileStream(filename, FileMode.Open);
                var settings = (Settings)serializer.Deserialize(fileStream);
                return settings;
            }
            catch (Exception ex)
            {
                logger.Log("ERROR: " + ex.Message);
                throw new Exception("Cannot load settings");
            }
        }


        private static string ServiceUrlFromEndPointName(string endPointName)
        {
            var ec2Endpoints = new Ec2Endpoints();
            var serviceUrl = string.Empty;
            ec2Endpoints.Endpoints.TryGetValue(endPointName, out serviceUrl);
            return serviceUrl;
        }

        public static bool ValidateSettings(Settings settings, ChaosLogger logger)
        {
            var settingsValid = true;
            if (string.IsNullOrEmpty(settings.ServiceUrl) && !string.IsNullOrEmpty(settings.Ec2Endpoint))
            {
                settings.ServiceUrl = ServiceUrlFromEndPointName(settings.Ec2Endpoint);
                if (string.IsNullOrEmpty(settings.ServiceUrl))
                {
                    logger.Log("ERROR: Cannot find service url for endpoint '" + settings.Ec2Endpoint + "'");
                    settingsValid = false;
                }
            }

            if (string.IsNullOrEmpty(settings.ServiceUrl))
            {
                logger.Log("ERROR: No service URL found");
                settingsValid = false;
            }

            if (string.IsNullOrEmpty(settings.Tagkey))
            {
                logger.Log("ERROR: Tag key needed");
                settingsValid = false;
            }

            if (string.IsNullOrEmpty(settings.TagValue))
            {
                logger.Log("ERROR: Tag value needed");
                settingsValid = false;
            }

            return settingsValid;
        }

        public static void UnleashChaos(Settings settings, ChaosLogger logger)
        {
            var ec2Factory = new Ec2Factory(settings.AwsAccessKey, settings.AwsSecretKey, settings.ServiceUrl, logger);
            var random = new Random();
            if (settings.Repeat == 0)
            {
                settings.Repeat = 1;
            }
            
            if (settings.Repeat > 1)
            {
                logger.Log(string.Format("Repeating {0} times", settings.Repeat));
            }
            
            for (var t = 0; t < settings.Repeat; t++)
            {
                logger.Log(string.Format("Looking for instances in {0} with tag {1}={2}", settings.ServiceUrl, settings.Tagkey, settings.TagValue));

                List<Reservation> instances;
                try
                {
                    instances = ec2Factory.ListInstancesByTag(settings.Tagkey, settings.TagValue);
                }
                catch (Exception ex)
                {
                    logger.Log("ERROR: " + ex.Message);
                    return;
                }

                if (instances.Count == 0)
                {
                    logger.Log("No instances found");
                }
                else
                {
                    logger.Log(string.Format("Found {0} candidate instance(s) for chaos!", instances.Count));
                    var victimIndex = random.Next(0, instances.Count);
                    var instanceId = instances[victimIndex].RunningInstance[0].InstanceId;
                    logger.Log(string.Format("Randomly chosen instance {0} ({1}) as the chaos victim.", instanceId, instances[victimIndex].RunningInstance[0].PublicDnsName));
                    logger.Log(string.Format("Terminating {0}...", instanceId));

                    ec2Factory.TerminateInstance(instanceId);
                    logger.Log(string.Format("{0} terminated", instanceId));
                }

                if (settings.Delay <= 0) continue;

                logger.Log(string.Format("Waiting {0} ms", settings.Delay));
                System.Threading.Thread.Sleep(settings.Delay);
            }
        }
    }
}
