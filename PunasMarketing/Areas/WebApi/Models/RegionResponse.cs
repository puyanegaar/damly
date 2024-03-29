﻿using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class RegionResponse
    {
        public short Id { get; set; }

        [Display(Name = "نام منطقه شهری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "نام شهر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short CityId { get; set; }
    }
}