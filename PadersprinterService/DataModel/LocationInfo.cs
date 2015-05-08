using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    /// <summary>
    /// The result of a location based query.
    /// </summary>
    [DataContract]
    class LocationInfo
    {
        /// <summary>
        /// The stops found by the query.
        /// </summary>
        [DataMember(Name = "stopPoints")]
        public Stop[] Stops { get; set; }
    }
}
