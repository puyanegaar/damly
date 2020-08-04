using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.LocalModel
{
    public class JsInventoryCheckingItems
    {
        public int Index { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string MainUnit { get; set; }
        public double RealMainUnitCount { get; set; }
        public double SystemMainUnitCount { get; set; }
        public double Difference { get; set; }
        public bool IsCorrected { get; set; }
        public string Description { get; set; }
        public int InventoryCheckingItemId { get; set; }
    }
}