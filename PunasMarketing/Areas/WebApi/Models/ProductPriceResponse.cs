using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class ProductPriceTypeResponse
    {
       
        public short Id { get; set; }

        [Display(Name = "نوع قیمت")]
        public string Name { get; set; }

        [Display(Name = "قیمت")]
        public decimal Price { get; set; }
    }
}