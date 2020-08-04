using PunasMarketing.Models.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class FiscalPeriodMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "عنوان سال مالی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "تاریخ شروع سال مالی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب کنید")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان سال مالی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب کنید")]
        public DateTime EndDate { get; set; }

        [Display(Name = "نرخ مالیات بر ارزش افزوده")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public byte Vat { get; set; }

        [Display(Name = "وضعیت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب کنید")]
        public bool IsClosed { get; set; }

        [Display(Name = "تاریخ بستن حساب")]
        public DateTime? ClosedDate { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(FiscalPeriodMetaData))]
    public partial class FiscalPeriod
    {

    }
}