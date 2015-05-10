namespace ChaosMonkey
{
    using System;
    using Domain;
    using Infrastructure;
    using NDesk.Options;
    enum ApplicationMode
    {
        Default,
        Pluggable
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLineParser parser = new CommandLineParser(args);
            CommandLineSettings commandLineSettings= parser.GetCommandLineSettings();
            var logger = new ChaosLogger(commandLineSettings.Settings.LogFileName);
            try
            {
                LoadSettings(commandLineSettings.LoadSettingsFile, logger);

                if (IsDisclaimerAccepted(commandLineSettings.AcceptDisclaimer, logger))
                {

                }
                else
                {
                    logger.Log("Disclaimer not accepted, exiting");
                    return;
                }
                if (commandLineSettings.ApplicationMode.Equals("Pluggable"))
                {
                }
                else
                {
                    if (!CommandLineValidator.ValidateSettings(commandLineSettings.Settings, logger))
                    {
                        logger.Log("Invalid settings. use '-?' for help on parameters. Exiting.");
                        return;
                    }
                }
                

                Tasks.UnleashChaos(commandLineSettings.Settings, logger);
                SaveSettingsFile(commandLineSettings.SaveSettingsFile, commandLineSettings.Settings, logger);
            }
            finally
            {
                logger.Close();
                if (commandLineSettings.ShowHelp)
                {
                 parser.WriteOptionDescriptions(Console.Out);
                }
            }

        }

        private static bool IsDisclaimerAccepted(bool acceptDisclaimer, ChaosLogger logger)
        {
            if (!acceptDisclaimer)
            {
                if (IsDisclaimerAcceptedViaDialog(logger))
                {
                    logger.Log("Disclaimer accepted.");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                logger.Log("Disclaimer accepted via startup parameters.");
                return true;
            }
        }

        private static bool IsDisclaimerAcceptedViaDialog(ChaosLogger logger)
        {
            Console.WriteLine("WARNING!!! ChaosMonkey is going to break stuff. Press 'D' to indemnify the ChaosMonkey or its authors/contributors to your actions");
            var key = Console.ReadKey();
            Console.WriteLine();
            if (key.KeyChar != 'D')
            {
                return true;
            }
            return false;
        }

        private static void LoadSettings(string loadSettingsFile, ChaosLogger logger)
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
        }

        private static void SaveSettingsFile(string saveSettingsFile, Settings settings, ChaosLogger logger)
        {
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
    }
}
