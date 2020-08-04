using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Customer
{
    public class ChangeCustomerPasswordViewModel
    {
        public int CustomerId { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "تعداد کاراکتر باید بیش از {2} باشد")]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "کلمه عبور را مجددا وارد کنید")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "تعداد کاراکتر باید بیش از {2} باشد")]
        public string RepeatPassword { get; set; }
    }
}