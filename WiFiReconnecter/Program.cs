using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace WiFiReconnecter
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => new WiFiReconnecter());

            while(true);

            //while(true)
            //{
            //    if(!PingHost("8.8.8.8") && !PingHost("8.8.8.8") && !PingHost("8.8.8.8"))
            //    {
            //        //var cmd = $"wlan connect profile=\"{}\" ssid=\"{}\"";
            //        var proc = new Process();

            //        proc.StartInfo.FileName = "netsh.exe";
            //        proc.StartInfo.Arguments = cmd;
            //        proc.StartInfo.UseShellExecute = false;
            //        proc.StartInfo.RedirectStandardOutput = true;
            //        proc.Start();

            //        Console.WriteLine(proc.StandardOutput.ReadToEnd());
            //    }

            //    var oT = TimeSpan.FromMinutes(5);

            //    Console.WriteLine($"Connected, trying again in {oT.ToString()}");

            //    // Wait 5 minutes
            //    Thread.Sleep(oT);
            //}
        }

        //public static bool PingHost(string nameOrAddress)
        //{
        //    var pingable = false;
        //    var pinger = new Ping();

        //    try
        //    {
        //        var reply = pinger.Send(nameOrAddress);
        //        pingable = reply.Status == IPStatus.Success;
        //    }
        //    catch(PingException)
        //    {
        //        // ignore
        //    }

        //    return pingable;
        //}
    }
}
