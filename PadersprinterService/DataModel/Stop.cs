using System;
using System.Runtime.Serialization;

namespace PadersprinterService
{
    /// <summary>
    /// Haltestelle.
    /// Ist z.B. Teil von http://www.padersprinter.de/internetservice/services/tripInfo/tripPassages
    /// </summary>
    [DataContract]
    public class Stop
    {
        private const double coordFactor = 3600000;

        [DataMember(Name = "id")]
        public long ID { get; set; }
        
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "shortName")]
        public int ShortName { get; set; }

        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "latitude")]
        private int _latitude;

        public double Latitude { get { return _latitude / coordFactor; } }
        
        [DataMember(Name = "longitude")]
        private int _longitude;

        public double Longitude { get { return _longitude / coordFactor; } }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, ShortName);
        }
    }
}
