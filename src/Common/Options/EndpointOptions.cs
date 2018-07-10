using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub256.Common.Options
{
    public class EndpointOptions
    {
        public static readonly string SectionName = "Endpoints";

        public string IdentityUrl { get; set; }
    }
}
