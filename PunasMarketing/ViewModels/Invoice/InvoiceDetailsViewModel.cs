using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Invoice
{
    public class InvoiceDetailsViewModel
    {
        public Models.DomainModel.Invoice Invoice { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}