using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{

    public class PayTypeMetaData
    {
        [Display(Name = "شناسه")]
        public byte Id { get; set; }

        [Display(Name = "نوع پرداخت")]
        public string Name { get; set; }
    }
}