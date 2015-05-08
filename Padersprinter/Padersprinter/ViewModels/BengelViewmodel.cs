using PadersprinterService.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace Padersprinter.ViewModels
{
    class BengelViewmodel
    {
        private List<String> busLinien = new List<string>();
        public List<String> BusLinien { get { return this.busLinien; } }
        public Geopoint Location { get; set; }
        public BengelViewmodel()
        {
            FillList();
            Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = 8.7731854,
                Longitude = 51.7102551
            });
        }

        private async void FillList()
        {
            var routes = await Parser.GetAllRoutesAsync();
            busLinien.AddRange(routes.Select(r => r.Name));
        }

        private async Task<List<int>> getStopsByBusName(string busname)
        {
            return null;
        }
    }
}
