using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.Network;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4.UDP.DNS;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4;
using Cosmos.HAL;

namespace webkerneltest
{
    public class Kernel : Sys.Kernel
    {

        DnsClient dnsClient;
        TcpClient tcpClient;

        HtmlRenderer htmlRenderer;
        string htmlcode = "";

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");

            using (var xClient = new DHCPClient())
            {
                /** Send a DHCP Discover packet **/
                //This will automatically set the IP config after DHCP response
                xClient.SendDiscoverPacket();
            }

            try
            {

                dnsClient = new DnsClient();
                tcpClient = new TcpClient();

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk("szymekk.pl");

                Address address = dnsClient.Receive();
                dnsClient.Close();

                // tcp
                tcpClient.Connect(address, 80);

                // httpget
                string httpget = "GET /RadianceOS.html HTTP/1.1\r\n" +
                                 "User-Agent: RadianceOS\r\n" +
                                 "Accept: */*\r\n" +
                                 "Accept-Encoding: identity\r\n" +
                                 "Host: szymekk.pl\r\n" +
                                 "Connection: Keep-Alive\r\n\r\n";

                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                // get http response
                var ep = new EndPoint(Address.Zero, 0);
                var data = tcpClient.Receive(ref ep);
                tcpClient.Close();


                string httpresponse = Encoding.ASCII.GetString(data);


                string[] responseParts = httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                if (responseParts.Length == 2)
                {
                    string headers = responseParts[0];
                    string content = responseParts[1];
                    Console.WriteLine(content);
                    htmlcode = content;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        protected override void Run()
        {
            Console.Write("command: ");
            var input = Console.ReadLine().Split(' ');
            switch (input[0].ToLower())
            {

                case "ip":

                    Console.WriteLine(NetworkConfiguration.CurrentAddress.ToString());

                    break;

                case "render":

                    htmlRenderer = new HtmlRenderer(1280, 720);

                    htmlRenderer.Render(htmlcode);
                    break;

                case "request":

                    try
                    {

                        dnsClient = new DnsClient();
                        tcpClient = new TcpClient();

                        dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                        dnsClient.SendAsk("szymekk.pl");

                        Address address = dnsClient.Receive();
                        dnsClient.Close();

                        // tcp
                        tcpClient.Connect(address, 80);

                        // httpget
                        string httpget = $"GET /{input[2]} HTTP/1.1\r\n" +
                                         "User-Agent: RadianceOS\r\n" +
                                         "Accept: */*\r\n" +
                                         "Accept-Encoding: identity\r\n" +
                                         $"Host: {input[1]}\r\n" +
                                         "Connection: Keep-Alive\r\n\r\n";

                        tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                        // get http response
                        var ep = new EndPoint(Address.Zero, 0);
                        var data = tcpClient.Receive(ref ep);
                        tcpClient.Close();


                        string httpresponse = Encoding.ASCII.GetString(data);


                        string[] responseParts = httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                        if (responseParts.Length == 2)
                        {
                            string headers = responseParts[0];
                            string content = responseParts[1];
                            Console.WriteLine(content);
                            htmlcode = content;
                        }

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "pm":

                    try
                    {

                        dnsClient = new DnsClient();
                        tcpClient = new TcpClient();

                        dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                        dnsClient.SendAsk("fuebfewfewf.byethost8.com");

                        Address address = dnsClient.Receive();
                        dnsClient.Close();

                        // tcp
                        tcpClient.Connect(address, 80);

                        // httpget
                        string httpget = $"GET /test.html HTTP/1.1\r\n" +
                                         "User-Agent: RadianceOS\r\n" +
                                         "Accept: */*\r\n" +
                                         "Accept-Encoding: identity\r\n" +
                                         $"Host: fuebfewfewf.byethost8.com\r\n" +
                                         "Connection: Keep-Alive\r\n\r\n";

                        tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                        // get http response
                        var ep = new EndPoint(Address.Zero, 0);
                        var data = tcpClient.Receive(ref ep);
                        tcpClient.Close();


                        string httpresponse = Encoding.ASCII.GetString(data);


                        string[] responseParts = httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                        if (responseParts.Length == 2)
                        {
                            string headers = responseParts[0];
                            string content = responseParts[1];
                            Console.WriteLine(content);
                            htmlcode = content;
                        }

                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
