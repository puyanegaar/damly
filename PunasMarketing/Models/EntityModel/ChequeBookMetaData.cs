using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel.Cheque;

namespace PunasMarketing.Models.EntityModel.Cheque
{
    public class ChequeBookMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "حساب مربوطه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short BankAccountId { get; set; }

        [Display(Name = "تعداد برگ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public byte LeavesCount { get; set; }

        [Display(Name = "برگ های باقیمانده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public byte RemainingLeavesCount { get; set; }

        [Display(Name = "سریال اولین برگ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(20, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string FirstLeaf { get; set; }

        [Display(Name = "سریال آخرین برگ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(20, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string LastLeaf { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{

    [MetadataType(typeof(ChequeBookMetaData))]
    public partial class ChequeBook
    {

    }
}