using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class OrderItemRequest
    {
        public short ProductId { get; set; }
        public double? MainUnitCount { get; set; }
        //public double? SubUnitCount { get; set; }
        public string Description { get; set; }
    }
}