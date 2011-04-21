namespace ChaosMonkey
{
    using System;
    using Domain;
    using Infrastructure;
    using NDesk.Options;

    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Settings();
            var showHelp = false;
            var saveSettingsFile = string.Empty;
            var acceptDisclaimer = false;
            var loadSettingsFile = string.Empty;

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
                                            "i=|loadsettings=", "Load settings xml file", x => loadSettingsFile = x
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
                                            x => settings.TagValue = x
                                            }
                                    };

            options.Parse(args);

            var logger = new ChaosLogger(settings.LogFileName);
            try
            {
                if (!string.IsNullOrEmpty(loadSettingsFile))
                {
                    try
                    {
                        Tasks.LoadSettings(loadSettingsFile, logger);
                    }
                    catch (Exception ex)
                    {
                        logger.Log("ERROR: " + ex.Message);
                        return;
                    }
                }

                if (!acceptDisclaimer)
                {
                    Console.WriteLine("WARNING!!! ChaosMonkey is going to break stuff. Press 'D' to indemnify the ChaosMonkey or its authors/contributors to your actions");
                    var key = Console.ReadKey();
                    Console.WriteLine();
                    if (key.KeyChar != 'D')
                    {
                        logger.Log("Disclaimer not accepted, exiting");
                        return;
                    }

                    logger.Log("Disclaimer accepted.");
                }
                else
                {
                    logger.Log("Disclaimer accepted via startup parameters.");
                }

                if (!Tasks.ValidateSettings(settings, logger))
                {
                    logger.Log("Invalid settings. use '-?' for help on parameters. Exiting.");
                    return;
                }

                Tasks.UnleashChaos(settings, logger);

                if (!string.IsNullOrEmpty(saveSettingsFile))
                {
                    logger.Log(string.Format("Saving settings to {0}", saveSettingsFile));
                    try
                    {
                        Tasks.SaveSettings(saveSettingsFile, settings, logger);
                    }
                    catch (Exception ex)
                    {
                        logger.Log("ERROR: " + ex.Message);
                    }
                }
            }
            finally
            {
                logger.Close();
                if (showHelp)
                {
                    options.WriteOptionDescriptions(Console.Out);
                }
            }
            
        }
    }
}
