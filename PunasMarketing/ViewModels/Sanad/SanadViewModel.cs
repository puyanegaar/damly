using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.ViewModels.Sanad
{
    public class SanadViewModel
    {
        public IEnumerable<SelectListItem> SarFasl { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.Tafsili> Tafsili { get; set; }
        public PunasMarketing.Models.LocalModel.SanadModel sanad { get; set; }

        public IEnumerable<PunasMarketing.Models.LocalModel.SanadItemModel> SanadItem { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.SanadDetail_Result> SanadDetail { get; set; }

        [Display(Name = "شرح سند")]
        public string SanadDes { get; set; }

        [Display(Name = "تاریخ سند")]
        public string SanadDate { get; set; }

        public int SanadId { get; set; }
        public Models.DomainModel.Sanad Sanad { get; set; }

        public decimal TotalBedeh { get; set; }
        public decimal TotalBestan { get; set; }
        public decimal TotalDiff { get; set; }

        public string SanadItemDeleted { get; set; }
    }
}