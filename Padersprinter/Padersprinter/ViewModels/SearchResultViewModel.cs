using Microsoft.Practices.Prism.Mvvm;
using PadersprinterService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using PadersprinterService.Parsing;
using PadersprinterService.DataModel;
using Windows.Services.Maps;

namespace Padersprinter.ViewModels
{
    class SearchResultViewModel : BindableBase
    {
        //MapRouteFinder.GetDrivingRouteFromWaypointsAsync()


        private static BasicPosition position = new BasicPosition(8.7731854,51.7102551);
        private DateTime currentTime = DateTime.Now;
        private List<Route> routes;
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public ObservableCollection<TripViewModel> Results { get; set; }
        

        public DelegateCommand StartSearch
        {
            get
            {
                return DelegateCommand.FromAsyncHandler(GetStopsByPosition);
            }
        }

        private async Task<List<Route>> GetAllRoutes()
        {
            return (await Parser.GetAllRoutesAsync()).ToList();
        }

        private async Task<Stop[]> GetStopsByPosition()
        {
            //routes = (await Parser.GetAllRoutesAsync()).ToList();
            Stop[] stops = await Parser.FindStopsByLocationAsync(position, 100);
            foreach (Stop s in stops)
            {
                FilterRoutesByStops(s);
                //System.Diagnostics.Debug.WriteLine($"ID: {s.ID}, Name: {s.Name}, ShortName: {s.ShortName}");
            }
            
            return stops;
        }

        private async Task<List<Route>> FilterRoutesByStops(Stop stop) 
        {
            RealTimeDepartureInfo routeInfo =  await Parser.GetRealTimeDepartureInfoAsync(stop.ShortName, DirectionMode.Departure, currentTime, currentTime.AddMinutes(120));

            foreach (ActualInfo ai in routeInfo.Actual)
            {
                TripInfo ti = await Parser.GetTripInfoAsync(ai.TripID, DirectionMode.Departure);                
            }            

            foreach (Route r in routes)
            {
                
            }

            return null;
        }

    }
}
