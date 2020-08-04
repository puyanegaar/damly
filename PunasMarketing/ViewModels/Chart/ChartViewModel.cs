using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Chart
{
    public class ChartViewModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Color { get; set; }
    }

    public class BuyChartViewModel
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public string Color { get; set; }
    }
}