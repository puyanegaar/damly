using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Marketer
{
    public class AssignRegionViewModel
    {
        public int MarketerId { get; set; }
        public List<string> RegionsName { get; set; }
    }
}