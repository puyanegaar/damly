using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Product
{
    public class ProductManagementViewModel
    {
        public Models.DomainModel.Product Product { get; set; }
        public int IsUpdate { get; set; }
        public IEnumerable<Models.LocalModel.PriceTypeListModel> PriceTypeList { get; set; }
        public IEnumerable<KeyValuePair<byte, string>> ProductionStatuses { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}