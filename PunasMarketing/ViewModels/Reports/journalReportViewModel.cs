using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Reports
{
    public class journalReportViewModel
    {
        public IEnumerable<PunasMarketing.Models.DomainModel.journalReport_Result> JournalList { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.journalTotalAccountsReport__Result> JournalTAList { get; set; }
        public PunasMarketing.Models.DomainModel.SanadDetail_Result sanadDetail { get; set; }

        public int TotalItem { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
     
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}