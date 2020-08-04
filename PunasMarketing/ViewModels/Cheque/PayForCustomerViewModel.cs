using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Cheque
{
    public class PayForCustomerViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short CustomerId { get; set; }

        public IEnumerable<Models.DomainModel.Supplier> Suppliers { get; set; }
    }
}