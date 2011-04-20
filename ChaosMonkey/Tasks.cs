using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ChaosMonkey.Domain;

namespace ChaosMonkey
{
    public class Tasks
    {
        public static void SaveSettings(string filename, Settings settings)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, settings);
            writer.Close();
        }

        public static Settings LoadSettings(string filename)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            var fileStream = new FileStream(filename, FileMode.Open);
            return (Settings)serializer.Deserialize(fileStream);
        }


        private static string ServiceUrlFromEndPointName(string endPointName)
        {
            var ec2Endpoints = new Ec2Endpoints();
            var serviceUrl = string.Empty;
            ec2Endpoints.Endpoints.TryGetValue(endPointName, out serviceUrl);
            return serviceUrl;
        }

        public static bool ValidateSettings(Settings settings)
        {
            var settingsValid = true;
            if (string.IsNullOrEmpty(settings.ServiceUrl) && !string.IsNullOrEmpty(settings.Ec2Endpoint))
            {
                settings.ServiceUrl = ServiceUrlFromEndPointName(settings.Ec2Endpoint);
                if (string.IsNullOrEmpty(settings.ServiceUrl))
                {
                    Console.WriteLine("Cannot find service url for endpoint " + settings.Ec2Endpoint);
                    settingsValid = false;
                }
            }

            if (string.IsNullOrEmpty(settings.ServiceUrl))
            {
                Console.WriteLine("No service URL found");
                settingsValid = false;
            }

            return settingsValid;

        }

        public static void UnleashChaos(Settings settings)
        {

        }
    }
}
