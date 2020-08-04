using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class OrderItemResponse
    {
        public short ProductId { get; set; }
        public string ProductName { get; set; }
        public double? MainUnitCount { get; set; }
        public long UnitPrice { get; set; }
        public long UnitDiscount { get; set; }
        public string Description { get; set; }
    }
}