using PadersprinterService.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padersprinter.ViewModels
{
    class BengelViewmodel
    {
        public BengelViewmodel()
        {
            FillList();
        }

        private async void FillList()
        {
            var routes = await Parser.GetAllRoutesAsync();
            busLinien.AddRange(routes.Select(r => r.Name));
        }
        private List<String> busLinien = new List<string>();
        public List<String> BusLinien { get { return this.busLinien; } }



    }
}
