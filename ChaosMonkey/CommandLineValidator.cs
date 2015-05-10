using ChaosMonkey.Domain;
using ChaosMonkey.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey
{
    class CommandLineValidator
    {
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
    }
}
