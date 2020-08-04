using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class OfferItemMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        public short OfferId { get; set; }

        [Display(Name = "کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public short? ProductId { get; set; }

        [Display(Name = "نوع تخفیف")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب نمایید")]
        public byte OfferTypeId { get; set; }

        [Display(Name = "مبلغ ریالی تخفیف")]
        public decimal? DiscountAmount { get; set; }

        [Display(Name = "درصد تخفیف")]
        public byte? DiscountPercent { get; set; }

        [Display(Name = "حداقل تعداد کالا")]
        public double? MinProductCount { get; set; }

        [Display(Name = "کالای هدیه")]
        public short? GiftProductId { get; set; }

        [Display(Name = "تعداد کالای هدیه")]
        public double? GiftProductCount { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(OfferItemMetaData))]
    public partial class OfferItem
    {

    }
}