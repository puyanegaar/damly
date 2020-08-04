using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class CustomerMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "عنوان کسب و کار مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string BusinessName { get; set; }

        [Display(Name = "نام مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string OwnerName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "تعداد ارقام کد ملی باید {1} رقم باشد")]
        public string CodeMelli { get; set; }

        [Display(Name = "شماره موبایل اصلی (برای ارسال پیامک)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "شماره موبایل را وارد نمایید")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Mobile1 { get; set; }

        [Display(Name = "شماره موبایل 2")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Mobile2 { get; set; }

        [Display(Name = "تلفن ثابت")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Phone { get; set; }

        [Display(Name = "منطقه شهری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short RegionId { get; set; }

        [Display(Name = "نشانی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Address { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public double? Longitude { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public double? Latitude { get; set; }

        [Display(Name = "دسته بندی مشتری")]
        public short? CustomerCategoryId { get; set; }

        [Display(Name = "بازاریاب معرف")]
        public int? MarketerId { get; set; }

        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "کلمه عبور")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        //[StringLength(150, MinimumLength = 6, ErrorMessage = "تعداد کاراکتر باید بیش از {2} باشد")]
        public string Password { get; set; }

        public string SaltCode { get; set; }

        [Display(Name = "ایمیل")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string ImageName { get; set; }

        [Display(Name = "وضعیت حساب کاربری")]
        public bool IsActive { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer
    {
        [Display(Name = "استان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short ProvinceId { get; set; }

        [Display(Name = "شهر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short CityId { get; set; }
    }
}