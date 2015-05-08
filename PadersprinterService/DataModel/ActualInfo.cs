using System;
using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    [DataContract]
    public class ActualInfo
    {
        [DataMember(Name = "actualTime")]
        public string ActualTime { get; set; }

        [DataMember(Name = "plannedTime")]
        public string PlannedTime { get; set; }

        [DataMember(Name = "direction")]
        public string Direction { get; set; }

        [DataMember(Name = "mixedTime")]
        public string MixedTime { get; set; }

        [DataMember(Name = "patternText")]
        public string RouteName { get; set; }

        [DataMember(Name = "stop")]
        public Stop Stop { get; set; }

        [DataMember(Name = "vias")]
        public string[] Vias { get; set; }

        [DataMember(Name = "tripId")]
        public long TripID { get; set; }

        public TripStatus Status { get; private set; }

        //[DataMember]
        //private int stop_seq_num;
        //[DataMember]
        //private long passageid;
        //[DataMember]
        //private long routeId;
        [DataMember]
        public string status;
        //[DataMember]
        //private long vehicleId;

        public override string ToString()
        {
            return string.Format("[{0}] (direction: {3}): {1} ({4}, planned {2})", RouteName, MixedTime, PlannedTime, Direction, ActualTime ?? MixedTime);
        }

        public string ToTripInfoString()
        {
            return string.Format("{0} {1} {2}", ActualTime, Status.ToString().PadRight(9, ' '), Stop.Name);
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext e)
        {
            TripStatus s;
            Status = Enum.TryParse<TripStatus>(status, true, out s) ? s : TripStatus.Unknown;
        }
    }
}
