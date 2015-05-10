namespace ChaosMonkey
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using Amazon.EC2.Model;
    using Domain;
    using Infrastructure;
    using ChaosMonkey.Monkeys;

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


        
        

        public static void UnleashChaos(Settings settings, ChaosLogger logger)
        {
            if (settings.Repeat == 0)
            {
                settings.Repeat = 1;
            }
            
            if (settings.Repeat > 1)
            {
                logger.Log(string.Format("Repeating {0} times", settings.Repeat));
            }
            MonkeyKeeper keeper = new MonkeyKeeper(settings, logger, new HardCodedMonkeyListBuilder());
            for (var times = 0; times < settings.Repeat; times++)
            {
                keeper.UnleashRandomMonkey();
                if (settings.Delay <= 0) return;
                logger.Log(string.Format("Waiting {0} ms", settings.Delay));
                System.Threading.Thread.Sleep(settings.Delay);
            }
        }
    }
}
