using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Supplier
{
    public class SupplierManagementViewModel
    {
        public Models.DomainModel.Supplier Supplier { get; set; }
        public List<SupplierReport_Result> FinancialReport { get; set; }
    }
}