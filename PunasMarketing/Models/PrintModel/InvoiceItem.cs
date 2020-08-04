using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.PrintModel
{
    public class InvoiceItem
    {
        public int Row { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }
        public string UnitName { get; set; }
    }
}