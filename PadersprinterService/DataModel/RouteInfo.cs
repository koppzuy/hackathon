using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    /// <summary>
    /// Contains a collection of routes.
    /// </summary>
    [DataContract]
    class RouteInfo
    {
        /// <summary>
        /// The routes.
        /// </summary>
        [DataMember(Name = "routes")]
        public Route[] Routes { get; set; }
    }
}
