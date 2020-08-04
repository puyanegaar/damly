using System.Collections.Generic;

namespace PunasMarketing.ViewModels.Invoice
{
    public class InvoiceViewModel
    {
        
        public IEnumerable<Models.DomainModel.Warehouse> Warehouses { get; set; }
        public IEnumerable<Models.DomainModel.Section> Sections { get; set; }
        public IEnumerable<Models.DomainModel.Personnel> Personnels { get; set; }
        public Models.DomainModel.Invoice InvoiceModel { get; set; }
    }
}