using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.LocalModel
{
    public class CardModel
    {
        public int id { get; set; }

        public int SecondId { get; set; }

        [Display(Name = "مبلغ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Amount { get; set; }

        [Display(Name = "بانک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short BankAccountId { get; set; }


        [Display(Name = "کدرهگیری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string IssueTracking { get; set; }

        [Display(Name = "کارمزد")]
        public string Commission { get; set; }

        [Display(Name = "توضیح")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }


        public string CardAccountNumber { get; set; }
    }
}