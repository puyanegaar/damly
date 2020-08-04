using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class SanadItemResponse
    {
        public string Description { get; set; }
        public decimal Bedeh { get; set; }
        public decimal Bestan { get; set; }
        public DateTime ConfirmDate { get; set; }
    }
}