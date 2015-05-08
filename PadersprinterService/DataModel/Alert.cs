using System.Runtime.Serialization;

namespace PadersprinterService.DataModel
{
    [DataContract]
    public class Alert
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
