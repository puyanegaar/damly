using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.LocalModel
{
    public class BackFactorItems
    {
        public short ProductId { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }
        public short MainUnitId { get; set; }
        public string MainUnitName { get; set; }
        public short? SubUnitId { get; set; }
        public string SubUnitName { get; set; }
        public double? UnitsRate { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal Tax { get; set; }
        public decimal Commission { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal FinalPrice { get; set; }
        public string Description { get; set; }
    }
}