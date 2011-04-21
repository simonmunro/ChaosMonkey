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
        private ChaosLogger logger;

        public Ec2Factory(string AWSAccessKey, string AWSSecretKey, string serviceUrl, ChaosLogger logger)
        {
            this.logger = logger;
            if (!serviceUrl.Contains("http://"))
            {
                serviceUrl = "http://" + serviceUrl;
            }

            amazonEc2 = AWSClientFactory.CreateAmazonEC2Client(
                AWSAccessKey, 
                AWSSecretKey,
                new AmazonEC2Config().WithServiceURL(serviceUrl));
        }

        public List<Reservation> ListAllInstances()
        {
            var describeInstancesRequest = new DescribeInstancesRequest();
            var describeInstancesResponse = amazonEc2.DescribeInstances(describeInstancesRequest);
            return describeInstancesResponse.DescribeInstancesResult.Reservation;
        }

        public List<Reservation> ListInstancesByTag(string tagKey, string tagValue)
        {
            var describeInstancesRequest = new DescribeInstancesRequest()
            {
                Filter = new List<Filter> 
							{ 
								new Filter() 
									{ 
										Name = "tag:" + tagKey,
										Value = new List<string> 
											{ 
												tagValue 
											},
									},
								new Filter() 
									{ 
										Name = "instance-state-name",
										Value = new List<string> 
											{ 
												"running" 
											},
									}
							}
            };
            try
            {
                var describeInstancesResponse = amazonEc2.DescribeInstances(describeInstancesRequest);
                return describeInstancesResponse.DescribeInstancesResult.Reservation;
            }
            catch (Exception ex)
            {
                logger.Log("AWS ERROR: " + ex.Message);
                throw new Exception("Cannot list instances");
            }
            
        }

        public void TerminateInstance(string instanceId)
        {
            TerminateInstancesRequest terminateInstancesRequest = new TerminateInstancesRequest()
                                                                      {
                                                                          InstanceId = new List<string>() { instanceId }
                                                                      };
            var terminateInstancesResponse = amazonEc2.TerminateInstances(terminateInstancesRequest);
        }
        
    }
}
