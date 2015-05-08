using PadersprinterService.DataModel;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace PadersprinterService.Parsing
{
    /// <summary>
    /// Provides methods to access the Padersprinter Web API.
    /// </summary>
    public class Parser
    {
        private const string _autoCompleteUrl = "http://www.padersprinter.de/internetservice/services/lookup/autocomplete/json?query=";
        private const string _realtimeInfoUrl = "http://www.padersprinter.de/internetservice/services/passageInfo/stopPassages/stop";
        private const string _geoUrl = "http://www.padersprinter.de/internetservice/geoserviceDispatcher/services/stopinfo/stopPoints";
        private const string _stopInfoUrl = "http://www.padersprinter.de/internetservice/services/stopInfo/stop";
        private const string _stopUrl = "http://www.padersprinter.de/internetservice/geoserviceDispatcher/services/stopinfo/stop";
        private const string _tripInfoUrl = "http://www.padersprinter.de/internetservice/services/tripInfo/tripPassages";
        private const string _routeInfoUrl = "http://www.padersprinter.de/internetservice/services/routeInfo/route";

        private static HttpClient _client;

        static Parser()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip;
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko");
            _client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            _client.DefaultRequestHeaders.Add("Accept", "application/json,text/javascript, text/html, application/xml, text/xml, */*");
            //_client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            _client.DefaultRequestHeaders.Add("Host", "www.padersprinter.de");
            _client.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.8,en-US;q=0.5,en;q=0.3");
        }

        /// <summary>
        /// Finds bus stops matching the specified query text.
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Array of Stop objects</returns>
        public async static Task<Stop[]> FindStopsByNameAsync(string query)
        {
            var response = await _client.GetAsync(new Uri(_autoCompleteUrl + query));
            return (await DeserializeAsync<SearchResult[]>(response))
                .Where(result => result.Type == "stop")
                .Select(result => new Stop { Name = result.Name, ShortName = (int)result.ID })
                .ToArray();
        }

        /// <summary>
        /// Finds bus stops matching the area specified by center location and radius.
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="radius">Rectangular search "radius" in kilometers</param>
        /// <returns>Array of Stop objects</returns>
        public static async Task<Stop[]> FindStopsByLocationAsync(BasicPosition pos, int radius)
        {
            var box = MapExtensions.GetBoundingBox(pos, radius);
            var left = (int)(box.MinPoint.Longitude * 3600000);
            var right = (int)(box.MaxPoint.Longitude * 3600000);
            var top = (int)(box.MaxPoint.Latitude * 3600000);
            var bottom = (int)(box.MinPoint.Latitude * 3600000);
            var content = new StringContent("left=" + left + "&bottom=" + bottom + "&right=" + right + "&top=" + top);
            var response = await _client.PostAsync(_geoUrl, content);
            var locationInfo = await DeserializeAsync<LocationInfo>(response);
            return locationInfo.Stops;
        }

        /// <summary>
        /// Gets a RealTimeDepartureInfo object containing information about
        /// future arrivals or departures of vehicles.
        /// </summary>
        /// <param name="stopShortName">Stop short name</param>
        /// <param name="mode">Direction mode</param>
        /// <returns>RealTimeDepartureInfo</returns>
        public async static Task<RealTimeDepartureInfo> GetRealTimeDepartureInfoAsync(int stopShortName, DirectionMode mode)
        {
            var response = await _client.GetAsync(new Uri(_realtimeInfoUrl + "?stop=" + stopShortName + "&mode=" + mode));
            return await DeserializeAsync<RealTimeDepartureInfo>(response);
        }

        /// <summary>
        /// Gets a RealTimeDepartureInfo object containing information about
        /// future arrivals or departures of vehicles.
        /// </summary>
        /// <param name="stopShortName">Stop short name</param>
        /// <param name="mode">Direction mode</param>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <returns>RealTimeDepartureInfo</returns>
        public async static Task<RealTimeDepartureInfo> GetRealTimeDepartureInfoAsync(int stopShortName, DirectionMode mode, DateTime startTime, DateTime endTime)
        {
            var time = ((long)startTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds) * 1000;
            var buster = ((long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds) * 1000;
            var content = new StringContent("stop=" + stopShortName + "&mode=" + mode + "&startTime=" + time + "&timeFrame=" + (endTime - startTime).TotalMinutes + "&cacheBuster=" + buster);
            var response = await _client.PostAsync(new Uri(_realtimeInfoUrl), content);
            return await DeserializeAsync<RealTimeDepartureInfo>(response);
        }

        /// <summary>
        /// Returns a Stop related to the specified short name
        /// including longitude and latitude information.
        /// </summary>
        /// <param name="stopShortName">Stop short name</param>
        /// <returns>Stop</returns>
        public static async Task<Stop> GetStopFromShortNameAsync(int stopShortName)
        {
            // First request to obtain ID
            var content = new StringContent("stop=" + stopShortName);
            var response = await _client.PostAsync(_stopInfoUrl, content);
            var stop = await DeserializeAsync<Stop>(response); // Dummy object for getting ID

            // Second request to obtain location information
            content = new StringContent("stopId=" + stop.ID);
            response = await _client.PostAsync(_stopUrl, content);
            return await DeserializeAsync<Stop>(response);
        }

        /// <summary>
        /// Returns trip information for the specified trip.
        /// The resulting TripInfo object contains a list of all the stops
        /// on the specified trip with predicted arrival or departure times.
        /// </summary>
        /// <param name="tripID">Trip ID (can be obtained from an ActualInfo object)</param>
        /// <param name="mode">Determines whether departure or arrival times are obtained</param>
        /// <returns></returns>
        public static async Task<TripInfo> GetTripInfoAsync(long tripID, DirectionMode mode)
        {
            var buster = ((long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds) * 1000;
            var content = new StringContent("tripId=" + tripID + "&mode=" + mode + "&cacheBuster=" + buster);
            var response = await _client.PostAsync(new Uri(_tripInfoUrl), content);
            return await DeserializeAsync<TripInfo>(response);
        }

        /// <summary>
        /// Gets a collection of all defined routes.
        /// </summary>
        /// <returns>Array of Route objects</returns>
        public static async Task<Route[]> GetAllRoutesAsync()
        {
            var response = await _client.PostAsync(new Uri(_routeInfoUrl), new StringContent(""));
            var routeInfo = await DeserializeAsync<RouteInfo>(response);
            return routeInfo.Routes;
        }

        private static async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            var s = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
                return (T)serializer.ReadObject(stream);
        }
    }

    public enum DirectionMode { Departure, Arrival }
}
