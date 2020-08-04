using PunasMarketing.Models.EntityModel;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class BankAccountMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "بانک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short BankId { get; set; }

        [Display(Name = "شماره شبا")]
        [StringLength(24, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Iban { get; set; }

        [Display(Name = "شماره حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(20, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string AccountNum { get; set; }

        [Display(Name = "نام صاحب حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Owner { get; set; }

        [Display(Name = "نام شعبه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string BranchName { get; set; }

        [Display(Name = "کد شعبه")]
        [StringLength(10, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string BranchCode { get; set; }

        [Display(Name = "کارت خوان")]
        public bool HasPos { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsAvailable { get; set; }
    }
}
namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(BankAccountMetaData))]
    public partial class BankAccount
    {
        public string BankNameAcc => $"{Bank.Name}-{AccountNum}";
    }
}