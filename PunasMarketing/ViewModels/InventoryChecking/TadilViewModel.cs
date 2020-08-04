using Newtonsoft.Json;
using PunasMarketing.Models.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.InventoryChecking
{
    public class TadilViewModel
    {
        public short InventoryCheckingId { get; set; }

        public int InventoryCheckingItemId { get; set; }

        public Models.DomainModel.Invoice Invoice { get; set; }

        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }

        public string InvoiceItemsJson { get; set; }

        [Display(Name = "نام انبار")]
        public string WarehouseName { get; set; }

        public string ProductName { get; set; }

        public string UnitName { get; set; }

    }
}