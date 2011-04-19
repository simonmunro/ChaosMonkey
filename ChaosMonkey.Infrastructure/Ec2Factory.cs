using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace ChaosMonkey.Infrastructure
{
    public class Ec2Factory
    {
        private AmazonEC2 amazonEc2;

        public Ec2Factory(string AWSAccessKey, string AWSSecretKey)
        {
            amazonEc2 = AWSClientFactory.CreateAmazonEC2Client(
                AWSAccessKey, 
                AWSSecretKey,
                new AmazonEC2Config().WithServiceURL("https://eu-west-1.ec2.amazonaws.com"));
        }

        public List<Reservation> ListAllInstances()
        {
            var describeInstancesRequest = new DescribeInstancesRequest();
            var describeInstancesResponse = amazonEc2.DescribeInstances(describeInstancesRequest);
            return describeInstancesResponse.DescribeInstancesResult.Reservation;
        }
    }
}
