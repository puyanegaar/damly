using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Cheque
{
    public class PayForSupplierViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "تأمین کننده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short SupplierId { get; set; }

        public IEnumerable<Models.DomainModel.Supplier> Suppliers { get; set; }
    }
}