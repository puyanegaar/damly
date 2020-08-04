using System.Collections.Generic;
using PunasMarketing.ViewModels.Chart;

namespace PunasMarketing.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<Models.DomainModel.Cheque> PendingReceiveCheques { get; set; }
        public IEnumerable<Models.DomainModel.Cheque> PendingPayCheques { get; set; }
        public List<ChartViewModel> ChartViewModels { get; set; }
        public List<BuyChartViewModel> BuyChartViewModel { get; set; }
        public InfoViewModel Info { get; set; }
    }

    public class InfoViewModel
    {
        public decimal SellPrice { get; set; }
        public decimal BuyPrice { get; set; }
        public int ProductCount { get; set; }
        public int PersonnelCount { get; set; }
    }
  
}