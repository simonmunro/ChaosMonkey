namespace ChaosMonkey.Domain
{
    using System.Collections.Generic;

    public class Ec2Endpoints
    {
        public Ec2Endpoints()
        {
            Endpoints = new Dictionary<string, string>()
                            {
                                { "US-East", "ec2.us-east-1.amazonaws.com" },
                                { "US-West", "ec2.us-west-1.amazonaws.com" },
                                { "EU", "ec2.eu-west-1.amazonaws.com" },
                                { "Asia-Pacific-Singapore", "ec2.ap-southeast-1.amazonaws.com" },
                                { "Asia-Pacific-Japan", "ec2.ap-northeast-1.amazonaws.com" },
                            };
        }

        public Dictionary<string, string> Endpoints { get; private set; }
    }
}
