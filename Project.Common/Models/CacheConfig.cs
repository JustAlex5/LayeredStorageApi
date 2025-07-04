using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models
{
    public class CacheConfig
    {
        public string ConnectionString { get; set; }
        public TimeSpan ExperationTime { get; set; }
        public bool UseSsl { get; set; }
        public string PfxPath { get; set; }
        public string PfxPass { get; set; }
        public string HostName { get; set; }
        public string InstanceName { get; set; }

    }
}
