using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.PrintModel
{
    public class FactorItem
    {
        public int Row { get; set; }
        public string ProductName { get; set; }
        public double Count { get; set; }
        public string UnitName { get; set; }
        public string UnitPrice { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string TotalPrice { get; set; }
        public string FinalPrice { get; set; }
        public string Description { get; set; }
    }
}