using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.ProductPrice
{
    public class ProductPriceViewModel
    {
        public ProductPriceList ProductPriceList { get; set; }
        public IEnumerable<PriceType> PriceTypes { get; set; }
        public IEnumerable<Models.DomainModel.Product> Products { get; set; }
    }
}