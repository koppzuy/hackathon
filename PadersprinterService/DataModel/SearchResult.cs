using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    /// <summary>
    /// Autocompletion search result.
    /// </summary>
    [DataContract]
    class SearchResult
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        /// <summary>
        /// The ID associated with the search result.
        /// In case of a stop, this is the stop's short name.
        /// In case of a route, this is the route's ID.
        /// </summary>
        [DataMember(Name = "id")]
        public long ID { get; set; }
    }
}
