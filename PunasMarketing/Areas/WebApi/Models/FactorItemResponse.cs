using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class FactorItemResponse
    {
        public short ProductId { get; set; }
        public string ProductName { get; set; }
        public double? MainUnitCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public string Description { get; set; }
    }
}