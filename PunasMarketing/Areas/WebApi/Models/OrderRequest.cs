using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class OrderRequest
    {
        public int CustomerId { get; set; }
        public int? MarketerId { get; set; }
        public string Description { get; set; }

        public List<OrderItemRequest> Items { get; set; }
    }
}