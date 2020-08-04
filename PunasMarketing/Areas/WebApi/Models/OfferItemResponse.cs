using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class OfferItemResponse
    {
        [Display(Name = "شناسه کالا")]
        public short? ProductId { get; set; }

        [Display(Name = "نام کالا")]
        public string ProductName { get; set; }

        [Display(Name = "شناسه نوع تخفیف")]
        public byte OfferTypeId { get; set; }

        [Display(Name = "عنوان نوع تخفیف")]
        public string OfferTypeName { get; set; }

        [Display(Name = "مبلغ ریالی تخفیف")]
        public decimal? DiscountAmount { get; set; }

        [Display(Name = "درصد تخفیف")]
        public byte? DiscountPercent { get; set; }

        [Display(Name = "حداقل تعداد کالا")]
        public short? MinProductCount { get; set; }

        [Display(Name = "شناسه کالای هدیه")]
        public short? GiftProductId { get; set; }

        [Display(Name = "نام کالای هدیه")]
        public string GiftProductName { get; set; }

        [Display(Name = "تعداد کالای هدیه")]
        public short? GiftProductCount { get; set; }
    }
}