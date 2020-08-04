using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Product
{
    public class ProductListViewModel
    {
        public IEnumerable<Models.DomainModel.Product> Products { get; set; }
        public IEnumerable<KeyValuePair<byte, string>> ProductionStatuses { get; set; } 
        public IEnumerable<Models.DomainModel.Warehouse> Warehouses { get; set; }
        public IEnumerable<Models.DomainModel.ProductCategory> ProductCategories { get; set; }
    }
}