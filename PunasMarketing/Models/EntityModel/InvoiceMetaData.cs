using System;
using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class InvoiceMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "رسید / حواله")]
        public bool IsReceive { get; set; }

        [Display(Name = "انبار مبدا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short ThisWareHouseId { get; set; }

        [Display(Name = "انبار مقصد")]
        public Nullable<short> OtherWareHouseId { get; set; }

        [Display(Name = "پرسنل")]
        public Nullable<int> PersonnelId { get; set; }

        [Display(Name = "شماره فاکتور")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string FactorNum { get; set; }

        [Display(Name = "تاریخ ثبت")]
        public System.DateTime CreatedDateTime { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public int CreatorUserId { get; set; }

        [Display(Name = "وضعیت اتمام")]
        public bool IsCompleted { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{

    [MetadataType(typeof(InvoiceMetaData))]
    public partial class Invoice
    {
        [Display(Name = "نام بخش")]
        public short? SectionId { get; set; }
    }
}