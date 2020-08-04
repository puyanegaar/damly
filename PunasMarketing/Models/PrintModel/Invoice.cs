using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.PrintModel
{
    public class Invoice
    {
        public int Id { get; set; }
        public string ResidOrHavaleh { get; set; }
        public string InvocieType { get; set; }
        public string ThisWarehouseName { get; set; }
        public string Date { get; set; }
        public string CreatorUserFullName { get; set; }
        public string Description { get; set; }
    }
}