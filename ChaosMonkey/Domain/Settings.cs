namespace ChaosMonkey.Domain
{
    public class SettingsBase
    {
        public int Repeat { get; set; }
        public int Delay { get; set; }
        public bool AcceptedDisclaimer { get; set; }
        public string LogFileName { get; set; }
    }
    public class Settings : SettingsBase
    {
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string Ec2Endpoint { get; set; }
        public string Tagkey { get; set; }
        public string TagValue { get; set; }
        public string ServiceUrl { get; set; }
    }
}
