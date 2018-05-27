using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SimpleWifi;

namespace WiFiReconnecter
{
    public class WiFiReconnecter
    {
        private List<AccessPoint> oAPs;
        private AccessPoint oAP;

        public WiFiReconnecter(Wifi oWiFi = null)
        {
            WriteToConsole(ConsoleType.ServiceStart);
            WiFi = oWiFi ?? new Wifi();
            oAPs = WiFi?.GetAccessPoints();
            oAP = oAPs.Where(x => x.IsConnected)?.FirstOrDefault();

            if(oAP == null && CheckForProblems())
                return;

            WiFi.ConnectionStatusChanged += WiFi_ConnectionStatusChanged;
            WriteToConsole(ConsoleType.WiFiConnected);
        }

        public Wifi WiFi { get; set; }

        private void WiFi_ConnectionStatusChanged(object oSender, WifiStatusEventArgs oArgs)
        {
            if(oArgs.NewStatus == WifiStatus.Disconnected)
                Reconnect();
        }

        private void Reconnect(TimeSpan? WaitTime = null)
        {
            if(oAP != null)
            {
                var r = new AuthRequest(oAP);

                if(WaitTime.HasValue)
                    Thread.Sleep(WaitTime.Value);

                oAP.ConnectAsync(r, false, x =>
                {
                    if(!x)
                    {
                        WriteToConsole(ConsoleType.WiFiReconnectionProblem);
                        Reconnect(TimeSpan.FromMinutes(1));
                    }
                    else
                        WriteToConsole(ConsoleType.WiFiReconnected);
                }); 
            }
        }

        private bool CheckForProblems()
        {
            oAP = oAPs.Where(x => x.HasProfile).FirstOrDefault();

            if(oAP == null)
            {
                WriteToConsole(ConsoleType.ServiceStop);
                return true;
            }

            Reconnect();
            return false;
        }

        private void WriteToConsole(ConsoleType eType)
        {
            var Now = DateTime.Now.ToLocalTime();
            var TimeStamp = $"{Now.ToShortDateString()} - {Now.ToLongTimeString()}";
            switch(eType)
            {
                case ConsoleType.ServiceStart: Console.WriteLine($"{TimeStamp}: WiFi Reconnecter service started...");  break;
                case ConsoleType.ServiceStop: Console.WriteLine($"{TimeStamp}: Not connected to a wifi access point and can't connect to one. Service stopped."); break;
                case ConsoleType.WiFiConnected: Console.WriteLine($"{TimeStamp}: WiFi is connected!");  break;
                case ConsoleType.WiFiReconnected: Console.WriteLine($"{TimeStamp}: WiFi reconnected"); break;
                case ConsoleType.WiFiReconnectionProblem: Console.WriteLine($"{TimeStamp}: Reconnection problem! Trying again in 1 minute..."); break;
            }
        }
    }
}