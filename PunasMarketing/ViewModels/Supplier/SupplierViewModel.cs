using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Supplier
{
    public class SupplierViewModel
    {
        public Models.DomainModel.Supplier Supplier { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Province> Provinces { get; set; }
        public IEnumerable<Models.DomainModel.Region> Regions { get; set; }
    }
}