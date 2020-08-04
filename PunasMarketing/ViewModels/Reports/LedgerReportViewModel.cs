using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Reports
{
    public class LedgerReportViewModel
    {
        public IEnumerable<PunasMarketing.Models.DomainModel.Sarfasl> Sarfasls { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.Tafsili> GetTForL { get; set; }

        public IEnumerable<PunasMarketing.Models.DomainModel.GetLedgerReport_Result> ledgerReport { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public short SarfaslId { get; set; }

        public string SarfaslName { get; set; }

        public decimal TBed { get; set; }

        public decimal TBes { get; set; }

        public decimal Mondeh { get; set; }

        public bool MondehAzGhable { get; set; }

        public string BeforeFromDate { get; set; }
    }
}