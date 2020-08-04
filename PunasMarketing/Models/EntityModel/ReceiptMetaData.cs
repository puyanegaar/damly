using PunasMarketing.Models.EntityModel;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class ReceiptMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "نوع فیش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public bool IsReceive { get; set; }

        [Display(Name = "طرف حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int CounterPartyId { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int CreatedByUserId { get; set; }

        [Display(Name = "تاریخ ایجاد ")]
        public System.DateTime CreateDate { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(ReceiptMetaData))]
    public partial class Receipt
    {
        [Display(Name = "نوع طرف حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int CounterPartyType { get; set; }
    }
}