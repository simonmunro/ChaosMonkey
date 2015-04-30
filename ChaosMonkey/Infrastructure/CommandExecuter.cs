using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosMonkey.Infrastructure
{
    class CommandExecuter
    {
        public void ExecuteCommandSync(string command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;

                procStartInfo.CreateNoWindow = true;

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                string result = proc.StandardOutput.ReadToEnd();

                Console.WriteLine(result);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
