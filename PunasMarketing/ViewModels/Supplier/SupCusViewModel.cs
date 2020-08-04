using PunasMarketing.Models.DomainModel;
using System.Collections.Generic;

namespace PunasMarketing.ViewModels.SupCus
{
    public class SupCusViewModel
    {
        public Models.DomainModel.Supplier SupplierCustomer { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Province> Provinces { get; set; }
    }
}