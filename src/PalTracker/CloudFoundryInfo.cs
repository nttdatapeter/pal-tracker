
namespace PalTracker
{
    public class CloudFoundryInfo 
    {
        public string Port {get; set;}
        public string MemoryLimit {get; set;}
        public string CfInstanceIndex {get; set;}
        public string CfInstanceAddr {get; set;}

        public CloudFoundryInfo(string port, string memoryLimit,
                                string cfInstanceIndex, string cfInstanceAddr      )
        {
            Port = port;
            MemoryLimit = memoryLimit;
            CfInstanceAddr = cfInstanceAddr;
            CfInstanceIndex = cfInstanceIndex;
        }
    }
}