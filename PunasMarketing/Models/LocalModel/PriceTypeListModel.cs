namespace PunasMarketing.Models.LocalModel
{
    public class PriceTypeListModel
    {
        public int PriceTypeId { get; set; }

        public string PriceTypeTitle { get; set; }
        public bool Isvisiable { get; set; }
        public decimal PriceValue { get; set; }
    }
}