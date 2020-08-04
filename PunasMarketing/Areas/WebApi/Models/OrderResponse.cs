using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? MarketerId { get; set; }
        public string MarketerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsVerified { get; set; }
        public int? FactorId { get; set; }
        public long TotalPrice { get; set; }
        public string Description { get; set; }

        public List<OrderItemResponse> Items { get; set; }
    }
}