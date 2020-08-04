using System.Collections.Generic;
using System.Web.Mvc;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Invoice
{
    public class InvoiceItemViewModel
    {
        public IEnumerable<Models.DomainModel.Product> Products { get; set; }
        public Models.DomainModel.Invoice Invoice { get; set; }
        public InvoiceItem InvoiceItemModel { get; set; }
    }
}