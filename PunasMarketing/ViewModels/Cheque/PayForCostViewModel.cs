using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Cheque
{
    public class PayForCostViewModel
    {
        public int ChequeId { get; set; }

        [Display(Name = "هزینه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short CostId { get; set; }

        public IEnumerable<Models.DomainModel.Cost> Costs { get; set; }
    }
}