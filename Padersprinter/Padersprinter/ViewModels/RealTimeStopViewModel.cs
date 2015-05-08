using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using PadersprinterService;
using PadersprinterService.DataModel;
using PadersprinterService.Parsing;

namespace Padersprinter.ViewModels
{
    class RealTimeStopViewModel : BindableBase
    {
        private Stop stop;
        private ObservableCollection<Stop> stops = new ObservableCollection<Stop>();
        private ObservableCollection<ActualInfo> items = new ObservableCollection<ActualInfo>();
        private int stopShortName;


        private Stop itemSelected;

        public Stop ItemSelected
        {
            get
            {
                return this.itemSelected;
            }
            set
            {
                if (value != this.itemSelected)
                {
                    this.stop = value;
                    this.Refresh(null);
                }
            }
        }

        public string Linie
        {
            get
            {
                if (stop != null)
                {
                    return this.stop.Name;
                }
                return null;
            }
        }

        public ObservableCollection<Stop> Stops
        {
            get
            {
                return stops;
            }
        }
        public ObservableCollection<ActualInfo> Items
        {
            get
            {
                return items;
            }
        }

        public RealTimeStopViewModel(int stopShortName)
        {
            this.stopShortName = stopShortName;
            this.Refresh(null);
        }

        private async void Refresh(object state)
        {
            if (this.stops.Count == 0)
            {
                Stop[] s = (await Parser.FindStopsByLocationAsync(new BasicPosition(8.768432611, 51.72760648), 100));
                foreach (Stop item in s)
                {
                    this.Stops.Add(item);
                }
            }

            if (this.stop == null && this.ItemSelected == null)
            {
                this.stop = (await Parser.GetStopFromShortNameAsync(this.stopShortName));
                this.OnPropertyChanged("Linie");
                this.GetRealTimeInfo();
            }
            if (this.stop != null )
            {
                    this.GetRealTimeInfo();
            }
            
        }

        private async void GetRealTimeInfo()
        {
            this.Items.Clear();
            RealTimeDepartureInfo info = await Parser.GetRealTimeDepartureInfoAsync(this.stop.ShortName, DirectionMode.Arrival);
            foreach (ActualInfo item in info.Actual)
            {
                this.items.Add(item);
            }

        }
    }
}
