using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChaosMonkey.Domain;
using NDesk.Options;

namespace ChaosMonkey
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Settings();
            var showHelp = false;
            var saveSettingsFile = string.Empty;
            var acceptDisclaimer = false;

            var options = new OptionSet()
                                    {
                                        {
                                            "a=|awsaccesskey=",
                                            "Access key of AWS IAM user that can list and terminate instances",
                                            x => settings.AwsAccessKey = x
                                            },
                                        {
                                            "d=|delay=",
                                            "Delay (milliseconds) before chaos is unleashed again (if repeat option set)",
                                            (int x) => settings.Delay = x
                                            },
                                        {
                                            "D|acceptdisclaimer",
                                            "Chaos Monkey is designed to break stuff, setting this option means that you acknowledge this",
                                            x => acceptDisclaimer = x != null
                                            },
                                        {
                                            "e=|endpoint=",
                                            "AWS endpoint name (US-East, US-West, EU, Asia-Pacific-Singapore, Asia-Pacific-Japan)",
                                            x => settings.Ec2Endpoint = x
                                            },
                                        {
                                            "h|?|help",
                                            "Show help (this screen)",
                                            x => showHelp = x != null
                                            },
                                        {
                                            "i=|loadsettings=", "Load settings xml file", x => settings = Tasks.LoadSettings(x)
                                            },
                                        {
                                            "l=|log=", "Save log to file", x => settings.LogFileName = x
                                            },
                                        {
                                            "o=|savesettings=", "Save settings to xml file", x => saveSettingsFile = x
                                            },
                                        {
                                            "r=|repeat=",
                                            "Number of times chaos is unleashed (default 1)",
                                            (int x) => settings.Repeat = x
                                            },
                                        {
                                            "s=|awssecretkey=",
                                            "Access key of AWS IAM user that can list and terminate instances",
                                            x => settings.AwsSecretKey = x
                                            },
                                        {
                                            "S=|serviceurl=",
                                            "URL of EC2 service endpoint (use e|endpoint to use defaults)",
                                            x => settings.ServiceUrl = x
                                            },
                                        {
                                            "t=|tagkey=", "Key of Tag that will be search for in instances e.g. if EC2 tag is chaos=1, ChaosMonkey TagKey=chaos",
                                            x => settings.Tagkey = x
                                            },
                                        {
                                            "v=|tagvalue=", "Value of Tag that will be search for in instances e.g. if EC2 tag is chaos=1, ChaosMonkey TagValue=1",
                                            x => settings.Tagkey = x
                                            }

                                    };

            options.Parse(args);


            if (!acceptDisclaimer)
            {
                Console.WriteLine("WARNING!!! Chaos Monkey is going to break stuff. Press 'D' to indemnify the Chaos Monkey or its authors/contributors to your actions");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.KeyChar != 'D')
                {
                    Console.WriteLine("Cannot accept that, exiting");
                    return;
                }
            }

            if (!Tasks.ValidateSettings(settings))
            {
                Console.WriteLine("Invalid settings. Exiting.");
            }

            if (!string.IsNullOrEmpty(saveSettingsFile))
            {
                Tasks.SaveSettings(saveSettingsFile, settings);
            }
            
            if (showHelp)
            {
                options.WriteOptionDescriptions(Console.Out);
            }
            Console.ReadKey();
        }



    }
}
