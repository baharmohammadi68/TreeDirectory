using System;
using System.Collections.Generic;
using System.Text;

namespace _1000WebServersCodingChallenge
{
    public class ServerResponse
    {
        public string Application { get; set; }
        public string Version { get; set; }
        public long Uptime { get; set; }
        public long Request_Count { get; set; }
        public long Error_Count { get; set; }
        public long Success_Count { get; set; }

    }
}
