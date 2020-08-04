using System;

namespace PunasMarketing.Models.PrintModel
{
    public class Factor
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string FactorType { get; set; }
        public string TotalPrice { get; set; }
        public string DiscountOnFactor { get; set; }
        public string TotalDiscount { get; set; }
        public string Additions { get; set; }
        public string Deductions { get; set; }
        public string TotalTax { get; set; }
        public string FinalPrice { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}