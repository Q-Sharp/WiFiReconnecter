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
            Console.WriteLine("WiFi Reconnecter service running...");
            WiFi = oWiFi ?? new Wifi();
            oAPs = WiFi?.GetAccessPoints();
            oAP = oAPs.Where(x => x.IsConnected)?.FirstOrDefault();

            if(oAP == null && CheckForProblems())
                return;

            WiFi.ConnectionStatusChanged += WiFi_ConnectionStatusChanged;
            Console.WriteLine("WiFi is connected!");
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
                        Console.WriteLine("Reconnection problem! Trying again in 1 minute...");
                        Reconnect(TimeSpan.FromMinutes(1));
                    }
                    else
                        Console.WriteLine("reconnected");
                }); 
            }
        }

        private bool CheckForProblems()
        {
            oAP = oAPs.Where(x => x.HasProfile).FirstOrDefault();

            if(oAP == null)
            {
                Console.WriteLine("Not connected to a wifi access point and can't connect to one. Service stopped.");
                return true;
            }

            Reconnect();
            return false;
        }
    }
}