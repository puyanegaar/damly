using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.EntityModel
{
    [MetadataType(typeof(OfferTypeMetaData))]
    public class OfferTypeMetaData
    {
        [Display(Name = "شناسه")]
        public byte Id { get; set; }

        [Display(Name = "نوع تخفیف")]
        public string Name { get; set; }
    }
}