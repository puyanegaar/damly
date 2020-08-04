using PunasMarketing.Models.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class PersonnelMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "کد پرسنلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string PersonnelCode { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string LastName { get; set; }

        [Display(Name = "نام پدر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string FatherName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(maximumLength: 10, MinimumLength = 10, ErrorMessage = "تعداد ارقام {0} باید {1} رقم باشد")]
        public string CodeMelli { get; set; }

        [Display(Name = "جنسیت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public bool IsMale { get; set; }

        [Display(Name = "وضعیت تأهل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public bool IsMarried { get; set; }

        [Display(Name = "شماره موبایل اصلی (برای ارسال پیامک)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "شماره موبایل را وارد نمایید")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Mobile1 { get; set; }

        [Display(Name = "شماره موبایل ")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Mobile2 { get; set; }

        [Display(Name = "تلفن ثابت")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string Phone { get; set; }

        [Display(Name = "تلفن آشنایان")]
        [StringLength(15, ErrorMessage = "تعداد ارقام نمی تواند بیش از {1} باشد")]
        public string RelatedPhone { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "شهر محل تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public int BirthCityId { get; set; }

        [Display(Name = "شهر محل سکونت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short HomeCityId { get; set; }

        [Display(Name = "نشانی محل سکونت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string HomeAddress { get; set; }

        [Display(Name = "تصویر")]
        public string ImageName { get; set; }

        [Display(Name = "ایمیل")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "تاریخ استخدام")]
        public DateTime HireDate { get; set; }

        [Display(Name = "تاریخ قطع همکاری")]
        public DateTime? FireDate { get; set; }

        [Display(Name = "مدرک تحصیلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string AcademicDegree { get; set; }

        [Display(Name = "عنوان شغلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short JobTitleId { get; set; }

        [Display(Name = "آیا کارمند حقوق بگیر است؟")]
        public bool IsGettingSalary { get; set; }

        [Display(Name = "شماره کارت بانکی")]
        public string BankCardNum { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }

        [Display(Name = "شبا")]
        [StringLength(26, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Iban { get; set; }

        [Display(Name = "نام بانک")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string BankName { get; set; }

        [Display(Name = "رشته تحصیلی")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string FieldOfStudy { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(PersonnelMetaData))]
    public partial class Personnel
    {
        [Display(Name = "بخش کاری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short SectionId { get; set; }

        [Display(Name = "استان محل تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short BirthStateId { get; set; }

        [Display(Name = "استان محل سکونت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short HomeStateId { get; set; }

        public string FullName => FirstName + " " + LastName;

        public bool SystemUser { get; set; }
    }
}