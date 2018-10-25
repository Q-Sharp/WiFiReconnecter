using System;

namespace WiFiReconnecter
{
    public static class ConsoleTypeHelper
    {
        public static string GetConsoleTypeString(this ConsoleType ct, string pr = "")
        {
            string r = string.Empty;
            var ots = GetTimeStamp();

            switch(ct)
            {
                case ConsoleType.ServiceStart: r = $"{ots}: WiFi Reconnecter service started..."; break;
                case ConsoleType.ServiceStop: r = $"{ots}: Not connected to a wifi access point and can't connect to one. Service stopped."; break;
                case ConsoleType.WiFiConnected: r = $"{ots}: WiFi {pr} is connected!"; break;
                case ConsoleType.WiFiReconnected: r = $"{ots}: WiFi {pr} reconnected"; break;
                case ConsoleType.WiFiReconnectionProblem: r = $"{ots}: Reconnection problem! Trying again in 1 minute..."; break;
            }

            return r;
        }

        private static string GetTimeStamp()
        {
            var Now = DateTime.Now.ToLocalTime();
            return $"{Now.ToShortDateString()} - {Now.ToLongTimeString()}";
        }
    }
}
