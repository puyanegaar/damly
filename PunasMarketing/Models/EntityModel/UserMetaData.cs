using PunasMarketing.Models.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class UserMetaData
    {
        [Display(Name = "شناسه کاربر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public int PersonnelId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Username { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "تعداد کاراکتر باید بیش از {2} باشد")]
        public string Password { get; set; }

        [Display(Name = "وضعیت حساب کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب کنید")]
        public bool IsActive { get; set; }

        [Display(Name = "زمان آخرین ورود")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public DateTime LastLogin { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        [Display(Name = "تکرار کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "کلمه عبور را مجددا وارد کنید")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "تعداد کاراکتر باید بیش از {2} باشد")]
        public string RepeatPassword { get; set; }
    }
}
