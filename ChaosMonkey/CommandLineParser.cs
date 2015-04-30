using ChaosMonkey.Domain;
using NDesk.Options;
using System;
using System.IO;

namespace ChaosMonkey
{
    internal class CommandLineParser
    {
        OptionSet _options;
        string[] _args;
        internal CommandLineParser(string [] args)
        {
            _args = args;
        }
        internal CommandLineSettings GetCommandLineSettings()
        {
            var commandLineSettings = new CommandLineSettings() { Settings = new Settings() };

            _options = new OptionSet()
                                    {
                                        {
                                            "a=|awsaccesskey=",
                                            "Access key of AWS IAM user that can list and terminate instances",
                                            x =>commandLineSettings.Settings.AwsAccessKey = x
                                            },
                                        {
                                            "d=|delay=",
                                            "Delay (milliseconds) before chaos is unleashed again (if repeat option set)",
                                            (int x) => commandLineSettings.Settings.Delay = x
                                            },
                                        {
                                            "D|acceptdisclaimer",
                                            "Chaos Monkey is designed to break stuff, setting this option means that you acknowledge this",
                                            x => commandLineSettings.AcceptDisclaimer = x != null
                                            },
                                        {
                                            "e=|endpoint=",
                                            "AWS endpoint name (US-East, US-West, EU, Asia-Pacific-Singapore, Asia-Pacific-Japan)",
                                            x => commandLineSettings.Settings.Ec2Endpoint = x
                                            },
                                        {
                                            "h|?|help",
                                            "Show help (this screen)",
                                            x =>commandLineSettings.ShowHelp = x != null
                                            },
                                        {
                                            "i=|loadsettings=", "Load settings xml file", x => commandLineSettings.LoadSettingsFile = x
                                            },
                                        {
                                            "l=|log=", "Save log to file", x => commandLineSettings.Settings.LogFileName = x
                                            },
                                        {
                                            "o=|savesettings=", "Save settings to xml file",
                                            x => commandLineSettings.SaveSettingsFile = x
                                            },
                                        {
                                            "r=|repeat=",
                                            "Number of times chaos is unleashed (default 1)",
                                            (int x) => commandLineSettings.Settings.Repeat = x
                                            },
                                        {
                                            "s=|awssecretkey=",
                                            "Access key of AWS IAM user that can list and terminate instances",
                                            x => commandLineSettings.Settings.AwsSecretKey = x
                                            },
                                        {
                                            "S=|serviceurl=",
                                            "URL of EC2 service endpoint (use e|endpoint to use defaults)",
                                            x => commandLineSettings.Settings.ServiceUrl = x
                                            },
                                        {
                                            "t=|tagkey=", "Key of Tag that will be search for in instances e.g. if EC2 tag is chaos=1, ChaosMonkey TagKey=chaos",
                                            x => commandLineSettings.Settings.Tagkey = x
                                            },
                                        {
                                            "v=|tagvalue=", "Value of Tag that will be search for in instances e.g. if EC2 tag is chaos=1, ChaosMonkey TagValue=1",
                                            x => commandLineSettings.Settings.TagValue = x
                                            },
                                        {
                                            "m=|mode=",
                                            "Application mode which decides whether to run in AWS only mode or load plugins.eg. m=Pluggable for loading other plugins",
                                            (ApplicationMode x)=>commandLineSettings.ApplicationMode=x
                                        }
                                    };

            _options.Parse(_args);
            return commandLineSettings;
        }

        internal void WriteOptionDescriptions(TextWriter writer)
        {
            _options.WriteOptionDescriptions(writer);
        }
    }
}