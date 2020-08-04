using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.EntityModel;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.ViewModels.Offer
{
    public class OfferViewModel
    {
        public Models.DomainModel.Offer Offer { get; set; }
        public IEnumerable<CustomerCategory> CustomerCategories { get; set; }
        public IEnumerable<Models.DomainModel.Product> Products { get; set; }
        public OfferItem OfferItems { get; set; }
        public List<OfferTypeMetaData> OfferTypes { get; set; }

        [Display(Name = "تاریخ آغاز جشنواره")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string StartDate { get; set; }

        [Display(Name = "تاریخ پایان جشنواره")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string ExpDate { get; set; }
    }
}