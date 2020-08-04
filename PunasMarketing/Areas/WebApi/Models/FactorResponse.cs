using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class FactorResponse
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal DiscountOnFactor { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal Additions { get; set; }
        public decimal Deductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsSent { get; set; }
        public bool? IsConfirmed { get; set; }
        public string Description { get; set; }

        public List<FactorItemResponse> Items { get; set; }
    }
}