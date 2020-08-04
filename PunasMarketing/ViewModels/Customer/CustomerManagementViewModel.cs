using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Customer
{
    public class CustomerManagementViewModel
    {
        public Models.DomainModel.Customer Customer { get; set; }
        public List<CustomerlReport_Result> FinancialReport { get; set; }
        public IQueryable<CustomerCategory> CustomerCategories { get; set; }
    }
}