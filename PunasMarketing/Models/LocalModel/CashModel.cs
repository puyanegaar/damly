using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.LocalModel
{
    public class CashModel
    {
        public int id { get; set; }

        public int SecondId { get; set; }

        [Display(Name = "مبلغ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Amount { get;set;}

        [Display(Name = "صندوق")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short CashDeskId { get; set; }



        [Display(Name = "توضیح")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }
    }
}