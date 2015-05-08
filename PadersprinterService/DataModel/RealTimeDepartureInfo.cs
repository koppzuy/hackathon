using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    [DataContract]
    public class RealTimeDepartureInfo
    {
        [DataMember(Name = "actual")]
        public ActualInfo[] Actual { get; set; }

        [DataMember(Name = "old")]
        public ActualInfo[] Old { get; set; }

        [DataMember(Name = "routes")]
        public Route[] Routes { get; set; }

        [DataMember(Name = "stopName")]
        public string StopName { get; set; }

        [DataMember(Name = "stopShortName")]
        public string StopShortName { get; set; }

        [DataMember(Name = "generalAlerts")]
        public Alert[] GeneralAlerts { get; set; }
    }
}
