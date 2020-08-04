using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class OfferResponse
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "عنوان جشنواره")]
        public string Name { get; set; }

        [Display(Name = "تاریخ آغاز جشنواره")]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاریخ پایان جشنواره")]
        public DateTime ExpDate { get; set; }

        [Display(Name = "تصویر جشنواره")]
        public string ImageName { get; set; }

        [Display(Name = "نمایش در اسلایدر")]
        public bool ShowInSlider { get; set; }

        [Display(Name = "آیتم ها")]
        public List<OfferItemResponse> Items { get; set; }
    }
}