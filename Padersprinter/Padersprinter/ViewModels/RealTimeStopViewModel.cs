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
        private int stopShortName;
        private Stop stop;
        private ObservableCollection<ActualInfo> items = new ObservableCollection<ActualInfo>();
        

        public string Linie
        {
            get
            {
                return this.stop.ShortName.ToString();
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
            Timer refreshTimer = new Timer(Refresh, null, 60000, 60000);
            this.stopShortName = stopShortName;
            this.Refresh(null);
        }

        private async void Refresh(object state)
        {
            if (this.stop != null)
            {
                this.stop = await Parser.GetStopFromShortNameAsync(this.stopShortName);
            }
            this.GetRealTimeInfo();
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
