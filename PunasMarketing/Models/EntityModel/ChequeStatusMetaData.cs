using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    [MetadataType(typeof(ChequeStatusMetaData))]
    public class ChequeStatusMetaData
    {
        [Display(Name = "شناسه")]
        public byte Id { get; set; }

        [Display(Name = "وضعیت چک")]
        public string Name { get; set; }
    }
}