using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    [DataContract]
    public class Route
    {
        [DataMember(Name = "id")]
        public long ID { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "shortName")]
        public string ShortName { get; set; }

        [DataMember(Name = "routeType")]
        public string RouteType { get; set; }

        [DataMember(Name = "alerts")]
        public string[] Alerts { get; set; }

        [DataMember(Name = "authority")]
        public string Authority { get; set; }

        [DataMember(Name = "directions")]
        public string[] Directions { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1} ({2})", ShortName, Name, string.Join(" - ", Directions ?? new string[0]));
        }
    }
}
