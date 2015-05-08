using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    /// <summary>
    /// Returned by http://www.padersprinter.de/internetservice/services/tripInfo/tripPassages
    /// </summary>
    [DataContract]
    public class TripInfo
    {
        /// <summary>
        /// Route name.
        /// </summary>
        [DataMember(Name = "routeName")]
        public string RouteName { get; set; }

        /// <summary>
        /// Direction name.
        /// </summary>
        [DataMember(Name = "directionText")]
        public string Direction { get; set; }

        /// <summary>
        /// Current and upcoming stops.
        /// </summary>
        [DataMember(Name = "actual")]
        public ActualInfo[] Actual { get; set; }

        /// <summary>
        /// Stops in the past.
        /// </summary>
        [DataMember(Name = "old")]
        public ActualInfo[] Old { get; set; }
    }
}
