using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Factor
{
    public class InvoiceForFactorViewModel
    {
        public Models.DomainModel.Invoice Invoice { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public List<PunasMarketing.Models.DomainModel.Factor> factor { get; set; }

        public int FactorId { get; set; }

        [Display(Name = "نام انبار")]
        public string WarehouseName { get; set; }

        [Display(Name = "شناسه انبار")]
        public short WarehouseId { get; set; }
    }
}