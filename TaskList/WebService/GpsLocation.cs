using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TaskList.WebService
{
    [DataContract]
    class GpsLocation
    {
        [DataMember(Name = "resourceSets")]
        public List<ResourceSets> ResourceSets { get; set; }
    }

    [DataContract]
    class ResourceSets
    {
        [DataMember(Name = "estimatedTotal")]
        public int EstimatedTotal { get; set; }
        [DataMember(Name="resources")]
        public List<Resources> Resources { get; set; }
    }

    [DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1", Name="Location")]
    class Resources
    {
        [DataMember(Name = "name")]
        public string Address { get; set; }
    }
}
