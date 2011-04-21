namespace ChaosMonkey.Domain
{
    public class Settings
    {
        public string AwsAccessKey { get; set; }
        public string AwsSecretKey { get; set; }
        public string Ec2Endpoint { get; set; }
        public string Tagkey { get; set; }
        public string TagValue { get; set; }
        public string ServiceUrl { get; set; }
        public int Repeat { get; set; }
        public int Delay { get; set; }
        public bool AcceptedDisclaimer { get; set; }
        public string LogFileName { get; set; }
    }
}
