using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public Models.DomainModel.Customer Customer { get; set; }
        public IEnumerable<Models.DomainModel.Customer> Customers { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Province> Provinces { get; set; }
        public IEnumerable<Models.DomainModel.Region> Regions { get; set; }
        public IEnumerable<CustomerCategory> CustomerCategories { get; set; }
        public IEnumerable<Models.DomainModel.Personnel> Marketers { get; set; }
    }
}