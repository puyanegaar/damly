using PunasMarketing.Models.DomainModel;
using System.Collections.Generic;

namespace PunasMarketing.ViewModels.Product
{
    public class ProductViewModel
    {
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<Models.DomainModel.Warehouse> Warehouses { get; set; }
        public IEnumerable<KeyValuePair<byte, string>> ProductionStatuses { get; set; }
        public Models.DomainModel.Product ProductModel { get; set; }
    }
}