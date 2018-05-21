using System.Threading.Tasks;

namespace WiFiReconnecter
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => new WiFiReconnecter());
            while(true);
        }
    }
}