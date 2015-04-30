using ChaosMonkey.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey
{
    class CommandLineSettings
    {
        public Settings Settings { get; set; }
        public bool ShowHelp { get; set; } = false;
        public string SaveSettingsFile { get; set; } = string.Empty;
        public bool AcceptDisclaimer { get; set; } = false;
        public string LoadSettingsFile { get; set; } = string.Empty;
        public ApplicationMode ApplicationMode { get; set; } = ApplicationMode.Default;
    }
}
