using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SimpleWifi;

namespace WiFiReconnecter
{
    public class WiFiReconnecter
    {
        private IList<AccessPoint> oAPs;
        private AccessPoint oAP;

        public WiFiReconnecter(Wifi oWiFi = null)
        {
            WriteToConsole(ConsoleType.ServiceStart);
            WiFi = oWiFi ?? new Wifi();
            oAPs = WiFi?.GetAccessPoints()?.Where(x => x.HasProfile).ToList();
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
                        WriteToConsole(ConsoleType.WiFiReconnectionProblem, oAP.Name);
                        oAP = GetNext(oAPs, oAP);
                        Reconnect(TimeSpan.FromMinutes(1));
                    }
                    else
                        WriteToConsole(ConsoleType.WiFiReconnected);
                }); 
            }
        }

        private bool CheckForProblems()
        {
            oAP = oAPs.FirstOrDefault();

            if(oAP == null)
            {
                WriteToConsole(ConsoleType.ServiceStop);
                return true;
            }

            Reconnect();
            return false;
        }

        private void WriteToConsole(ConsoleType eType, string Parameter = "")
        {
            Console.WriteLine(eType.GetConsoleTypeString(), Parameter);
        }

        private static T GetNext<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return list.FirstOrDefault();
            }
        }
    }
}
