﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class CustomerUpdateRequest
    {
        [Display(Name = "شناسه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public int Id { get; set; }

        [Display(Name = "عنوان کسب و کار مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string BusinessName { get; set; }

        [Display(Name = "نام مشتری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string OwnerName { get; set; }

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

        [Display(Name = "ایمیل")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

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

        [Display(Name = "عکس")]
        public string ImageBase64 { get; set; }
    }
}