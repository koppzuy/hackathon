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

namespace Padersprinter.ViewModels
{
    class SearchResultViewModel : BindableBase
    {
        private static BasicPosition position = new BasicPosition(51.7102551, 8.7731854);
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public ObservableCollection<TripViewModel> Results { get; set; }

        public DelegateCommand StartSearch = new DelegateCommand(
async () =>
{ Stop[] stops = await Parser.FindStopsByLocationAsync(position, 2); },
            () => { return true; });
    }
}
