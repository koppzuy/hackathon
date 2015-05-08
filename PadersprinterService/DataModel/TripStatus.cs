
namespace PadersprinterService.DataModel
{
    /// <summary>
    /// The status of a travelling vehicle.
    /// </summary>
    public enum TripStatus
    {
        /// <summary>
        /// The vehicle has departed from a stop.
        /// </summary>
        Departed,

        /// <summary>
        /// The vehicle is currently stopping at a stop.
        /// </summary>
        Stopping,

        /// <summary>
        /// The stop time is predicted.
        /// </summary>
        Predicted,

        /// <summary>
        /// The status is unknown.
        /// </summary>
        Unknown
    }
}
