namespace ChaosMonkey.Infrastructure
{
    using System;
    using System.IO;

    public class ChaosLogger
    {
        private readonly StreamWriter logStream;

        public ChaosLogger(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                logStream = File.AppendText(fileName);
            }
            else
            {
                logStream =new StreamWriter( Console.OpenStandardOutput());
            }
        }

        public void Log(string message)
        {
            var logText = string.Concat(DateTime.Now, " - ", message);
            if (logStream != null)
            {
                logStream.WriteLine(logText);
            }

            Console.WriteLine(message);
        }

        public void Close()
        {
            if (logStream != null)
            {
                logStream.Close();
            }
        }
    }
}
