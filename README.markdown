# .NET Chaos Monkey

The Chaos Monkey originated with Netflix, who [built a Chaos Monkey](http://techblog.netflix.com/2010/12/5-lessons-weve-learned-using-aws.html) in order to test that they can consistently handle failure. Partly because of its name and partly because the idea of randomly terminating production servers is an absurd check of fault tolerate architectures, the Chaos Monkey concept took a life of its own and has become part of AWS legend. Werner Vogels even kicked off a [t-shirt design contest](http://99designs.com/t-shirt-design/contests/design-chaos-monkey-t-shirt-70909/brief).

<img src="http://99designs.com/designs/7480037-original" height="300px" />

This is a .NET implementation of the Chaos Monkey that randomly chooses from *tagged* instances at an endpoint (eg. US, EU etc). The application is a simple console executable that uses the AWS .NET API and has various options available, as listed below:

    -a, --awsaccesskey=VALUE		Access key of AWS IAM user that can list and terminate instances  
    -d, --delay=VALUE				Delay (milliseconds) before chaos is unleashed again (if repeat option set)  
    -D, --acceptdisclaimer          Chaos Monkey is designed to break stuff, setting this option means that you acknowledge this  
    -e, --endpoint=VALUE       		AWS endpoint name (US-East, US-West, EU, Asia-Pacific-Singapore, Asia-Pacific-Japan)  
    -h, -?, --help             		Show help (this screen)  
    -i, --loadsettings=VALUE   		Load settings xml file  
    -l, --log=VALUE            		Save log to file  
    -o, --savesettings=VALUE   		Save settings to xml file  
    -r, --repeat=VALUE         		Number of times chaos is unleashed (default 1)  
    -s, --awssecretkey=VALUE   		Access key of AWS IAM user that can list and terminate instances  
    -S, --serviceurl=VALUE     		URL of EC2 service endpoint (use e|endpoint to use defaults)  
    -t, --tagkey=VALUE         		Key of Tag that will be search for in instances e.g. if EC2 tag is chaos=1, ChaosMonkey -t=chaos -v=1  
    -v, --tagvalue=VALUE       		Value of Tag that will be search for in instances e.g. if EC2 tag is chaos=1, ChaosMonkey -v=1 -t=chaos  
  
The screenshot below illustrates how it runs.

![ScreenShot](https://github.com/simonmunro/ChaosMonkey/blob/master/ScreenShot.png?raw=true)

To install, you can download the binaries, extract the zip file and run ChaosMonkey.exe

*DISCLAIMER*

By design, the Chaos Monkey terminates servers randomly and without prejudice. Nobody can accept any responsibility if you use the Chaos Monkey to shoot yourself in the foot.

[Simon Munro](http://twitter.com/simonmunro)